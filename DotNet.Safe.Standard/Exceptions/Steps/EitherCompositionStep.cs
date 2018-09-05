﻿using DotNet.Safe.Standard.Events;
using DotNet.Safe.Standard.Util;
using System;
using System.Collections.Generic;

namespace DotNet.Safe.Standard.Exceptions.Steps
{
    /// <summary>
    /// Represents a step that will be executed on a successful composition
    /// </summary>
    /// <typeparam name="TParam"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    internal class EitherCompositionStep<TParam, TResult> : ICompositionStep
    {
        private readonly Func<TParam, Either<TResult>> _func;
        private IEnumerable<ICompositionListener> _listeners;
        private readonly int _num;

        /// <summary>
        /// Creates a new step
        /// </summary>
        /// <param name="func">Function</param>
        /// <param name="num">Step number</param>
        internal EitherCompositionStep(Func<TParam, Either<TResult>> func, int num)
        {
            _func = func;
            _num = num;
        }

        /// <summary>
        /// Invokes the step
        /// </summary>
        /// <param name="param">Result of the previous step</param>
        /// <param name="listeners">Composition listeners</param>
        /// <returns>Result of the current step</returns>
        public Either<object> Invoke(Either<object> param, CompositionParams @params, IEnumerable<ICompositionListener> listeners)
        {
            _listeners = listeners;

            if (param.Failed())
            {
                OnStepIgnored();
                return new Failure<object>(param.ErrorOrElse(Resources.MISSING_ERROR_MESSAGE));
            }

            OnStepBeginInvocation();
            var tmp = ErrorManager.Default().Attempt(_func, (TParam)param.GetOrElse(null));
            var tmq = tmp.Succeeded() ? tmp.GetOrElse(null) : new Failure<TResult>(tmp.ErrorOrElse(Resources.MISSING_ERROR_MESSAGE));

            if (tmq.Failed())
            {
                OnStepFailure(tmq.ErrorOrElse(Resources.MISSING_ERROR_MESSAGE));
                OnStepEndInvocation();
                return new Failure<object>(tmq.ErrorOrElse(Resources.MISSING_ERROR_MESSAGE));
            }
            else
            {
                OnStepEndInvocation();
                return new Success<object>(tmq.GetOrElse(default(TResult)) as object);
            }
        }

        /* ---- Event Handler helper methods ---- */

        private void OnStepIgnored()
        {
            foreach (var listener in _listeners)
            {
                listener.OnStepIgnored(this, new CompositionStep { Name = _func.ToString(), Number = _num });
            }
        }

        private void OnStepBeginInvocation()
        {
            foreach (var listener in _listeners)
            {
                listener.OnStepBeginInvocation(this, new CompositionStep { Name = _func.ToString(), Number = _num });
            }
        }

        private void OnStepEndInvocation()
        {
            foreach (var listener in _listeners)
            {
                listener.OnStepEndInvocation(this, new CompositionStep { Name = _func.ToString(), Number = _num });
            }
        }

        private void OnStepFailure(string error)
        {
            foreach (var listener in _listeners)
            {
                listener.OnStepFailure(this, new CompositionError { ErrorMessage = error, Number = _num });
            }
        }
    }
}
