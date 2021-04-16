using System;
using System.Collections.Generic;
using ASTLib;
using ASTLib.Exceptions;
using ASTLib.Interfaces;
using ASTLib.Nodes;
using ASTLib.Nodes.ExpressionNodes;
using ASTLib.Nodes.ExpressionNodes.CommonOperationNodes.ElementAndSetOperations;
using ASTLib.Nodes.ExpressionNodes.NumberOperationNodes;
using ASTLib.Nodes.ExpressionNodes.OperationNodes;
using ASTLib.Nodes.TypeNodes;
using TypeCheckerLib.Interfaces;

namespace TypeCheckerLib.Helpers
{
    public class CommonOperatorHelper : ICommonOperatorHelper
    {
        private List<FunctionNode> _functions;
        private Func<ExpressionNode, List<TypeNode>, TypeNode> _getType;

        public void Initialize(AST root, Func<ExpressionNode, List<TypeNode>, TypeNode> dispatcher)
        {
            _functions = root.Functions;
            _getType = dispatcher;
        }

        public TypeNode VisitAddition(AdditionExpression n, List<TypeNode> parameterTypes)
        {
            TypeNode left = GetType(n.Children[0], parameterTypes);
            TypeNode right = GetType(n.Children[1], parameterTypes);

            if (!IsAddableType(left.Type) || !IsAddableType(right.Type))
                throw new UnmatchableTypesException(n, left.Type, right.Type, "number");

            if (left.Type != right.Type)
            {
                CastToReal(n, left, 0);
                CastToReal(n, right, 1);
                return new TypeNode(TypeEnum.Real, 0, 0);
            }
            return new TypeNode(left.Type, 0, 0);
        }

        private bool IsAddableType(TypeEnum t)
        {
            return t == TypeEnum.Integer || t == TypeEnum.Real;
        }

        public TypeNode VisitSubtraction(SubtractionExpression n, List<TypeNode> parameterTypes)
        {
            TypeNode left = GetType(n.Children[0], parameterTypes);
            TypeNode right = GetType(n.Children[1], parameterTypes);

            if ((left.Type == TypeEnum.Integer && right.Type == TypeEnum.Real) || (left.Type == TypeEnum.Real && right.Type == TypeEnum.Integer))
            {
                CastToReal(n, left, 0);
                CastToReal(n, right, 1);
                return new TypeNode(TypeEnum.Real, 0, 0);
            }
            else if (left.Type == TypeEnum.Integer && right.Type == TypeEnum.Integer)
            {
                return new TypeNode(left.Type, 0, 0);
            }
            else if (left.Type == TypeEnum.Set && right.Type == TypeEnum.Set)
            {
                return new TypeNode(TypeEnum.Set, 0, 0);
            }
            else
            {
                throw new UnmatchableTypesException(n, left.Type, right.Type, "Unmatchable types for subtraction");
            }
        }

        public TypeNode VisitAbsoluteValue(AbsoluteValueExpression n, List<TypeNode> parameterTypes)
        {
            TypeNode childType = GetType(n.Children[0], parameterTypes);

            if (childType.Type == TypeEnum.Real)
            {
                n.Type = TypeEnum.Real;
                return childType;
            }
            else if (childType.Type == TypeEnum.Integer) //More types are added later, they all return integer 
            {
                n.Type = TypeEnum.Integer;
                return new TypeNode(TypeEnum.Integer, 0, 0);
            }
            else if (childType.Type == TypeEnum.Set)
            {
                n.Type = TypeEnum.Set;
                return new TypeNode(TypeEnum.Integer, 0, 0);
            }
            else
                throw new AbsoluteValueTypeException(n, childType.Type);
        }

        private TypeNode GetType(ExpressionNode node, List<TypeNode> parameterTypes)
        {
            return _getType(node, parameterTypes);
        }
        
        private void CastToReal(IExpressionNode binaryNode, TypeNode nodeType, int child)
        {
            if (nodeType.Type != TypeEnum.Real)
            {
                InsertCastNode(binaryNode, child);
            }
        }
        
        private void InsertCastNode(IExpressionNode binaryNode, int child)
        {
            CastFromIntegerExpression cast = new CastFromIntegerExpression(binaryNode.Children[child], 0, 0);
            binaryNode.Children[child] = cast;
        }

        public TypeNode VisitRelationalOperator(IRelationOperator node, List<TypeNode> parameterTypes)
        {
            TypeNode left = GetType(node.Children[0], parameterTypes);
            TypeNode right = GetType(node.Children[1], parameterTypes);

            if (!IsNumber(left.Type) || !IsNumber(right.Type))
                throw new UnmatchableTypesException((Node) node, left.Type, right.Type, "number");

            CastToReal(node, left, 0);
            CastToReal(node, right, 1);
            return new TypeNode(TypeEnum.Boolean, 0, 0);
        }

        public TypeNode VisitEquivalenceOperator(IEquivalenceOperator node, List<TypeNode> parameterTypes)
        {
            TypeNode left = GetType(node.Children[0], parameterTypes);
            TypeNode right = GetType(node.Children[1], parameterTypes);

            if (left.Type == right.Type)
            {
                node.Type = left.Type;
                return new TypeNode(TypeEnum.Boolean, 0, 0);
            }
            else if (left.Type == TypeEnum.Real && right.Type == TypeEnum.Integer)
            {
                node.Type = TypeEnum.Real;
                CastToReal(node, right, 1);
                return new TypeNode(TypeEnum.Boolean, 0, 0);
            }
            else if (left.Type == TypeEnum.Integer && right.Type == TypeEnum.Real)
            {
                node.Type = TypeEnum.Real;
                CastToReal(node, left, 0);
                return new TypeNode(TypeEnum.Boolean, 0, 0);
            }
            else
            {
                throw new UnmatchableTypesException((Node)node, left.Type, right.Type, "expected same type");
            }
                
        }

        private bool IsNumber(TypeEnum t)
        {
            return t == TypeEnum.Integer || t == TypeEnum.Real;
        }

        public TypeNode VisitIn(InExpression node, List<TypeNode> parameterTypes)
        {
            TypeNode left = GetType(node.Children[0], parameterTypes);
            TypeNode right = GetType(node.Children[1], parameterTypes);

            if (left.Type != TypeEnum.Element || right.Type != TypeEnum.Set)
                throw new UnmatchableTypesException(node, left.Type, right.Type, "Lhs and rhs must be of type 'Element' and 'Set' respectivly");

            return new TypeNode(TypeEnum.Boolean, 0, 0);
        }
    }
}