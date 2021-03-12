using System;
using System.Collections.Generic;
using System.Text;

namespace ASTLib.Nodes.ExpressionNodes
{
    public class MultiplicationExpression : ExpressionNode
    {
        public MultiplicationExpression(ExpressionNode leftExpression, ExpressionNode rightExpression) 
            : base(new List<ExpressionNode> { leftExpression, rightExpression }) {}
    }
}
