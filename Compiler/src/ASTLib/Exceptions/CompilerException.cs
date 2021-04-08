using System;
using ASTLib.Nodes;

namespace ASTLib.Exceptions
{
    public abstract class CompilerException: Exception
    {
        public Node Node { get; }

        public CompilerException(Node node, string message) : base(message)
        {
            Node = node;
        }

    }
}
