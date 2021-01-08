using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IfenLauncher.GameLoader
{
    public class Manual
    {
        public string lang { get; set; }
        public string fileName { get; set; }
        public string fileOpenUrl { get; set; }

        public Manual()
        {

        }

        public Manual(string lang, string fileOpenUrl)
        {
            this.lang = lang;
            this.fileOpenUrl = fileOpenUrl;
        }
    }

    public class LauncherJson
    {
        public string id { get; set; }
        public string exeTitle { get; set; }
        public string exeLocation { get; set; }
        public string logo { get; set; }
        public string url { get; set; }
        public string description { get; set; }
        public string name { get; set; }
        public string buttonText { get; set; }
        public string flag { get; set; }
        public bool serverNotRequired { get; set; }
        public string rootDirName { get; set; }
        public string version { get; set; }


        //public string manualDir { get; set; }
        public string manualBtnText { get; set; }
        public List<Manual> manuals { get; set; }

    }

}
