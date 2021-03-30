using ASTLib.Nodes;

namespace ASTLib.Exceptions
{
    public class UnimplementedASTException : CompilerException
    {
 
        public UnimplementedASTException(string caseString, string category) : 
            base(null, $"'{caseString}' is not an accepted {category}")
        {
        }

    }
}
