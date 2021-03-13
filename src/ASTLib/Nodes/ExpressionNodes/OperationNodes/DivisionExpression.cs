using System;
using System.Collections.Generic;
using System.Text;

namespace ASTLib.Nodes.ExpressionNodes
{
    public class DivisionExpression : ExpressionNode
    {
        public DivisionExpression(ExpressionNode devidend, ExpressionNode devisor, int line, int letter) 
            : base(new List<ExpressionNode> { devidend, devisor }, line, letter) {}
    }
}
