using ASTLib.Nodes;
using ASTLib.Nodes.TypeNodes;

namespace ASTLib.Exceptions.Invalid
{
    public class InvalidCastException : CompilerException
    {
        public TypeEnum ReturnType { get; private set; }
        public TypeEnum ExpectedType { get; private set; }

        public InvalidCastException(Node node, TypeEnum returnType, TypeEnum expectedType) :
            base(node, GetMessage(node, returnType, expectedType))
        {
            ReturnType = returnType;
            ExpectedType = expectedType;
        }

        private static string GetMessage(Node node, TypeEnum returnType, TypeEnum expectedType)
        {
            string message = $"Cannot cast { returnType } to { expectedType }";
            return message;
        }
    }
}
