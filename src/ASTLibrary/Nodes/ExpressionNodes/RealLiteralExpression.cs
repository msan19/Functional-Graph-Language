using System;
using System.Collections.Generic;
using System.Text;

namespace ASTLibrary.Nodes.ExpressionNodes
{
    public class RealLiteralExpression : ExpressionNode
    {
        public double Value { get; set; }

        public RealLiteralExpression(string token) : base(null) 
        {
            //Todo set value
        }
    }
}
