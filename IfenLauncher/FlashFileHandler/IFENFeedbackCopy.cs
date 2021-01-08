using System;
using System.IO;
using System.Diagnostics;

namespace IfenLauncher.FlashFileHandler
{
    class IFENFeedbackCopy: BaseFlashFileCopier
    {

        public override string GetFileName()
        {
            return "BrainCell.swf";
        }

        public override string GetMyIfenGameDir()
        {
            return @"C:\IFEN Games\Etc\Feedback";
        }

        public void HandleBrainCellFile()
        {

            if (!File.Exists(GetMyIfenGameDir() + @"\" + GetFileName()))
            {
                CopyToIfenGamesEtcFolder();
            }
            else
            {
                Debug.WriteLine("BrainCell.swf is already present in Etc folder");
            }
        }

        public void HandleBrainCellFileOld()
        {

            if (IsFileExists())
            {
                FileInfo fileInfo = new FileInfo(fileFullName);
                var size = fileInfo.Length;
                if (size <= 500)
                {
                    try
                    {
                        File.Delete(fileFullName);
                        Debug.WriteLine("Deleted");
                        CopyResourceToDisk();
                        Debug.WriteLine("Copy done.");
                    }
                    catch (IOException e)
                    {
                        Debug.WriteLine("Error: " + e.Message);
                    }
                }
                else if (size > 1500)
                {
                    try
                    {
                        File.Move(fileFullName, flashPlayerPath + "BrainCell000.swf");
                        Debug.WriteLine("Rename done.");
                        CopyResourceToDisk();
                        Debug.WriteLine("Copy done.");
                    }
                    catch (IOException e)
                    {
                        Debug.WriteLine("Error: " + e.Message);
                    }
                }
                else
                {
                    Debug.WriteLine("Everything OK! : " + fileName);
                }
            }
            else
            {
                Debug.WriteLine("BrainCell.swf not found");
                CopyResourceToDisk();
            }
        }
    }
}
