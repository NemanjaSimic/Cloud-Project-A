using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ContainerService
{
    class Program
    {
        public static string path;
        
        static void Main(string[] args)
        {
            Console.WriteLine("Starting WCF server...");
            if(!Int32.TryParse(args[0], out int port))
            {
                Console.WriteLine("Port is invalid. It needs to be integer!");
                return;
            }
            string folderPath = args[1];
            path = String.Format(folderPath + "\\package.dll");
            
            WCFServer conatinerServer = new WCFServer(port);

            Console.ReadLine();                      
        }
    }
}
