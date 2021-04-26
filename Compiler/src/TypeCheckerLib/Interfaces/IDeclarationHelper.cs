using System;
using System.Collections.Generic;
using ASTLib;
using ASTLib.Nodes;
using ASTLib.Nodes.ExpressionNodes;
using ASTLib.Nodes.TypeNodes;

namespace TypeCheckerLib.Interfaces
{
    public interface IDeclarationHelper : ITypeHelper
    {
        TypeNode VisitAnonymousFunction(AnonymousFunctionExpression node, List<TypeNode> parameterTypes);
        void VisitExport(ExportNode exportNode);
        void VisitFunction(FunctionNode functionNode);
        TypeNode VisitFunctionCall(FunctionCallExpression funcCallExpNode, List<TypeNode> parameterTypes);
        TypeNode VisitIdentifier(IdentifierExpression idExpressionNode, List<TypeNode> parameterTypes);
        TypeNode VisitIntegerLiteral();
        TypeNode VisitRealLiteral();
        TypeNode VisitBooleanLiteral();
        TypeNode VisitStringLiteral();

        TypeNode VisitEmptySetLiteral();
    }
}