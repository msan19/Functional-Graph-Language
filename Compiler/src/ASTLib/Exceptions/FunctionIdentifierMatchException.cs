using ASTLib.Nodes;

namespace ASTLib.Exceptions
{
    public class FunctionIdentifierMatchException : CompilerException
    {
 
        public FunctionIdentifierMatchException(string funcID, string typeID) : 
            base(null, $"{typeID} and {funcID} should be equivalent")
        {
        }

    }
}
