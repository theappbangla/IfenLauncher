using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;

namespace IfenLauncher.FlashFileHandler
{
    class FolderCopy
    {
        string source_dir, destination_dir;

        public FolderCopy(string source_dir, string destination_dir)
        {
            this.source_dir = source_dir;
            this.destination_dir = destination_dir;
        }

        public FolderCopy()
        {
            source_dir = Environment.CurrentDirectory + @"\Assets\Protocols";
            destination_dir = @"C:\ProgramData\BrainMaster\Settings";
        }

        public void DoCopy()
        {
            // substring is to remove destination_dir absolute path (E:\).

            // Create subdirectory structure in destination    
            foreach (string dir in Directory.GetDirectories(source_dir, "*", SearchOption.AllDirectories))
            {
                Directory.CreateDirectory(Path.Combine(destination_dir, dir.Substring(source_dir.Length + 1)));
                // Example:
                //     > C:\sources (and not C:\E:\sources)
            }

            foreach (string file_name in Directory.GetFiles(source_dir, "*", SearchOption.AllDirectories))
            {
                /*Debug.WriteLine("File Name: " + file_name);
                Debug.WriteLine("File Path: " + des_file_path);
                Debug.WriteLine("File Exists: " + File.Exists(des_file_path));
                Debug.WriteLine("\n");*/
                string des_file_path = Path.Combine(destination_dir, file_name.Substring(source_dir.Length + 1)).ToString();
                
                if(!File.Exists(des_file_path))
                {
                    File.Copy(file_name, Path.Combine(destination_dir, file_name.Substring(source_dir.Length + 1)));
                    Console.WriteLine("Copied: " + des_file_path) ;
                }
                else
                {
                    Console.WriteLine("Already Exists: " + des_file_path);
                }
                Console.WriteLine("\n");
            }
        }
    }
}
