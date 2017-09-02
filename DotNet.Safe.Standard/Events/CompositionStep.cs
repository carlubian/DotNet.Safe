using System;
using System.Collections.Generic;
using System.Text;

namespace DotNet.Safe.Standard.Events
{
    public class CompositionStep : EventArgs
    {
        public string Name { get; internal set; }
        public int Number { get; internal set; }
    }
}
