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
        [OperationContract]
        List<Parameter> GetWriteAbleParameterList();

        [OperationContract]
        List<Parameter> GetReadableAbleParameterList();

        [OperationContract]
        List<Parameter> GetActChannelPara();

        [OperationContract]
        bool SetChannelPara(List<Parameter> parameters, int key);

        //-- OperatingCommands
        [OperationContract]
        string SendCommand(int commandId, int key);

        [OperationContract]
        List<Parameter> GetCommands();

        [OperationContract]
        int GetKey();

        [OperationContract]
        int ReleaseKey(int key);

        //--- Devices
        [OperationContract]
        bool CheckDeviceAvailable(int deviceId);

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
