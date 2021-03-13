using System;
using System.Collections.Generic;
using System.Text;

namespace ASTLib.Nodes.ExpressionNodes
{
    public class AbsoluteValueExpression : ExpressionNode
    {
          public AbsoluteValueExpression(ExpressionNode child, int line, int letter) 
            : base(new List<ExpressionNode> { child }, line, letter) {}
    }
}
