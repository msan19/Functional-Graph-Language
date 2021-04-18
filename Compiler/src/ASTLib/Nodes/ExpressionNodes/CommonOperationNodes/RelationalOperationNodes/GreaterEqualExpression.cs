using System.Collections.Generic;
using ASTLib.Interfaces;
using ASTLib.Nodes.TypeNodes;

namespace ASTLib.Nodes.ExpressionNodes.CommonOperationNodes.RelationalOperationNodes
{
    public class GreaterEqualExpression : ExpressionNode, INonIdentifierExpression, IRelationOperator
    {
        public GreaterEqualExpression(ExpressionNode leftExpression, ExpressionNode rightExpression, int line, int letter) 
            : base(new List<ExpressionNode> { leftExpression, rightExpression }, line, letter) {}

    }
}
