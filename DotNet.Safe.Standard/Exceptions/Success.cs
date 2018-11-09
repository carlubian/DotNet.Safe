namespace DotNet.Safe.Standard
{
    /// <summary>
    /// Represents a successful Either.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Success<T> : Either<T>
    {
        /// <summary>
        /// Creates a new success
        /// </summary>
        /// <param name="value"></param>
        public Success(T value)
        {
            _value = value;
        }

        /// <summary>
        /// Gets the error message or an alternative if
        /// it can't be recovered.
        /// </summary>
        /// <param name="other">Alternative message</param>
        /// <returns>Error message</returns>
        public override string ErrorOrElse(string other) => other;

        /// <summary>
        /// Determines whether this Either failed.
        /// </summary>
        /// <returns></returns>
        public override bool Failed() => false;

        /// <summary>
        /// Gets the result or an alternative if
        /// it can't be recovered.
        /// </summary>
        /// <param name="other">Alternative result</param>
        /// <returns>Result</returns>
        public override T GetOrElse(T other) => _value;

        /// <summary>
        /// Determines whether this Either succeeded.
        /// </summary>
        /// <returns></returns>
        public override bool Succeeded() => true;
    }
}
