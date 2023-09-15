using System;
using ImageMagick;
using System.IO;
using System.Runtime.CompilerServices;

namespace automontronic.heic
{
    class Program
    {
        static void Main(string[] args)
        {
			bool retry = true;
			bool dirProcessing = false;
			string path;
			bool deleteJpgs;
            
            Console.WriteLine("Enter the path (absolute or relative) to a HEIC file to convert to JPG.");
            Console.WriteLine("Alternativelly, enter path to a directory and all HEIC files in it and its subdirectories will be converted.");
			do
			{
				Console.Write("> ");
                Console.ForegroundColor = ConsoleColor.Green;
                path = Console.ReadLine();
				Console.ResetColor();
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
					Console.ForegroundColor = ConsoleColor.Red;
					Console.WriteLine("'{0}' is not a valid file or directory.", path);
					Console.ResetColor();
				}
			} while (retry);

			retry = true;
			Console.WriteLine("\nDo you want the HEIC files to be deleted when their conversion is complete? (y/n)");
            if (dirProcessing)
            {
                Console.WriteLine("Keep in mind that this will affect all HEIC files in the selected directory AND ALL SUBDIRECTORIES.");
            }
            deleteJpgs = YesOrNoInput();

            Console.WriteLine();

			if (dirProcessing)
			{
				ProcessDirectory(path, deleteJpgs);
			}
			else
			{
				ProcessFile(path, deleteJpgs);
			}

            Console.WriteLine("\nDone!\nPress any key to continue...");
            Console.ReadKey(true);
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
                ProcessDirectory(subdirectory, deleteConverted);
        }

        // Insert logic for processing found files here.
        public static void ProcessFile(string path, bool deleteConverted = false)
        {
            using (MagickImage image = new MagickImage(path))
            {
                string newFile = path.Replace(Path.GetExtension(path), ".jpg");
                if (File.Exists(newFile))
				{
                    Console.WriteLine("File '{0}' already exists. Overwrite? (y/n)");
                    bool overwrite = YesOrNoInput();
                    if (!overwrite)
                    {
                        return;
                    }
				}
				image.Write(newFile);
            }
            Console.WriteLine("Processed file '{0}'.", path);
			if (deleteConverted)
			{
				File.Delete(path);
				Console.WriteLine("Deleted file '{0}'.", path);
			}
        }

        /// <summary>
        /// Prompts the user in the console with the classic "type 'y' or 'n'" dialog.
        /// As long as the user doesn't type one of these letters (or their uppercase variants), the dialogue keeps appearing.
        /// </summary>
        /// <returns>True, if the user entered 'y' (or 'Y'), False, if they entered 'n' (or 'N')</returns>
        public static bool YesOrNoInput()
        {
            string result;
            bool retry = true;
            do
            {
                Console.Write("> ");
                Console.ForegroundColor = ConsoleColor.Green;
                result = Console.ReadLine().ToLower();
                Console.ResetColor();
                if (result != "y" && result != "n")
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Use 'y' for 'Yes' or 'n' for 'No'.");
                    Console.ResetColor();
                }
                else
                {
                    retry = false;
                }
            } while (retry);

            return (result == "y");
        }
    }
}
