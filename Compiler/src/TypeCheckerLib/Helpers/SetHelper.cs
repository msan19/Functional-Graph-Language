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
using ASTLib.Nodes.ExpressionNodes.CommonOperationNodes.ElementAndSetOperations;
using System.Linq;

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
                throw new UnmatchableTypesException((Node)binaryNode, left.Type, right.Type, "set");
            }
            return new TypeNode(TypeEnum.Set, 0, 0);
        }

        public TypeNode VisitSubset(SubsetExpression node, List<TypeNode> parameterTypes)
        {
            TypeNode left = _getType(node.Children[0], parameterTypes);
            TypeNode right = _getType(node.Children[1], parameterTypes);

            if (!IsSet(left.Type) || !IsSet(right.Type))
            {
                throw new UnmatchableTypesException((Node)node, left.Type, right.Type, "boolean");
            }
            return new TypeNode(TypeEnum.Boolean, 0, 0);
        }

        private bool IsSet(TypeEnum t)
        {
            return t == TypeEnum.Set;
        }

        public TypeNode VisitSet(SetExpression node, List<TypeNode> parameterTypes)
        {
            List<TypeNode> indexTypes = new List<TypeNode>();
            foreach(BoundNode n in node.Bounds)
            {
                CheckType(n.MaxValue, parameterTypes, TypeEnum.Integer);
                CheckType(n.MinValue, parameterTypes, TypeEnum.Integer);
                indexTypes.Add(new TypeNode(TypeEnum.Integer, 0,0));
            }

            parameterTypes.Add(new TypeNode(TypeEnum.Element, 0, 0));
            parameterTypes.AddRange(indexTypes);
            CheckType(node.Predicate, parameterTypes, TypeEnum.Boolean);
            parameterTypes.RemoveRange(parameterTypes.Count - node.Bounds.Count - 1, node.Bounds.Count + 1);
            return new TypeNode(TypeEnum.Set, 0, 0);
        }

        private void CheckType(ExpressionNode n, List<TypeNode> parameterTypes, TypeEnum expected)
        {
            TypeEnum t = _getType(n, parameterTypes).Type;

            if (t != expected)
                throw new InvalidSetTypeException(n, t, expected);
        }
    }
}
