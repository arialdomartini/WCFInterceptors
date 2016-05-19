using Autofac;
using FluentAssertions;
using NUnit.Framework;

namespace WCFInterceptors.Test
{
  public class TestContainer
  {
    private IContainer _container;
    private ILifetimeScope _scope;

    [SetUp]
    public void SetUp()
    {
      var builder = new Autofac.ContainerBuilder();
      builder.RegisterModule(new MyModule());
      _container = builder.Build();
      _scope = _container.BeginLifetimeScope();
    }

    [TearDown]
    public void TearDown()
    {
      _scope.Dispose();
      _container.Dispose();
    }

    [Test]
    public void should_resolve_Consume()
    {
      _scope.Resolve<Consumer>().Should().BeOfType<Consumer>();
    }
  }
}