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
        public List<ElementNode> Elements { get; } = new List<ElementNode>();

        public ConditionNode(ExpressionNode returnExpression, int line, int letter) : base(line, letter)
        {
            ReturnExpression = returnExpression;
        }

        public ConditionNode(ExpressionNode conditionExpression, ExpressionNode returnExpression, int line, int letter) : base(line, letter)
        {
            ReturnExpression = returnExpression;
            Condition = conditionExpression;
        }

        public ConditionNode(List<ElementNode> elements, ExpressionNode conditionExpression, ExpressionNode returnExpression, int line, int letter) : base(line, letter)
        {
            ReturnExpression = returnExpression;
            Condition = conditionExpression;
            Elements = elements;
        }

        public bool IsDefaultCase => Elements.Count == 0 && Condition == null;
    }
}
