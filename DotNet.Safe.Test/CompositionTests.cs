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

        private int GetNumber() => 2;

        private int GetFaulty() => throw new System.Exception("Error");

        private int Multiply(int n) => n * 2;
    }
}
