using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using MyService;

namespace MyServiceHost
{
    class Program
    {
        static void Main()
        {
            CacheImmoProperty.PropInit();
            using (var host = new ServiceHost(typeof(MyService.Service)))
            {
                host.Open();
                Console.WriteLine("Host started...");
                Console.ReadLine();
            }
        }
    }
}
