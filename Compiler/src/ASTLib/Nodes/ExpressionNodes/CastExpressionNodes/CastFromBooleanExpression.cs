using ASTLib.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace ASTLib.Nodes.ExpressionNodes.CastExpressionNodes
{
    public class CastFromBooleanExpression : ExpressionNode, INonIdentifierExpression
    {
        public CastFromBooleanExpression(ExpressionNode child, int line, int letter) : base(new List<ExpressionNode> { child }, line, letter) { }
    }
}
