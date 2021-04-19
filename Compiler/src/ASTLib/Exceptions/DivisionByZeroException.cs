using ASTLib.Nodes;
using System;
using System.Collections.Generic;
using System.Text;

namespace ASTLib.Exceptions
{
    public class DivisionByZeroException : CompilerException
    {

        public DivisionByZeroException(Node node) : base(node, "Cannot divide by zero")
        {
        }

    }
}
