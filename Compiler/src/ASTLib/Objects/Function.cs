using System;
using System.Collections.Generic;
using System.Text;

namespace ASTLib.Objects
{
    public class Function
    {
        public int Reference { get; }

        public Function(int reference)
        {
            Reference = reference;
        }
    }
}
