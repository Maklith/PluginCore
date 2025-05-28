using System;
using OpenCvSharp;

namespace PluginCore.Onnx;

public class OnnxInputDataTool
{
    public static unsafe Memory<float> InputTensor(Mat dst,int size)
    {
        Mat[] imgArray = dst.Split();
        for (var i = 0; i < imgArray.Length; i++)
        {
                
            if (!(imgArray[i].IsContinuous()))
            {
                imgArray[i] = imgArray[i].Clone(); // 强制复制为连续内存[citation:2]
            }
            
        }
        Memory<float> targetMemory = new float[size];
        const int i1 = sizeof(float)/sizeof(byte);
        using var memoryHandle = targetMemory.Pin();
        Buffer.MemoryCopy(
            imgArray[0].DataPointer,
            memoryHandle.Pointer,
            targetMemory.Length * i1 ,
            imgArray[0].Total() * i1
        );
        
        Buffer.MemoryCopy(
            imgArray[1].DataPointer,
            (void*)((IntPtr)memoryHandle.Pointer + imgArray[0].Total() *i1),
            targetMemory.Length * i1,
            imgArray[1].Total() * i1
        );
        
        Buffer.MemoryCopy(
            imgArray[2].DataPointer,
            (void*)((IntPtr)memoryHandle.Pointer + (imgArray[0].Total() + imgArray[1].Total()) * i1),
            targetMemory.Length * i1,
            imgArray[2].Total() *i1
        );
        imgArray[0].Dispose();
        imgArray[1].Dispose();
        imgArray[2].Dispose();
        return targetMemory;
    }
}