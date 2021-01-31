using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;

namespace IfenLauncher.Installer
{
    class SensoxInstaller
    {
        string sensoxSetup = @"SensoX\setup.exe";
        string sensoxSetupFullPath;
        string sensoxInstalledExeFullPath;

        public SensoxInstaller(string sensoxSetup):this()
        {
            this.sensoxSetup = sensoxSetup;
        }

        public SensoxInstaller()
        {
            sensoxSetupFullPath = Path.Combine(Environment.CurrentDirectory, @"Assets\" + sensoxSetup);

            if (Environment.Is64BitOperatingSystem)
            {
                sensoxInstalledExeFullPath = @"C:\Program Files (x86)\Sensox\SensoX.exe";
            }
            else
            {
                sensoxInstalledExeFullPath = @"C:\Program Files\Sensox\SensoX.exe";
            }

        }

        private bool IsInstalled()
        {
            if (sensoxInstalledExeFullPath == null) return false;

            return File.Exists(sensoxInstalledExeFullPath);
        }

        private void RunInstaller()
        {
            Process.Start(sensoxSetupFullPath);
        }

        private void RunProgram()
        {
            Process.Start(sensoxInstalledExeFullPath);
        }

        public void RunInstallerOrProgram()
        {
            if (!IsInstalled())
            {
                RunInstaller();
            }
            else
            {
                RunProgram();
            }
        }
    }
}
