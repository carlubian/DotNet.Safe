using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FluentAssertions;
using DotNet.Safe.Standard.Exceptions;

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
        public void TestEitherUnwrap()
        {
            Try.This(GetEither).Now()
                .Should().BeOfType(typeof(Success<int>))
                .And.NotBeOfType(typeof(Success<Success<int>>));

            Try.This(GetFailure).Now()
                .Should().BeOfType(typeof(Failure<int>))
                .And.NotBeOfType(typeof(Success<Failure<int>>));
        }

        private int GetNumber() => 2;

        private int GetFaulty() => throw new System.Exception("Error");

        private int Multiply(int n) => n * 2;

        private Either<int> GetEither() => new Success<int>(1024);

        private Either<int> GetFailure() => new Failure<int>("Kaboom!");
    }
}
