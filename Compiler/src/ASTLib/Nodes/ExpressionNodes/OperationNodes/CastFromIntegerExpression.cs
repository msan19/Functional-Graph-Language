using ASTLib.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace ASTLib.Nodes.ExpressionNodes
{
    public class CastFromIntegerExpression : ExpressionNode, INonIdentifierExpression
    {
          public CastFromIntegerExpression(ExpressionNode child, int line, int letter) : base(new List<ExpressionNode> { child }, line, letter) {}
    }
}
