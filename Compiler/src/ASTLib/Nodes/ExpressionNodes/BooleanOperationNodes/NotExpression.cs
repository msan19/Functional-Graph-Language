using System.Collections.Generic;
using ASTLib.Interfaces;

namespace ASTLib.Nodes.ExpressionNodes.BooleanOperationNodes
{
    public class NotExpression : ExpressionNode, INonIdentifierExpression, IBooleanOperator
    {
        public NotExpression(ExpressionNode expression, int line, int letter) 
            : base(new List<ExpressionNode> { expression }, line, letter) {}
    }
}
