using System;
using System.IO;
using System.Threading;
using FFmpeg.AutoGen;

namespace PluginCore.Media;

/// <summary>Owns a transformed native frame. Use on one thread at a time and dispose after encoding.</summary>
public sealed unsafe class FfmpegImage : IDisposable
{
    private AVFrame* _frame;
    private readonly FfmpegImageFormat _format;

    public int Width { get; }
    public int Height { get; }
    public int OriginalWidth { get; }
    public int OriginalHeight { get; }

    internal FfmpegImage(string path, FfmpegImageFormat format, int resizePercent, int maxDimension,
        CancellationToken cancellationToken)
    {
        _format = format;
        AVFormatContext* input = ffmpeg.avformat_alloc_context();
        AVCodecContext* decoder = null;
        AVPacket* packet = ffmpeg.av_packet_alloc();
        AVFrame* decoded = ffmpeg.av_frame_alloc();
        AVFrame* first = null;
        AVDictionary* options = null;
        AVIOInterruptCB_callback interrupt = _ => cancellationToken.IsCancellationRequested ? 1 : 0;
        try
        {
            if (input == null || packet == null || decoded == null) throw new OutOfMemoryException();
            input->interrupt_callback.callback = interrupt;
            Ffmpeg.Check(ffmpeg.av_dict_set(&options, "protocol_whitelist", "file", 0), "set input protocols");
            Ffmpeg.Check(ffmpeg.avformat_open_input(&input, path, null, &options), "open image", cancellationToken);
            Ffmpeg.Check(ffmpeg.avformat_find_stream_info(input, null), "read image information", cancellationToken);
            var streamIndex = ffmpeg.av_find_best_stream(input, AVMediaType.AVMEDIA_TYPE_VIDEO, -1, -1, null, 0);
            Ffmpeg.Check(streamIndex, "find image stream", cancellationToken);
            var stream = input->streams[streamIndex];
            var codec = ffmpeg.avcodec_find_decoder(stream->codecpar->codec_id);
            if (codec == null) throw new NotSupportedException("No decoder is available for this image.");
            decoder = ffmpeg.avcodec_alloc_context3(codec);
            if (decoder == null) throw new OutOfMemoryException();
            Ffmpeg.Check(ffmpeg.avcodec_parameters_to_context(decoder, stream->codecpar), "configure decoder");
            Ffmpeg.Check(ffmpeg.avcodec_open2(decoder, codec, null), "open decoder", cancellationToken);

            // Drain packets and flush the decoder to detect delayed animation frames as well.
            var flushing = false;
            while (true)
            {
                cancellationToken.ThrowIfCancellationRequested();
                var received = ffmpeg.avcodec_receive_frame(decoder, decoded);
                if (received >= 0)
                {
                    if (first != null) throw new NotSupportedException("Animated images are not supported.");
                    first = ffmpeg.av_frame_clone(decoded);
                    if (first == null) throw new OutOfMemoryException();
                    ffmpeg.av_frame_unref(decoded);
                    continue;
                }
                if (received == ffmpeg.AVERROR_EOF) break;
                if (received != ffmpeg.AVERROR(ffmpeg.EAGAIN)) Ffmpeg.Check(received, "decode image", cancellationToken);
                if (flushing) throw new InvalidDataException("The image decoder did not finish flushing.");
                int read;
                do
                {
                    ffmpeg.av_packet_unref(packet);
                    read = ffmpeg.av_read_frame(input, packet);
                    cancellationToken.ThrowIfCancellationRequested();
                } while (read >= 0 && packet->stream_index != streamIndex);
                if (read == ffmpeg.AVERROR_EOF)
                {
                    flushing = true;
                    Ffmpeg.Check(ffmpeg.avcodec_send_packet(decoder, null), "flush decoder", cancellationToken);
                }
                else
                {
                    Ffmpeg.Check(read, "read image", cancellationToken);
                    Ffmpeg.Check(ffmpeg.avcodec_send_packet(decoder, packet), "send image packet", cancellationToken);
                }
            }
            if (first == null || first->width <= 0 || first->height <= 0)
                throw new InvalidDataException("The file contains no image frame.");
            var rotation = RotationFilter(first, stream->codecpar, out var swapAxes);
            OriginalWidth = swapAxes ? first->height : first->width;
            OriginalHeight = swapAxes ? first->width : first->height;
            var scale = resizePercent / 100.0;
            if (maxDimension > 0) scale = Math.Min(scale, (double)maxDimension / Math.Max(OriginalWidth, OriginalHeight));
            Width = Math.Max(1, (int)(OriginalWidth * scale));
            Height = Math.Max(1, (int)(OriginalHeight * scale));
            _frame = Transform(first, rotation, Width, Height, format, cancellationToken);
        }
        finally
        {
            ffmpeg.av_dict_free(&options);
            ffmpeg.av_frame_free(&first);
            ffmpeg.av_frame_free(&decoded);
            ffmpeg.av_packet_free(&packet);
            ffmpeg.avcodec_free_context(&decoder);
            ffmpeg.avformat_close_input(&input);
            GC.KeepAlive(interrupt);
        }
    }

