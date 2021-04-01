using System.Collections.Generic;
using ASTLib.Interfaces;

namespace ASTLib.Nodes.ExpressionNodes.CommonOperationNodes.RelationalOperationNodes
{
    public class GreaterEqualExpression : ExpressionNode, INonIdentifierExpression, IRelationOperator
    {
        private ExpressionNode literal;
        private int v1;
        private int v2;

        /*public GreaterEqualExpression(ExpressionNode literal, int v1, int v2)
        {
            this.literal = literal;
            this.v1 = v1;
            this.v2 = v2;
        }*/

        public GreaterEqualExpression(ExpressionNode leftExpression, ExpressionNode rightExpression, int line, int letter) 
            : base(new List<ExpressionNode> { leftExpression, rightExpression }, line, letter) {}
    }
}
