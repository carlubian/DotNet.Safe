using DotNet.Safe.Standard.Util;
using System;

namespace DotNet.Safe.Standard.Exceptions.Steps
{
    /// <summary>
    /// Represents a step that will execute on a failed composition.
    /// </summary>
    /// <typeparam name="TParam"></typeparam>
    public class OtherwiseCompositionStep<TParam> : ICompositionStep
    {
        private Action<string> _action;

        /// <summary>
        /// Creates a new step
        /// </summary>
        /// <param name="action">Action</param>
        public OtherwiseCompositionStep(Action<string> action)
        {
            _action = action;
        }

        /// <summary>
        /// Creates a new step
        /// </summary>
        /// <param name="action">Action</param>
        public OtherwiseCompositionStep(Action action)
        {
            _action = (str) => action();
        }

        /// <summary>
        /// Invoke the  step
        /// </summary>
        /// <param name="param">Result of the previous step</param>
        /// <returns>Result of the current step</returns>
        public Either<object> Invoke(Either<object> param)
        {
            if (param.Succeeded())
                return param;

            ErrorManager.Default().Attempt(_action, param.ErrorOrElse(Resources.MISSING_ERROR_MESSAGE));

            return param;
        }
    }
}
