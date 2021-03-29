using System.Collections.Generic;
using ASTLib.Interfaces;

namespace ASTLib.Nodes.ExpressionNodes.CommonOperationNodes.RelationalOperationNodes
{
    public class EqualExpression : ExpressionNode, INonIdentifierExpression, IEquivalenceOperator
    {
        public TypeNodes.TypeEnum Type { get; set; }

        public EqualExpression(ExpressionNode leftExpression, ExpressionNode rightExpression, int line, int letter) 
            : base(new List<ExpressionNode> { leftExpression, rightExpression }, line, letter) {}
    }
}
