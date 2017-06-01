using DotNet.Safe.Standard.Util;
using System;

namespace DotNet.Safe.Standard.Exceptions
{
    /// <summary>
    /// ErrorManagers attempt to execute actions and
    /// functions. Those executions return an instance
    /// of Either with the result.
    /// </summary>
    public class ErrorManager
    {
        private static readonly ErrorManager _default = new ErrorManager();

        /// <summary>
        /// Gets a default ErrorManager ready to attempt things.
        /// </summary>
        /// <returns>Default ErrorManager</returns>
        public static ErrorManager Default() => _default;

        /// <summary>
        /// Attempt to execute an action
        /// </summary>
        /// <param name="action">Action</param>
        /// <returns>Either</returns>
        public Either<Unit> Attempt(Action action)
        {
            try
            {
                action();
                return new Success<Unit>(Unit.Instance());
            } catch(Exception ex)
            {
                return new Failure<Unit>(ex.Message.Equals("") ? ex.ToString() : ex.Message);
            }
        }

        /// <summary>
        /// Attempt to execute an action
        /// </summary>
        /// <typeparam name="TParam">Type of the parameter</typeparam>
        /// <param name="action">Action</param>
        /// <param name="param">Parameter</param>
        /// <returns>Either</returns>
        public Either<Unit> Attempt<TParam>(Action<TParam> action, TParam param)
        {
            try
            {
                action(param);
                return new Success<Unit>(Unit.Instance());
            }
            catch (Exception ex)
            {
                return new Failure<Unit>(ex.Message.Equals("") ? ex.ToString() : ex.Message);
            }
        }

        /// <summary>
        /// Attempt to execute a function
        /// </summary>
        /// <typeparam name="TResult">Type of the result</typeparam>
        /// <param name="func">Function</param>
        /// <returns>Either</returns>
        public Either<TResult> Attempt<TResult>(Func<TResult> func)
        {
            try
            {
                return new Success<TResult>(func());
            }
            catch (Exception ex)
            {
                return new Failure<TResult>(ex.Message.Equals("") ? ex.ToString() : ex.Message);
            }
        }

        /// <summary>
        /// Attempt to execute a function
        /// </summary>
        /// <typeparam name="TParam">Type of the parameter</typeparam>
        /// <typeparam name="TResult">Type of the result</typeparam>
        /// <param name="func">Function</param>
        /// <param name="param">Parameter</param>
        /// <returns>Either</returns>
        public Either<TResult> Attempt<TParam, TResult>(Func<TParam, TResult> func, TParam param)
        {
            try
            {
                return new Success<TResult>(func(param));
            }
            catch (Exception ex)
            {
                return new Failure<TResult>(ex.Message.Equals("") ? ex.ToString() : ex.Message);
            }
        }
    }
}
