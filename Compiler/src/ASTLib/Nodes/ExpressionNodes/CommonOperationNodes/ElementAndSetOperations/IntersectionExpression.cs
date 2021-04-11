using System.Collections.Generic;
using ASTLib.Interfaces;

namespace ASTLib.Nodes.ExpressionNodes.CommonOperationNodes.ElementAndSetOperations
{
    public class IntersectionExpression : ExpressionNode, INonIdentifierExpression
    {
        public IntersectionExpression(ExpressionNode leftExpression, ExpressionNode rightExpression, int line, int letter) 
            : base(new List<ExpressionNode> { leftExpression, rightExpression }, line, letter) {}
    }
}
