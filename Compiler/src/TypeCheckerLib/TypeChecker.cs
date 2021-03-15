using System;
using ASTLib;

namespace TypeCheckerLib
{
    public class TypeChecker
    {
        private readonly TypeHelper _helper;

        public TypeChecker(TypeHelper helper)
        {
            _helper = helper;
            _helper.TypeChecker = this;
        }

        public void CheckTypes(AST root)
        {
            _helper.CheckTypes(root);
        }

        public void Dispatch()
        {
            // Call visit methods in a switch
        }
    }
}