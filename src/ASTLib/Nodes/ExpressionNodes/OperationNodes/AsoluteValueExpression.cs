using System;
using System.Collections.Generic;
using System.Text;

namespace ASTLib.Nodes.ExpressionNodes
{
    public class AbsoluteValueExpression : ExpressionNode
    {
          public AbsoluteValueExpression(ExpressionNode child) : base(new List<ExpressionNode> { child }) {}
    }
}
