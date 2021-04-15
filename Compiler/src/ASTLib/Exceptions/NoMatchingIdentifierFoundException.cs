using ASTLib.Nodes;

namespace ASTLib.Exceptions
{
    public class NoMatchingIdentifierFoundException : CompilerException
    {
        public NoMatchingIdentifierFoundException(ElementNode node, string elementId) 
            : base(node, $"There was not found a identifier in the parameterlist which matched the ElementNode name {elementId}")
        {
        }
    }
}