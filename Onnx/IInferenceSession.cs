using System;
using System.Collections.Generic;
using System.Threading;

namespace PluginCore.Onnx;


public interface IInferenceSession : IDisposable
{
    public string Device { get; }
    public void InitSession(string modelPath);

    public void InitSession(string modelPath, bool useCpuMemoryArena)
    {
        InitSession(modelPath);
    }

    public void InitSession(byte[] modelData);
    
    public IReadOnlyList<string> InputNames { get; }
    
    public IReadOnlyList<int[]> OutputShape { get; }
    public Memory<float> Infer(List<(string,Memory<int>,Memory<float>)> inputs);

    public Memory<float> Infer(List<(string, Memory<int>, Memory<float>)> inputs, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var result = Infer(inputs);
        cancellationToken.ThrowIfCancellationRequested();
        return result;
    }

    /// <summary>
    /// Runs models whose inputs are token ids or masks instead of image tensors.
    /// Existing runtime plugins can keep loading because the default implementation is opt-in.
    /// </summary>
    public Memory<float> InferInt64(List<(string, Memory<int>, Memory<long>)> inputs)
    {
        throw new NotSupportedException($"The {Device} inference runtime does not support Int64 inputs.");
    }

    /// <summary>
    /// Runs a token-id model and reads the named output. Models that expose both token states and a pooled
    /// sentence embedding need this overload to avoid relying on ONNX output ordering.
    /// </summary>
    public Memory<float> InferInt64(
        List<(string, Memory<int>, Memory<long>)> inputs,
        string outputName)
    {
        throw new NotSupportedException(
            $"The {Device} inference runtime does not support selecting the '{outputName}' output.");
    }

    public Memory<float> InferInt64(
        List<(string, Memory<int>, Memory<long>)> inputs,
        string outputName,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var result = InferInt64(inputs, outputName);
        cancellationToken.ThrowIfCancellationRequested();
        return result;
    }
    
    

}
