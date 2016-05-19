
using System;
using System.ServiceModel;

namespace WCFInterceptors
{
  public class Hostme
  {
    public void Run()
    {
       var _binding = new BasicHttpBinding();
              var _address = new Uri("http://localhost:8889/MyService");
              var _host = new ServiceHost(typeof(MyService));
              _host.AddServiceEndpoint(typeof(IMyService), _binding, _address);
              _host.Open();

    }
  }

    public class MyService : IMyService
    {
      public string Crash(string s)
      {
                throw new CommunicationException();
        return "ok";
      }

      public string SayHello(string name)
        {
            return string.Format("Hello, {0}!", name);
        }
    }
}