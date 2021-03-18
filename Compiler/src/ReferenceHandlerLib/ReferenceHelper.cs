using ASTLib;
using ASTLib.Nodes;
using ASTLib.Nodes.ExpressionNodes;
using System;
using System.Collections.Generic;

namespace ReferenceHandlerLib
{
    public class ReferenceHelper : IReferenceHelper
    {
        public void VisitExport(ExportNode node)
        {
            throw new NotImplementedException();
        }

        public void VisitFunction(FunctionNode node)
        {
            throw new NotImplementedException();
        }

        private void VisitCondition(ConditionNode node, List<string> identifiers)
        {
            throw new NotImplementedException();
        }

        public void VisitNonIdentifiers(ExpressionNode node, List<string> identifiers)
        {
            throw new NotImplementedException();
        }

        public void VisitIdentifier(IdentifierExpression node, List<string> identifiers)
        {
            throw new NotImplementedException();
        }

        public void VisitFunctionCall(FunctionCallExpression node, List<string> identifiers)
        {
            throw new NotImplementedException();
        }


    }
}