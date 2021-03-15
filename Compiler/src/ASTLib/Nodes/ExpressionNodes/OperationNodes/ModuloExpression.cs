using ASTLib.Interfaces;
using System;
using System.Collections.Generic;
using ASTLib.Interfaces;

namespace ASTLib.Nodes.ExpressionNodes
{
    public class ModuloExpression : ExpressionNode, INonIdentifierExpression, IBinaryNumberOperator
    {
        public ModuloExpression(ExpressionNode devidend, ExpressionNode devisor, int line, int letter) 
            : base(new List<ExpressionNode> { devidend, devisor }, line, letter) {}
    }
}
