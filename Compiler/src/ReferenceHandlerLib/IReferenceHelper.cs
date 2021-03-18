using ASTLib.Nodes;
using ASTLib.Nodes.ExpressionNodes;
using System.Collections.Generic;

namespace ReferenceHandlerLib
{
    public interface IReferenceHelper
    {
        ReferenceHandler Handler { get; set; }

        void VisitExport(ExportNode node);
        void VisitFunction(FunctionNode node);
        void VisitFunctionCall(FunctionCallExpression node, List<string> identifiers);
        void VisitIdentifier(IdentifierExpression node, List<string> identifiers);
        void VisitNonIdentifiers(ExpressionNode node, List<string> identifiers);
    }
}