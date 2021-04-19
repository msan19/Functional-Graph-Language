using ASTLib.Nodes;
using ASTLib.Nodes.ExpressionNodes;
using ASTLib.Nodes.TypeNodes;

namespace ASTLib.Exceptions.Invalid
{
    public class InvalidSetTypeException : CompilerException
    {
        public InvalidSetTypeException(ExportNode node) : base(node, "Export should be given a set in this iteration")
        {
        }

        public InvalidSetTypeException(ExpressionNode node, TypeEnum type, TypeEnum expected) :
            base(node, GetMessage(type, expected))
        {
        }

        private static string GetMessage(TypeEnum type, TypeEnum expected) 
        {
            return expected switch
            {
                TypeEnum.Integer => $"Expected integer types for bounds but found '{type}'",
                TypeEnum.Boolean => $"Expected a boolean type for predicate but found '{type}'",
                _ => ""
            };

        }

    }
}
