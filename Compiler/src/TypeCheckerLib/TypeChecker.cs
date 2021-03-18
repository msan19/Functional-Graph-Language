using System;
using ASTLib;

namespace TypeCheckerLib
{
    public class TypeChecker : ITypeChecker
    {
        private readonly ITypeHelper _helper;

        public TypeChecker(ITypeHelper helper)
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