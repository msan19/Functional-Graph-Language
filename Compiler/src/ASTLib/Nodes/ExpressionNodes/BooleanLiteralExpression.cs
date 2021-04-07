using ASTLib.Interfaces;
using ASTLib.Nodes.TypeNodes;
using System;
using System.Collections.Generic;
using System.Text;

namespace ASTLib.Nodes.ExpressionNodes
{
    public class BooleanLiteralExpression : ExpressionNode, INonIdentifierExpression
    {
        public bool Value { get; }

        public BooleanLiteralExpression(bool value, int line, int letter) : base(new List<ExpressionNode>(), line, letter) 
        {
            Value = value;
        }
    }
}
