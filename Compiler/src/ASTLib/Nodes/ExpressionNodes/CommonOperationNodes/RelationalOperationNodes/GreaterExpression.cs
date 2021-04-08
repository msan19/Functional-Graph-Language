using System.Collections.Generic;
using ASTLib.Interfaces;
using ASTLib.Nodes.TypeNodes;

namespace ASTLib.Nodes.ExpressionNodes.CommonOperationNodes.RelationalOperationNodes
{
    public class GreaterExpression : ExpressionNode, INonIdentifierExpression, IRelationOperator
    {
        public GreaterExpression(ExpressionNode leftExpression, ExpressionNode rightExpression, int line, int letter) 
            : base(new List<ExpressionNode> { leftExpression, rightExpression }, line, letter) {}

        public TypeEnum Type { get; set; }
    }
}
