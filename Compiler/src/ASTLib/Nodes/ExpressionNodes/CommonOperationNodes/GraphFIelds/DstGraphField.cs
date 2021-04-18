using System;
using System.Collections.Generic;
using ASTLib.Interfaces;

namespace ASTLib.Nodes.ExpressionNodes.OperationNodes
{
    public class DstGraphField : ExpressionNode, INonIdentifierExpression, ISetGraphField
    {
        public DstGraphField(ExpressionNode graph, int line, int letter) 
            : base(new List<ExpressionNode> { graph }, line, letter) {}

    }
}
