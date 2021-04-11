using System.Collections.Generic;
using ASTLib.Interfaces;

namespace ASTLib.Nodes.ExpressionNodes.CommonOperationNodes.ElementAndSetOperations
{
    public class SubsetExpression : ExpressionNode, INonIdentifierExpression
    {
        public SubsetExpression(ExpressionNode leftExpression, ExpressionNode rightExpression, int line, int letter) 
            : base(new List<ExpressionNode> { leftExpression, rightExpression }, line, letter) {}
    }
}
