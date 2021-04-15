using System.Collections.Generic;
using ASTLib.Interfaces;

namespace ASTLib.Nodes.ExpressionNodes.NumberOperationNodes
{
    public class DivisionExpression : ExpressionNode, INonIdentifierExpression, IBinaryNumberOperator
    {
        public DivisionExpression(ExpressionNode dividend, ExpressionNode divisor, int line, int letter) 
            : base(new List<ExpressionNode> { dividend, divisor }, line, letter) {}
    }
}
