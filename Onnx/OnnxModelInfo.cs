namespace PluginCore.Onnx;

public enum TargetDevice
{
    CPU,
    GPU,
    NPU
}
public class OnnxModelInfo
{
    public string Name { get; set; }
    public string Description { get; set; }
    
    public string SignName { get; set; }
    public string ModelPath { get; set; }
}