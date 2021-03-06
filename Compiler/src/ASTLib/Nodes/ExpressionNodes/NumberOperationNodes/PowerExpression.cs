using System.Collections.Generic;
using ASTLib.Interfaces;

namespace ASTLib.Nodes.ExpressionNodes.NumberOperationNodes
{
    public class PowerExpression : ExpressionNode, INonIdentifierExpression, IBinaryNumberOperator
    {
        public PowerExpression(ExpressionNode baseExpression, ExpressionNode exponent, int line, int letter) 
            : base(new List<ExpressionNode> { baseExpression, exponent }, line, letter) {}
    }
}
