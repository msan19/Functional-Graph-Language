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
        public FunctionTypeNode Type { get; set; }

        public BooleanLiteralExpression(bool value, int line, int letter) : base(null, line, letter) 
        {
            Value = value;
            Children = new List<ExpressionNode>();
        }
    }
}
