using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using FluentAssertions;
using DotNet.Safe;

namespace DotNet.Safe.Test
{
    [TestClass]
    public class TestUnit
    {
        [TestMethod]
        public void TestUnitHashcode()
        {
            Unit.Value.GetHashCode()
                .Should().Be(0);
        }

        [TestMethod]
        public void TestUnitToString()
        {
            Unit.Value.ToString()
                .Should().Be("[Unit]");
        }

        [TestMethod]
        public void TestUnitEquals()
        {
            Unit.Value.Equals(Unit.Value)
                .Should().BeTrue();

            Unit.Value.Equals(Unit.Value as object)
                .Should().BeTrue();

            Unit.Value.Equals(new object())
                .Should().BeFalse();

            Unit.Value.Equals(0)
                .Should().BeFalse();
        }

        [TestMethod]
        public void TestEqualsNull()
        {
            Unit.Value.Equals(null)
                .Should().BeFalse();

            object.Equals(null, Unit.Value)
                .Should().BeFalse();

            object.Equals(Unit.Value, null)
                .Should().BeFalse();
        }
    }
}
