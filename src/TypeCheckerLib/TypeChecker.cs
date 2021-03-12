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
        }

        public void CheckTypes(AST root)
        {
            _helper.CheckTypes(root);
        }
    }
}