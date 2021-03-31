using ASTLib.Nodes;
using ASTLib.Nodes.TypeNodes;

namespace ASTLib.Exceptions
{
    public class AbsoluteValueTypeException : CompilerException
    {
        public AbsoluteValueTypeException(Node node, TypeEnum type) : 
            base(node, $"Cannot take the absolute value of {type}")
        {
        }

    }
}
