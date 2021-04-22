using System.Collections.Generic;
using ASTLib.Interfaces;

namespace ASTLib.Nodes.ExpressionNodes.CommonOperationNodes.GraphFields
{
    public class SrcGraphField : ExpressionNode, INonIdentifierExpression, IFunctionGraphField
    {
        public SrcGraphField(ExpressionNode graph, int line, int letter) 
            : base(new List<ExpressionNode> { graph }, line, letter) {}

    }
}
