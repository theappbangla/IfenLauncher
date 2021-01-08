using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IfenLauncher.Server
{
    interface IpDetectListener
    {
        void IpDefaultGatewayNotFound();

        void IpAddressFound(string ipAddress);

        void IpAddressNotFound();
    }
}
