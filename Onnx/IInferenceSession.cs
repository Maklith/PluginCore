using System;
using System.Collections.Generic;
using System.Threading;

namespace PluginCore.Onnx;


public interface IInferenceSession : IDisposable
{
    public string Device { get; }

    /// <summary>
    /// Probes this device without initializing or replacing the application's model session.
    /// Return null if diagnostics are unsupported; throw with diagnostic details on failure.
    /// Accelerator probes must disable CPU fallback. The host runs this on a worker thread.
    /// </summary>
    public bool? CheckAvailability() => null;

    public void InitSession(string modelPath);

    public void InitSession(string modelPath, bool useCpuMemoryArena)
    {
        InitSession(modelPath);
    }

    /// <summary>
    /// Limits inference workers without changing the host process's CPU affinity.
    /// Older plugins keep their existing initialization behavior.
    /// </summary>
    public void InitSession(string modelPath, bool useCpuMemoryArena, int intraOpNumThreads)
    {
        InitSession(modelPath, useCpuMemoryArena);
    }

    /// <summary>
    /// Bounds the CUDA arena while preserving session-local buffer reuse. Zero means no explicit limit.
    /// Older plugins retain their existing initialization behavior; driver/workspace memory is excluded.
    /// CUDA allocation failures are reported as OutOfMemoryException.
    /// </summary>
    public void InitSession(string modelPath, bool useCpuMemoryArena, int intraOpNumThreads, long gpuMemoryLimitBytes)
    {
        InitSession(modelPath, useCpuMemoryArena, intraOpNumThreads);
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

    /// <summary>
    /// Runs a mixed-input model, such as a multimodal encoder with Int64 tokens and float features.
    /// The default implementation preserves compatibility with existing runtime plugins.
    /// CUDA allocation failures are reported as OutOfMemoryException so the host can recover without an ORT dependency.
    /// </summary>
    public Memory<float> Infer(
        List<(string, Memory<int>, Memory<long>)> int64Inputs,
        List<(string, Memory<int>, Memory<float>)> floatInputs,
        string outputName,
        CancellationToken cancellationToken)
    {
        throw new NotSupportedException($"The {Device} inference runtime does not support mixed Int64/float inputs.");
    }
    
    

}
