using Nancy;
using System.Diagnostics;

namespace IfenLauncher.Server
{

    public class MainModule : NancyModule
    {
        Input input = Input.GetInstance();
        public MainModule()
        {
            Get("/items/flash", parameters =>
            {
                input.In1 = this.Request.Query["In1"];
                input.In2 = this.Request.Query["In2"];
                input.In3 = this.Request.Query["In3"];
                input.In4 = this.Request.Query["In4"];
                input.In5 = this.Request.Query["In5"];
                input.Inkey = this.Request.Query["Inkey"];
                input.lu = System.DateTime.Now.Millisecond;
                return input.GetJson();
            });

            Get("/items/flash/values", parameters =>
            {
                return input.GetJson();
            });
        }
    }
}