    private static string RotationFilter(AVFrame* frame, AVCodecParameters* parameters, out bool swapAxes)
    {
        var sideData = ffmpeg.av_frame_get_side_data(frame, AVFrameSideDataType.AV_FRAME_DATA_DISPLAYMATRIX);
        var streamData = ffmpeg.av_packet_side_data_get(parameters->coded_side_data, parameters->nb_coded_side_data,
            AVPacketSideDataType.AV_PKT_DATA_DISPLAYMATRIX);
        var data = sideData != null ? sideData->data : streamData != null ? streamData->data : null;
        var size = sideData != null ? sideData->size : streamData != null ? streamData->size : 0;
        swapAxes = false;
        if (data == null || size < 9 * sizeof(int)) return "";
        var rotation = -ffmpeg.av_display_rotation_get(in *(int_array9*)data);
        if (!double.IsFinite(rotation)) return "";
        var angle = ((int)Math.Round(rotation) % 360 + 360) % 360;
        swapAxes = angle is 90 or 270;
        var values = (int*)data;
        // Match FFmpeg's autorotation rules, including mirrored EXIF orientations.
        return angle switch
        {
            90 => values[3] > 0 ? "transpose=cclock_flip," : "transpose=clock,",
            180 => (values[0] < 0 ? "hflip," : "") + (values[4] < 0 ? "vflip," : ""),
            270 => values[3] < 0 ? "transpose=clock_flip," : "transpose=cclock,",
            _ => values[4] < 0 ? "vflip," : ""
        };
    }

    private static AVFrame* Transform(AVFrame* input, string rotation, int width, int height,
        FfmpegImageFormat format, CancellationToken cancellationToken)
    {
        var graph = ffmpeg.avfilter_graph_alloc();
        var inputs = ffmpeg.avfilter_inout_alloc();
        var outputs = ffmpeg.avfilter_inout_alloc();
        AVFilterContext* source = null;
        AVFilterContext* sink = null;
        var frame = ffmpeg.av_frame_alloc();
        try
        {
            if (graph == null || inputs == null || outputs == null || frame == null) throw new OutOfMemoryException();
            var arguments = $"video_size={input->width}x{input->height}:pix_fmt={input->format}:time_base=1/25:pixel_aspect=1/1:" +
                $"colorspace={(int)input->colorspace}:range={(int)input->color_range}:alpha_mode={(int)input->alpha_mode}:chroma_location={(int)input->chroma_location}";
            Ffmpeg.Check(ffmpeg.avfilter_graph_create_filter(&source, ffmpeg.avfilter_get_by_name("buffer"),
                "input", arguments, null, graph), "create image source");
            Ffmpeg.Check(ffmpeg.avfilter_graph_create_filter(&sink, ffmpeg.avfilter_get_by_name("buffersink"),
                "output", null, null, graph), "create image sink");
            outputs->name = ffmpeg.av_strdup("in");
            outputs->filter_ctx = source;
            inputs->name = ffmpeg.av_strdup("out");
            inputs->filter_ctx = sink;
            var scale = $"{rotation}scale={width}:{height}:flags=lanczos";
            var filter = format == FfmpegImageFormat.JPEG
                ? $"[in]{scale},format=rgba,split[fg][bg];[bg]lutrgb=r=255:g=255:b=255:a=255[white];[white][fg]overlay=shortest=1,format=yuvj444p[out]"
                : $"[in]{scale},format={(format == FfmpegImageFormat.WebP ? "bgra" : "rgba")}[out]";
            Ffmpeg.Check(ffmpeg.avfilter_graph_parse_ptr(graph, filter, &inputs, &outputs, null), "parse image filters");
            Ffmpeg.Check(ffmpeg.avfilter_graph_config(graph, null), "configure image filters", cancellationToken);
            input->pts = 0;
            Ffmpeg.Check(ffmpeg.av_buffersrc_add_frame_flags(source, input, (int)AvBuffersrcFlag.AV_BUFFERSRC_FLAG_KEEP_REF),
                "transform image", cancellationToken);
            Ffmpeg.Check(ffmpeg.av_buffersrc_add_frame_flags(source, null, 0), "finish image filters", cancellationToken);
            Ffmpeg.Check(ffmpeg.av_buffersink_get_frame(sink, frame), "get transformed image", cancellationToken);
            ffmpeg.av_frame_remove_side_data(frame, AVFrameSideDataType.AV_FRAME_DATA_DISPLAYMATRIX);
            ffmpeg.av_frame_remove_side_data(frame, AVFrameSideDataType.AV_FRAME_DATA_EXIF);
            ffmpeg.av_dict_free(&frame->metadata);
            var result = frame;
            frame = null;
            return result;
        }
        finally
        {
            ffmpeg.av_frame_free(&frame);
            ffmpeg.avfilter_inout_free(&inputs);
            ffmpeg.avfilter_inout_free(&outputs);
            ffmpeg.avfilter_graph_free(&graph);
        }
    }

