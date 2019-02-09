using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel; // for WCF - need Verweis
using IRemoteHandler;
using System.Timers;
using Microsoft.Build.Utilities;
using System.Diagnostics;

namespace Storage   //changed namespace, to show its managed from host
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)] // singleton
    class HWManager : IRemoteHandler.IRemoteGetSet
    {
        private bool displayLog = true;
        private bool fileLog = false;

        #region DataMembers
        private int refValue = 0;
        private Random ranVal = new Random();
        private int expireTime = 60000;
        private int maxClients = 20;
        private List<Parameter> WritableParamters;
        private List<Parameter> ReadableParameter;
        private List<Parameter> Commands;
        private List<Parameter> DeviceList;

        private List<FireKey> QueuedFireKeys;

        private string LogPath;
        #endregion

        #region ctor
        public HWManager()
        {
            QueuedFireKeys = new List<FireKey>(maxClients);
            Console.WriteLine("\nHWManager constructed");
            LoadTestParameter();
            Console.WriteLine("\nParameter loaded");
            this.LogPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().CodeBase);
            this.LogPath = LogPath.Remove(0, 6);
            this.LogPath = LogPath + @"\Log.txt";
            Console.WriteLine("Path for LogFile");
            Console.WriteLine(this.LogPath);
            //System.IO.File.AppendAllText(LogPath, "mymsg\n");
            LogMessage("Server Started");
            Console.WriteLine("\nLog started");
        }
        #endregion

        #region Internal Methodes
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
        /// <summary>
        /// Method for generating a firekey
        /// uses the last stored key and adds arandom number
        /// </summary>
        /// <returns></returns>
        private int GenerateKeyNumber()
        {
            int randomRange = 5; // max added random number

            // generating random key
            // make sure key keeps in positiv intager Range
            if (refValue > int.MaxValue - randomRange)
            {
                refValue = 0;
            }
            var keynumber = refValue + ranVal.Next(1, randomRange); 
            refValue = keynumber;

            return keynumber;
        }
        /// <summary>
        /// compares the keyvalue with the actual enabled key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private bool CompareFireKey(int key)
        {
            // TODO: deleate after testing
            if (key == 0)
            {
                return true;
            }
            if (QueuedFireKeys.Count > 0 && key == QueuedFireKeys.First().KeyNumber)
            {
                return true;
            }
            return false;
        }

        void FireKey_Expired(object sender, EventArgs e)
        {
            var key = (FireKey)sender;
            DisplayMessage("EXPIRED KEY detected: " + key.KeyNumber.ToString());
            DeleateFireKey(key);
        }

        private void DeleateFireKey(FireKey key)
        {
            key.KeyExpired -= FireKey_Expired;
            QueuedFireKeys.Remove(key);
            DisplayMessage("EXPIRED KEY deleated: " + key.KeyNumber.ToString());
        }

        /// <summary>
        /// private console.WriteLine if member displayLog is true
        /// </summary>
        /// <param name="s">string to display</param>
        private void DisplayMessage(string s)
        {
            if (this.displayLog)
            {
                Console.WriteLine(s);
            }
            if (fileLog)
            {
                LogMessage(s);
            }
        }
        /// <summary>
        /// Logg the given string to the log File if File logging is true
        /// </summary>
        /// <param name="s">the string to log</param>
        private void LogMessage(string s)
        {
            if (!this.fileLog)
            {
                return;
            }
            string tap = "   ";
            StringBuilder m = new StringBuilder();
            DateTime aktTime = new DateTime();
            aktTime = DateTime.Now;
            m.Append(aktTime);
            m.Append(tap);
            m.Append(s);
            m.AppendLine();
            //DisplayMessage(this.LogPath);
            //DisplayMessage(m.ToString());
            try
            {
                System.IO.File.AppendAllText(this.LogPath, m.ToString());
            }
            catch (Exception e)
            {
                Console.WriteLine("oh no");
                Console.WriteLine(e.Message);
            }
        }
        #endregion

        #region ------interface methodes
        /// <summary>
        /// return boolean value if a "Fire Key" is activ
        /// There is a Client currently working on the Server
        /// </summary>
        /// <returns>true if active, false if inactive</returns>
        public bool IsFireKeyActiv()
        {
            DisplayMessage("isFireKeyActiv requested");

            if (QueuedFireKeys.Count > 0)
            {
                DisplayMessage("Fire key activ");
                return false;
            }
            DisplayMessage("No Fire key in List");
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
            DisplayMessage("RenewFireKey requested with key: " + key.ToString());
            int pos=-1;
            foreach (FireKey IterationKey in QueuedFireKeys)
            {
                if (IterationKey.KeyNumber == key)
                {
                    IterationKey.RenewTimer();
                    pos = QueuedFireKeys.IndexOf(IterationKey);
                }
            }
            if (pos > -1)
            {
                DisplayMessage("key renewed on position " + pos.ToString());
            }
            else
            {
                DisplayMessage("key not in List");
            }
            
            return pos;
        }

        public bool CheckDeviceAvailable(string deviceId)
        {
            DisplayMessage("CheckDeviceAvailable requested with deviceId: " + deviceId);
            // check Device with the given device id
            // ...
            return true;
        }

        public List<Parameter> GetActChannelPara()
        {
            DisplayMessage("GetActChannelPara requested");
            // collecting data from Hardware
            // ...
            return ReadableParameter;
        }

        public List<Parameter> GetCommands()
        {
            DisplayMessage("GetCommands requested");
            return Commands;
        }

        public List<Parameter> GetDeviceList()
        {
            DisplayMessage("GetDeviceList requested");
            return DeviceList;
        }

        public int GetKey()
        {
            DisplayMessage("GetKey requested");
            // check if a new key is allowed
            if (QueuedFireKeys.Count < this.maxClients)
            {
                var genKey = GenerateKeyNumber();

                //generating key
                FireKey key = new FireKey(genKey, this.expireTime); // setting exprie Time
                key.KeyExpired += FireKey_Expired; // adding expired listener

                //queue key
                QueuedFireKeys.Add(key);
                DisplayMessage("new key is " + genKey.ToString());
                return genKey;
            }
            DisplayMessage("no key available");
            return -1;
        }

        public List<Parameter> GetReadableParameterList()
        {
            DisplayMessage("GetReadableParameterList requested");
            return ReadableParameter;
        }

        public List<Parameter> GetWriteAbleParameterList()
        {
            DisplayMessage("GetWriteAbleParameterList requested");
            return WritableParamters;
        }

        public bool ReleaseKey(int key)
        {
            DisplayMessage("ReleaseKey requested with key: " + key.ToString());
            FireKey deleateKey = null;
            // iterate through Key storage and remove key
            foreach (FireKey iterationKey in QueuedFireKeys)
            {
                if (iterationKey.KeyNumber == key)
                {
                    deleateKey = iterationKey;
                }
            }
            if (deleateKey != null)
            {
                DisplayMessage("key deleted");
                DeleateFireKey(deleateKey);
                return true;
            }
            // key was expired return false
            DisplayMessage("key was expired");
            return false;
        }

        public string SendCommand(string commandId, int key)
        {
            StringBuilder s = new StringBuilder();
            DisplayMessage("SendCommand " + commandId +" requested wit key: " + key.ToString());
            if (CompareFireKey(key))
            {
                DisplayMessage("Key Accepted");

                s.Append("Key Accepted");
                // TODO: check if command is correct and add information to string builder or wahtever

                return s.ToString();
            }
            DisplayMessage("Key not Accepted");
            s.Append("Key not accepted");
            return s.ToString();
        }

        public bool SetChannelPara(List<Parameter> parameters, int key)
        {
            DisplayMessage("SetChannelPara requested with key: " +key.ToString());
            if (CompareFireKey(key))
            {
                DisplayMessage("key Accepted");
                // set Parameters on Devices
                // ....
                return true;
            }
            // retrun false if key is not correct
            DisplayMessage("key not Accepted");
            return false;
        }

        #endregion
    }
}
