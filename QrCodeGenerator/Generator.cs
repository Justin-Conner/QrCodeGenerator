using System;
using System.Drawing;
using System.Drawing.Imaging;
using ZXing;
using ZXing.QrCode;

namespace QrCodeGenerator
{
    public class Generator
    {
        public static void GenerateQRCode(string content, string filePath)
        {
            var qrWriter = new BarcodeWriterPixelData
            {
                Format = BarcodeFormat.QR_CODE,
                Options = new QrCodeEncodingOptions
                {
                    Width = 500,
                    Height = 500,
                    Margin = 0
                }
            };

            var pixelData = qrWriter.Write(content);
            using var bitmap = new Bitmap(pixelData.Width, pixelData.Height, PixelFormat.Format32bppRgb);
            var bitmapData = bitmap.LockBits(new Rectangle(0, 0, pixelData.Width, pixelData.Height), ImageLockMode.WriteOnly, PixelFormat.Format32bppRgb);
            try
            {
                System.Runtime.InteropServices.Marshal.Copy(pixelData.Pixels, 0, bitmapData.Scan0, pixelData.Pixels.Length);
            }
            finally
            {
                bitmap.UnlockBits(bitmapData);
            }

            var desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            bitmap.Save($@"{desktopPath}\{filePath}", ImageFormat.Png);
        }
    }
}
