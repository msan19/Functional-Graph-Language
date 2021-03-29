using System;
using ASTLib.Nodes;

namespace ASTLib.Exceptions
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
