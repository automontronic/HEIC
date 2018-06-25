using System;
using ImageMagick;
using System.IO;

namespace automontronic.heic
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Enter path to convert HEIC files to JPG and press enter:");
            string path = Console.ReadLine();
            if (File.Exists(path))
            {
                // This path is a file
                ProcessFile(path);
            }
            else if (Directory.Exists(path))
            {
                // This path is a directory
                ProcessDirectory(path);
            }
            else
            {
                Console.WriteLine("{0} is not a valid file or directory.", path);
            }

            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }
        // Process all files in the directory passed in, recurse on any directories 
        // that are found, and process the files they contain.
        public static void ProcessDirectory(string targetDirectory)
        {
            // Process the list of files found in the directory.
            string[] fileEntries = Directory.GetFiles(targetDirectory);
            foreach (string fileName in fileEntries)
            {
                if (Path.GetExtension(fileName).ToLower() == ".heic")
                {
                    ProcessFile(fileName);
                }
            }


            // Recurse into subdirectories of this directory.
            string[] subdirectoryEntries = Directory.GetDirectories(targetDirectory);
            foreach (string subdirectory in subdirectoryEntries)
                ProcessDirectory(subdirectory);
        }

        // Insert logic for processing found files here.
        public static void ProcessFile(string path)
        {
            using (MagickImage image = new MagickImage(path))
            {
                string newFile = path.Replace(Path.GetExtension(path), ".jpg");
                image.Write(newFile);
            }
            Console.WriteLine("Processed file '{0}'.", path);
        }
    }
}
