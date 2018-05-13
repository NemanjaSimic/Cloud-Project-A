using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Container
{
    class Program
    {
        static void Main(string[] args)
        {
            
            ContainerService service = new ContainerService(args[0]);
            Console.WriteLine("Container started at port {0}", args[0]);
            Console.ReadKey();
            service.Stop();

        }

    }
}
