using System.Collections.Generic;
using ASTLib.Interfaces;

namespace ASTLib.Nodes.ExpressionNodes.BooleanOperationNodes
{
    public class AndExpression : ExpressionNode, INonIdentifierExpression, IBooleanOperator, IBinaryBooleanOperator
    {
        public AndExpression(ExpressionNode leftExpression, ExpressionNode rightExpression, int line, int letter) 
            : base(new List<ExpressionNode> { leftExpression, rightExpression }, line, letter) {}
    }
}
