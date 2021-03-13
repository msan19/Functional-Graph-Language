using System;
using System.Collections.Generic;
using System.Text;

namespace ASTLib.Nodes.ExpressionNodes
{
    public class IntegerCastExpression : ExpressionNode
    {
          public IntegerCastExpression(ExpressionNode child, int line, int letter) : base(new List<ExpressionNode> { child }, line, letter) {}
    }
}
