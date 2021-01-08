using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IfenLauncher.FlashFileHandler
{
    abstract class BaseFlashFileCopier
    {
        protected string flashPlayerPath;
        protected string fileName;
        protected string fileFullName;
        protected string embeddedResourceFile;
        protected string myIfenGameDir;

        public abstract string GetFileName();
        public abstract string GetMyIfenGameDir();

        public bool IsFileExists()
        {
            return File.Exists(fileFullName) && File.Exists(GetMyIfenGameDir() + @"\" + GetFileName());
        }

        public string GetFlashPlayerDirectoryPath()
        {
            return flashPlayerPath;
        }

        public BaseFlashFileCopier()
        {
            fileName = GetFileName();
            myIfenGameDir = GetMyIfenGameDir();

            embeddedResourceFile = "IfenLauncher" + ".Assets." + fileName;

            ConfigureBMFlashPlayerPath();

        }



        private void ConfigureBMFlashPlayerPath()
        {
            if (Environment.Is64BitOperatingSystem)
            {
                flashPlayerPath = @"C:\Program Files (x86)\BrainMaster\Avatar\Flash Player\";
            }
            else
            {
                flashPlayerPath = @"C:\Program Files\BrainMaster\Avatar\Flash Player\";
            }

            fileFullName = flashPlayerPath + fileName;
        }


        protected void CopyResourceToDisk()
        {
            CopyResourceToDisk(embeddedResourceFile, fileFullName);
            CopyToIfenGamesEtcFolder();
        }

        protected void CopyResourceToDisk(string embeddedResourceStr, string fileCopyPath)
        {
            //Debug.WriteLine("thi : " + GetType().Assembly.GetManifestResourceNames()[0]);

            using (Stream resource = GetType().Assembly.GetManifestResourceStream(embeddedResourceStr))
            {
                if (resource == null)
                {
                    Debug.WriteLine("No Resources: " + embeddedResourceStr);
                }
                using (Stream output = File.OpenWrite(fileCopyPath))
                {
                    resource.CopyTo(output);
                    Debug.WriteLine("Copy done: " + fileCopyPath);
                }
            }
        }

        protected void CopyToIfenGamesEtcFolder()
        {
            if (!Directory.Exists(myIfenGameDir))
            {
                Directory.CreateDirectory(myIfenGameDir);
            }

            CopyResourceToDisk("IfenLauncher.Assets." + GetFileName(), myIfenGameDir + @"\" + GetFileName());
        }
    }
}
