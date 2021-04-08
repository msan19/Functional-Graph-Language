using ASTLib.Nodes.TypeNodes;

namespace ASTLib.Interfaces
{
    public interface IRelationOperator : IExpressionNode
    {
        TypeEnum Type { get; set; }
    }
}