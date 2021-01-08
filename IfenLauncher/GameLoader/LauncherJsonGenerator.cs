using IfenLauncher.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace IfenLauncher.GameLoader
{
    class LauncherJsonGenerator
    {
        public static string LINK_BASE_URL = "https://ifen-applications.appspot.com/link/";

        public static LauncherJson GetCapito()
        {
            LauncherJson capito = new LauncherJson();
            capito.exeTitle = "FlashPlayer"; // no effect
            //capito.exeLocation = processStartManager.GetFlashPlayerExePath(); // no effect
            capito.description = "Free with IFEN Neuroscience";
            capito.name = "Capito";
            capito.logo = Environment.CurrentDirectory + @"\Assets\capito_logo.png";
            capito.buttonText = "Open";
            capito.flag = "capito";
            capito.manualBtnText = "Manuals";
            capito.manuals = new List<Manual>() {
                new Manual("English", ManualPathManager.CAPITO_ENGLISH),
                new Manual("German", ManualPathManager.CAPITO_GERMAN)
            };
            return capito;
        }

        public static LauncherJson GetMakeRoom()
        {
            LauncherJson makeRoom = new LauncherJson();
            makeRoom.exeTitle = "FlashPlayer"; // no effect
            // makeRoom.exeLocation = processStartManager.GetFlashPlayerExePath(); // no effect
            makeRoom.description = "Free with IFEN Neuroscience";
            makeRoom.name = "Make Room";
            makeRoom.logo = Environment.CurrentDirectory + @"\Assets\make_room_logo.png";
            makeRoom.buttonText = "Run Game";
            makeRoom.flag = "makeroom";
            makeRoom.manualBtnText = "Manuals";
            makeRoom.manuals = new List<Manual>() {
                new Manual("English", ManualPathManager.MAKE_ROOM_ENGLISH),
                new Manual("German", ManualPathManager.MAKE_ROOM_GERMAN)
            };
            makeRoom.version = "2.0";
            return makeRoom;
        }

        public static LauncherJson GetPinball()
        {
            LauncherJson pinballBuy = new LauncherJson();
            pinballBuy.description = "Download Brain Pinball 3D NOW!";
            pinballBuy.name = "Brain Pinball 3D";
            pinballBuy.logo = Environment.CurrentDirectory + @"\Assets\brain_pinball_3d_logo.png";
            pinballBuy.buttonText = "Download";
            pinballBuy.url = LINK_BASE_URL + "pinball-pc";
            pinballBuy.flag = "url";
            pinballBuy.manualBtnText = "Manuals";
            pinballBuy.manuals = new List<Manual>() {
                    new Manual("English", ManualPathManager.MAKE_ROOM_ENGLISH),
                    new Manual("German", ManualPathManager.MAKE_ROOM_GERMAN)
            };
            return pinballBuy;
        }

        public static LauncherJson GetIfenRoads()
        {
            LauncherJson ifenRoads = new LauncherJson();
            ifenRoads.description = "Download IFEN Roads NOW!";
            ifenRoads.name = "IFEN Roads";
            ifenRoads.logo = Environment.CurrentDirectory + @"\Assets\ifen_roads_logo.png";
            ifenRoads.buttonText = "Download";
            ifenRoads.url = LINK_BASE_URL + "ifen-roads";
            ifenRoads.flag = "url";
            ifenRoads.manualBtnText = "Manuals";
            ifenRoads.manuals = new List<Manual>();
            return ifenRoads;
        }

        public static LauncherJson GetAirgame()
        {
            LauncherJson buy = new LauncherJson();
            buy.description = "Download AirGame NOW!";
            buy.name = "IFEN AirGame";
            buy.logo = Environment.CurrentDirectory + @"\Assets\ifen_airgame_logo_trial.png";
            buy.buttonText = "Download";
            buy.url = LINK_BASE_URL + "airgame-pc";
            buy.flag = "url";
            buy.manualBtnText = "Manuals";
            buy.manuals = new List<Manual>() ;
            return buy;
        }

        public static LauncherJson GetAdvancedDimmer()
        {
            LauncherJson dimmer = new LauncherJson();
            dimmer.id = "advanced-dimmer";
            dimmer.description = "IFEN Advanced Dimmer";
            dimmer.name = "Advanced Dimmer";
            dimmer.logo = Environment.CurrentDirectory + @"\Assets\dimmer_logo.png";
            dimmer.buttonText = "Open";
            dimmer.url = null;
            dimmer.flag = "advanceddimmer";
            dimmer.manualBtnText = "Manuals";
            dimmer.manuals = new List<Manual>() {
                    new Manual("English", ManualPathManager.ADVANCED_DIMMER_ENGLISH),
                };
            dimmer.version = "2.0";
            return dimmer;
        }

        public static LauncherJson GetIfenErpAnalyzer()
        {
            LauncherJson erpAnalyzer = new LauncherJson();
            erpAnalyzer.id = "erp-analyzer";
            erpAnalyzer.description = "IFEN ERP Analyzer";
            erpAnalyzer.name = "IFEN ERP Analyzer";
            erpAnalyzer.logo = Environment.CurrentDirectory + @"\Assets\erp_analyzer.png";
            erpAnalyzer.buttonText = "Download";
            erpAnalyzer.url = LINK_BASE_URL + "erp-analyzer";
            erpAnalyzer.flag = "url";
            erpAnalyzer.manualBtnText = "Manuals";
            erpAnalyzer.manuals = new List<Manual>();
            erpAnalyzer.serverNotRequired = true;
            erpAnalyzer.rootDirName = @"C:\IFEN\IFEN ERP Analyzer";
            erpAnalyzer.exeTitle = "IFEN ERP Analyzer";
            erpAnalyzer.exeLocation = @"C:\IFEN\IFEN ERP Analyzer\IFEN ERP Analyzer.exe";
            return GetProcessedLauncherJson(erpAnalyzer, "Open");
        }

        public static LauncherJson GetIfenUtilities()
        {
            LauncherJson launcherJson = new LauncherJson();
            launcherJson.id = "ifen-utilities";
            launcherJson.description = "Utility tools for IFEN Neuroscience";
            launcherJson.name = "IFEN Utilities";
            launcherJson.logo = Environment.CurrentDirectory + @"\Assets\utilities_logo.png";
            launcherJson.buttonText = "Open";
            launcherJson.url = null;
            launcherJson.flag = "ifenutilities";
            launcherJson.manualBtnText = "Manuals";
            launcherJson.manuals = new List<Manual>();
            launcherJson.serverNotRequired = true;
            return launcherJson;
        }

        public static LauncherJson GetSwingleAssessment()
        {
            LauncherJson launcherJson = new LauncherJson();
            launcherJson.id = "swingle-assessment";
            launcherJson.description = "Swingle Assessment developed by IFEN";
            launcherJson.name = "Swingle Assessment";
            launcherJson.logo = Environment.CurrentDirectory + @"\Assets\swingle_assessment_logo.png";
            launcherJson.buttonText = "Download";
            launcherJson.url = LINK_BASE_URL + "swingle-assessment";
            launcherJson.flag = "url";
            launcherJson.manualBtnText = "Manuals";
            launcherJson.manuals = new List<Manual>() {
                new Manual("English", ManualPathManager.SWINGLE_ASSESSMENT_ENGLISH),
                new Manual("German", ManualPathManager.SWINGLE_ASSESSMENT_GERMAN)
            };
            launcherJson.serverNotRequired = true;
            launcherJson.rootDirName = @"C:\Program Files (x86)\BrainMaster\Swingle Assessment Report Generator";
            launcherJson.exeTitle = "Swingle Assessment Report Generator";
            launcherJson.exeLocation = @"C:\Program Files (x86)\BrainMaster\Swingle Assessment Report Generator\Swingle Assessment Report Generator.exe";
            return GetProcessedLauncherJson(launcherJson, "Run");
        }





        private static LauncherJson GetProcessedLauncherJson(LauncherJson launcherJson, string buttonText)
        {
            if (File.Exists(launcherJson.exeLocation))
            {
                launcherJson.flag = null;
                launcherJson.buttonText = buttonText;

                string launcherPath = launcherJson.rootDirName + @"\launcher.ifen";
                if (!File.Exists(launcherPath)) return launcherJson ;

                try
                {
                    using (StreamReader file = File.OpenText(launcherPath))
                    using (JsonTextReader reader = new JsonTextReader(file))
                    {
                        JObject o2 = (JObject)JToken.ReadFrom(reader);
                        // Debug.WriteLine(o2);
                        LauncherJson launcherJsonLocal = o2.ToObject<LauncherJson>();
                        launcherJsonLocal.rootDirName = launcherJson.rootDirName;
                        launcherJsonLocal.exeLocation = launcherJson.exeLocation;
                        launcherJsonLocal.logo = launcherJson.rootDirName + @"\" + launcherJsonLocal.logo;
                        if (launcherJsonLocal.buttonText == null)
                        {
                            launcherJsonLocal.buttonText = buttonText;
                        }

                        launcherJsonLocal.manualBtnText = "Manuals";

                        if (launcherJsonLocal.manuals != null)
                        {
                            foreach (Manual m in launcherJsonLocal.manuals)
                            {
                                m.fileOpenUrl = launcherJson.rootDirName + @"\Manuals\" + m.fileName;
                            }
                        }

                        return launcherJsonLocal;
                    }
                }
                catch { }
            }
            return launcherJson;
        }
    }
}
