using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using OpenCvSharp;

namespace PluginCore;

/// <summary>
/// Local OCR owned by the desktop host. Plugins may consume it without owning model sessions.
/// </summary>
public interface IOcrService
{
    bool IsAvailable { get; }

    Task<IReadOnlyList<OcrTextRegion>> RecognizeAsync(Mat image, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<OcrTextRegion>> RecognizeFileAsync(string imagePath, CancellationToken cancellationToken = default);
}

public sealed record OcrTextRegion(string Text, int Left, int Top, int Width, int Height);
