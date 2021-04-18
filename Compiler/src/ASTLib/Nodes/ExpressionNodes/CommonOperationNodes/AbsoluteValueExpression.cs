using System;
using System.Collections.Generic;
using ASTLib.Interfaces;

namespace ASTLib.Nodes.ExpressionNodes.OperationNodes
{
    public class AbsoluteValueExpression : ExpressionNode, INonIdentifierExpression
    {
        public TypeNodes.TypeEnum Type { get; set; }

        public AbsoluteValueExpression(ExpressionNode child, int line, int letter) 
            : base(new List<ExpressionNode> { child }, line, letter) {}

    }
}
