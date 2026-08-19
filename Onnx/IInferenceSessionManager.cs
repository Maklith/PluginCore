

namespace PluginCore.Onnx;

public interface IInferenceSessionManager
{
    public IInferenceSession GetSession(string modelSignName);

    public IInferenceSession GetSession(string modelSignName, bool useCpuMemoryArena)
    {
        return GetSession(modelSignName);
    }
}
