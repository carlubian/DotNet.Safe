namespace DotNet.Safe.Standard.Exceptions
{
    /// <summary>
    /// Represents a failed Either.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Failure<T> : Either<T>
    {
        /// <summary>
        /// Creates a new failure
        /// </summary>
        /// <param name="error">Error message</param>
        public Failure(string error)
        {
            _error = error;
        }

        /// <summary>
        /// Gets the error message or an alternative if it
        /// can't be recovered.
        /// </summary>
        /// <param name="other">Alternative message</param>
        /// <returns>Error message</returns>
        public override string ErrorOrElse(string other) => _error;

        /// <summary>
        /// Determines whether this Either failed.
        /// </summary>
        /// <returns></returns>
        public override bool Failed() => true;

        /// <summary>
        /// Gets the result or an alternative if it
        /// can't be recovered.
        /// </summary>
        /// <param name="other">Alternative result</param>
        /// <returns>Result</returns>
        public override T GetOrElse(T other) => other;

        /// <summary>
        /// Determines whether this Either succeeded.
        /// </summary>
        /// <returns></returns>
        public override bool Succeeded() => false;
    }
}
