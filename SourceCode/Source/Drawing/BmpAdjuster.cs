/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
namespace Sheng.SailingEase.Drawing
{
    public class BmpAdjuster
    {
        public delegate ColorPalette PaletteAdjustEvent(ColorPalette plt);
        public unsafe delegate void ConvertScanLineEvent(IntPtr srcLine, IntPtr dstLine, int width, int srcPixBit, int dstPixBit, Bitmap srcBmp, Bitmap dstBmp);
        private int alpha = 255;
        public BmpAdjuster()
        {
        }
        public BmpAdjuster(int alpha)
        {
            this.alpha = alpha;
        }
        public void AdjustColor(ref Bitmap bmp, PixelFormat format, PaletteAdjustEvent PalleteAdjust, ConvertScanLineEvent ConvertScanLine)
        {
            Rectangle rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
            Bitmap bmpOut = new Bitmap(bmp.Width, bmp.Height, format);
            bmpOut.Palette = PalleteAdjust(bmpOut.Palette);
            PixelFormat srcFmt = bmp.PixelFormat;
            PixelFormat dstFmt = bmpOut.PixelFormat;
            int srcPixBit = GetPixelSize(srcFmt);
            int dstPixBit = GetPixelSize(dstFmt);
            BitmapData srcData = null;
            BitmapData dstData = null;
            try
            {
                srcData = bmp.LockBits(rect, ImageLockMode.ReadOnly, srcFmt);
                dstData = bmpOut.LockBits(rect, ImageLockMode.WriteOnly, dstFmt);
                unsafe
                {
                    byte* srcLine = (byte*)srcData.Scan0.ToPointer();
                    byte* dstLine = (byte*)dstData.Scan0.ToPointer();
                    for (int L = 0; L < srcData.Height; L++)
                    {
                        ConvertScanLine((IntPtr)srcLine, (IntPtr)dstLine, srcData.Width, srcPixBit, dstPixBit, bmp, bmpOut);
                        srcLine += srcData.Stride;
                        dstLine += dstData.Stride;
                    }
                }
            }
            finally
            {
                bmp.UnlockBits(srcData);
                bmpOut.UnlockBits(dstData);
            }
            bmp = bmpOut;
        }
        internal int GetPixelSize(PixelFormat format)
        {
            switch (format)
            {
                case PixelFormat.Format16bppRgb555: return 16;
                case PixelFormat.Format16bppRgb565: return 16;
                case PixelFormat.Format24bppRgb: return 24;
                case PixelFormat.Format32bppRgb: return 32;
                case PixelFormat.Format1bppIndexed: return 1;
                case PixelFormat.Format4bppIndexed: return 4;
                case PixelFormat.Format8bppIndexed: return 8;
                case PixelFormat.Format16bppArgb1555: return 16;
                case PixelFormat.Format32bppPArgb: return 32;
                case PixelFormat.Format16bppGrayScale: return 16;
                case PixelFormat.Format48bppRgb: return 48;
                case PixelFormat.Format64bppPArgb: return 64;
                case PixelFormat.Canonical: return 32;
                case PixelFormat.Format32bppArgb: return 32;
                case PixelFormat.Format64bppArgb: return 64;
            }
            return 0;
        }
        public unsafe void Monochrome(ref Bitmap bmp)
        {
            AdjustColor(ref bmp, PixelFormat.Format1bppIndexed,
                new PaletteAdjustEvent(SetBlackWhitePallete),
                new ConvertScanLineEvent(ConvertBlackWhiteScanLine));
        }
        ColorPalette SetBlackWhitePallete(ColorPalette plt)
        {
            plt.Entries[0] = Color.Black;
            plt.Entries[1] = Color.White;
            return plt;
        }
        unsafe void ConvertBlackWhiteScanLine(IntPtr srcLine, IntPtr dstLine, int width, int srcPixBit, int dstPixBit, Bitmap srcBmp, Bitmap dstBmp)
        {
            byte* src = (byte*)srcLine.ToPointer();
            byte* dst = (byte*)dstLine.ToPointer();
            int srcPixByte = srcPixBit / 8;
            int x, v, t = 0;
            for (x = 0; x < width; x++)
            {
                v = 28 * src[0] + 151 * src[1] + 77 * src[2];
                t = (t << 1) | (v > 200 * 256 ? 1 : 0);
                src += srcPixByte;
                if (x % 8 == 7)
                {
                    *dst = (byte)t;
                    dst++;
                    t = 0;
                }
            }
            if ((x %= 8) != 7)
            {
                t <<= 8 - x;
                *dst = (byte)t;
            }
        }
        public void Gray(ref Bitmap bmp)
        {
            AdjustColor(ref bmp, PixelFormat.Format8bppIndexed,
                new PaletteAdjustEvent(SetGrayPallete),
                new ConvertScanLineEvent(ConvertGaryScanLine));
        }
        ColorPalette SetGrayPallete(ColorPalette plt)
        {
            for (int i = plt.Entries.Length - 1; i >= 0; i--)
                plt.Entries[i] = Color.FromArgb(alpha, i, i, i);
            return plt;
        }
        unsafe void ConvertGaryScanLine(IntPtr srcLine, IntPtr dstLine, int width, int srcPixBit, int dstPixBit, Bitmap srcBmp, Bitmap dstBmp)
        {
            byte* src = (byte*)srcLine.ToPointer();
            byte* dst = (byte*)dstLine.ToPointer();
            int srcPixByte = srcPixBit / 8;
            for (int x = 0; x < width; x++)
            {
                *dst = (byte)((28 * src[0] + 151 * src[1] + 77 * src[2]) >> 8);
                src += srcPixByte;
                dst++;
            }
        }
        public static Bitmap MonochromeLockBits(Bitmap pimage)
        {
            Bitmap source = null;
            if (pimage.PixelFormat != PixelFormat.Format32bppArgb)
            {
                source = new Bitmap(pimage.Width, pimage.Height, PixelFormat.Format32bppArgb);
                source.SetResolution(pimage.HorizontalResolution, pimage.VerticalResolution);
                using (Graphics g = Graphics.FromImage(source))
                {
                    g.DrawImageUnscaled(pimage, 0, 0);
                }
            }
            else
            {
                source = pimage;
            }
            BitmapData sourceData = source.LockBits(new Rectangle(0, 0, source.Width, source.Height), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
            int imageSize = sourceData.Stride * sourceData.Height;
            byte[] sourceBuffer = new byte[imageSize];
            Marshal.Copy(sourceData.Scan0, sourceBuffer, 0, imageSize);
            source.UnlockBits(sourceData);
            Bitmap destination = new Bitmap(source.Width, source.Height, PixelFormat.Format1bppIndexed);
            BitmapData destinationData = destination.LockBits(new Rectangle(0, 0, destination.Width, destination.Height), ImageLockMode.WriteOnly, PixelFormat.Format1bppIndexed);
            imageSize = destinationData.Stride * destinationData.Height;
            byte[] destinationBuffer = new byte[imageSize];
            int sourceIndex = 0;
            int destinationIndex = 0;
            int pixelTotal = 0;
            byte destinationValue = 0;
            int pixelValue = 128;
            int height = source.Height;
            int width = source.Width;
            int threshold = 500;
            for (int y = 0; y < height; y++)
            {
                sourceIndex = y * sourceData.Stride;
                destinationIndex = y * destinationData.Stride;
                destinationValue = 0;
                pixelValue = 128;
                for (int x = 0; x < width; x++)
                {
                    pixelTotal = sourceBuffer[sourceIndex + 1] + sourceBuffer[sourceIndex + 2] + sourceBuffer[sourceIndex + 3];
                    if (pixelTotal > threshold)
                    {
                        destinationValue += (byte)pixelValue;
                    }
                    if (pixelValue == 1)
                    {
                        destinationBuffer[destinationIndex] = destinationValue;
                        destinationIndex++;
                        destinationValue = 0;
                        pixelValue = 128;
                    }
                    else
                    {
                        pixelValue >>= 1;
                    }
                    sourceIndex += 4;
                }
                if (pixelValue != 128)
                {
                    destinationBuffer[destinationIndex] = destinationValue;
                }
            }
            Marshal.Copy(destinationBuffer, 0, destinationData.Scan0, imageSize);
            destination.UnlockBits(destinationData);
            if (source != pimage)
            {
                source.Dispose();
            }
            return destination;
        }
    }
}
