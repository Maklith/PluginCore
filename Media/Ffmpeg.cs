using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using FFmpeg.AutoGen;

namespace PluginCore.Media;

/// <summary>Loads bundled native FFmpeg libraries and decodes still images.</summary>
public sealed unsafe class Ffmpeg
{
    private static readonly Lazy<string> NativeVersion = new(Initialize);

    public string Version => NativeVersion.Value;

    private static string Initialize()
    {
        if (!OperatingSystem.IsWindows() || RuntimeInformation.ProcessArchitecture is not (Architecture.X64 or Architecture.Arm64))
            throw new PlatformNotSupportedException("Bundled LGPL FFmpeg libraries support Windows x64 and ARM64.");
        var directory = Path.Combine(AppContext.BaseDirectory, "tools", "ffmpeg");
        // Load dependencies from the bundle without changing the process DLL search path.
        foreach (var library in new[] { "avutil-61", "swresample-7", "swscale-10", "avcodec-63", "avformat-63", "avfilter-12", "avdevice-63" })
            NativeLibrary.Load(Path.Combine(directory, library + ".dll"), typeof(Ffmpeg).Assembly,
                DllImportSearchPath.UseDllDirectoryForDependencies);
        ffmpeg.RootPath = directory;
        if (ffmpeg.avcodec_version() >> 16 != ffmpeg.LIBAVCODEC_VERSION_MAJOR ||
            ffmpeg.avformat_version() >> 16 != ffmpeg.LIBAVFORMAT_VERSION_MAJOR ||
            ffmpeg.avutil_version() >> 16 != ffmpeg.LIBAVUTIL_VERSION_MAJOR ||
            ffmpeg.avfilter_version() >> 16 != ffmpeg.LIBAVFILTER_VERSION_MAJOR ||
            ffmpeg.swscale_version() >> 16 != ffmpeg.LIBSWSCALE_VERSION_MAJOR)
            throw new NotSupportedException("The bundled FFmpeg libraries do not match FFmpeg.AutoGen.");
        var configuration = ffmpeg.avcodec_configuration();
        if (configuration.Contains("--enable-gpl", StringComparison.Ordinal) ||
            configuration.Contains("--enable-nonfree", StringComparison.Ordinal))
            throw new NotSupportedException("Kitopia requires an LGPL FFmpeg build.");
        foreach (var encoder in new[] { "libwebp", "mjpeg", "png" })
            if (ffmpeg.avcodec_find_encoder_by_name(encoder) == null)
                throw new NotSupportedException($"The bundled FFmpeg lacks the {encoder} encoder.");
        return ffmpeg.av_version_info();
    }

    /// <summary>Decodes and transforms once, allowing repeated encoding at different qualities.</summary>
    public FfmpegImage OpenImage(string path, FfmpegImageFormat format, int resizePercent = 100,
        int maxDimension = 0, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(path);
        if (!Enum.IsDefined(format)) throw new ArgumentOutOfRangeException(nameof(format));
        if (resizePercent is < 1 or > 100) throw new ArgumentOutOfRangeException(nameof(resizePercent));
        if (maxDimension is < 0 or > 32768) throw new ArgumentOutOfRangeException(nameof(maxDimension));
        cancellationToken.ThrowIfCancellationRequested();
        _ = Version;
        return new FfmpegImage(Path.GetFullPath(path), format, resizePercent, maxDimension, cancellationToken);
    }

    internal static void Check(int result, string operation, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        if (result >= 0) return;
        var buffer = stackalloc byte[1024];
        ffmpeg.av_strerror(result, buffer, 1024);
        throw new InvalidDataException($"FFmpeg {operation}: {Marshal.PtrToStringUTF8((IntPtr)buffer)}");
    }
}
