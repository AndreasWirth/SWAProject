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
            Console.WriteLine("Hello and Welcome to our super duper SWA Server");
            //creating the service 
            ServiceHost myHost = new ServiceHost(typeof(Storage.HWManager));

            myHost.Open();

            
            //inforamtion
            Console.WriteLine();
            Console.WriteLine("Waiting for request");
            Console.WriteLine();
            Console.WriteLine();


            // keeping server open
            Console.WriteLine("enter faceroll for closing");
            Console.ReadLine();

            //close the Service
            myHost.Close();
            Console.WriteLine("Service closed");

        }
    }
}
