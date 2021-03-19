using System;
using System.Collections.Generic;
using ASTLib;
using ASTLib.Interfaces;
using ASTLib.Nodes;
using ASTLib.Nodes.ExpressionNodes;

namespace ReferenceHandlerLib
{
    public class ReferenceHandler : IReferenceHandler
    {
        private readonly IReferenceHelper _helper;

        public ReferenceHandler(IReferenceHelper helper)
        {
            _helper = helper;
            _helper.ReferenceHandler = this;
        }

        public void InsertReferences(AST root)
        {
            _helper.BuildTable(root.Functions);
            foreach (FunctionNode f in root.Functions) 
                _helper.VisitFunction(f);

            foreach (ExportNode e in root.Exports)
                _helper.VisitExport(e);
        }

        public void Dispatch(ExpressionNode node, List<string> parameters)
        {
            switch(node)
            {
                case IdentifierExpression       e: _helper.VisitIdentifier(e, parameters);      break;
                case INonIdentifierExpression   e: _helper.VisitNonIdentifier(e, parameters);   break;
                case FunctionCallExpression     e: _helper.VisitFunctionCall(e, parameters);    break;

                default: throw new Exception($"{node.GetType()} is not implmented yet in reference handler");
            }
        }


    }
}