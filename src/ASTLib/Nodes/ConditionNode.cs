using ASTLib.Nodes.ExpressionNodes;
using System;
using System.Collections.Generic;
using System.Text;

namespace ASTLib.Nodes
{
    public abstract class ConditionNode: Node
    {
        public ExpressionNode Condition { get; private set; }
        public ExpressionNode ReturnExpression { get; private set; }
        public List<ElementNode> Elements { get; private set; }

        public ConditionNode(ExpressionNode returnExpression)
        {
            ReturnExpression = returnExpression;
        }
    }
}
