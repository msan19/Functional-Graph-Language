using ASTLib.Interfaces;
using ASTLib.Nodes;
using ASTLib.Nodes.ExpressionNodes;
using System;
using System.Collections.Generic;

namespace ReferenceHandlerLib
{
    public interface IReferenceHelper
    {
        void VisitAnonymousFunction(AnonymousFunctionExpression node, List<string> parameters);
        void SetDispatch(Action<ExpressionNode, List<string>> dispatch);
        void BuildTables(List<FunctionNode> functions);
        void VisitExport(ExportNode node);
        void VisitFunction(FunctionNode node);
        void VisitFunctionCall(FunctionCallExpression node, List<string> identifiers);
        void VisitIdentifier(IdentifierExpression node, List<string> identifiers);
        void VisitNonIdentifier(INonIdentifierExpression node, List<string> identifiers);
        void VisitSet(SetExpression node, List<string> identifiers);
    }
}