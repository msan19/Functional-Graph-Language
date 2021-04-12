using System;
using System.Collections.Generic;
using System.Text;
using ASTLib;
using ASTLib.Nodes;
using ASTLib.Nodes.ExpressionNodes;
using ASTLib.Nodes.TypeNodes;
using ASTLib.Interfaces;
using TypeCheckerLib.Interfaces;
using ASTLib.Exceptions;

namespace TypeCheckerLib.Helpers
{
    public class SetHelper : ISetHelper
    {
        private List<FunctionNode> _functions;
        private Func<ExpressionNode, List<TypeNode>, TypeNode> _getType;

        public void Initialize(AST root, Func<ExpressionNode, List<TypeNode>, TypeNode> dispatcher)
        {
            _functions = root.Functions;
            _getType = dispatcher;
        }

        public TypeNode VisitBinarySetOp(IBinarySetOperator binaryNode, List<TypeNode> parameterTypes)
        {
            TypeNode left = _getType(binaryNode.Children[0], parameterTypes);
            TypeNode right = _getType(binaryNode.Children[1], parameterTypes);

            if (!IsSet(left.Type) || !IsSet(right.Type))
            {
                throw new UnmatchableTypesException((Node)binaryNode, left.Type, right.Type, "bool");
            }
            return new TypeNode(TypeEnum.Set, 0, 0);
        }
        private bool IsSet(TypeEnum t)
        {
            return t == TypeEnum.Set;
        }
    }
}
