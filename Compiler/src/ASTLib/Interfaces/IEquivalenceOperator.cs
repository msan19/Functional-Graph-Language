using ASTLib.Nodes.TypeNodes;

namespace ASTLib.Interfaces
{
    public interface IEquivalenceOperator : IExpressionNode
    {
        TypeEnum Type { get; set; }
    }
}