using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel; // for WCF - need Verweis

namespace Storage   //changed namespace, because its managed from host
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)] // singleton
    class HWManager : IRemoteHandler.IRemoteGetSet
    {
        #region DataMembers
        private string storage;
        private bool initialized;
        #endregion

        #region ctor
        public ValueManager()
        {
            Console.WriteLine("\nValueManager constructed");

            initialized = false;
            storage = null;
        }
        #endregion

        #region Internal Methodes
         // no methodes needed
        #endregion

        #region interface methodes
        public string GetValue()
        {
            return storage;
        }

        public bool Initialize()
        {
            // not needet in this project
            // data could be loaded or calculated
            storage = "this is my first value";
            initialized = true;

            return true;
        }

        public bool isInitialized()
        {
            return initialized;
        }

        public bool SetValue(string value)
        {
            storage = value;
            if (initialized == false)
            {
                initialized = true;
            }

            Console.WriteLine("Got new Value: \n" + value);
            return true;
        }
        #endregion
    }
}
