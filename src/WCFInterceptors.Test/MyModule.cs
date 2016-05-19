using System.ServiceModel;
using Autofac;
using Autofac.Integration.Wcf;

namespace WCFInterceptors.Test
{
  public class MyModule : Module
  {
    protected override void Load(ContainerBuilder builder)
    {
      builder.RegisterType<Consumer>();
      builder
        .Register(c => new ChannelFactory<IMyService>(
          new BasicHttpBinding(),
          new EndpointAddress("http://localhost:8888/MyService")))
        .SingleInstance();

      builder
        .Register(c => c.Resolve<ChannelFactory<IMyService>>().CreateChannel())
        .As<IMyService>();
        //.UseWcfSafeRelease();
    }
  }
}