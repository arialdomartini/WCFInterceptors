using System;
using System.ServiceModel;

namespace WCFInterceptors
{
    [ServiceContract]
    public interface IMyService
    {
        [OperationContract]
        string SayHello(String name);
    }
}

