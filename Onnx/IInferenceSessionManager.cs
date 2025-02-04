

namespace PluginCore.Onnx;

public interface IInferenceSessionManager
{
    public IInferenceSession GetSession(string modelSignName);
}