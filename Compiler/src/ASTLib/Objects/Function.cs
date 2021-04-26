using System;
using System.Collections.Generic;
using System.Text;

namespace ASTLib.Objects
{
    public class Function
    {
        public int Reference { get; }
        public List<object> Scope { get; }

        public Function(int reference)
        {
            Reference = reference;
            Scope = new List<object>();
        }

        public Function(int reference, List<object> scope)
        {
            Reference = reference;
            Scope = scope;
        }
    }
}
