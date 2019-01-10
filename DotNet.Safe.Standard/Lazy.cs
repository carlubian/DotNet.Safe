using System;

namespace DotNet.Safe.Standard
{
    /// <summary>
    /// Represents a composition ready to be executed on demand.
    /// </summary>
    /// <typeparam name="TResult">Type of the result</typeparam>
    public class Lazy<TResult>
    {
        private readonly Func<Either<TResult>> _func;

        internal Lazy(Func<Either<TResult>> func)
        {
            _func = func;
        }

        /// <summary>
        /// Invoke the composition and return the result as an
        /// instance of Either.
        /// </summary>
        /// <returns>Either</returns>
        public Either<TResult> Eval() => _func();
    }
}
