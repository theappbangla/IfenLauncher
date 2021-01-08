using System;
using System.Diagnostics;
using System.IO;

namespace IfenLauncher.FlashFileHandler
{
    class MakeRoomCopy : BaseFlashFileCopier
    {
        public override string GetFileName()
        {
            return "IFEN Make Room v2.0.swf";
        }

        public override string GetMyIfenGameDir()
        {
            return @"C:\IFEN Games\Etc\MakeRoom";
        }

        public void HandleMakeRoomFile()
        {
            if(!IsFileExists())
            {
                try
                {
                    try
                    {
                        if (Directory.Exists(GetMyIfenGameDir()))
                        {
                            DirectoryInfo di = new DirectoryInfo(GetMyIfenGameDir());
                            foreach (FileInfo file in di.GetFiles())
                            {
                                if (file.Name.Contains("IFEN Make Room"))
                                {
                                    file.Delete();
                                }
                            }
                        }

                    } catch(Exception e)
                    {
                        Debug.WriteLine("Error: HandleMakeRoomFile(): make room remove: " + e.Message);
                    }
                    CopyResourceToDisk();
                    CopyResourceToDisk("IfenLauncher.Assets.make_room_logo.png", myIfenGameDir + @"\make_room_logo.png");
                } 
                catch(Exception e)
                {
                    Debug.WriteLine("Error: HandleMakeRoomFile(): " + e.Message);
                }
            } else
            {
                Debug.WriteLine("Swf Already Exists: " + fileName);
            }
        }

    }
}
