using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            using (ServiceHost host = new ServiceHost(typeof(Service.ChatService))) {
                host.Open();
                Console.WriteLine("WCFChat server is running");
                while (true)
                {
                    Thread.Sleep(1000);
                }
            }
        }
    }
}
