namespace DotNet.Safe.Standard.Exceptions.Steps
{
    /// <summary>
    /// Represents each of the steps in a composition.
    /// </summary>
    public interface ICompositionStep
    {
        /// <summary>
        /// Invoke the step.
        /// </summary>
        /// <param name="param">Result of the previous step</param>
        /// <returns>Result of the current step</returns>
        Either<object> Invoke(Either<object> param);
    }
}
