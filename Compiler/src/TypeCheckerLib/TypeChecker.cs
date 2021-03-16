using System;
using ASTLib;
using ASTLib.Nodes.ExpressionNodes;
using ASTLib.Nodes.TypeNodes;

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

        public TypeNode Dispatch(ExpressionNode node)
        {
            throw new NotImplementedException();
        }
    }
}