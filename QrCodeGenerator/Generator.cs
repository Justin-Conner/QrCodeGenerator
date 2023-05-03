using System;
using MySqlConnector;
using Org.BouncyCastle.Utilities;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net.NetworkInformation;
using ZXing;
using ZXing.QrCode;
using MySql.Data.MySqlClient;

namespace QrCodeGenerator
{
    public class Generator
    {
        /* This code defines a class called Generator in the namespace QrCodeGenerator. The class contains a single static method called GenerateQRCode, which takes in three parameters: the content for the QR code, the filename to save the QR code as, and a connection string to the MySQL database. */
        /* The GenerateQRCode method in the Generator class, which generates and saves the QR code image to the database. This method could be extracted into a separate class or controller method. */
        public static void GenerateQRCode(string content, string fileName, string connectionString)
        {
            /*These lines create a QR code using the ZXing library. The BarcodeWriterPixelData object generates a pixel data representation of the QR code with specified dimensions and options. The resulting pixel data is stored in the pixelData variable.  */
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
            /*These lines create a bitmap image from the pixel data, convert it to a PNG format and save it to a memory stream.The image bytes are then stored in the imageBytes variable.*/
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

            /*These lines connect to the MySQL database using the connection string provided as an argument. Then, a SQL statement is created to insert the filename and image bytes into the qrcodes table. The command.Parameters.AddWithValue method is used to set the values for the parameters in the SQL statement, and the command.ExecuteNonQuery method is used to execute the SQL statement.  */
            using var connection = new MySqlConnection(connectionString);
            connection.Open();
            //the code (sql statement) below will be changed to take in the inventory data
            using var command = new MySqlCommand("INSERT INTO qrcodes (filename, content) VALUES (@fileName, @content)", connection);
            //we will need to add new statements below to add the inventory data that comes from the addInventory viewModel
            command.Parameters.AddWithValue("@fileName", fileName);
            command.Parameters.AddWithValue("@content", imageBytes);
            command.ExecuteNonQuery();
        }
    }
}
