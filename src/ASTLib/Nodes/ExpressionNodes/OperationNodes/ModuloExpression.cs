using System;
using System.Collections.Generic;
using System.Text;

namespace ASTLib.Nodes.ExpressionNodes
{
    public class ModuloExpression : ExpressionNode
    {
        public ModuloExpression(ExpressionNode devidend, ExpressionNode devisor) 
            : base(new List<ExpressionNode> { devidend, devisor }) {}
    }
}
