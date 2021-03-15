using System;
using ASTLib;

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




    }
}