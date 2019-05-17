using OneOf;
using System;

namespace DotNet.Safe.Core
{
    internal interface ICompositionStep
    {
        OneOf<object, Exception> Invoke(OneOf<object, Exception> param);
    }
}
