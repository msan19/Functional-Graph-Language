using ASTLib.Nodes;

namespace ASTLib.Exceptions
{
    public class UnimplementedTypeCheckerException : CompilerException
    {
 
        public UnimplementedTypeCheckerException(Node node, string dispatcher) : 
            base(node, $"'{node.GetType()}' is not handled by the {dispatcher} function in the TypeChecker")
        {
        }

    }
}