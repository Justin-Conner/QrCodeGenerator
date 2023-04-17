using System;
using System.IO;
using MySqlConnector;
using ZXing;
using ZXing.QrCode;

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

            string connectionString = "server=localhost;database=qrcodes;user=qrcodes;password=qrcodes;";
            Generator.GenerateQRCode(content, fileName, connectionString);

            Console.WriteLine("QR code generated and saved to database.");

            Console.Write("Enter the FileName of the QR code to retrieve: ");
            var fileNameT = Console.ReadLine();


            var imageData = RetrieveImageFromDatabase(fileName, connectionString);

            var desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            var imagePath = Path.Combine(desktopPath, "image.png");
            File.WriteAllBytes(imagePath, imageData);

            Console.WriteLine($"QR code with fileName {fileName} saved to {imagePath}");
            Console.ReadKey();
        }

        static byte[] RetrieveImageFromDatabase(string fileName, string connectionString)
        {
            using var connection = new MySqlConnection(connectionString);
            connection.Open();

            using var command = new MySqlCommand("SELECT content FROM qrcodes WHERE fileName = @fileName", connection);
            command.Parameters.AddWithValue("@fileName", fileName);
            var imageData = (byte[])command.ExecuteScalar();

            return imageData;
        }
    }
}
