using DotNet.Safe.Standard.Events;
using DotNet.Safe.Standard.Util;
using System;
using System.Collections.Generic;

namespace DotNet.Safe.Standard.Exceptions.Steps
{
    /// <summary>
    /// Represents a step that will execute on a failed composition.
    /// </summary>
    internal class OtherwiseCompositionStep : ICompositionStep
    {
        private readonly Action<string> _action;
        private IEnumerable<ICompositionListener> _listeners;
        private readonly int _num;

        /// <summary>
        /// Creates a new step
        /// </summary>
        /// <param name="action">Action</param>
        /// <param name="num">Step number</param>
        internal OtherwiseCompositionStep(Action<string> action, int num)
        {
            _action = action;
            _num = num;
        }

        /// <summary>
        /// Creates a new step
        /// </summary>
        /// <param name="action">Action</param>
        /// <param name="num">Step number</param>
        internal OtherwiseCompositionStep(Action action, int num)
        {
            _action = (str) => action();
            _num = num;
        }

        /// <summary>
        /// Invoke the  step
        /// </summary>
        /// <param name="param">Result of the previous step</param>
        /// <param name="listeners">Composition listeners</param>
        /// <returns>Result of the current step</returns>
        public Either<object> Invoke(Either<object> param, CompositionParams @params, IEnumerable<ICompositionListener> listeners)
        {
            _listeners = listeners;

            if (param.Succeeded() || @params.ErrorHandled)
            {
                OnOtherwiseIgnored();
                return param;
            }

            OnOtherwiseBeginInvocation();
            @params.ErrorHandled = true;
            var tmp = ErrorManager.Default().Attempt(_action, param.ErrorOrElse(Resources.MISSING_ERROR_MESSAGE));

            if(tmp.Failed())
            {
                OnOtherwiseFailure(tmp.ErrorOrElse(Resources.MISSING_ERROR_MESSAGE));
            }

            OnOtherwiseEndInvocation();
            return param;
        }

        /* ---- Event Handler helper methods ---- */

        private void OnOtherwiseIgnored()
        {
            foreach (var listener in _listeners)
            {
                listener.OnOtherwiseIgnored(this, new OtherwiseStep { Name = _action.ToString(), Number = _num });
            }
        }

        private void OnOtherwiseBeginInvocation()
        {
            foreach (var listener in _listeners)
            {
                listener.OnOtherwiseBeginInvocation(this, new OtherwiseStep { Name = _action.ToString(), Number = _num });
            }
        }

        private void OnOtherwiseEndInvocation()
        {
            foreach (var listener in _listeners)
            {
                listener.OnOtherwiseEndInvocation(this, new OtherwiseStep { Name = _action.ToString(), Number = _num });
            }
        }

        private void OnOtherwiseFailure(string error)
        {
            foreach (var listener in _listeners)
            {
                listener.OnOtherwiseFailure(this, new CompositionError { ErrorMessage = error, Number = _num });
            }
        }
    }
}
