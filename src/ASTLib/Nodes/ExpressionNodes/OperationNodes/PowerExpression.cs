using System;
using System.Collections.Generic;
using System.Text;
using ASTLib.Interfaces;

namespace ASTLib.Nodes.ExpressionNodes
{
    public class PowerExpression : ExpressionNode, IBinaryNumberOperator
    {
        public PowerExpression(ExpressionNode baseExpression, ExpressionNode exponent, int line, int letter) 
            : base(new List<ExpressionNode> { baseExpression, exponent }, line, letter) {}
    }
}
