using DotNet.Safe.Standard.Exceptions.Steps;
using DotNet.Safe.Standard.Events;
using DotNet.Safe.Standard.Util;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DotNet.Safe.Standard.Exceptions
{
    /// <summary>
    /// Represents an in-progress composition. This can be
    /// finalized eagerly or lazily.
    /// </summary>
    /// <typeparam name="TCurrent">Current type of the composition</typeparam>
    public class Composition<TCurrent>
    {
        private readonly IList<ICompositionStep> _steps;
        private readonly IList<ICompositionListener> _listeners = new List<ICompositionListener>();
        private int _num;
        private readonly CompositionParams _params = new CompositionParams();

        internal Composition(ICompositionStep step, int num)
        {
            _steps = new List<ICompositionStep>
            {
                step
            };
            _num = num;
        }

        internal Composition(IList<ICompositionStep> steps, int num)
        {
            _steps = steps;
            _num = num;
        }

        /// <summary>
        /// Attaches an event listener to this composition.
        /// </summary>
        /// <param name="listener">Composition listener</param>
        /// <returns>In-progress composition</returns>
        public Composition<TCurrent> Attach(ICompositionListener listener)
        {
            _listeners.Add(listener);
            return this;
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
            }, ++_num));

            return new Composition<Unit>(_steps, _num);
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
            }, ++_num));

            return new Composition<Unit>(_steps, _num);
        }

        /// <summary>
        /// Append a function to this composition.
        /// </summary>
        /// <typeparam name="TResult">Type of the result</typeparam>
        /// <param name="func">Function</param>
        /// <returns>In-progress composition</returns>
        public Composition<TResult> Then<TResult>(Func<TResult> func)
        {
            _steps.Add(new ThenCompositionStep<TCurrent, TResult>((param) => func(), ++_num));

            return new Composition<TResult>(_steps, _num);
        }

        /// <summary>
        /// Append a function to this composition.
        /// </summary>
        /// <typeparam name="TResult">Type of the result</typeparam>
        /// <param name="func">Function</param>
        /// <returns>In-progress composition</returns>
        public Composition<TResult> Then<TResult>(Func<Either<TResult>> func)
        {
            _steps.Add(new EitherCompositionStep<TCurrent, TResult>((param) => func(), ++_num));

            return new Composition<TResult>(_steps, _num);
        }

        /// <summary>
        /// Append a function to this composition.
        /// </summary>
        /// <typeparam name="TResult">Type of the result</typeparam>
        /// <param name="func">Function</param>
        /// <returns>In-progress composition</returns>
        public Composition<TResult> Then<TResult>(Func<TCurrent, TResult> func)
        {
            _steps.Add(new ThenCompositionStep<TCurrent, TResult>(func, ++_num));

            return new Composition<TResult>(_steps, _num);
        }

        /// <summary>
        /// Append a function to this composition.
        /// </summary>
        /// <typeparam name="TResult">Type of the result</typeparam>
        /// <param name="func">Function</param>
        /// <returns>In-progress composition</returns>
        public Composition<TResult> Then<TResult>(Func<TCurrent, Either<TResult>> func)
        {
            _steps.Add(new EitherCompositionStep<TCurrent, TResult>(func, ++_num));

            return new Composition<TResult>(_steps, _num);
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
            _steps.Add(new OtherwiseCompositionStep(action, ++_num));

            return new Composition<TCurrent>(_steps, _num);
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
            _steps.Add(new OtherwiseCompositionStep(action, ++_num));

            return new Composition<TCurrent>(_steps, _num);
        }

        /// <summary>
        /// Finalize the composition and execute it eagerly. The
        /// result will be returned as an instance of Either.
        /// </summary>
        /// <returns>Either</returns>
        public Either<TCurrent> Now()
        {
            Either<object> tmp = new Success<object>(null);

            OnCompositionStarted();
            tmp = _steps.Aggregate(tmp, (current, step) => step.Invoke(current, _params, _listeners));
            OnCompositionFinished();

            if (tmp.Failed())
            {
                return new Failure<TCurrent>(tmp.ErrorOrElse(Resources.MISSING_ERROR_MESSAGE));
            }

            return new Success<TCurrent>((TCurrent)tmp.GetOrElse(default(TCurrent)));
        }

        /// <summary>
        /// Finalize the composition and return an instance of Lazy
        /// ready to be invoked at a later time.
        /// </summary>
        /// <returns>Lazy</returns>
        public Lazy<TCurrent> Later()
        {
            Either<TCurrent> Func()
            {
                Either<object> tmp = new Success<object>(null);

                OnCompositionStarted();
                tmp = _steps.Aggregate(tmp, (current, step) => step.Invoke(current, _params, _listeners));
                OnCompositionFinished();

                if (tmp.Failed())
                {
                    return new Failure<TCurrent>(tmp.ErrorOrElse(Resources.MISSING_ERROR_MESSAGE));
                }

                return new Success<TCurrent>((TCurrent) tmp.GetOrElse(default(TCurrent)));
            }

            return new Lazy<TCurrent>(Func);
        }

        /* ---- Event Handler helper methods ---- */

        private void OnCompositionStarted()
        {
            foreach (var listener in _listeners)
            {
                listener.OnCompositionStarted(this, new CompositionStatus());
            }
        }

        private void OnCompositionFinished()
        {
            foreach (var listener in _listeners)
            {
                listener.OnCompositionFinished(this, new CompositionStatus());
            }
        }
    }
}
