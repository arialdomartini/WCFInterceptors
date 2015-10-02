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

namespace WCFInterceptors.Test
{
    public class MyServiceTest
    {
        ServiceHost _host;
        IMyService _sut;
        IDispatchMessageInspector _dispatchMessageInspector;

        [SetUp]
        public void SetUp()
        {
            var binding = new BasicHttpBinding ();
            var address = new Uri ("http://localhost:8888/MyService");
            _host = new ServiceHost (typeof(MyService));

            var behavior = _host.Description.Behaviors.Find<ServiceDebugBehavior>();
            behavior.IncludeExceptionDetailInFaults = true;

            _dispatchMessageInspector = Substitute.For<IDispatchMessageInspector>();

            _host.Description.Behaviors.Add(new ServiceBehavior(_dispatchMessageInspector));

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
        public void TheServiceReplies()
        {
            _sut.SayHello("world").Should().Be("Hello, world!");
        }

        [Test]
        public void InterceptorBeforeReceiveRequestIsInvoked()
        {
            var message = Arg.Any<Message>();
            var invoked = false;
            _dispatchMessageInspector.When(x => x.BeforeSendReply(ref message, Arg.Any<object>()))
                .Do(x => { invoked = true; });

            _sut.SayHello("foo");
            
            invoked.Should().BeTrue();
        }

        [Test]
        public void InterceptorAfterReceiveRequestIsInvoked()
        {
            var message = Arg.Any<Message>();
            var invoked = false;
            _dispatchMessageInspector.When(x => x.AfterReceiveRequest(ref message, Arg.Any<IClientChannel>(), Arg.Any<InstanceContext>()))
                .Do(x => { invoked = true; });

            _sut.SayHello("foo");

            invoked.Should().BeTrue();
        }
    }
}

