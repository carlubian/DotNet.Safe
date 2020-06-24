using OneOf;
using System;
using System.Collections.Generic;

namespace DotNet.Safe.Core
{
    internal class ErrorStep<TCurrent> : ICompositionStep
    {
        private readonly Action<Exception> _action;
        private readonly IDictionary<string, string> _properties;

        internal ErrorStep(Action<Exception> action, IDictionary<string, string> properties)
        {
            _action = action;
            _properties = properties;
        }

        public OneOf<object, Exception> Invoke(OneOf<object, Exception> param)
        {
            // If previous step succeeded, propagate success
            if (param.IsT0)
                return param.AsT0;

            if (_properties["AlreadyHandled"] is "True")
                return param.AsT1;

            // Otherwise, execute current step and propagate failure
            try
            {
                _properties["AlreadyHandled"] = "True";
                _action(param.AsT1);
            }
            catch 
            {
                // Intentionally left empty
            }

            return param.AsT1;
        }
    }
}
