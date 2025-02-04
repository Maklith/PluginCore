namespace PluginCore.Onnx;

public class OnnxModelInfoWrapper
{
    public string PluginStr { get; set; }      // 插件唯一标识
    public OnnxModelInfo Model { get; set; }  // 原始模型信息
}