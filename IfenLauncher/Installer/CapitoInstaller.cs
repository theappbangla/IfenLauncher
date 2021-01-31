using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;

namespace IfenLauncher.Installer
{
    class CapitoInstaller
    {
        string capitoSetup = @"CAPiTO Setup 2.10.21.38.exe";
        string capitoSetupFullPath;
        string capitoInstalledExeFullPath;

        public CapitoInstaller(string capitoSetup):this()
        {
            this.capitoSetup = capitoSetup;
        }

        public CapitoInstaller()
        {
            capitoSetupFullPath = Path.Combine(Environment.CurrentDirectory, @"Assets\" + capitoSetup);

            if (Environment.Is64BitOperatingSystem)
            {
                capitoInstalledExeFullPath = @"C:\Program Files (x86)\CAPiTO\CAPITO.exe";
            }
            else
            {
                capitoInstalledExeFullPath = @"C:\Program Files\CAPiTO\CAPITO.exe";
            }

        }

        private bool IsInstalled()
        {
            if (capitoInstalledExeFullPath == null) return false;

            return File.Exists(capitoInstalledExeFullPath);
        }

        private void RunInstaller()
        {
            Process.Start(capitoSetupFullPath);
        }

        private void RunProgram()
        {
            Process.Start(capitoInstalledExeFullPath);
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
