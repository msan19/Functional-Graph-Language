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
<<<<<<< HEAD
            SmallestValue = smallestValue;
            LargestValue = largestValue;
=======
            MinValue = smallesValue;
            MaxValue = largestValue;
>>>>>>> 3917c3f5a47dd5fc44b21065fd69d2f6ff43d882
        }
    }
}
