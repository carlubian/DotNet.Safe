namespace DotNet.Safe.Standard.Exceptions
{
    /// <summary>
    /// Represents either a success or a failure.
    /// Either cannot be directly instanced; Use Success
    /// or Failure instead.
    /// </summary>
    /// <typeparam name="T">Type within</typeparam>
    public abstract class Either<T>
    {
        internal T _value = default(T);
        internal string _error = "";

        /// <summary>
        /// Determines whether this Either succeeded.
        /// </summary>
        /// <returns></returns>
        public abstract bool Succeeded();

        /// <summary>
        /// Determines whether this Either failed.
        /// </summary>
        /// <returns></returns>
        public abstract bool Failed();

        /// <summary>
        /// Gets the value within. If the Either failed
        /// or the value cannot be recovered, other will
        /// be returned instead.
        /// </summary>
        /// <param name="other">Alternative value</param>
        /// <returns>Value</returns>
        public abstract T GetOrElse(T other);

        /// <summary>
        /// Gets the error message. If the Either succeeded
        /// or the error cannot be recovered, other will
        /// be returned instead.
        /// </summary>
        /// <param name="other">Alternative message</param>
        /// <returns>Error message</returns>
        public abstract string ErrorOrElse(string other);
    }
}
