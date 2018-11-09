using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using FluentAssertions;
using DotNet.Safe.Standard;

namespace DotNet.Safe.Test
{
    [TestClass]
    public class TestUnit
    {
        [TestMethod]
        public void TestUnitHashcode()
        {
            Unit.Instance().GetHashCode()
                .Should().Be(0);
        }

        [TestMethod]
        public void TestUnitToString()
        {
            Unit.Instance().ToString()
                .Should().Be("[Unit]");
        }

        [TestMethod]
        public void TestUnitEquals()
        {
            Unit.Instance().Equals(Unit.Instance())
                .Should().BeTrue();

            Unit.Instance().Equals(Unit.Instance() as object)
                .Should().BeTrue();

            Unit.Instance().Equals(new object())
                .Should().BeFalse();

            Unit.Instance().Equals(0)
                .Should().BeFalse();
        }

        [TestMethod]
        public void TestEqualsNull()
        {
            Unit.Instance().Equals(null)
                .Should().BeFalse();

            object.Equals(null, Unit.Instance())
                .Should().BeFalse();

            object.Equals(Unit.Instance(), null)
                .Should().BeFalse();
        }
    }
}
