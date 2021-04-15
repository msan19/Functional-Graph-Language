using ASTLib.Nodes;

namespace ASTLib.Exceptions
{
    public class InvalidAbsoluteIntegerException : CompilerException
    {
        public InvalidAbsoluteIntegerException(Node node) :
            base(node, $"'{node}' should be of type integer, but is of type {node.GetType()}")
        {
        }
    }
}