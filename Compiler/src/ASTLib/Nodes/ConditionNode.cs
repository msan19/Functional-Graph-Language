using ASTLib.Nodes.ExpressionNodes;
using System;
using System.Collections.Generic;
using System.Text;

namespace ASTLib.Nodes
{
    public class ConditionNode : Node
    {
        public ExpressionNode Condition { get; }
        public ExpressionNode ReturnExpression { get; set; }
        public List<ElementNode> Elements { get; }

        public ConditionNode(ExpressionNode returnExpression, int line, int letter) : base(line, letter)
        {
            ReturnExpression = returnExpression;
        }
    }
}
