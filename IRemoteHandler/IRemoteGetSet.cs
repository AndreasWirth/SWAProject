using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel; // verweis hinzufügen

namespace IRemoteHandler
{
    [ServiceContract]
    public interface IRemoteGetSet
    {
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
}
