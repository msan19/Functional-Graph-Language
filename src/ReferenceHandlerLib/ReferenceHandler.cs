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
        }

        public void InsertReferences(AST root)
        {
            _helper.InsertReferences(root);
        }
    }
}