using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FluentAssertions;
using DotNet.Safe;
using OneOf;

namespace DotNet.Safe.Test
{
    [TestClass]
    public class CompositionTests
    {
        [TestMethod]
        public void TestSingleCall()
        {
            var result = Try.This(GetNumber).Now();
            result.IsT0.Should().BeTrue();
            result.IsT1.Should().BeFalse();
            result.AsT0.Should().Be(2);

            result = Try.This(GetFaulty).Now();
            result.IsT0.Should().BeFalse();
            result.IsT1.Should().BeTrue();
            result.AsT1.Should().NotBeNull().And.BeOfType<ArgumentException>();
        }

        [TestMethod]
        public void TestAction()
        {
            var result = Try.This(() => Console.WriteLine("Foo")).Now();
            result.IsT0.Should().BeTrue();
            result.IsT1.Should().BeFalse();
            result.AsT0.Should().Be(Unit.Value);

            result = Try.This(GetNumber)
                        .Then(n => Console.WriteLine(n)).Now();
            result.IsT0.Should().BeTrue();
            result.IsT1.Should().BeFalse();
            result.AsT0.Should().Be(Unit.Value);
        }

        [TestMethod]
        public void TestNullAction()
        {
            Action act = null;
            var result = Try.This(act).Now();
            result.IsT0.Should().BeFalse();
            result.IsT1.Should().BeTrue();
        }

        [TestMethod]
        public void TestConsumer()
        {
            var result = Try.This((str) => Console.WriteLine(str), "Foo").Now();
            result.IsT0.Should().BeTrue();
            result.IsT1.Should().BeFalse();
            result.AsT0.Should().Be(Unit.Value);
        }

        [TestMethod]
        public void TestFunction()
        {
            var result = Try.This(Multiply, 12).Now();
            result.IsT0.Should().BeTrue();
            result.IsT1.Should().BeFalse();
            result.AsT0.Should().Be(24);
        }

        [TestMethod]
        public void TestChainCalls()
        {
            var result = Try.This(GetNumber)
                            .Then(Multiply).Now();
            result.IsT0.Should().BeTrue();
            result.IsT1.Should().BeFalse();
            result.AsT0.Should().Be(4);

            result = Try.This(GetFaulty)
                        .Then(Multiply).Now();
            result.IsT0.Should().BeFalse();
            result.IsT1.Should().BeTrue();
        }

        [TestMethod]
        public void TestFailure()
        {
            var result = Try.This(GetFaulty)
                            .Otherwise(exc => Console.WriteLine(exc))
                            .Now();
            result.IsT0.Should().BeFalse();
            result.IsT1.Should().BeTrue();
            result.AsT1.Should().NotBeNull().And.BeOfType<ArgumentException>();
        }

        [TestMethod]
        public void TestNullOtherwise()
        {
            Action<Exception> act = null;
            var result = Try.This(GetFaulty)
                            .Otherwise(act)
                            .Now();
            result.IsT0.Should().BeFalse();
            result.IsT1.Should().BeTrue();
            result.AsT1.Should().NotBeNull().And.BeOfType<ArgumentException>();
        }

        [TestMethod]
        public void TestEitherUnwrap()
        {
            var result = Try.This(GetOneOf).Now();
            result.IsT0.Should().BeTrue();
            result.AsT0.GetType().Should().Be(typeof(int));

            result = Try.This(GetNumber)
                        .Then(GetOneOf).Now();
            result.IsT0.Should().BeTrue();
            result.AsT0.GetType().Should().Be(typeof(int));
        }

        [TestMethod]
        public void TestMultiOtherwise()
        {
            int handles = 0;

            Try.This(GetNumber)
                .Otherwise(exc => handles++)
                .Then(GetFaulty)
                .Otherwise(exc => handles++)
                .Otherwise(exc => handles++)
                .Then(GetNumber)
                .Otherwise(exc => handles++)
                .Now();

            handles.Should().Be(1);
        }

        private int GetNumber() => 2;

        private int GetFaulty() => throw new ArgumentException("Some error");

        private int Multiply(int n) => n * 2;

        private OneOf<int, Exception> GetOneOf() => 1024;
    }
}
