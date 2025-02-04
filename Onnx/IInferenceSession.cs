using System;
using System.Collections.Generic;

namespace PluginCore.Onnx;


public interface IInferenceSession : IDisposable
{
    public TargetDevice Device { get; }
    public void InitSession(string modelPath);
    public void InitSession(byte[] modelData);
    
    public IReadOnlyList<string> InputNames { get; }
    
    public IReadOnlyList<int[]> OutputShape { get; }
    public Memory<float> Infer(List<(string,Memory<int>,Memory<float>)> inputs);
    
    

}