using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel; // verweis hinzufügen
using System.Threading;
using System.Timers;

namespace IRemoteHandler
{
    [ServiceContract]
    public interface IRemoteGetSet
    {
        //-- functional Methodes
        /// <summary>
        /// return boolean value if a "Fire Key" is activ
        /// There is a Client currently working on the Server
        /// </summary>
        /// <returns>true if active, false if inactive</returns>
        [OperationContract]
        bool IsFireKeyActiv();
        /// <summary>
        /// Updates time boundings for given key
        /// and returns position in List
        /// </summary>
        /// <param name="key">requested fire key</param>
        /// <returns>position in List</returns>
        [OperationContract]
        int RenewFireKey(int key);

        //-- Parameter Methodes
        /// <summary>
        /// Returns a List of all Writable Parameters
        /// Writeable Paramters are Paraemters which can be set by de Client
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<Parameter> GetWriteAbleParameterList();
        /// <summary>
        /// Returns a List of all Readable Parameters
        /// Readable Paramters are Paraemters which can only be read be the Client
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<Parameter> GetReadableParameterList();
        /// <summary>
        /// Returns the actual set values of the channel
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<Parameter> GetActChannelPara();
        /// <summary>
        /// Sets the Channel Parameters
        /// If the Key matches the actuall allowed key, the Server will set the channel parameters
        /// </summary>
        /// <param name="parameters">List of the parameters to set</param>
        /// <param name="key">"Fire Key" which has to match with Server key</param>
        /// <returns></returns>
        [OperationContract]
        bool SetChannelPara(List<Parameter> parameters, int key);

        //-- OperatingCommands
        /// <summary>
        /// Send a specific command to the Server
        /// The command will be executed if the key matches the server key
        /// </summary>
        /// <param name="commandId">ID of the command</param>
        /// <param name="key">"Fire Key"</param>
        /// <returns></returns>
        [OperationContract]
        string SendCommand(int commandId, int key);
        /// <summary>
        /// returns a List of all posiible Commands
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<Parameter> GetCommands();
        /// <summary>
        /// Queue in the Server
        /// retruns a "Fire Key" which cis necessary for operation mode
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        int GetKey();
        /// <summary>
        /// Logg out from server to allow operation for next user
        /// </summary>
        /// <param name="key">"Fire Key"</param>
        /// <returns></returns>
        [OperationContract]
        int ReleaseKey(int key);

        //--- Devices
        /// <summary>
        /// Checks if the divice Id is available
        /// </summary>
        /// <param name="deviceId">device id to check</param>
        /// <returns></returns>
        [OperationContract]
        bool CheckDeviceAvailable(int deviceId);
        /// <summary>
        /// returns A list of all available Devices
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<Parameter> GetDeviceList();
    }

    [Serializable]
    public class Parameter
    {
        public string ID;
        public Object Value;
        public string Beschreibung;

        public Parameter(string id, Object value, string beschreibung)
        {
            this.ID = id;
            this.Value = value;
            this.Beschreibung = beschreibung;
        }
    }

    public class FireKey
    {
        public System.Timers.Timer KeyTimer { get; private set; }
        public int KeyNumber { get; private set; }
        public event EventHandler KeyExpired;

        /// <summary>
        /// Construcotr for FireKey Class
        /// </summary>
        /// <param name="keyNumber">number of the key</param>
        /// <param name="timerIntervall">Inervall for Key expired</param>
        public FireKey(int keyNumber, int timerIntervall)
        {
            this.KeyNumber = keyNumber;
            SetTimer(timerIntervall);
            RenewTimer();
        }

        private void SetTimer(int timerIntervall)
        {
            this.KeyTimer = new System.Timers.Timer(timerIntervall);
            this.KeyTimer.Elapsed += KeyTimer_Elapsed;
            this.KeyTimer.AutoReset = false;
            this.KeyTimer.Enabled = true;
        }
        
        public void RenewTimer()
        {
            this.KeyTimer.Stop();
            this.KeyTimer.Start();
        }

        private void KeyTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            OnKeyExpired(EventArgs.Empty);
        }

        protected virtual void OnKeyExpired(EventArgs e)
        {
            EventHandler handler = KeyExpired;
            if (handler != null)
            {
                handler(this, e);
            }
        }
    }
}
