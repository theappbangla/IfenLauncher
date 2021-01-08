using Nancy;
using Nancy.Hosting.Self;
using System;
using System.Diagnostics;
using IfenLauncher.ProcessManager;
using System.Net;
using System.Net.Sockets;

namespace IfenLauncher.Server
{
    class IfenHost
    {
        HostConfiguration hostConfigs;
        NancyHost nancyHost;

        string url;
        //Process flashPlayerProcess;
        bool serverStarted = false;

        public IfenHost(): this("http://localhost:3000") { }

        public IfenHost(string url)
        {
            this.url = url;
            hostConfigs = new HostConfiguration()
            {
                RewriteLocalhost = false,
                UrlReservations = new UrlReservations() { CreateAutomatically = true }
            };

            //nancyHost = new NancyHost(new Uri(url), new DefaultNancyBootstrapper(), hostConfigs);
            nancyHost = new NancyHost(new Uri(this.url));
        }

        public void StartServer()
        {
            nancyHost.Start();
            Debug.WriteLine("Server started at: " + url);
            serverStarted = true;
            //Process.Start("netsh.exe", @"http add urlacl url=http://+:3000/flash/values user=domain\user");


            /*Process proc = new Process();
            proc.StartInfo.FileName = "netsh.exe";
            proc.StartInfo.Arguments = @"http add urlacl url=http://+:3000/items/flash/values user=domain\user";
            proc.StartInfo.UseShellExecute = true;
            proc.StartInfo.Verb = "runas";
            proc.Start();*/
        }


        /*public void RunBmFlashPlayer()
        {
            if (processStartManager.IsBmFlashPlayerRunning()) return;

            flashPlayerProcess = processStartManager.StartBmFlashPlayerProcess();
        }*/


        public void StopServer()
        {
            nancyHost.Stop();
            Debug.WriteLine(url + " : server stopped");
            serverStarted = false;
        }

        public bool IsServerStarted()
        {
            return serverStarted;
        }

    }
}
