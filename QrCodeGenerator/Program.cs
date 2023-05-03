using System;
<<<<<<< Updated upstream
=======
using System.IO;
using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using ZXing;
using ZXing.QrCode;
>>>>>>> Stashed changes

namespace QrCodeGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Enter the URL or text to encode: ");
            var content = Console.ReadLine();

            Console.Write("Enter the filename for the QR code (without extension): ");
            var fileName = Console.ReadLine();

<<<<<<< Updated upstream
            Generator.GenerateQRCode(content, $"{fileName}.png");

            Console.WriteLine("QR code generated and saved to desktop.");
            Console.ReadKey();
        }
=======
            string connectionString = "server=localhost;database=qrcodes;user=qrcodes;password=qrcodes;";
            /* The GenerateQRCode method in the Generator class, which generates and saves the QR code image to the database. This method could be extracted into a separate class or controller method. */
            Generator.GenerateQRCode(content, fileName, connectionString);

            Console.WriteLine("QR code generated and saved to database.");

            Console.Write("Enter the FileName of the QR code to retrieve: ");
            var fileNameT = Console.ReadLine();
            /*The RetrieveImageFromDatabase method, which retrieves the image data from the database. This method could be extracted into a separate class or controller method.
            */
            var imageData = RetrieveImageFromDatabase(fileName, connectionString);

            var desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            var imagePath = Path.Combine(desktopPath, "image.png");
            File.WriteAllBytes(imagePath, imageData);

            Console.WriteLine($"QR code with fileName {fileName} saved to {imagePath}");
            Console.ReadKey();
        }
        /*The RetrieveImageFromDatabase method, which retrieves the image data from the database. This method could be extracted into a separate class or controller method.
*/
        static byte[] RetrieveImageFromDatabase(string fileName, string connectionString)
        {
            using var connection = new MySqlConnection(connectionString);
            connection.Open();

            using var command = new MySqlCommand("SELECT content FROM qrcodes WHERE fileName = @fileName", connection);
            command.Parameters.AddWithValue("@fileName", fileName);
            var imageData = (byte[])command.ExecuteScalar();

            return imageData;
        }
>>>>>>> Stashed changes
    }
}
