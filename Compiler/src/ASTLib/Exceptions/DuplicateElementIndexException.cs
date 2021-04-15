using ASTLib.Nodes;

namespace ASTLib.Exceptions
{
    public class DuplicateElementIndexException : CompilerException
    {
        public DuplicateElementIndexException(ElementNode node, string indexId) 
            : base(node, $"The index identifier {indexId} appeared several times in the condition.")
        {
        }
    }
}