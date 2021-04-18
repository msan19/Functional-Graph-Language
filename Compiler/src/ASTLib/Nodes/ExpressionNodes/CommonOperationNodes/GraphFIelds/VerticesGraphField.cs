using System;
using System.Collections.Generic;
using ASTLib.Interfaces;

namespace ASTLib.Nodes.ExpressionNodes.OperationNodes
{
    public class VerticesGraphField : ExpressionNode, INonIdentifierExpression, ISetGraphField
    {
        public VerticesGraphField(ExpressionNode graph, int line, int letter) 
            : base(new List<ExpressionNode> { graph }, line, letter) {}

    }
}
