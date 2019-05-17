using Microsoft.VisualStudio.TestTools.UnitTesting;
using FluentAssertions;
using DotNet.Safe;
using System;
using OneOf;

namespace DotNet.Safe.Test
{
    [TestClass]
    public class LazyTests
    {
        [TestMethod]
        public void TestSingleCall()
        {
            var lazy = Try.This(GetNumber).Later();
            
            lazy()
                .Should().BeOfType<OneOf<int, Exception>>()
                .And.Match<OneOf<int, Exception>>(n => n.AsT0 == 2);

            lazy = Try.This(GetFaulty).Later();

            lazy()
                .Should().BeOfType<OneOf<int, Exception>>()
                .And.Match<OneOf<int, Exception>>(n => n.IsT1);
        }

        [TestMethod]
        public void TestChainCalls()
        {
            var lazy = Try.This(GetNumber)
                .Then(Multiply).Later();

            lazy()
                .Should().BeOfType<OneOf<int, Exception>>()
                .And.Match<OneOf<int, Exception>>(n => n.AsT0 == 4);

            lazy = Try.This(GetFaulty)
                .Then(Multiply).Later();

            lazy()
                .Should().BeOfType<OneOf<int, Exception>>()
                .And.Match<OneOf<int, Exception>>(n => n.IsT1);
        }

        private int GetNumber() => 2;

        private int GetFaulty() => throw new ArgumentException("Some error");

        private int Multiply(int n) => n * 2;
    }
}
