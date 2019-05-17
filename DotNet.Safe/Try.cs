using DotNet.Safe.Core;
using OneOf;
using System;

[assembly: CLSCompliant(true)]
namespace DotNet.Safe
{
    /// <summary>
    /// This class is the entry point for DotNet.Safe
    /// function compositions.
    /// </summary>
    public static class Try
    {
        /// <summary>
        /// Start a composition with a function that takes
        /// no parameters and retuns void.
        /// </summary>
        /// <param name="action">Action</param>
        /// <returns>Composition</returns>
        public static Composition<Unit> This(Action action)
        {
            return new Composition<Unit>(new ValidStep<Unit, Unit>(unit =>
                         {
                             action();
                             return Unit.Value;
                         }));
        }

        /// <summary>
        /// Start a composition with a function that takes a
        /// parameter and returns void.
        /// </summary>
        /// <typeparam name="TParam">Parameter type</typeparam>
        /// <param name="action">Action</param>
        /// <param name="param">Parameter</param>
        /// <returns>Composition</returns>
        public static Composition<Unit> This<TParam>(Action<TParam> action, TParam param)
        {
            return new Composition<Unit>(new ValidStep<Unit, Unit>(unit =>
                         {
                             action(param);
                             return Unit.Value;
                         }));
        }

        /// <summary>
        /// Start a composition with a function that takes no
        /// parameters and returns a result.
        /// </summary>
        /// <typeparam name="TResult">Result type</typeparam>
        /// <param name="func">Function</param>
        /// <returns>Composition</returns>
        public static Composition<TResult> This<TResult>(Func<TResult> func)
        {
            return new Composition<TResult>(new ValidStep<Unit, TResult>(unit => func()));
        }

        /// <summary>
        /// Start a composition with a function that takes no
        /// parameters and returns a result.
        /// </summary>
        /// <typeparam name="TResult">Result type</typeparam>
        /// <param name="func">Function</param>
        /// <returns>Composition</returns>
        public static Composition<TResult> This<TResult>(Func<OneOf<TResult, Exception>> func)
        {
            return new Composition<TResult>(new ValidStep<Unit, TResult>(unit =>
            {
                var result = func();
                if (result.IsT0)
                    return result.AsT0;
                else
                    throw result.AsT1;
            }));
        }

        /// <summary>
        /// Start a composition with a function that takes a
        /// parameter and returns a result.
        /// </summary>
        /// <typeparam name="TParam">Parameter type</typeparam>
        /// <typeparam name="TResult">Result type</typeparam>
        /// <param name="func">Function</param>
        /// <param name="param">Parameter</param>
        /// <returns>Composition</returns>
        public static Composition<TResult> This<TParam, TResult>(Func<TParam, TResult> func, TParam param)
        {
            return new Composition<TResult>(new ValidStep<Unit, TResult>(unit => func(param)));
        }

        /// <summary>
        /// Start a composition with a function that takes a
        /// parameter and returns a result.
        /// </summary>
        /// <typeparam name="TParam">Parameter type</typeparam>
        /// <typeparam name="TResult">Result type</typeparam>
        /// <param name="func">Function</param>
        /// <param name="param">Parameter</param>
        /// <returns>Composition</returns>
        public static Composition<TResult> This<TParam, TResult>(Func<TParam, OneOf<TResult, Exception>> func, TParam param)
        {
            return new Composition<TResult>(new ValidStep<Unit, TResult>(unit =>
            {
                var result = func(param);
                if (result.IsT0)
                    return result.AsT0;
                else
                    throw result.AsT1;
            }));
        }
    }
}
