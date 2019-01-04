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
        public HWManager()
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

        public bool SendCommand()
        {
            throw new NotImplementedException();
        }

        public bool SetChannel()
        {
            throw new NotImplementedException();
        }

        public bool CheckDevice()
        {
            throw new NotImplementedException();
        }

        public bool GetChannel()
        {
            throw new NotImplementedException();
        }

        public bool GetDeviceList()
        {
            throw new NotImplementedException();
        }

        public string GetParameterList()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
