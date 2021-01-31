using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IfenLauncher.UpdateCenter
{
    public static class GameUtils
    {
        public static string GAME_ID = "ifen-neuroscience";
        public static string GAME_ROOT_URL = "https://ifen-game-verification.appspot.com";
        // public static string GAME_ROOT_URL = "http://localhost:8080";
        public static string GAME_LOGIN_URL = GAME_ROOT_URL + "/login";
        public static string GAME_UPDATE_URL = GAME_ROOT_URL + "/update";
        public static string GAME_BUY_URL = "https://www.neurofeedback-partner.de/product_info.php?info=p350_road-neurofeedback-game-by-ifen.html";
        public static float GAME_VERSION = 2.9f;
    }
}