using System;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace IfenLauncher.Server
{
    class IpDetect
    {
        IpDetectListener listener;
        IPAddress defaultGateway;

        public IpDetect(IpDetectListener listener)
        {
            this.listener = listener;
            defaultGateway = GetDefaultGateway();
        }

        public void RefreshIpMatchesDG()
        {
            
            if (defaultGateway == null)
            {
                Console.WriteLine("No default gateway");
                listener.IpDefaultGatewayNotFound();
                return;
            }
            Console.WriteLine("Default Gateway: " + defaultGateway);
            string[] defaultGatewayArr = defaultGateway.ToString().Split('.');
            if (defaultGatewayArr == null || defaultGatewayArr.Length < 3)
            {
                listener.IpAddressNotFound();
                return;
            }

            try
            {

                string defaultGatewayIdentifier = defaultGatewayArr[0] + "." + defaultGatewayArr[1] + "." + defaultGatewayArr[2];
                Console.WriteLine("Default Gateway Identifier: " + defaultGatewayIdentifier);

                IPAddress[] hostAddresses = Dns.GetHostAddresses("");

                foreach (IPAddress hostAddress in hostAddresses)
                {
                    if (hostAddress.AddressFamily == AddressFamily.InterNetwork &&
                        !IPAddress.IsLoopback(hostAddress) &&  // ignore loopback addresses
                        !hostAddress.ToString().StartsWith("169.254.") && // ignore link-local addresses
                        hostAddress.ToString().StartsWith(defaultGatewayIdentifier)) // matches with default gateway
                    {
                        Console.WriteLine("Found IP: " + hostAddress.ToString());
                        listener.IpAddressFound(hostAddress.ToString());
                        return;
                    }
                    else
                    {
                        Console.WriteLine("I called!!");
                        listener.IpAddressNotFound();
                    }
                }
            } catch (Exception e)
            {
                listener.IpAddressNotFound();
            }
        }

        private void FindIPAddress(string defaultGatewayIdentifier)
        {
            IPAddress[] hostAddresses = Dns.GetHostAddresses("");

            foreach (IPAddress hostAddress in hostAddresses)
            {
                if (hostAddress.AddressFamily == AddressFamily.InterNetwork &&
                    !IPAddress.IsLoopback(hostAddress) &&  // ignore loopback addresses
                    !hostAddress.ToString().StartsWith("169.254.") && // ignore link-local addresses
                    hostAddress.ToString().StartsWith(defaultGatewayIdentifier)) // matches with default gateway
                {
                    Console.WriteLine("Found!");
                    listener.IpAddressFound(hostAddress.ToString());
                }
                else
                {
                    listener.IpAddressNotFound();
                }
            }
        }

        private IPAddress GetDefaultGateway()
        {
            return NetworkInterface
                .GetAllNetworkInterfaces()
                .Where(n => n.OperationalStatus == OperationalStatus.Up)
                .Where(n => n.NetworkInterfaceType != NetworkInterfaceType.Loopback)
                .SelectMany(n => n.GetIPProperties()?.GatewayAddresses)
                .Select(g => g?.Address)
                .Where(a => a != null)
                // .Where(a => a.AddressFamily == AddressFamily.InterNetwork)
                // .Where(a => Array.FindIndex(a.GetAddressBytes(), b => b != 0) >= 0)
                .FirstOrDefault();
        }
    }
}
