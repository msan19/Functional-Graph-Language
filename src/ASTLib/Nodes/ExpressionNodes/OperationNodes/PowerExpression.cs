using System;
using System.Collections.Generic;
using System.Text;

namespace ASTLib.Nodes.ExpressionNodes
{
    public class PowerExpression : ExpressionNode
    {
        public PowerExpression(ExpressionNode baseExpression, ExpressionNode exponent, int line, int letter) 
            : base(new List<ExpressionNode> { baseExpression, exponent }, line, letter) {}
    }
}
