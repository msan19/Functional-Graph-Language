using System.Collections.Generic;
using ASTLib.Interfaces;

namespace ASTLib.Nodes.ExpressionNodes.CommonOperationNodes.RelationalOperationNodes
{
    public class LessEqualExpression : ExpressionNode, INonIdentifierExpression, IRelationOperator
    {
        public LessEqualExpression(ExpressionNode leftExpression, ExpressionNode rightExpression, int line, int letter) 
            : base(new List<ExpressionNode> { leftExpression, rightExpression }, line, letter) {}
    }
}
