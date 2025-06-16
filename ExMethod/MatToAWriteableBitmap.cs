using System;
using Avalonia;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using OpenCvSharp;

namespace PluginCore.ExMethod;

public static class MatToAWriteableBitmap
{
    public static WriteableBitmap ToAWriteableBitmap(this Mat mat)
    {
        if (!mat.IsContinuous())
        {
            mat= mat.Clone();
        }
        var writeableBitmap = new WriteableBitmap(
            new PixelSize(mat.Width, mat.Height),
            new Vector(96, 96), PixelFormat.Bgra8888);
        using (var l = writeableBitmap.Lock())
        {
            unsafe
            {
                var destinationSizeInBytes = mat.Width * 4 *mat.Height;
                    
                Buffer.MemoryCopy(mat.DataPointer,(void*)l.Address,destinationSizeInBytes,destinationSizeInBytes);
                    
                
            }
        }
        return writeableBitmap;
    }   
}