using ASTLib.Nodes;

namespace ASTLib.Exceptions
{
    public class UnimplementedInterpreterException : CompilerException
    {
 
        public UnimplementedInterpreterException(Node node, string dispatcher) : 
            base(node, $"'{node.GetType()}' is not handled by the {dispatcher} function in the interpreter")
        {
        }

    }
}
