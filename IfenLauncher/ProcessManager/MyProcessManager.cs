using System;
using System.Diagnostics;
using System.IO;

namespace IfenLauncher.ProcessManager
{
    class MyProcessManager
    {
        public enum GameDirName
        {
            MakeRoom
        }

        string programFilesPath;

        ProcessStartInfo brainAvatarProcessStartInfo;
        ProcessStartInfo neuroGuideProcessStartInfo;

        string bmFlProcessName = "FlashPlayer";
        string brainAvatarProcessName = "Master40";
        string neuroGuideProcessName = "ng";
        string flashPlayerExePath;

        Process serverFlashPlayerProcess;
        Process brainAvatarProcess;
        Process neuroGuideProcess;


        public MyProcessManager()
        {
            InitProgramFilesLocation();

            flashPlayerExePath = programFilesPath + @"\BrainMaster\Avatar\Flash Player\" + bmFlProcessName + ".exe";

            serverFlashPlayerProcess = new Process();
            serverFlashPlayerProcess.StartInfo.WorkingDirectory = @"C:\IFEN Games\Etc\Feedback";
            serverFlashPlayerProcess.StartInfo.FileName = flashPlayerExePath;
            serverFlashPlayerProcess.StartInfo.WindowStyle = ProcessWindowStyle.Minimized;


            brainAvatarProcessStartInfo = new ProcessStartInfo();
            brainAvatarProcessStartInfo.WorkingDirectory = programFilesPath + @"\BrainMaster\Avatar\Bin";
            brainAvatarProcessStartInfo.FileName = programFilesPath + @"\BrainMaster\Avatar\Bin\" + brainAvatarProcessName + ".exe";

            neuroGuideProcessStartInfo = new ProcessStartInfo();
            neuroGuideProcessStartInfo.WorkingDirectory = programFilesPath + @"\NeuroGuide";
            neuroGuideProcessStartInfo.FileName = programFilesPath + @"\NeuroGuide\" + neuroGuideProcessName + ".exe";
        }

        private void InitProgramFilesLocation()
        {
            if (Environment.Is64BitOperatingSystem)
            {
                programFilesPath = @"C:\Program Files (x86)";
            }
            else
            {
                programFilesPath = @"C:\Program Files";
            }
        }

        public string GetFlashPlayerExePath()
        {
            return flashPlayerExePath;
        }

        public void StartServerFlashPlayer()
        {
            serverFlashPlayerProcess.Start();
        }

        public void RegisterServerFlExitListener(EventHandler exitHandler)
        {
            serverFlashPlayerProcess.EnableRaisingEvents = true;
            serverFlashPlayerProcess.Exited += exitHandler;
        }

        public void StartFlashPlayerFor(GameDirName gameDirName)
        {
            ProcessStartInfo processStartInfo = new ProcessStartInfo();
            processStartInfo.WorkingDirectory = @"C:\IFEN Games\Etc\" + gameDirName.ToString();
            processStartInfo.FileName = flashPlayerExePath;
            Process.Start(processStartInfo);
        }

        /*public void RunBmFlashPlayer(bool runMultiple)
        {
            if (!IsBrainAvatarInstalled()) return;

            if (runMultiple || !IsBmFlashPlayerRunning())
            {
                flashPlayerProcess = Process.Start(serverFlashPlayerProcessInfo);
            }
        }*/

        public void RunBrainAvatar(bool runMultiple)
        {
            if (!IsBrainAvatarInstalled()) return;

            if (runMultiple || !IsBrainAvatarRunning())
            {
                brainAvatarProcess = Process.Start(brainAvatarProcessStartInfo);
            }
        }

        public void RunNeuroGuide(bool runMultiple)
        {
            if (!IsNeuroGuideInstalled()) return;

            if (runMultiple || !IsNeuroGuideRunning())
            {
                neuroGuideProcess = Process.Start(neuroGuideProcessStartInfo);
            }
        }

        public void KillBmFlashPlayer()
        {
            serverFlashPlayerProcess.Kill();
            return;
            if (serverFlashPlayerProcess != null)
            {
                try
                {
                    serverFlashPlayerProcess.Kill();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Maybe Already Killed: FlashPlayer.exe");
                    Debug.WriteLine("Error: flashPlayerProcess.Kill(): " + ex.Message);
                }
                serverFlashPlayerProcess = null;
            }
        }

        public bool IsBmFlashPlayerRunning()
        {
            return IsProcessRunning(bmFlProcessName);
        }
        public bool IsBrainAvatarRunning()
        {
            return IsProcessRunning(brainAvatarProcessName);
        }
        public bool IsNeuroGuideRunning()
        {
            return IsProcessRunning(neuroGuideProcessName);
        }

        public bool IsProcessRunning(string processName)
        {
            Process[] pname = Process.GetProcessesByName(processName);
            if (pname.Length > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool IsBrainAvatarInstalled()
        {
            return File.Exists(programFilesPath + @"\BrainMaster\Avatar\Bin\" + brainAvatarProcessName + ".exe");
        }

        public bool IsNeuroGuideInstalled()
        {
            return File.Exists(programFilesPath + @"\NeuroGuide\" + neuroGuideProcessName + ".exe");
        }
    }
}
