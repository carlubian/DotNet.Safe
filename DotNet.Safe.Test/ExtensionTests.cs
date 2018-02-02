using Microsoft.VisualStudio.TestTools.UnitTesting;
using FluentAssertions;
using DotNet.Safe.Standard.Events;

namespace DotNet.Safe.Test
{
    [TestClass]
    public class ExtensionTests
    {
        [TestMethod]
        public void TestPluginMethods()
        {
            typeof(CompositionListener).Methods()
                .ThatArePublicOrInternal
                .Should().BeVirtual();
        }
    }
}
