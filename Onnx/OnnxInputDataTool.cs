using System;
using OpenCvSharp;

namespace PluginCore.Onnx;

public class OnnxInputDataTool
{
    private static readonly int[] ChannelMap = [0, 0, 1, 1, 2, 2];

    public static unsafe void InputTensor(Mat image, Memory<float> destination)
    {
        var planeLength = image.Rows * image.Cols;
        var tensorLength = 3 * planeLength;
        if (image.Type() != MatType.CV_32FC3 || destination.Length < tensorLength)
        {
            throw new ArgumentException("OCR input must be a CV_32FC3 image with a sufficiently large tensor.", nameof(destination));
        }

        using var handle = destination[..tensorLength].Pin();
        var planeBytes = planeLength * sizeof(float);
        using var firstChannel = Mat.FromPixelData(image.Rows, image.Cols, MatType.CV_32FC1, (IntPtr)handle.Pointer);
        using var secondChannel = Mat.FromPixelData(image.Rows, image.Cols, MatType.CV_32FC1,
            IntPtr.Add((IntPtr)handle.Pointer, planeBytes));
        using var thirdChannel = Mat.FromPixelData(image.Rows, image.Cols, MatType.CV_32FC1,
            IntPtr.Add((IntPtr)handle.Pointer, 2 * planeBytes));
        Cv2.MixChannels([image], [firstChannel, secondChannel, thirdChannel], ChannelMap);
    }

    public static Memory<float> InputTensor(Mat image, int size)
    {
        var tensor = new float[size];
        InputTensor(image, tensor);
        return tensor;
    }
}
