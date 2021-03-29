using System.Collections.Generic;
using ASTLib.Interfaces;

namespace ASTLib.Nodes.ExpressionNodes.OperationNodes
{
    public class AdditionExpression : ExpressionNode, INonIdentifierExpression
    {
        public AdditionExpression(ExpressionNode leftExpression, ExpressionNode rightExpression, int line, int letter) 
            : base(new List<ExpressionNode> { leftExpression, rightExpression }, line, letter) {}
    }
}
