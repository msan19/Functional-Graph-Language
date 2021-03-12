using System;
using System.Collections.Generic;
using System.Text;

namespace ASTLib.Nodes.ExpressionNodes
{
    public class IntegerLiteralExpression : ExpressionNode
    {
        public int Value { get; set; }

        public IntegerLiteralExpression(string token) : base(null) 
        {
            //Todo set value
        }
    }
}
