using ASTLib.Nodes.ExpressionNodes;
using System.Collections.Generic;

namespace ASTLib.Interfaces
{
    public interface IExpressionNode
    {
        List<ExpressionNode> Children { get; }
    }
}