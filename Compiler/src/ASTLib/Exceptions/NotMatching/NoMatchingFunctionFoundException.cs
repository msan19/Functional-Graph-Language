using ASTLib.Nodes.ExpressionNodes;

namespace ASTLib.Exceptions.NotMatching
{
    public class NoMatchingFunctionFoundException : CompilerException
    {
        public NoMatchingFunctionFoundException(FunctionCallExpression node) :
            base(node, GetMessage(node))
        {
        }
        private static string GetMessage(FunctionCallExpression node)
        {
            string message =
                $"No function matching " +
                $"the name { node.Identifier } was found.";
            return message;
        }
    }
}
