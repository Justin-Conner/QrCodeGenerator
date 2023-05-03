using System;
using System.IO;
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

            Generator.GenerateQRCode(content, $"{fileName}.png");

            Console.WriteLine("QR code generated and saved to desktop.");
            Console.ReadKey();
        }
    }
}
