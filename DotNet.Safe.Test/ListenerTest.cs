using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using FluentAssertions;
using DotNet.Safe.Standard;
using DotNet.Safe.Standard.Events;

namespace DotNet.Safe.Test
{
    [TestClass]
    public class ListenerTest
    {
        internal int counter;

        [TestMethod]
        public void TestAttachListener()
        {
            counter = 0;

            Try.This(() => 2)
                .Then(n => n + 1)
                .Then(() => 4)
                .Then(n => 0)
                .Attach(new TestListener(this))
                .Now();

            counter.Should().Be(4);
        }

        private class TestListener : CompositionListener
        {
            private readonly ListenerTest _super;

            public TestListener(ListenerTest Enclosing)
            {
                _super = Enclosing;
            }

            public override void OnStepBeginInvocation(object sender, CompositionStep args)
            {
                _super.counter++;
            }
        }
    }
}
