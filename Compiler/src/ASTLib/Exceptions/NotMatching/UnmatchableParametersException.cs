using ASTLib.Nodes;

namespace ASTLib.Exceptions.NotMatching
{
    public class UnmatchableParametersException : CompilerException
    {
        public UnmatchableParametersException(FunctionNode node) 
            : base(node, $"The number of parameters (count: {node.ParameterIdentifiers.Count}) for function " +
                         $"{node.Identifier} did not match the number of type parameters (count: {node.FunctionType.ParameterTypes.Count})." )
        {
        }
    }
}