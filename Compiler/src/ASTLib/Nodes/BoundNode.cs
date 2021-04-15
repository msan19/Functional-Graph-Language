using ASTLib.Nodes.ExpressionNodes;
using System;
using System.Collections.Generic;
using System.Text;

namespace ASTLib.Nodes
{
    public class BoundNode: Node
    {
        public ExpressionNode MinValue { get; }
        public ExpressionNode MaxValue { get; }
        public string Identifier { get; }

        public BoundNode(string identifier, ExpressionNode smallestValue, ExpressionNode largestValue, int line, int letter) : base(line, letter)
        {
            Identifier = identifier;
            MinValue = smallestValue;
            MaxValue = largestValue;
        }
    }
}
