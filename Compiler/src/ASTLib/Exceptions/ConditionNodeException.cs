using ASTLib.Nodes;

namespace ASTLib.Exceptions
{
    public class ConditionNodeException : CompilerException
    {
        public ConditionNodeException(ConditionNode node) 
            : base(node, $"This condition evaluated to true.")
        {
        }
    }
}