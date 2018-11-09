using Microsoft.VisualStudio.TestTools.UnitTesting;
using FluentAssertions;
using DotNet.Safe.Standard;

namespace DotNet.Safe.Test
{
    [TestClass]
    public class LazyTests
    {
        [TestMethod]
        public void TestSingleCall()
        {
            var lazy = Try.This(GetNumber).Later();
            
            lazy.Eval()
                .Should().BeOfType(typeof(Success<int>))
                .And.Match<Either<int>>(n => n.GetOrElse(-1) == 2)
                .And.Match<Either<int>>(n => n.ErrorOrElse("") == "");

            lazy = Try.This(GetFaulty).Later();

            lazy.Eval()
                .Should().BeOfType(typeof(Failure<int>))
                .And.Match<Either<int>>(n => n.GetOrElse(-1) == -1)
                .And.Match<Either<int>>(n => n.ErrorOrElse("") == "Error");
        }

        [TestMethod]
        public void TestChainCalls()
        {
            var lazy = Try.This(GetNumber)
                .Then(Multiply).Later();

            lazy.Eval()
                .Should().BeOfType(typeof(Success<int>))
                .And.Match<Either<int>>(n => n.GetOrElse(-1) == 4)
                .And.Match<Either<int>>(n => n.ErrorOrElse("") == "");

            lazy = Try.This(GetFaulty)
                .Then(Multiply).Later();

            lazy.Eval()
                .Should().BeOfType(typeof(Failure<int>))
                .And.Match<Either<int>>(n => n.GetOrElse(-1) == -1)
                .And.Match<Either<int>>(n => n.ErrorOrElse("") == "Error");
        }

        private int GetNumber() => 2;

        private int GetFaulty() => throw new System.Exception("Error");

        private int Multiply(int n) => n * 2;
    }
}
