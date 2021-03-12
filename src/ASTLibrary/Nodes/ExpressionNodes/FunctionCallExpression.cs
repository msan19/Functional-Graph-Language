using System;
using System.Collections.Generic;
using System.Text;

namespace ASTLibrary.Nodes.ExpressionNodes
{
    public class FunctionCallExpression : ExpressionNode
    {
        public List<int> references { get; set; }

        public FunctionCallExpression(List<ExpressionNode> children) : base(children) {}
    }
}
