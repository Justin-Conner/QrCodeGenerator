
using MySqlConnector;
using System.Drawing;
using System.Drawing.Imaging;
using ZXing;
using ZXing.QrCode;

namespace QrCodeGenerator
{
    public class Generator
    {
        public static void GenerateQRCode(string content, string fileName, string connectionString)
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

            using var ms = new MemoryStream();
            bitmap.Save(ms, ImageFormat.Png);
            var imageBytes = ms.ToArray();

            

            using var connection = new MySqlConnection($"server=localhost;database=qrcodes;user=qrcodes;password=qrcodes;");
            connection.Open();

            using var command = new MySqlCommand("INSERT INTO qrcodes (filename, content) VALUES (@fileName, @content)", connection);
            command.Parameters.AddWithValue("@fileName", fileName);
            command.Parameters.AddWithValue("@content", imageBytes);
            command.ExecuteNonQuery();
        }
    }
}
