using System;
using ASTLib;
using ASTLib.Nodes.ExpressionNodes;
using ASTLib.Nodes.TypeNodes;

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

        public TypeNode Dispatch(ExpressionNode node)
        {
            throw new NotImplementedException();
        }
    }
}