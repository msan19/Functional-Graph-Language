using ASTLib.Interfaces;
using ASTLib.Nodes.TypeNodes;
using ASTLib.Objects;
using System;
using System.Collections.Generic;
using System.Text;

namespace ASTLib.Nodes.ExpressionNodes
{
    public class EmptySetLiteralExpression : ExpressionNode, INonIdentifierExpression
    {
        public Set Value { get; }

        public EmptySetLiteralExpression(int line, int letter) : base(new List<ExpressionNode>(), line, letter) 
        {
            Value = new Set();
        }
    }
}
