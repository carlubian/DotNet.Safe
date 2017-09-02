using System;
using System.Collections.Generic;
using System.Text;

namespace DotNet.Safe.Standard.Events
{
    public class CompositionError : EventArgs
    {
        public string ErrorMessage { get; internal set; }
        public int Number { get; internal set; }
    }
}
