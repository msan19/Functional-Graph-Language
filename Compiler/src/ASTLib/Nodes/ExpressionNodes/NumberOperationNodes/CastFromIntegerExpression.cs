using System.Collections.Generic;
using ASTLib.Interfaces;

namespace ASTLib.Nodes.ExpressionNodes.NumberOperationNodes
{
    public class CastFromIntegerExpression : ExpressionNode, INonIdentifierExpression
    {
          public CastFromIntegerExpression(ExpressionNode child, int line, int letter) : base(new List<ExpressionNode> { child }, line, letter) {}
    }
}
