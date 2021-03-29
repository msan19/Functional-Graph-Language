using ASTLib.Nodes;

namespace ASTLib.Exceptions
{
    public class FunctionIdentifierMatchException : CompilerException
    {
 
        public FunctionIdentifierMatchException(string funcID, string typeID, Node node) : 
            base(node, $"{typeID} and {funcID} should be equivalent")
        {
        }

    }
}
