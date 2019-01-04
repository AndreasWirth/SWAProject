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
        bool isInitialized();

        [OperationContract]
        bool SetValue( string value);

        [OperationContract]
        string GetValue();

        [OperationContract]
        bool Initialize();
    }
}
