using OneOf;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DotNet.Safe.Core
{
    /// <summary>
    /// Contains a function composition in progress
    /// of being built.
    /// </summary>
    /// <typeparam name="TCurrent">Current result type</typeparam>
    public class Composition<TCurrent>
    {
        private readonly IList<ICompositionStep> _steps;
        private IDictionary<string, string> _properties;

        internal Composition(ICompositionStep step)
        {
            _steps = new List<ICompositionStep>
            {
                step
            };
            _properties = new LenientDictionary<string, string>();
        }

        internal Composition(IList<ICompositionStep> steps, IDictionary<string, string> properties)
        {
            _steps = steps;
            _properties = properties;
        }

        /// <summary>
        /// Appends a step to this composition using a function that
        /// takes no parameters and returns void. The current result
        /// (if present) will be discarded.
        /// </summary>
        /// <param name="action">Action</param>
        /// <returns>Composition</returns>
        public Composition<Unit> Then(Action action)
        {
            _steps.Add(new ValidStep<TCurrent, Unit>(value =>
            {
                action();
                return Unit.Value;
            }));

            return new Composition<Unit>(_steps, _properties);
        }

        /// <summary>
        /// Appends a step to this composition using a function that
        /// takes a parameter and returns void.
        /// </summary>
        /// <param name="action">Action</param>
        /// <returns>Composition</returns>
        public Composition<Unit> Then(Action<TCurrent> action)
        {
            _steps.Add(new ValidStep<TCurrent, Unit>(value =>
            {
                action(value);
                return Unit.Value;
            }));

            return new Composition<Unit>(_steps, _properties);
        }

        /// <summary>
        /// Appends a step to this composition using a function that
        /// takes no parameters and returns a result. The current
        /// result (if present) will be discarded.
        /// </summary>
        /// <typeparam name="TResult">Result type</typeparam>
        /// <param name="func">Function</param>
        /// <returns>Composition</returns>
        public Composition<TResult> Then<TResult>(Func<TResult> func)
        {
            _steps.Add(new ValidStep<TCurrent, TResult>(value => func()));

            return new Composition<TResult>(_steps, _properties);
        }

        /// <summary>
        /// Appends a step to this composition using a function that
        /// takes no parameters and returns a result. The current
        /// result (if present) will be discarded.
        /// </summary>
        /// <typeparam name="TResult">Result type</typeparam>
        /// <param name="func">Function</param>
        /// <returns>Composition</returns>
        public Composition<TResult> Then<TResult>(Func<OneOf<TResult, Exception>> func)
        {
            _steps.Add(new ValidStep<Unit, TResult>(unit =>
            {
                var result = func();
                if (result.IsT0)
                    return result.AsT0;
                else
                    throw result.AsT1;
            }));

            return new Composition<TResult>(_steps, _properties);
        }

        /// <summary>
        /// Appends a step to this composition using a function that
        /// takes a parameter and returns a result.
        /// </summary>
        /// <typeparam name="TResult">Result type</typeparam>
        /// <param name="func">Function</param>
        /// <returns>Composition</returns>
        public Composition<TResult> Then<TResult>(Func<TCurrent, TResult> func)
        {
            _steps.Add(new ValidStep<TCurrent, TResult>(value => func(value)));

            return new Composition<TResult>(_steps, _properties);
        }

        /// <summary>
        /// Appends a step to this composition using a function that
        /// takes a parameter and returns a result.
        /// </summary>
        /// <typeparam name="TResult">Result type</typeparam>
        /// <param name="func">Function</param>
        /// <returns>Composition</returns>
        public Composition<TResult> Then<TResult>(Func<TCurrent, OneOf<TResult, Exception>> func)
        {
            _steps.Add(new ValidStep<TCurrent, TResult>(value =>
            {
                var result = func(value);
                if (result.IsT0)
                    return result.AsT0;
                else
                    throw result.AsT1;
            }));

            return new Composition<TResult>(_steps, _properties);
        }

        /// <summary>
        /// Appends a step to this composition that will only be
        /// executed in case of failure. If the previous steps are
        /// successful, this function will be ignored.
        /// </summary>
        /// <param name="action">Action</param>
        /// <returns>Composition</returns>
        public Composition<TCurrent> Otherwise(Action<Exception> action)
        {
            _steps.Add(new ErrorStep<TCurrent>(action, _properties));

            return new Composition<TCurrent>(_steps, _properties);
        }

        /// <summary>
        /// Executes the composition as described up to this point,
        /// and return the result of doing so.
        /// </summary>
        /// <returns>Result</returns>
        public OneOf<TCurrent, Exception> Now()
        {
            OneOf<object, Exception> tmp = new object();
            tmp = _steps.Aggregate(tmp, (current, step) => step.Invoke(current));

            if (tmp.IsT0)
                return (TCurrent)tmp.AsT0;

            return tmp.AsT1;
        }

        /// <summary>
        /// Store the composition as a Function ready to be executed
        /// at a later date.
        /// </summary>
        /// <returns>Function</returns>
        public Func<OneOf<TCurrent, Exception>> Later() => () => Now();
    }
}
