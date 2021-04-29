using System.Collections.Generic;
using ASTLib.Nodes;
using ASTLib.Nodes.ExpressionNodes;
using ASTLib.Nodes.TypeNodes;
using Hime.Redist;

namespace LexParserLib.ASTBuilding
{
    public interface IExpressionHelper
    {
        FunctionTypeNode CreateFunctionTypeNode(ASTNode himeNode);
        void VisitExpressions(ASTNode himeNode, List<ExpressionNode> expressions);
        ExpressionNode DispatchExpression(ASTNode himeNode);
        ElementNode GetElementNode(ASTNode himeNode);
        List<string> VisitIdentifiers(ASTNode himeNode);
    }
}