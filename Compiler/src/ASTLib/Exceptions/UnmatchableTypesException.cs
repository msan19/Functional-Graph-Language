using ASTLib.Nodes;
using ASTLib.Nodes.TypeNodes;

namespace ASTLib.Exceptions
{
    public class UnmatchableTypesException : CompilerException
    {
 
        public UnmatchableTypesException(Node node, TypeEnum left, TypeEnum right, string expected) : 
            base(node, $"Cannot match the types '{left}' and '{right}' to a {expected}")
        {
        }

    }
}
