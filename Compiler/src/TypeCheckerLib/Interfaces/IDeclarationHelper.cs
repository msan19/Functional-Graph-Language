using System;
using System.Collections.Generic;
using ASTLib;
using ASTLib.Nodes;
using ASTLib.Nodes.ExpressionNodes;
using ASTLib.Nodes.TypeNodes;

namespace TypeCheckerLib.Interfaces
{
    public interface IDeclarationHelper
    {
        void Initialize(AST root, Func<ExpressionNode, List<TypeNode>, TypeNode> dispatcher);
        void VisitExport(ExportNode exportNode);
        void VisitFunction(FunctionNode functionNode);
        TypeNode VisitFunctionCall(FunctionCallExpression funcCallExpNode, List<TypeNode> parameterTypes);
        TypeNode VisitIdentifier(IdentifierExpression idExpressionNode, List<TypeNode> parameterTypes);
        TypeNode VisitIntegerLiteral(IntegerLiteralExpression intLiteralExpressionNode, List<TypeNode> parameterTypes);
        TypeNode VisitRealLiteral(RealLiteralExpression realLiteralExpressionNode, List<TypeNode> parameterTypes);
    }
}