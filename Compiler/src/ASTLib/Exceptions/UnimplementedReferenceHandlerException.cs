using ASTLib.Nodes;

namespace ASTLib.Exceptions
{
    public class UnimplementedReferenceHandlerException : CompilerException
    {
 
        public UnimplementedReferenceHandlerException(Node node) : 
            base(node, $"'{node.GetType()}' is not handled by the ReferenceHandler")
        {
        }

    }
}
