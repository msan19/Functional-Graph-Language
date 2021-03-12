using System;
using System.Collections.Generic;
using System.Text;

namespace ASTLib.Nodes.ExpressionNodes
{
    public class IntegerCastExpression : ExpressionNode
    {
          public IntegerCastExpression(ExpressionNode child) : base(new List<ExpressionNode> { child }) {}
    }
}
