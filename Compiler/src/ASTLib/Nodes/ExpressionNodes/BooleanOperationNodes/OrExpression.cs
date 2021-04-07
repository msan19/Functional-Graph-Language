using System.Collections.Generic;
using ASTLib.Interfaces;

namespace ASTLib.Nodes.ExpressionNodes.BooleanOperationNodes
{
    public class OrExpression : ExpressionNode, INonIdentifierExpression, IBooleanOperator, IBinaryBooleanOperator
    {
        public OrExpression(ExpressionNode leftExpression, ExpressionNode rightExpression, int line, int letter) 
            : base(new List<ExpressionNode> { leftExpression, rightExpression }, line, letter) {}
    }
}
