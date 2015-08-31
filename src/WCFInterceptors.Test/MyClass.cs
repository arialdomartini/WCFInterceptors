using NUnit.Framework;
using FluentAssertions;

namespace WCFInterceptors.Test
{
    public class MyClassTest
    {
        [Test]
        public void ShouldPass()
        {
            new MyClass().ReturnTrue().Should().Be(true);
        }
    }
}

