using System.Collections.Generic;
using ASTLib.Interfaces;

namespace ASTLib.Nodes.ExpressionNodes.CommonOperationNodes.GraphFields
{
    public class VerticesGraphField : ExpressionNode, INonIdentifierExpression, ISetGraphField
    {
        public VerticesGraphField(ExpressionNode graph, int line, int letter) 
            : base(new List<ExpressionNode> { graph }, line, letter) {}

    }
}
