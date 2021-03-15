using System;
using System.Collections.Generic;
using System.Text;
using ASTLib.Interfaces;

namespace ASTLib.Nodes.ExpressionNodes
{
    public class MultiplicationExpression : ExpressionNode, IBinaryNumberOperator
    {
        public MultiplicationExpression(ExpressionNode leftExpression, ExpressionNode rightExpression, int line, int letter) 
            : base(new List<ExpressionNode> { leftExpression, rightExpression }, line, letter) {}
    }
}
