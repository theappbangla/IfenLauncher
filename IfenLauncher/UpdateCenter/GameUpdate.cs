using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IfenLauncher.UpdateCenter
{
    [Serializable]
    public class GameUpdate
    {
        public string gameid;
        public float version;
        public string url;
        public string updateText;
        public string upToDateText;

        public string ToString()
        {
            return "ID: " + gameid + "\nVersion: " + version + "\nURL: " + url + "\nUpdate Text: " + updateText + "\nUp To Date Text: " + upToDateText;
        }
    }
}