using DotNet.Safe.Standard.Events;
using System.Collections.Generic;

namespace DotNet.Safe.Standard.Exceptions.Steps
{
    /// <summary>
    /// Represents each of the steps in a composition.
    /// </summary>
    internal interface ICompositionStep
    {
        /// <summary>
        /// Invoke the step.
        /// </summary>
        /// <param name="param">Result of the previous step</param>
        /// <param name="listeners">Composition event listeners</param>
        /// <returns>Result of the current step</returns>
        Either<object> Invoke(Either<object> param, IEnumerable<ICompositionListener> listeners);
    }
}
