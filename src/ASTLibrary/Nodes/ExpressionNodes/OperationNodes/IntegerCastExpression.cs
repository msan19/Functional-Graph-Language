using System;
using System.Collections.Generic;
using System.Text;

namespace ASTLib.Nodes.ExpressionNodes
{
    public class IntegerCastExpression : ExpressionNode
    {
        public double Value { get; set; }

        public IntegerCastExpression(ExpressionNode child) : base(new List<ExpressionNode> { child }) {}
    }
}
