using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IfenLauncher.GameLoader
{
    interface IfenResourceLoaderCallback
    {
        void GameMainDirectoryNotExists();

        void GamesNotFound();

        void OnResouceProcessFinish(List<LauncherJson> launchers);
    }
}
