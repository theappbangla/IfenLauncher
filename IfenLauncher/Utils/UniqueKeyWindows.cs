using System.Management;

namespace IfenLauncher.Utils
{
    public class UniqueKeyWindows
    {
        string cpuInfo = string.Empty;
        string volumeSerial = string.Empty;

        public UniqueKeyWindows()
        {
            GetCpuKey();
            GetVolumeSerialKey();
        }


        public string GetCpuKey()
        {
            ManagementClass mc = new ManagementClass("win32_processor");
            ManagementObjectCollection moc = mc.GetInstances();

            foreach (ManagementObject mo in moc)
            {
                cpuInfo = mo.Properties["processorID"].Value.ToString();
                break;
            }

            return cpuInfo;
        }

        public string GetVolumeSerialKey()
        {
            string drive = "C";
            ManagementObject dsk = new ManagementObject(
                @"win32_logicaldisk.deviceid=""" + drive + @":""");
            dsk.Get();
            volumeSerial = dsk["VolumeSerialNumber"].ToString();
            return volumeSerial;
        }

        public string GetUniqueKey()
        {
            return cpuInfo + volumeSerial;
        }
    }
}
