using ASTLib.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace ASTLib.Nodes.ExpressionNodes
{
    public abstract class ExpressionNode : Node, IExpressionNode
    {
        public List<ExpressionNode> Children { get; protected set; }

        public ExpressionNode(List<ExpressionNode> children, int line, int letter) : base(line, letter)
        {
            Children = children;
        }
    }
}
