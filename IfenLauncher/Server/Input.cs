using System;
using Newtonsoft.Json;

namespace IfenLauncher.Server
{
    public class Input
    {
        public float In1 = -1000f;
        public float In2 = -1000f;
        public float In3 = -1000f;
        public float In4 = -1000f;
        public float In5 = -1000f;
        public float Inkey = -1000f;
        public int lu; // last update

        private static Input input = null;

        public static Input GetInstance()
        {
            if (input == null)
            {
                input = new Input();
                return input;
            }
            else return input;
        }

        private Input()
        {
            Console.WriteLine("Instantiated!");
        }

        public string Text(string separator)
        {
            return "In1: " + In1 + separator +
                "In2: " + In2 + separator +
                "In3: " + In3 + separator +
                "In4: " + In4 + separator +
                "In5: " + In5 + separator +
                "Inkey: " + Inkey;
        }

        public string GetJson()
        {
            return JsonConvert.SerializeObject(this);
        }
    }

}
