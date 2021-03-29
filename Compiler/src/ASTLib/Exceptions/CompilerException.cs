using ASTLib.Nodes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Main.Exceptions
{
    public abstract class CompilerException: Exception
    {
        public Node _node { get; }

        public CompilerException(Node node, string message) : base(message)
        {
            _node = node;
        }

    }
}
