using System;
using System.IO;
using System.Threading;
using FFmpeg.AutoGen;

namespace PluginCore.Media;

/// <summary>Owns a transformed native frame. Use on one thread at a time and dispose after encoding.</summary>
public sealed unsafe class FfmpegImage : IDisposable
{
    private AVFrame* _frame;
    private readonly bool _pngHasPartialAlpha;

    public FfmpegImageFormat Format { get; }
    public int Width { get; }
    public int Height { get; }
    public int OriginalWidth { get; }
    public int OriginalHeight { get; }

    internal FfmpegImage(string path, FfmpegImageFormat format, int resizePercent, int maxDimension,
        CancellationToken cancellationToken)
    {
        Format = format;
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
            if (format == FfmpegImageFormat.Original)
                Format = stream->codecpar->codec_id switch
                {
                    AVCodecID.AV_CODEC_ID_WEBP => FfmpegImageFormat.WebP,
                    AVCodecID.AV_CODEC_ID_MJPEG or AVCodecID.AV_CODEC_ID_LJPEG => FfmpegImageFormat.JPEG,
                    AVCodecID.AV_CODEC_ID_PNG or AVCodecID.AV_CODEC_ID_APNG => FfmpegImageFormat.PNG,
                    AVCodecID.AV_CODEC_ID_BMP => FfmpegImageFormat.BMP,
                    AVCodecID.AV_CODEC_ID_AV1 => FfmpegImageFormat.AVIF,
                    _ => throw new NotSupportedException("The original image format has no supported encoder.")
                };
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
            var scaleFilter = $"{rotation}scale={Width}:{Height}:flags=lanczos";
            var pixelFormat = Format switch
            {
                FfmpegImageFormat.WebP or FfmpegImageFormat.BMP => "bgra",
                FfmpegImageFormat.AVIF => "yuv444p",
                _ => "rgba"
            };
            var filter = Format == FfmpegImageFormat.JPEG
                ? $"[in]{scaleFilter},format=rgba,split[fg][bg];[bg]lutrgb=r=255:g=255:b=255:a=255[white];[white][fg]overlay=shortest=1,format=yuvj444p[out]"
                : $"[in]{scaleFilter},format={pixelFormat}[out]";
            _frame = Transform(first, filter, cancellationToken);
            if (Format == FfmpegImageFormat.PNG)
            {
                for (var y = 0; y < Height && !_pngHasPartialAlpha; y++)
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    var row = _frame->data[0] + y * _frame->linesize[0];
                    for (var x = 0; x < Width; x++)
                        if (row[x * 4 + 3] is > 0 and < 255)
                        {
                            _pngHasPartialAlpha = true;
                            break;
                        }
                }
            }
        }
        catch
        {
            Dispose();
            throw;
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

    private static AVFrame* Transform(AVFrame* input, string filter, CancellationToken cancellationToken)
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
        => Encode(outputPath, quality, lossless: Format == FfmpegImageFormat.PNG, cancellationToken);

    /// <summary>Chooses lossless or lossy encoding for PNG and WebP; other formats use their supported encoder.</summary>
    public void Encode(string outputPath, int quality, bool lossless, CancellationToken cancellationToken = default)
    {
        ObjectDisposedException.ThrowIf(_frame == null, this);
        ArgumentException.ThrowIfNullOrWhiteSpace(outputPath);
        if (quality is < 1 or > 100) throw new ArgumentOutOfRangeException(nameof(quality));
        cancellationToken.ThrowIfCancellationRequested();
        var codec = ffmpeg.avcodec_find_encoder_by_name(Format switch
        {
            FfmpegImageFormat.WebP => "libwebp",
            FfmpegImageFormat.JPEG => "mjpeg",
            FfmpegImageFormat.PNG => "png",
            FfmpegImageFormat.BMP => "bmp",
            FfmpegImageFormat.AVIF => "libaom-av1",
            _ => throw new NotSupportedException("The requested image format has no supported encoder.")
        });
        if (codec == null) throw new NotSupportedException("The requested image encoder is unavailable.");
        var encoder = ffmpeg.avcodec_alloc_context3(codec);
        var packet = ffmpeg.av_packet_alloc();
        AVFrame* quantized = null;
        AVFormatContext* container = null;
        try
        {
            if (encoder == null || packet == null) throw new OutOfMemoryException();
            var frame = _frame;
            if (Format == FfmpegImageFormat.PNG && !lossless)
            {
                var colors = 3 + (int)Math.Round((quality - 1) * 253 / 99.0);
                // Indexed PNG supports binary transparency. Retain the original alpha plane for semitransparent images.
                var filter = _pngHasPartialAlpha
                    ? $"[in]split=3[color][histogram][alpha];[histogram]palettegen=max_colors={colors}:reserve_transparent=0:stats_mode=single[palette];" +
                      "[color][palette]paletteuse=dither=sierra2_4a:alpha_threshold=0[quantized];[alpha]alphaextract[mask];[quantized][mask]alphamerge,format=rgba[out]"
                    : $"[in]split[color][histogram];[histogram]palettegen=max_colors={colors}:stats_mode=single[palette];" +
                      "[color][palette]paletteuse=dither=sierra2_4a[out]";
                quantized = Transform(_frame, filter, cancellationToken);
                frame = quantized;
            }
            encoder->width = Width;
            encoder->height = Height;
            encoder->pix_fmt = (AVPixelFormat)frame->format;
            encoder->time_base = new AVRational { num = 1, den = 25 };
            encoder->color_range = frame->color_range;
            encoder->compression_level = Format == FfmpegImageFormat.PNG ? 9 : 6;
            if (Format == FfmpegImageFormat.JPEG)
            {
                encoder->flags |= ffmpeg.AV_CODEC_FLAG_QSCALE;
                encoder->global_quality = (2 + (int)Math.Round((100 - quality) * 29 / 99.0)) * ffmpeg.FF_QP2LAMBDA;
            }
            else encoder->global_quality = (Format == FfmpegImageFormat.WebP && lossless ? 100 : quality) * ffmpeg.FF_QP2LAMBDA;
            if (Format == FfmpegImageFormat.WebP)
                Ffmpeg.Check(ffmpeg.av_opt_set_int(encoder->priv_data, "lossless", lossless ? 1 : 0, 0), "set WebP compression type");
            if (Format == FfmpegImageFormat.AVIF)
            {
                Ffmpeg.Check(ffmpeg.avformat_alloc_output_context2(&container, null, "avif", outputPath), "create AVIF container");
                encoder->flags |= ffmpeg.AV_CODEC_FLAG_GLOBAL_HEADER;
                encoder->color_primaries = _frame->color_primaries;
                encoder->color_trc = _frame->color_trc;
                encoder->colorspace = _frame->colorspace;
                encoder->thread_count = Math.Min(Environment.ProcessorCount, 8);
                Ffmpeg.Check(ffmpeg.av_opt_set_int(encoder->priv_data, "crf", (int)Math.Round((100 - quality) * 63 / 99.0), 0), "set AVIF quality");
                Ffmpeg.Check(ffmpeg.av_opt_set_int(encoder->priv_data, "cpu-used", 6, 0), "set AVIF encoding speed");
                Ffmpeg.Check(ffmpeg.av_opt_set_int(encoder->priv_data, "still-picture", 1, 0), "set AVIF still image");
            }
            Ffmpeg.Check(ffmpeg.avcodec_open2(encoder, codec, null), "open image encoder", cancellationToken);
            if (container != null)
            {
                var stream = ffmpeg.avformat_new_stream(container, null);
                if (stream == null) throw new OutOfMemoryException();
                stream->time_base = encoder->time_base;
                Ffmpeg.Check(ffmpeg.avcodec_parameters_from_context(stream->codecpar, encoder), "configure AVIF stream");
                Ffmpeg.Check(ffmpeg.avio_open(&container->pb, outputPath, ffmpeg.AVIO_FLAG_WRITE), "open AVIF output", cancellationToken);
                Ffmpeg.Check(ffmpeg.avformat_write_header(container, null), "write AVIF header", cancellationToken);
            }
            frame->quality = encoder->global_quality;
            Ffmpeg.Check(ffmpeg.avcodec_send_frame(encoder, frame), "encode image", cancellationToken);
            Ffmpeg.Check(ffmpeg.avcodec_send_frame(encoder, null), "flush image encoder", cancellationToken);
            using var output = container == null ? File.Create(outputPath) : null;
            while (true)
            {
                var received = ffmpeg.avcodec_receive_packet(encoder, packet);
                if (received == ffmpeg.AVERROR_EOF) break;
                Ffmpeg.Check(received, "receive encoded image", cancellationToken);
                if (container == null) output!.Write(new ReadOnlySpan<byte>(packet->data, packet->size));
                else
                {
                    ffmpeg.av_packet_rescale_ts(packet, encoder->time_base, container->streams[0]->time_base);
                    packet->stream_index = 0;
                    packet->duration = 1;
                    Ffmpeg.Check(ffmpeg.av_interleaved_write_frame(container, packet), "write AVIF image", cancellationToken);
                }
                ffmpeg.av_packet_unref(packet);
            }
            if (container != null)
            {
                Ffmpeg.Check(ffmpeg.av_write_trailer(container), "finish AVIF output", cancellationToken);
                ffmpeg.avio_flush(container->pb);
            }
            if ((output?.Length ?? new FileInfo(outputPath).Length) == 0)
                throw new InvalidDataException("FFmpeg produced an empty image.");
            cancellationToken.ThrowIfCancellationRequested();
        }
        finally
        {
            ffmpeg.av_packet_free(&packet);
            ffmpeg.avcodec_free_context(&encoder);
            ffmpeg.av_frame_free(&quantized);
            if (container != null)
            {
                if (container->pb != null) ffmpeg.avio_closep(&container->pb);
                ffmpeg.avformat_free_context(container);
            }
        }
    }

    public void Dispose()
    {
        var frame = _frame;
        _frame = null;
        ffmpeg.av_frame_free(&frame);
    }
}
