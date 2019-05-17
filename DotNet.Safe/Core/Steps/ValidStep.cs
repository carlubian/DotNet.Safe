using OneOf;
using System;

namespace DotNet.Safe.Core
{
    internal class ValidStep<TPrevious, TCurrent> : ICompositionStep
    {
        private readonly Func<TPrevious, TCurrent> _func;

        internal ValidStep(Func<TPrevious, TCurrent> func)
        {
            _func = func;
        }

        public OneOf<object, Exception> Invoke(OneOf<object, Exception> param)
        {
            // If previous step failed, propagate failure
            if (param.IsT1)
                return param.AsT1;

            // Otherwise, execute current step
            try
            {
                if (typeof(TPrevious) == typeof(Unit))
                    return _func(default);
                return _func((TPrevious)param.AsT0);
            }
            catch (Exception e)
            {
                return e;
            }
        }
    }
}
