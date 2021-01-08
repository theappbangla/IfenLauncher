using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IfenLauncher.Model
{
    public class LoginUser
    {
        public string email { get; set; }
        public string password { get; set; }
        public string pcid { get; set; }
        public string gameid { get; set; }

        public Dictionary<string, string> ToDict()
        {
            return new Dictionary<string, string>
                {
                    { "email", email },
                    { "password", password },
                    { "pcid", pcid },
                    { "gameid", gameid },
                };
        }


        public string GetJson()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
