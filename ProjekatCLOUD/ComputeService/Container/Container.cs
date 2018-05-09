using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Contratcs;
using System.Reflection;

namespace ContainerService
{
    class Container : IContainer
    {
        public string Load(string assemblyName)
        {
            Console.WriteLine(assemblyName);
            string rez = "";
            
            try
            {
                var DLL = Assembly.LoadFile(@Program.path);
                
                Type theType = DLL.GetType("ClassLibrary1.Class1");


                object instanca = Activator.CreateInstance(theType);

                MethodInfo[] methods = theType.GetMethods();
                object res = methods[0].Invoke(instanca, new object[] { 5, 3 });
                rez = res.ToString();
                Console.WriteLine("DLL is loaded.");
                Console.WriteLine("Result is {0}",res);
                rez = "DLL is successfully loaded.";
            }
            catch (Exception)
            {

                throw;
            }
            return rez;
        }
    }
}
