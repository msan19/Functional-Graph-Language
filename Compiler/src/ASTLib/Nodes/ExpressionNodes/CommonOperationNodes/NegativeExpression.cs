using System.Collections.Generic;
using ASTLib.Interfaces;
using ASTLib.Nodes.TypeNodes;

namespace ASTLib.Nodes.ExpressionNodes.CommonOperationNodes
{
    public class NegativeExpression : ExpressionNode, INonIdentifierExpression
    {
        public NegativeExpression(List<ExpressionNode> children, int line, int letter) : base(children, line, letter)
        {
        }
    }
}