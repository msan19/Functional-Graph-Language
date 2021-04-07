using System;
using System.Collections.Generic;
using ASTLib;
using ASTLib.Exceptions;
using ASTLib.Nodes;
using ASTLib.Nodes.ExpressionNodes;
using ASTLib.Nodes.ExpressionNodes.BooleanOperationNodes;
using ASTLib.Nodes.TypeNodes;
using TypeCheckerLib.Interfaces;

namespace TypeCheckerLib.Helpers
{
    public class BooleanHelper : IBooleanHelper
    {
        private List<FunctionNode> _functions;
        private Func<ExpressionNode, List<TypeNode>, TypeNode> _getType;

        public void Initialize(AST root, Func<ExpressionNode, List<TypeNode>, TypeNode> dispatcher)
        {
            _functions = root.Functions;
            _getType = dispatcher;
        }

        public TypeNode VisitAnd(AndExpression node, List<TypeNode> parameterTypes)
        {
            TypeNode left = _getType(node.Children[0], parameterTypes);
            TypeNode right = _getType(node.Children[1], parameterTypes);

            if ( !IsBool(left.Type) && !IsBool(right.Type))
            {
                throw new UnmatchableTypesException((Node)node, left.Type, right.Type, "bool");
            }
            return new TypeNode(TypeEnum.Boolean, 0, 0);
        }

        public TypeNode VisitNot(NotExpression node, List<TypeNode> parameterTypes)
        {
            throw new NotImplementedException();
        }

        public TypeNode VisitOr(OrExpression node, List<TypeNode> parameterTypes)
        {
            TypeNode left = _getType(node.Children[0], parameterTypes);
            TypeNode right = _getType(node.Children[1], parameterTypes);

            if (!IsBool(left.Type) && !IsBool(right.Type))
            {
                throw new UnmatchableTypesException((Node)node, left.Type, right.Type, "bool");
            }
            return new TypeNode(TypeEnum.Boolean, 0, 0);
        }

        private bool IsBool(TypeEnum t)
        {
            return t == TypeEnum.Boolean;
        }

    }
}