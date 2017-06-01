using DotNet.Safe.Standard.Exceptions.Steps;
using DotNet.Safe.Standard.Util;
using System;

namespace DotNet.Safe.Standard.Exceptions
{
    /// <summary>
    /// Try exposes methods to start a function composition.
    /// </summary>
    public static class Try
    {
        /// <summary>
        /// Start a composition with an action.
        /// </summary>
        /// <param name="action">Action</param>
        /// <returns>In-progress composition</returns>
        public static Composition<Unit> This(Action action)
        {
            return new Composition<Unit>(new ThenCompositionStep<Unit, Unit>((param) =>
            {
                action();
                return Unit.Instance();
            }, 1), 1);
        }

        /// <summary>
        /// Start a composition with an action.
        /// </summary>
        /// <typeparam name="TParam">Type of the parameter</typeparam>
        /// <param name="action">Action</param>
        /// <param name="param">Parameter</param>
        /// <returns>In-progress composition</returns>
        public static Composition<Unit> This<TParam>(Action<TParam> action, TParam param)
        {
            return new Composition<Unit>(new ThenCompositionStep<TParam, Unit>((_param) =>
            {
                action(param);
                return Unit.Instance();
            }, 1), 1);
        }

        /// <summary>
        /// Start a composition with a function.
        /// </summary>
        /// <typeparam name="TResult">Type of the result</typeparam>
        /// <param name="func">Function</param>
        /// <returns></returns>
        public static Composition<TResult> This<TResult>(Func<TResult> func)
        {
            return new Composition<TResult>(new ThenCompositionStep<Unit, TResult>((param) => func(), 1), 1);
        }

        /// <summary>
        /// Start a composition with a function.
        /// </summary>
        /// <typeparam name="TParam">Type of the parameter</typeparam>
        /// <typeparam name="TResult">Type of the result</typeparam>
        /// <param name="func">Function</param>
        /// <param name="param">Parameter</param>
        /// <returns></returns>
        public static Composition<TResult> This<TParam, TResult>(Func<TParam, TResult> func, TParam param)
        {
            return new Composition<TResult>(new ThenCompositionStep<Unit, TResult>((_param) => func(param), 1), 1);
        }
    }
}
