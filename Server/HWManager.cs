<<<<<<< HEAD
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel; // for WCF - need Verweis
using IRemoteHandler;
using System.Timers;

namespace Storage   //changed namespace, because its managed from host
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)] // singleton
    class HWManager : IRemoteHandler.IRemoteGetSet
    {
        #region DataMembers
        int refValue = 0;
        Random ranVal = new Random();
        private List<Parameter> WritableParamters;
        private List<Parameter> ReadableParameter;
        private List<Parameter> Commands;
        private List<Parameter> DeviceList;

        private int ActualKey;

        private Timer ResetTimer;

        //private List<FireKey> QueuedFireKeys;
        private Dictionary<int, FireKey> QueuedFireKeys;
        #endregion

        #region ctor
        public HWManager()
        {
            // Create a timer with a 20 second interval.
            ResetTimer = new Timer(20000);
            // Hook up the Elapsed event for the timer. 
            ResetTimer.Elapsed += OnTimedEvent;
            ResetTimer.AutoReset = false;
            Console.WriteLine("\nHWManager constructed");
            LoadTestParameter();
            Console.WriteLine("\nParameter loaded");
        }


        #endregion

        #region Internal Methodes
        private void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            // Reset Key and stopp timer
            ResetTimer.Enabled = false;
            ActualKey = 0;
        }

        private void LoadTestParameter()
        {
            WritableParamters = new List<Parameter>();
            WritableParamters.Add(new Parameter("1", 30000, "Prüfspannung"));
            WritableParamters.Add(new Parameter("2", 300, "Prüfzeit"));
            WritableParamters.Add(new Parameter("3", true, "Abschneidestrecke automatisch einstellen"));
            WritableParamters.Add(new Parameter("4", false, "Polaritätsvorwahl"));
            WritableParamters.Add(new Parameter("5", false, "Ladespannung"));

            ReadableParameter = new List<Parameter>();
            ReadableParameter.Add(new Parameter("1", 0, "H/W Ready Modus ist aktiv"));
            ReadableParameter.Add(new Parameter("2", 0, "H/W Operate Modus ist aktiv "));
            ReadableParameter.Add(new Parameter("3", 20, "Primärstrom"));
            ReadableParameter.Add(new Parameter("4", 50, "Aktuelle Prüfzeit"));
            ReadableParameter.Add(new Parameter("5", true, "Polarität des Generators"));
            ReadableParameter.Add(new Parameter("6", false, "Einstellend er abschneidestrecke beendet"));
            ReadableParameter.Add(new Parameter("7", true, "IP Gerät meldet einen Fehler"));
            ReadableParameter.Add(new Parameter("8", true, "IP Gerät ist im Ready-Modus"));
            ReadableParameter.Add(new Parameter("9", true, "IP Gerät ist im Operate-Modus"));
            ReadableParameter.Add(new Parameter("10", true, "Zündung wurde ausgelöst"));
            ReadableParameter.Add(new Parameter("11", 20000, "Istwert der Ladespannung"));

            Commands = new List<Parameter>();
            Commands.Add(new Parameter("1", 0, "Schaltet das Gerät in TestStart"));
            Commands.Add(new Parameter("2", 1, "Schaltet das Gerät in TestHold"));
            Commands.Add(new Parameter("3", 2, "Schaltet das Gerät in TestStop"));
            Commands.Add(new Parameter("4", 4, "Schaltet das Gerät in den Ready Modus"));
            Commands.Add(new Parameter("5", 5, "Holt das Gerät aus dem Ready Modus"));
            Commands.Add(new Parameter("6", 6, "Schaltet das Gerät in den Operate Modus"));
            Commands.Add(new Parameter("7", 7, "Holt das Gerät aus dem Operate Modus"));

            DeviceList = new List<Parameter>();
            DeviceList.Add(new Parameter("1", 0, "H/W Device"));
            DeviceList.Add(new Parameter("2", 1, "IP Device"));
        }

        private int GenerateKey()
        {
            // starting the reset timer
            ResetTimer.Enabled = true;
            ResetTimer.Stop();
            ResetTimer.Start();

            // generating random key
            var keynumber = refValue + ranVal.Next(1, 5); 
            refValue = keynumber;

            //generating key
            FireKey key = new FireKey(keynumber, 60000);
            key.KeyExpired += FireKey_Expired;

            //queue key
            QueuedFireKeys.Add(keynumber,key);
            return keynumber;
        }

        void FireKey_Expired(object sender, EventArgs e)
        {
            var key = (FireKey)sender;
            DeleateFireKey(key);
        }

        private void DeleateFireKey(FireKey key)
        {
            key.KeyExpired -= FireKey_Expired;
            QueuedFireKeys.Remove(key.KeyNumber);
        }
        #endregion

        #region interface methodes
        /// <summary>
        /// return boolean value if a "Fire Key" is activ
        /// There is a Client currently working on the Server
        /// </summary>
        /// <returns>true if active, false if inactive</returns>
        public bool IsFireKeyActiv()
        {
            if (QueuedFireKeys.Count > 0)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Updates time boundings for given key
        /// and returns position in List
        /// </summary>
        /// <param name="key">requested fire key</param>
        /// <returns>position in List</returns>
        public int RenewFireKey(int key)
        {
            //TODO: renew time counter for key

            var pos = QueuedFireKeys

            return pos;
        }

        public bool CheckDeviceAvailable(int deviceId)
        {
            // check Device with the given device id
            // ...


            return true;
        }

        public List<Parameter> GetActChannelPara()
        {
            // collecting data from Hardware
            // ...
            return ReadableParameter;
        }

        public List<Parameter> GetCommands()
        {
            return Commands;
        }

        public List<Parameter> GetDeviceList()
        {
            return DeviceList;
        }

        public int GetKey()
        {
            //TODO: überprüfe ob get key verwendet werden darf
            ActualKey = GenerateKey();
            return ActualKey;
        }

        public List<Parameter> GetReadableParameterList()
        {
            return ReadableParameter;
        }

        public List<Parameter> GetWriteAbleParameterList()
        {
            return WritableParamters;
        }

        public int ReleaseKey(int key)
        {
            if (key == ActualKey)
            {
                ActualKey = 0;
                return 1;
            }

            return 0;
        }

        public string SendCommand(int commandId, int key)
        {
            if (key == ActualKey)
            {
                StringBuilder s = new StringBuilder();
                s.Append("Key Accepted");

                // TODO: check if command is correct and add information to string builder or wahtever

                return s.ToString();
            }

            return "Wrong Key";
        }

        public bool SetChannelPara(List<Parameter> parameters, int key)
        {
            // if key is not correct update timer for key
            if (key == ActualKey)
            {
                // set Parameters on Devices
                // ....
                return true;
            }
            // retrun false if key is not correct
            return false;
        }

        #endregion
    }
}
=======
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel; // for WCF - need Verweis
using IRemoteHandler;
using System.Timers;

