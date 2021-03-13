using System;
using System.Collections.Generic;
using System.Text;

namespace ASTLib.Nodes.ExpressionNodes
{
    public abstract class ExpressionNode : Node
    {
        public List<ExpressionNode> Children { get; }

        public ExpressionNode(List<ExpressionNode> children, int line, int letter) : base(line, letter)
        {
            Children = children;
        }
    }
}
