using DotNet.Safe.Standard.Exceptions.Steps;
using DotNet.Safe.Standard.Util;
using System;
using System.Collections.Generic;

namespace DotNet.Safe.Standard.Exceptions
{
    /// <summary>
    /// Represents an in-progress composition. This can be
    /// finalized eagerly or lazily.
    /// </summary>
    /// <typeparam name="TCurrent">Current type of the composition</typeparam>
    public class Composition<TCurrent>
    {
        private IList<ICompositionStep> _steps = new List<ICompositionStep>();

        internal Composition(ICompositionStep step)
        {
            _steps = new List<ICompositionStep>
            {
                step
            };
        }

        internal Composition(IList<ICompositionStep> steps)
        {
            _steps = steps;
        }

        /// <summary>
        /// Append an action to this composition.
        /// </summary>
        /// <param name="action">Action</param>
        /// <returns>In-progress composition</returns>
        public Composition<Unit> Then(Action action)
        {
            _steps.Add(new ThenCompositionStep<TCurrent, Unit>((param) =>
            {
                action();
                return Unit.Instance();
            }));

            return new Composition<Unit>(_steps);
        }

        /// <summary>
        /// Append an action to this composition.
        /// </summary>
        /// <param name="action">Action</param>
        /// <returns>In-progress composition</returns>
        public Composition<Unit> Then(Action<TCurrent> action)
        {
            _steps.Add(new ThenCompositionStep<TCurrent, Unit>((param) =>
            {
                action(param);
                return Unit.Instance();
            }));

            return new Composition<Unit>(_steps);
        }

        /// <summary>
        /// Append a function to this composition.
        /// </summary>
        /// <typeparam name="TResult">Type of the result</typeparam>
        /// <param name="func">Function</param>
        /// <returns>In-progress composition</returns>
        public Composition<TResult> Then<TResult>(Func<TResult> func)
        {
            _steps.Add(new ThenCompositionStep<TCurrent, TResult>((param) => func()));

            return new Composition<TResult>(_steps);
        }

        /// <summary>
        /// Append a function to this composition.
        /// </summary>
        /// <typeparam name="TResult">Type of the result</typeparam>
        /// <param name="func">Function</param>
        /// <returns>In-progress composition</returns>
        public Composition<TResult> Then<TResult>(Func<TCurrent, TResult> func)
        {
            _steps.Add(new ThenCompositionStep<TCurrent, TResult>((param) => func(param)));

            return new Composition<TResult>(_steps);
        }

        /// <summary>
        /// Execute an action if the composition has failed at
        /// this point. The action will be ignored if the
        /// composition is successful.
        /// </summary>
        /// <param name="action">Action</param>
        /// <returns>In-progress composition</returns>
        public Composition<TCurrent> Otherwise(Action action)
        {
            _steps.Add(new OtherwiseCompositionStep<TCurrent>(action));

            return new Composition<TCurrent>(_steps);
        }

        /// <summary>
        /// Execute an action if the composition has failed at
        /// this point. The action will be ignored if the
        /// composition is successful.
        /// </summary>
        /// <param name="action">Action</param>
        /// <returns>In-progress composition</returns>
        public Composition<TCurrent> Otherwise(Action<string> action)
        {
            _steps.Add(new OtherwiseCompositionStep<TCurrent>(action));

            return new Composition<TCurrent>(_steps);
        }

        /// <summary>
        /// Finalize the composition and execute it eagerly. The
        /// result will be returned as an instance of Either.
        /// </summary>
        /// <returns>Either</returns>
        public Either<TCurrent> Now()
        {
            Either<object> tmp = new Success<object>(null);

            foreach (var step in _steps)
                tmp = step.Invoke(tmp);

            if (tmp.Failed())
                return new Failure<TCurrent>(tmp.ErrorOrElse(Resources.MISSING_ERROR_MESSAGE));

            return new Success<TCurrent>((TCurrent)tmp.GetOrElse(default(TCurrent)));
        }

        /// <summary>
        /// Finalize the composition and return an instance of Lazy
        /// ready to be invoked at a later time.
        /// </summary>
        /// <returns>Lazy</returns>
        public Lazy<TCurrent> Later()
        {
            Func<Either<TCurrent>> func = () =>
            {
                Either<object> tmp = new Success<object>(null);

                foreach (var step in _steps)
                    tmp = step.Invoke(tmp);

                if (tmp.Failed())
                    return new Failure<TCurrent>(tmp.ErrorOrElse(Resources.MISSING_ERROR_MESSAGE));

                return new Success<TCurrent>((TCurrent)tmp.GetOrElse(default(TCurrent)));
            };

            return new Lazy<TCurrent>(func);
        }
    }
}
