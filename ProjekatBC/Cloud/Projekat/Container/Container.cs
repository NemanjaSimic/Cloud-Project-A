using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using Contracts;
using System.IO;
using System.Reflection;
using System.Threading;

namespace Container
{ 
    public class Container : IContainer
    {
        public string CheckState()
        {
            Console.WriteLine("I am alive");
            return "I am alive";
        }

        public string Load(string assemblyName)
        {
            Console.WriteLine("Server loaded " + assemblyName + "at port: " + ContainerService.port );

            byte[] bytes = File.ReadAllBytes(assemblyName);
            Assembly DLL = Assembly.Load(bytes);

            
            var theType = DLL.GetType("WorkerRole.WorkerRole");
            var c = Activator.CreateInstance(theType);
            var method = theType.GetMethod("Start");

            try
            {
                method.Invoke(c, new object[] { ContainerService.port.ToString() });
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return "Success";
        }

    }
}
