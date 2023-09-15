using System;
using ImageMagick;
using System.IO;

namespace automontronic.heic
{
    class Program
    {
        static void Main(string[] args)
        {
			bool retry = true;
			bool dirProcessing;
			string path;
			string delete;
            
            Console.WriteLine("Enter the path to a HEIC file to convert to JPG.");
            Console.WriteLine("Alternativelly, enter path to a directory and all HEIC files in it and its subdirectories will be converted.");
			do
			{
				Console.Write("> ");
                path = Console.ReadLine();
				if (File.Exists(path))
				{
					// This path is a file
					dirProcessing = false;
					retry = false;
				}
				else if (Directory.Exists(path))
				{
					// This path is a directory
					dirProcessing = true;
					retry = false;
				}
				else
				{
					Console.WriteLine("{0} is not a valid file or directory.", path);
				}
			} while (retry);

			retry = true;
			Console.WriteLine("Do you want the HEIC files to be deleted when their conversion is complete? (y/n)");
            if (dirProcessing)
            {
                Console.WriteLine("Keep in mind that this will affect all HEIC files in the selected directory AND ALL SUBDIRECTORIES.");
            }
			do
			{
                Console.Write("> ");
				delete = Console.ReadLine().ToLower();
				if (delete != "y" || delete != "n")
				{
					Console.WriteLine("Use 'y' to delete converted HEIC files, 'n' to keep them.");
				}
				else
				{
					retry = false;
				}
			} while (retry);
			
			if (dirProcessing)
			{
				ProcessDirectory(path, (delete == "y"));
			}
			else
			{
				ProcessFile(path, (delete == "y"));
			}

            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }
		
        // Process all files in the directory passed in, recurse on any directories 
        // that are found, and process the files they contain.
        public static void ProcessDirectory(string targetDirectory, bool deleteConverted = false)
        {
            // Process the list of files found in the directory.
            string[] fileEntries = Directory.GetFiles(targetDirectory);
            foreach (string fileName in fileEntries)
            {
                if (Path.GetExtension(fileName).ToLower() == ".heic")
                {
                    ProcessFile(fileName, deleteConverted);
                }
            }


            // Recurse into subdirectories of this directory.
            string[] subdirectoryEntries = Directory.GetDirectories(targetDirectory);
            foreach (string subdirectory in subdirectoryEntries)
                ProcessDirectory(subdirectory);
        }

        // Insert logic for processing found files here.
        public static void ProcessFile(string path, bool deleteConverted = false)
        {
            using (MagickImage image = new MagickImage(path))
            {
                string newFile = path.Replace(Path.GetExtension(path), ".jpg");
                image.Write(newFile);
            }
            Console.WriteLine("Processed file '{0}'.", path);
			if (deleteConverted)
			{
				File.Delete(path);
				Console.WriteLine("Deleted file '{0}'.", path);
			}
        }
    }
}
