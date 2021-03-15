using ASTLib.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace ASTLib.Nodes.ExpressionNodes
{
    public class AbsoluteValueExpression : ExpressionNode, INonIdentifierExpression
    {
          public AbsoluteValueExpression(ExpressionNode child, int line, int letter) 
            : base(new List<ExpressionNode> { child }, line, letter) {}
    }
}
