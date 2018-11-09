using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FluentAssertions;
using DotNet.Safe.Standard;

namespace DotNet.Safe.Test
{
    [TestClass]
    public class CompositionTests
    {
        [TestMethod]
        public void TestSingleCall()
        {
            Try.This(GetNumber).Now()
                .Should().BeOfType(typeof(Success<int>))
                .And.Match<Either<int>>(n => n.GetOrElse(-1) == 2)
                .And.Match<Either<int>>(n => n.ErrorOrElse("") == "");

            Try.This(GetFaulty).Now()
                .Should().BeOfType(typeof(Failure<int>))
                .And.Match<Either<int>>(n => n.GetOrElse(-1) == -1)
                .And.Match<Either<int>>(n => n.ErrorOrElse("") == "Error");
        }

        [TestMethod]
        public void TestAction()
        {
            Try.This(() => Console.WriteLine("Foo")).Now()
                .Should().BeOfType(typeof(Success<Unit>))
                .And.Match<Either<Unit>>(n => n.GetOrElse(null) == Unit.Instance())
                .And.Match<Either<Unit>>(n => n.ErrorOrElse("") == "");

            Try.This(GetNumber)
                .Then((n) => Console.WriteLine(n)).Now()
                .Should().BeOfType(typeof(Success<Unit>))
                .And.Match<Either<Unit>>(n => n.GetOrElse(null) == Unit.Instance())
                .And.Match<Either<Unit>>(n => n.ErrorOrElse("") == "");
        }

        [TestMethod]
        public void TestNullAction()
        {
            Action act = null;
            Try.This(act).Now()
                .Should().BeOfType(typeof(Failure<Unit>));
        }

        [TestMethod]
        public void TestConsumer()
        {
            Try.This((str) => Console.WriteLine(str), "Foo").Now()
                .Should().BeOfType(typeof(Success<Unit>))
                .And.Match<Either<Unit>>(n => n.GetOrElse(null) == Unit.Instance())
                .And.Match<Either<Unit>>(n => n.ErrorOrElse("") == "");
        }

        [TestMethod]
        public void TestFunction()
        {
            Try.This(Multiply, 12).Now()
                .Should().BeOfType(typeof(Success<int>))
                .And.Match<Either<int>>(n => n.GetOrElse(-1) == 24)
                .And.Match<Either<int>>(n => n.ErrorOrElse("") == "");
        }

        [TestMethod]
        public void TestChainCalls()
        {
            Try.This(GetNumber)
                .Then(Multiply).Now()
                .Should().BeOfType(typeof(Success<int>))
                .And.Match<Either<int>>(n => n.GetOrElse(-1) == 4)
                .And.Match<Either<int>>(n => n.ErrorOrElse("") == "");

            Try.This(GetFaulty)
                .Then(Multiply).Now()
                .Should().BeOfType(typeof(Failure<int>))
                .And.Match<Either<int>>(n => n.GetOrElse(-1) == -1)
                .And.Match<Either<int>>(n => n.ErrorOrElse("") == "Error");
        }

        [TestMethod]
        public void TestFailure()
        {
            Try.This(GetFaulty)
                .Otherwise(err => Console.WriteLine(err))
                .Now()
                .Should().BeOfType(typeof(Failure<int>))
                .And.Match<Either<int>>(n => n.GetOrElse(-1) == -1)
                .And.Match<Either<int>>(n => n.ErrorOrElse("") == "Error");
        }

        [TestMethod]
        public void TestNullOtherwise()
        {
            Action<string> act = null;
            Try.This(GetFaulty)
                .Otherwise(act)
                .Now()
                .Should().BeOfType(typeof(Failure<int>))
                .And.Match<Either<int>>(n => n.GetOrElse(-1) == -1)
                .And.Match<Either<int>>(n => n.ErrorOrElse("") == "Error");
        }

        [TestMethod]
        public void TestEitherUnwrap()
        {
            Try.This(GetEither).Now()
                .Should().BeOfType(typeof(Success<int>))
                .And.NotBeOfType(typeof(Success<Success<int>>));

            Try.This(GetFailure).Now()
                .Should().BeOfType(typeof(Failure<int>))
                .And.NotBeOfType(typeof(Success<Failure<int>>));

            Try.This(GetNumber)
                .Then(GetEither).Now()
                .Should().BeOfType(typeof(Success<int>))
                .And.NotBeOfType(typeof(Success<Success<int>>));
        }

        [TestMethod]
        public void TestMultiOtherwise()
        {
            int handles = 0;

            Try.This(GetNumber)
                .Otherwise(() => handles++)
                .Then(GetFaulty)
                .Otherwise(() => handles++)
                .Otherwise(() => handles++)
                .Then(GetNumber)
                .Otherwise(() => handles++)
                .Now();

            handles.Should().Be(1);
        }

        private int GetNumber() => 2;

        private int GetFaulty() => throw new System.Exception("Error");

        private int Multiply(int n) => n * 2;

        private Either<int> GetEither() => new Success<int>(1024);

        private Either<int> GetFailure() => new Failure<int>("Kaboom!");
    }
}