namespace Storage   //changed namespace, because its managed from host
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)] // singleton
    class HWManager : IRemoteHandler.IRemoteGetSet
    {
        #region DataMembers
        private List<Parameter> WritableParamters;
        private List<Parameter> ReadableParameter;
        private List<Parameter> Commands;
        private List<Parameter> DeviceList;

        private int ActualKey;

        private Timer ResetTimer;

        private List<int> QueuedKeys;
        private List<FireKey> QueuedFireKeys;
        #endregion

        #region ctor
        public HWManager()
        {
            // Create a timer with a 20 second interval.
            ResetTimer = new Timer(20000);
            // Hook up the Elapsed event for the timer. 
            ResetTimer.Elapsed += OnTimedEvent;
            ResetTimer.AutoReset = false;
            Console.WriteLine("\nHWManager constructed");
            LoadTestParameter();
            Console.WriteLine("\nParameter loaded");
        }


        #endregion

        #region Internal Methodes
        private void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            // Reset Key and stopp timer
            ResetTimer.Enabled = false;
            ActualKey = 0;
        }

        private void LoadTestParameter()
        {
            WritableParamters = new List<Parameter>();
            WritableParamters.Add(new Parameter("1", 30000, "Prüfspannung"));
            WritableParamters.Add(new Parameter("2", 300, "Prüfzeit"));
            WritableParamters.Add(new Parameter("3", true, "Abschneidestrecke automatisch einstellen"));
            WritableParamters.Add(new Parameter("4", false, "Polaritätsvorwahl"));
            WritableParamters.Add(new Parameter("5", false, "Ladespannung"));

            ReadableParameter = new List<Parameter>();
            ReadableParameter.Add(new Parameter("1", 0, "H/W Ready Modus ist aktiv"));
            ReadableParameter.Add(new Parameter("2", 0, "H/W Operate Modus ist aktiv "));
            ReadableParameter.Add(new Parameter("3", 20, "Primärstrom"));
            ReadableParameter.Add(new Parameter("4", 50, "Aktuelle Prüfzeit"));
            ReadableParameter.Add(new Parameter("5", true, "Polarität des Generators"));
            ReadableParameter.Add(new Parameter("6", false, "Einstellend er abschneidestrecke beendet"));
            ReadableParameter.Add(new Parameter("7", true, "IP Gerät meldet einen Fehler"));
            ReadableParameter.Add(new Parameter("8", true, "IP Gerät ist im Ready-Modus"));
            ReadableParameter.Add(new Parameter("9", true, "IP Gerät ist im Operate-Modus"));
            ReadableParameter.Add(new Parameter("10", true, "Zündung wurde ausgelöst"));
            ReadableParameter.Add(new Parameter("11", 20000, "Istwert der Ladespannung"));

            Commands = new List<Parameter>();
            Commands.Add(new Parameter("1", 0, "Schaltet das Gerät in TestStart"));
            Commands.Add(new Parameter("2", 1, "Schaltet das Gerät in TestHold"));
            Commands.Add(new Parameter("3", 2, "Schaltet das Gerät in TestStop"));
            Commands.Add(new Parameter("4", 4, "Schaltet das Gerät in den Ready Modus"));
            Commands.Add(new Parameter("5", 5, "Holt das Gerät aus dem Ready Modus"));
            Commands.Add(new Parameter("6", 6, "Schaltet das Gerät in den Operate Modus"));
            Commands.Add(new Parameter("7", 7, "Holt das Gerät aus dem Operate Modus"));

            DeviceList = new List<Parameter>();
            DeviceList.Add(new Parameter("1", 0, "H/W Device"));
            DeviceList.Add(new Parameter("2", 1, "IP Device"));
        }

        private int GenerateKey()
        {
            // starting the reset timer
            ResetTimer.Enabled = true;
            ResetTimer.Stop();
            ResetTimer.Start();

            var key = 5; // Random Generated key

            FireKey test = new FireKey(key, 60000);
            test.KeyExpired += FireKey_Expired;

            QueuedFireKeys.Add(test);
            QueuedKeys.Add(key);
            return key;
        }

        void FireKey_Expired(object sender, EventArgs e)
        {
            var key = (FireKey)sender;
            DeleateFireKey(key);
        }

        private void DeleateFireKey(FireKey key)
        {
            //var pos = QueuedFireKeys.IndexOf(key);
            QueuedFireKeys.Remove(key);
        }
        #endregion

        #region interface methodes
        /// <summary>
        /// return boolean value if a "Fire Key" is activ
        /// There is a Client currently working on the Server
        /// </summary>
        /// <returns>true if active, false if inactive</returns>
        public bool IsFireKeyActiv()
        {
            if (QueuedKeys.Count > 0)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Updates time boundings for given key
        /// and returns position in List
        /// </summary>
        /// <param name="key">requested fire key</param>
        /// <returns>position in List</returns>
        public int RenewFireKey(int key)
        {
            //TODO: renew time counter for key

            var pos = QueuedKeys.IndexOf(key);

            return pos;
        }

        public bool CheckDeviceAvailable(int deviceId)
        {
            // check Device with the given device id
            // ...


            return true;
        }

        public List<Parameter> GetActChannelPara()
        {
            // collecting data from Hardware
            // ...
            return ReadableParameter;
        }

        public List<Parameter> GetCommands()
        {
            return Commands;
        }

        public List<Parameter> GetDeviceList()
        {
            return DeviceList;
        }

        public int GetKey()
        {
            //TODO: überprüfe ob get key verwendet werden darf
            ActualKey = GenerateKey();
            return ActualKey;
        }

        public List<Parameter> GetReadableAbleParameterList()
        {
            return ReadableParameter;
        }

        public List<Parameter> GetWriteAbleParameterList()
        {
            return WritableParamters;
        }

        public int ReleaseKey(int key)
        {
            if (key == ActualKey)
            {
                ActualKey = 0;
                return 1;
            }

            return 0;
        }

        public string SendCommand(int commandId, int key)
        {
            if (key == ActualKey)
            {
                StringBuilder s = new StringBuilder();
                s.Append("Key Accepted");

                // TODO: check if command is correct and add information to string builder or wahtever

                return s.ToString();
            }

            return "Wrong Key";
        }

        public bool SetChannelPara(List<Parameter> parameters, int key)
        {
            // if key is not correct update timer for key
            if (key == ActualKey)
            {
                // set Parameters on Devices
                // ....
                return true;
            }
            // retrun false if key is not correct
            return false;
        }

        #endregion
    }
}
>>>>>>> d3f508fb3f64d29dbd94cb45ac0122519de886d3