    public void Encode(string outputPath, int quality, CancellationToken cancellationToken = default)
    {
        ObjectDisposedException.ThrowIf(_frame == null, this);
        ArgumentException.ThrowIfNullOrWhiteSpace(outputPath);
        if (quality is < 1 or > 100) throw new ArgumentOutOfRangeException(nameof(quality));
        cancellationToken.ThrowIfCancellationRequested();
        var codec = ffmpeg.avcodec_find_encoder_by_name(_format switch
            { FfmpegImageFormat.WebP => "libwebp", FfmpegImageFormat.JPEG => "mjpeg", _ => "png" });
        if (codec == null) throw new NotSupportedException("The requested image encoder is unavailable.");
        var encoder = ffmpeg.avcodec_alloc_context3(codec);
        var packet = ffmpeg.av_packet_alloc();
        try
        {
            if (encoder == null || packet == null) throw new OutOfMemoryException();
            encoder->width = Width;
            encoder->height = Height;
            encoder->pix_fmt = (AVPixelFormat)_frame->format;
            encoder->time_base = new AVRational { num = 1, den = 25 };
            encoder->color_range = _frame->color_range;
            encoder->compression_level = _format == FfmpegImageFormat.PNG ? 9 : 6;
            if (_format == FfmpegImageFormat.JPEG)
            {
                encoder->flags |= ffmpeg.AV_CODEC_FLAG_QSCALE;
                encoder->global_quality = (2 + (int)Math.Round((100 - quality) * 29 / 99.0)) * ffmpeg.FF_QP2LAMBDA;
            }
            else encoder->global_quality = quality * ffmpeg.FF_QP2LAMBDA;
            Ffmpeg.Check(ffmpeg.avcodec_open2(encoder, codec, null), "open image encoder", cancellationToken);
            _frame->quality = encoder->global_quality;
            Ffmpeg.Check(ffmpeg.avcodec_send_frame(encoder, _frame), "encode image", cancellationToken);
            Ffmpeg.Check(ffmpeg.avcodec_send_frame(encoder, null), "flush image encoder", cancellationToken);
            using var output = File.Create(outputPath);
            while (true)
            {
                var received = ffmpeg.avcodec_receive_packet(encoder, packet);
                if (received == ffmpeg.AVERROR_EOF) break;
                Ffmpeg.Check(received, "receive encoded image", cancellationToken);
                output.Write(new ReadOnlySpan<byte>(packet->data, packet->size));
                ffmpeg.av_packet_unref(packet);
            }
            if (output.Length == 0) throw new InvalidDataException("FFmpeg produced an empty image.");
            cancellationToken.ThrowIfCancellationRequested();
        }
        finally
        {
            ffmpeg.av_packet_free(&packet);
            ffmpeg.avcodec_free_context(&encoder);
        }
    }

    public void Dispose()
    {
        var frame = _frame;
        _frame = null;
        ffmpeg.av_frame_free(&frame);
    }
}
