using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IfenLauncher.Model
{
    public class User
    {
        public string name { get; set; }
        public List<string> items { get; set; }
        public string email { get; set; }
        public bool passwordChanged { get; set; }


        public string GetString()
        {
            return name + ", " + email + ", " + items.ToString() + ", " + passwordChanged;
        }
    }
}
