using NUnit.Framework;
using FluentAssertions;
using System.ServiceModel;
using System;
using System.ServiceModel.Description;

namespace WCFInterceptors.Test
{
    public class MyServiceTest
    {
        ServiceHost _host;
        IMyService _sut;

        [SetUp]
        public void SetUp()
        {
            var binding = new BasicHttpBinding ();
            var address = new Uri ("http://localhost:8888/MyService");
            _host = new ServiceHost (typeof(MyService));

            var behavior = _host.Description.Behaviors.Find<ServiceDebugBehavior>();
            behavior.IncludeExceptionDetailInFaults = true;

            _host.AddServiceEndpoint (typeof(IMyService), binding, address);
            _host.Open ();

            _sut = new ChannelFactory<IMyService>(binding, new EndpointAddress(address)).CreateChannel();
        }
        [TearDown]

        public void TearDown()
        {
            _host.Close();
        }

        [Test]
        public void ShouldPass()
        {
            _sut.SayHello("world").Should().Be("Hello, world!");
        }
    }
}

