using System;
using System.Threading.Tasks;
using System.Threading;

namespace PluginCore;

public interface IScreenCaptureWindow
{
    public void CaptureScreen();
    public void RequestUserSelectScreenInfo(Action<ScreenCaptureInfo> action);
    public void RequestUserSelectScreenBytes(Action<ScreenCaptureResult> action, Action cancel );

    public async Task<ScreenCaptureResult> RequestUserSelectScreenBytesAsync(CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var completion = new TaskCompletionSource<ScreenCaptureResult>(TaskCreationOptions.RunContinuationsAsynchronously);
        using var registration = cancellationToken.Register(() => completion.TrySetCanceled(cancellationToken));
        RequestUserSelectScreenBytes(result =>
        {
            if (!completion.TrySetResult(result)) result.Source?.Dispose();
        }, () => completion.TrySetCanceled());
        return await completion.Task.ConfigureAwait(false);
    }

    public Task<ScreenCaptureInfo> GetScreenCaptureInfo();
}
