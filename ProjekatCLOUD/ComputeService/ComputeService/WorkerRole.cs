using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using Contratcs;

namespace ComputeService
{
    public class WorkerRole : IWorkerRole
    {
        private IContainer proxy;
        public IContainer Proxy { get { return proxy; } }

        public void Start(string containerId)
        {
            NetTcpBinding binding = new NetTcpBinding();
            Int32.TryParse(containerId, out int id);
            Program.listContainersID.TryGetValue(id, out int port);
            string adress = String.Format("net.tcp://localhost:{0}/Container", port);
            ChannelFactory<IContainer> factory = new ChannelFactory<IContainer>(binding, new EndpointAddress(adress));
            proxy = factory.CreateChannel();
            string result = "";
            try
            {
               result = proxy.Load("package.dll");
            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
            }
            if(result != "")
                Console.WriteLine(result);
            else
                Console.WriteLine("Error occured while loading dll.");
            factory.Close();
        }

        public void Stop()
        {
           
        }
    }
}
