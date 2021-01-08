using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IfenLauncher.Utils
{
    class ManualPathManager
    {
        private static string manualPath = Path.Combine(Environment.CurrentDirectory, @"Assets\Manuals\");

        public static string CAPITO_ENGLISH = manualPath + "CAPTIO USER MANUAL BASIC EN.pdf";
        public static string CAPITO_GERMAN = manualPath + "CAPTIO USER MANUAL DE.pdf";

        public static string MAKE_ROOM_ENGLISH = manualPath + "IFEN-Pinball-Manual-English.pdf";

        public static string MAKE_ROOM_GERMAN = manualPath + "IFEN-Pinball-Manual-German.pdf";

        public static string ADVANCED_DIMMER_ENGLISH = manualPath + "Advanced-Dimmer-Manual-EN.pdf";

        public static string SWINGLE_ASSESSMENT_ENGLISH = manualPath + "Swingle Assessment Manual EN.pdf";
        public static string SWINGLE_ASSESSMENT_GERMAN = manualPath + "Swingle Assessment Manual DE.pdf";

    }
}
