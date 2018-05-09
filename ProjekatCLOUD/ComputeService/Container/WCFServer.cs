using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using Contratcs;

namespace ContainerService
{
    public class WCFServer
    {
        private ServiceHost serviceHost;

        public WCFServer(int port)
        {
            Start(port);
        }

        public void Start(int port)
        {
            serviceHost = new ServiceHost(typeof(Container));
            NetTcpBinding binding = new NetTcpBinding();
            string uri = String.Format("net.tcp://localhost:{0}/Container", port);
            serviceHost.AddServiceEndpoint(typeof(IContainer), binding, new Uri(uri));
            serviceHost.Open();
            Console.WriteLine("WCF service started at port:{0}.",port);
        }

        public void Stop()
        {
            serviceHost.Close();
            Console.WriteLine("WCF service has been stopped.");
        }
    }
}
