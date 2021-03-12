using System;
using System.Collections.Generic;
using System.Text;

namespace ASTLib.Nodes.ExpressionNodes
{
    public abstract class ExpressionNode: Node
    {
        public List<ExpressionNode> Children { get; private set; }
        

        public ExpressionNode(List<ExpressionNode> children)
        {
            Children = children;
        }
    }
}
