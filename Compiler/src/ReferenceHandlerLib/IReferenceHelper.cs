using ASTLib.Interfaces;
using ASTLib.Nodes;
using ASTLib.Nodes.ExpressionNodes;
using System.Collections.Generic;

namespace ReferenceHandlerLib
{
    public interface IReferenceHelper
    {
        IReferenceHandler ReferenceHandler { get; set; }

        void BuildTable(List<FunctionNode> functions);
        void VisitExport(ExportNode node);
        void VisitFunction(FunctionNode node);
        void VisitFunctionCall(FunctionCallExpression node, List<string> identifiers);
        void VisitIdentifier(IdentifierExpression node, List<string> identifiers);
        void VisitNonIdentifier(INonIdentifierExpression node, List<string> identifiers);
    }
}