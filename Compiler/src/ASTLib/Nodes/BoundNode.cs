﻿using ASTLib.Nodes.ExpressionNodes;
using System;
using System.Collections.Generic;
using System.Text;

namespace ASTLib.Nodes
{
    public class BoundNode: Node
    {
        public ExpressionNode SmallestValue { get; }
        public ExpressionNode LargestValue { get; }
        public string Identifier { get; }

        public BoundNode(string identifier, ExpressionNode smallesValue, ExpressionNode largestValue, int line, int letter) : base(line, letter)
        {
            Identifier = identifier;
            SmallestValue = smallesValue;
            LargestValue = largestValue;
        }
    }
}
