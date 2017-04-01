using DotNet.Safe.Standard.Util;
using System;

namespace DotNet.Safe.Standard.Exceptions.Steps
{
    /// <summary>
    /// Represents a step that will be executed on a successful composition
    /// </summary>
    /// <typeparam name="TParam"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    public class ThenCompositionStep<TParam, TResult> : ICompositionStep
    {
        private Func<TParam, TResult> _func;

        /// <summary>
        /// Creates a new step
        /// </summary>
        /// <param name="func">Function</param>
        public ThenCompositionStep(Func<TParam, TResult> func)
        {
            _func = func;
        }

        /// <summary>
        /// Invokes the step
        /// </summary>
        /// <param name="param">Result of the previous step</param>
        /// <returns>Result of the current step</returns>
        public Either<object> Invoke(Either<object> param)
        {
            if (param.Failed())
                return new Failure<object>(param.ErrorOrElse(Resources.MISSING_ERROR_MESSAGE));

            Either<TResult> tmp = ErrorManager.Default().Attempt(_func, (TParam)param.GetOrElse(null));

            if (tmp.Failed())
                return new Failure<object>(tmp.ErrorOrElse(Resources.MISSING_ERROR_MESSAGE));
            return new Success<object>(tmp.GetOrElse(default(TResult)) as object);
        }
    }
}
