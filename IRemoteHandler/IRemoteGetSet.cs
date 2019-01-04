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
        [OperationContract]
        bool GetChannel();

        [OperationContract]
        bool SetChannel();

        [OperationContract]
        string GetParameterList();

        [OperationContract]
        bool SendCommand();

        [OperationContract]
        bool CheckDevice();

        [OperationContract]
        bool GetDeviceList();
    }
}
