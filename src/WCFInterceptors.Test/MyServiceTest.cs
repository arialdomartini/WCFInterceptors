
using NUnit.Framework;
using FluentAssertions;
using System.ServiceModel;
using System;
using System.ServiceModel.Description;
using NSubstitute;
using NSubstitute.Core.Arguments;
using System.ServiceModel.Configuration;
using System.ServiceModel.Dispatcher;
using System.ServiceModel.Channels;
using Autofac;
using Autofac.Core;

namespace WCFInterceptors.Test
{
  public class Consumer
  {
    private readonly IMyService _myservice;

    public Consumer(IMyService myservice)
    {
      _myservice = myservice;
    }

    public string MyMethod()
    {
      return _myservice.SayHello("world");
    }

    public void Crash()
    {
      try
      {
        _myservice.Crash("foo");
      }
      catch (FaultException)
      {
        throw new ArgumentException();
      }

    }
  }

  public class MyServiceTest
  {
    private ServiceHost _host;
    private Consumer _sut;
    private IContainer _container;
    private ILifetimeScope _scope;
    private BasicHttpBinding _binding;
    private Uri _address;

    [SetUp]
    public void SetUp()
    {
        _binding = new BasicHttpBinding();
        _address = new Uri("http://localhost:8888/MyService");
        _host = new ServiceHost(typeof(MyService));
        _host.AddServiceEndpoint(typeof(IMyService), _binding, _address);
        _host.Open();

        var builder = new Autofac.ContainerBuilder();
        builder.RegisterModule(new MyModule());
        _container = builder.Build();
        _scope = _container.BeginLifetimeScope();
    }

    [TearDown]
    public void TearDown()
    {
      _host.Close();
      _scope.Dispose();
      _container.Dispose();
    }

    [Test]
    public void TestingService()
    {
        var sut = new ChannelFactory<IMyService>(_binding, new EndpointAddress(_address)).CreateChannel();

      sut.SayHello("world").Should().Be("Hello, world!");
    }

    [Test]
    public void TestingServiceCrash()
    {
        var sut = new ChannelFactory<IMyService>(_binding, new EndpointAddress(_address)).CreateChannel();

        sut.Invoking(s => s.Crash("foo")).ShouldThrow<FaultException>();
    }

    [Test]
    public void TestConsumer()
    {
      var consumer = _container.BeginLifetimeScope().Resolve<Consumer>();

      consumer.MyMethod().Should().Be("Hello, world!");
    }

    [Test]
    public void TestConsumerCrash()
    {
      var consumer = _container.BeginLifetimeScope().Resolve<Consumer>();

      consumer.Invoking(c => c.Crash()).ShouldThrow<ArgumentException>();
    }


    [Test]
    public void TestConsumerCallAfterACrash()
    {
      var consumer = _container.BeginLifetimeScope().Resolve<Consumer>();

      consumer.Invoking(c => c.Crash()).ShouldThrow<ArgumentException>();

      consumer.MyMethod().Should().Be("Hello, world!");
    }


    [Test]
    public void TestConsumerCallAfterACrash_manual_build()
    {
      string myaddress = "http://localhost:8889/MyService";
      var channelFactory = new ChannelFactory<IMyService>(_binding, new EndpointAddress(myaddress));
      var myservice = channelFactory.CreateChannel();
      //var consumer = new Consumer(myservice);

      try
      {
        myservice.Crash("foo");
      }
      catch
      {

                ((IClientChannel) myservice).State.Should().Be(CommunicationState.Faulted);

      }
      //consumer.Invoking(c => c.Crash()).ShouldThrow<ArgumentException>();



      //consumer.MyMethod().Should().Be("Hello, world!");
    }





  }
}

