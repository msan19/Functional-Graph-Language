using System.Collections.Generic;
using ASTLib.Interfaces;

namespace ASTLib.Nodes.ExpressionNodes.SetOperationNodes
{
    public class IntersectionExpression : ExpressionNode, INonIdentifierExpression, IBinarySetOperator
    {
        public IntersectionExpression(ExpressionNode leftExpression, ExpressionNode rightExpression, int line, int letter) 
            : base(new List<ExpressionNode> { leftExpression, rightExpression }, line, letter) {}
    }
}
