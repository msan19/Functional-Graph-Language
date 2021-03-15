using System;
using System.Collections.Generic;
using ASTLib;
using ASTLib.Nodes.ExpressionNodes;

namespace ReferenceHandlerLib
{
    public class ReferenceHandler
    {
        private readonly ReferenceHelper _helper;

        public ReferenceHandler(ReferenceHelper helper)
        {
            _helper = helper;
            _helper.Handler = this;
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