using System;
using System.Collections.Generic;
using System.Text;

namespace ASTLib.Nodes.ExpressionNodes
{
    public class PowerExpression : ExpressionNode
    {
        public PowerExpression(ExpressionNode baseExpression, ExpressionNode exponent) 
            : base(new List<ExpressionNode> { baseExpression, exponent }) {}
    }
}
