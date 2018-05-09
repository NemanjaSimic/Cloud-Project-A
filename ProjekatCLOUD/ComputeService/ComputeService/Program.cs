using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Diagnostics;

namespace ComputeService
{
    class Program
    {
        public static Dictionary<int, int> listContainersID = new Dictionary<int, int>();
        private static ProcessStartInfo containerProcess = new ProcessStartInfo();
        private static int numberOfContainers = 4;
        private static int computePort = 10010;
        private static int containerPort;
        private static string arguments;
        private static string pathDLL;
        private static int numberOfInstances;
        private static string pathContainerFolder = @"C:\Users\Nemanja\Desktop\ProjekatCLOUD\Container";
        private static WorkerRole worker = new WorkerRole();
        private static XmlReader reader;
        static void Main(string[] args)
        {
            Console.WriteLine("Compute Service started....");
            Console.WriteLine("Startig containers processes....");

            LoadContainers();

            CheckForPackage();

            Console.ReadLine();
        }

        private static Tuple<int,string> ReadPackage(string putanja)
        {
            Tuple<int, string> package;
            int numberOfInstances = 0;
            string result = "";
            
           // using (XmlReader reader = XmlReader.Create(@putanja))
           // {
            reader = XmlReader.Create(@putanja);
            while (reader.Read())
                {
                    XmlReader temp_reader = reader;
                    if (temp_reader.IsStartElement())
                    {
                        string ime = temp_reader.Name;
                        if (ime == "Lokacija")
                        {
                            temp_reader.Read();
                            result = temp_reader.Value;
                        }
                        if (ime == "Broj")
                        {
                            temp_reader.Read();
                            Int32.TryParse(temp_reader.Value,out numberOfInstances);
                        }
                    }
                }
            reader.Close();
         //   }
            if (numberOfInstances < 1 || numberOfInstances > 4)
                result = "";
            package = new Tuple<int, string>(numberOfInstances, result);
            return package;
        }

        private static void CheckForPackage()
        {

            FileSystemWatcher fsw = new FileSystemWatcher(@"C:\Users\Nemanja\Desktop\ProjekatCLOUD\Folder", "*.xml")
            {
                EnableRaisingEvents = true
            };



            fsw.Created += (sender, arg) =>
            {
                Console.WriteLine("Package detected !");
                Tuple<int, string> package;
                package = ReadPackage(arg.FullPath);
                numberOfInstances = package.Item1;
                pathDLL = package.Item2;

                if (pathDLL == "")
                {
                    Console.WriteLine("Configuration is invalid !");           
                    File.Delete(arg.FullPath);
                    //fsw.Dispose();
                }
                else
                {
                    SendPackage();
                    StartContainers();
                }
                
            };
           
        }

        private static void LoadContainers()
        {
            for (int i = 0; i < numberOfContainers; i++)
            {
                containerPort = computePort + 10 + i * 10;
                arguments = String.Format("{0} "+pathContainerFolder+"{1}", containerPort, i+1);
                containerProcess.Arguments = arguments;
                containerProcess.FileName = @"ComputeService\bin\Debug\Container";
                Process.Start(containerProcess);
                listContainersID.Add(i + 1, containerPort);
            }
           
            Console.WriteLine("All containers have been loaded!");
        }

        private static void SendPackage()
        {
            for (int i = 1; i <= numberOfInstances; i++)
            {
                string destination = String.Format(pathContainerFolder + i + "\\package.dll");
                File.Copy(pathDLL, destination);   
            }
        }
        private static void StartContainers()
        {
            for (int i = 1; i <= numberOfInstances; i++)
            {
                worker.Start(i.ToString());
            }
        }
    }
}
