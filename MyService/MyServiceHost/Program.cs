using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using MyService;
using CommonLib.Models;
using CommonLib.Repositories;

namespace MyServiceHost
{
    class Program
    {
        static void Main()
        {
            Service.ImmoProperty = typeof(Immovables).GetProperties();
            using (var host = new ServiceHost(typeof(MyService.Service)))
            {
                host.Open();
                Console.WriteLine("Host started...");
                Console.ReadLine();
            }
        }
    }
}
