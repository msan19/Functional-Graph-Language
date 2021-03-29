using ASTLib.Nodes;

namespace ASTLib.Exceptions
{
    public class UnimplementedTypeException : CompilerException
    {
 
        public UnimplementedTypeException(string type) : 
            base(null, $"'{type}' is not an accepted type")
        {
        }

    }
}
