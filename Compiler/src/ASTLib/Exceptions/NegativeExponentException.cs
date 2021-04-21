using ASTLib.Nodes;
using System;
using System.Collections.Generic;
using System.Text;

namespace ASTLib.Exceptions
{
    public class NegativeExponentException : CompilerException
    {
        public NegativeExponentException(Node node) :
            base(node, $"'{node}' has a negative exponent")
        {
        }
    }
}
