using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            //creating the service 
            ServiceHost myHost = new ServiceHost(typeof(Storage.HWManager));

            myHost.Open();

            //inforamtion
            Console.WriteLine();
            Console.WriteLine("Waiting for request");
            Console.WriteLine();
            Console.WriteLine();

            // keeping server open
            Console.WriteLine("faceroll and enter for closing");
            Console.ReadLine();

            //close the Service
            myHost.Close();
            
        }
    }
}
