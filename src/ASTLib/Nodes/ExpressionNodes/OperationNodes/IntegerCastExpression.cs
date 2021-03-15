using ASTLib.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace ASTLib.Nodes.ExpressionNodes
{
    public class IntegerCastExpression : ExpressionNode, INonIdentifierExpression
    {
          public IntegerCastExpression(ExpressionNode child, int line, int letter) : base(new List<ExpressionNode> { child }, line, letter) {}
    }
}
