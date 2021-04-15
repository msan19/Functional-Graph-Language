using System.Collections.Generic;
using ASTLib.Interfaces;

namespace ASTLib.Nodes.ExpressionNodes.NumberOperationNodes
{
    public class MultiplicationExpression : ExpressionNode, INonIdentifierExpression, IBinaryNumberOperator
    {
        public MultiplicationExpression(ExpressionNode leftExpression, ExpressionNode rightExpression, int line, int letter) 
            : base(new List<ExpressionNode> { leftExpression, rightExpression }, line, letter) {}
    }
}
