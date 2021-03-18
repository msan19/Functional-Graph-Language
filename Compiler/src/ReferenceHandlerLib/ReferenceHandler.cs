using System;
using System.Collections.Generic;
using ASTLib;
using ASTLib.Nodes.ExpressionNodes;

namespace ReferenceHandlerLib
{
    public class ReferenceHandler : IReferenceHandler
    {
        private readonly IReferenceHelper _helper;

        public ReferenceHandler(IReferenceHelper helper)
        {
            _helper = helper;
        }

        public void InsertReferences(AST root)
        {
            //_helper.Visit(root.Functions);
        }

        public void Dispatch(ExpressionNode node, List<string> parameters)
        {
            throw new NotImplementedException();
        }


    }
}