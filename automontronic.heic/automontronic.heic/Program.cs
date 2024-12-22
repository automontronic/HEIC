using System;
using ImageMagick;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.Serialization;
using System.Text;
namespace adastra.heic
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("===== HEIC to PNG Converter =====");

            // Prompt user for source folder
            Console.Write("Enter the path to the folder containing HEIC files: ");
            string sourceFolder = Console.ReadLine();

            // Validate source folder
            if (string.IsNullOrWhiteSpace(sourceFolder) || !Directory.Exists(sourceFolder))
            {
                Console.WriteLine("Invalid source folder. Please ensure the path is correct.");
                return;
            }

            // Prompt user for output folder
            Console.Write("Enter the path to the folder where PNG files will be saved: ");
            string outputFolder = Console.ReadLine();

            // Validate output folder or create it
            if (string.IsNullOrWhiteSpace(outputFolder))
            {
                Console.WriteLine("Invalid output folder. Please ensure the path is correct.");
                return;
            }
            Directory.CreateDirectory(outputFolder);

            // Get all HEIC files from the source folder
            string[] heicFiles = Directory.GetFiles(sourceFolder, "*.heic", SearchOption.TopDirectoryOnly);

            if (heicFiles.Length == 0)
            {
                Console.WriteLine("No HEIC files found in the source folder.");
                return;
            }

            Console.WriteLine($"Found {heicFiles.Length} HEIC file(s). Converting...");

            foreach (string heicFile in heicFiles)
            {
                try
                {
                    string fileName = Path.GetFileNameWithoutExtension(heicFile);
                    string outputFilePath = Path.Combine(outputFolder, $"{fileName}.png");

                    // Convert HEIC to PNG
                    using (MagickImage image = new MagickImage(heicFile))
                    {
                        image.Write(outputFilePath);
                    }

                    Console.WriteLine($"Converted: {fileName}.heic to {fileName}.png");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to convert {heicFile}: {ex.Message}");
                }
            }

            Console.WriteLine("Conversion complete. Press any key to exit.");
            Console.ReadKey();
        }
    }
}
