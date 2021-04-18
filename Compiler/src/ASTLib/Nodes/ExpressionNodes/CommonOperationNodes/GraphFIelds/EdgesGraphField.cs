using System;
using System.Collections.Generic;
using ASTLib.Interfaces;

namespace ASTLib.Nodes.ExpressionNodes.OperationNodes
{
    public class EdgesGraphField : ExpressionNode, INonIdentifierExpression, ISetGraphField
    {
        public EdgesGraphField(ExpressionNode graph, int line, int letter) 
            : base(new List<ExpressionNode> { graph }, line, letter) {}

    }
}
