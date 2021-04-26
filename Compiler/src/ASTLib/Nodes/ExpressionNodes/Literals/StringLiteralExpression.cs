using ASTLib.Interfaces;
using ASTLib.Nodes.TypeNodes;
using System;
using System.Collections.Generic;
using System.Text;

namespace ASTLib.Nodes.ExpressionNodes
{
    public class StringLiteralExpression : ExpressionNode, INonIdentifierExpression
    {
        public string Value { get; }

        public StringLiteralExpression(string value, int line, int letter) : base(new List<ExpressionNode>(), line, letter) 
        {
            Value = value;
        }

    }
}
