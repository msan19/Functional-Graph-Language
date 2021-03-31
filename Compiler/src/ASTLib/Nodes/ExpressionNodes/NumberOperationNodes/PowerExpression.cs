using System.Collections.Generic;
using ASTLib.Interfaces;

namespace ASTLib.Nodes.ExpressionNodes.OperationNodes
{
    public class PowerExpression : ExpressionNode, INonIdentifierExpression
    {
        public PowerExpression(ExpressionNode baseExpression, ExpressionNode exponent, int line, int letter) 
            : base(new List<ExpressionNode> { baseExpression, exponent }, line, letter) {}
    }
}
