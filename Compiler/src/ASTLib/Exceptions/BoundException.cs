using ASTLib.Nodes;
using System;
using System.Collections.Generic;
using System.Text;

namespace ASTLib.Exceptions
{
    public class BoundException : CompilerException
    {
        public BoundException(Node node, string message) : base(node, message)
        {
        }
    }
}
