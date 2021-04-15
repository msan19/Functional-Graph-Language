using System.Collections.Generic;
using ASTLib.Interfaces;

namespace ASTLib.Nodes.ExpressionNodes.NumberOperationNodes
{
    public class NegativeExpression : ExpressionNode, INonIdentifierExpression
    {
        public NegativeExpression(List<ExpressionNode> children, int line, int letter) : base(children, line, letter)
        {
        }
    }
}