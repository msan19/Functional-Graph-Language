using ASTLib.Nodes;
using ASTLib.Nodes.ExpressionNodes;
using Hime.Redist;
using System.Collections.Generic;

namespace LexParserLib
{
    public interface IExpressionHelper
    {
        ExpressionNode DispatchExpression(ASTNode himeNode);
        ElementNode GetElementNode(ASTNode himeNode);
        List<string> VisitIdentifiers(ASTNode himeNode);
    }
}