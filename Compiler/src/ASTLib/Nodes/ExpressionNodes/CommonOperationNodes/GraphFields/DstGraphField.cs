using System.Collections.Generic;
using ASTLib.Interfaces;

namespace ASTLib.Nodes.ExpressionNodes.CommonOperationNodes.GraphFields
{
    public class DstGraphField : ExpressionNode, INonIdentifierExpression, IFunctionGraphField
    {
        public DstGraphField(ExpressionNode graph, int line, int letter)
            : base(new List<ExpressionNode> { graph }, line, letter) { }

    }
}
