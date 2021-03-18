using System.Collections.Generic;
using ASTLib.Interfaces;

namespace ASTLib.Nodes.ExpressionNodes.OperationNodes
{
    public class DivisionExpression : ExpressionNode, INonIdentifierExpression, IBinaryNumberOperator
    {
        public DivisionExpression(ExpressionNode devidend, ExpressionNode devisor, int line, int letter) 
            : base(new List<ExpressionNode> { devidend, devisor }, line, letter) {}
    }
}
