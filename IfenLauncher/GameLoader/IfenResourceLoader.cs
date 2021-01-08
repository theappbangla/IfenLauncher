using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics;

namespace IfenLauncher.GameLoader
{
    class IfenResourceLoader
    {
        string gamesDir;
        List<LauncherJson> allGameLaunchers;
        List<string> allGameDirs;
        DirectoryInfo mainDirectory;
        DirectoryInfo[] gameDirectories;

        IfenResourceLoaderCallback callback;

        public bool IsMainDirectoryExists() { return mainDirectory.Exists; }

        public bool IsGameExists() { return gameDirectories.Length > 0; }

        public List<string> GetAllGameDirs() { return allGameDirs; }

        public List<LauncherJson> GetIfenLaunchers() { return allGameLaunchers; }

        public IfenResourceLoader(IfenResourceLoaderCallback callback)
        {
            this.callback = callback;
            gamesDir = @"C:\IFEN Games";
            allGameLaunchers = new List<LauncherJson>();
            allGameDirs = new List<string>();
            mainDirectory = new DirectoryInfo(gamesDir);

            // ProcessAllLaunchers() --> call from client

            CheckErrors();
        }

        private void CheckErrors()
        {
            if (!mainDirectory.Exists)
            {
                callback.GameMainDirectoryNotExists();
                return;
            }
            else
            {
                gameDirectories = mainDirectory.GetDirectories();
            }
            // var files = directoryInfo.GetFiles("*.dexe").OrderBy(x => x.CreationTimeUtc);

            if (!IsGameExists())
            {
                callback.GamesNotFound();
                return;
            }
        }

        public void ProcessAllLaunchers()
        {
            if (gameDirectories == null) 
            {
                return;
            }

            foreach (DirectoryInfo dir in gameDirectories)
            {
                string launcherPath = gamesDir + @"\" + dir + @"\launcher.ifen";
                Debug.WriteLine("gamesDir: " + gamesDir);
                if (!File.Exists(launcherPath)) continue;

                allGameDirs.Add(dir.FullName);

                try
                {
                    using (StreamReader file = File.OpenText(launcherPath))
                    using (JsonTextReader reader = new JsonTextReader(file))
                    {
                        JObject o2 = (JObject)JToken.ReadFrom(reader);
                       // Debug.WriteLine(o2);
                        LauncherJson launcherJson = o2.ToObject<LauncherJson>();
                        launcherJson.rootDirName = dir.FullName;
                        launcherJson.exeLocation = dir.FullName + @"\" + launcherJson.exeTitle + ".exe";
                        launcherJson.logo = dir.FullName + @"\" + launcherJson.logo;
                        if (launcherJson.buttonText == null)
                        {
                            launcherJson.buttonText = "Run Game";
                        }

                        launcherJson.manualBtnText = "Manuals";
                        //launcherJson.manualDir = dir.FullName + @"\" + "Manuals";
                        if(launcherJson.manuals != null)
                        {
                            foreach(Manual m in launcherJson.manuals)
                            {
                                m.fileOpenUrl = dir.FullName + @"\Manuals\" + m.fileName;
                            }
                        }

                        allGameLaunchers.Add(launcherJson);
                    }
                }
                catch (System.IO.IOException e)
                {
                    Debug.WriteLine("IO Exception: " + e.Message);
                }
            }

            callback.OnResouceProcessFinish(allGameLaunchers);

        }

        public void PrintLaunchersInfo()
        {
            for (int i = 0; i < allGameLaunchers.Count; i++)
            {
                LauncherJson launcher = allGameLaunchers[i];
                Debug.WriteLine("[" + i + "] : " + launcher.exeLocation + " <----> " + launcher.url + " <----> " + launcher.logo);
            }
        }
    }
}
