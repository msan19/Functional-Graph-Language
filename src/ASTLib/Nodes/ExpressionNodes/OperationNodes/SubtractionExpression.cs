using System;
using System.Collections.Generic;
using System.Text;

namespace ASTLib.Nodes.ExpressionNodes
{
    public class SubstractionExpression : ExpressionNode
    {
        public SubstractionExpression(ExpressionNode leftExpression, ExpressionNode rightExpression) 
            : base(new List<ExpressionNode> { leftExpression, rightExpression }) {}
    }
}
