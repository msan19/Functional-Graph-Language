using System;
using System.Collections.Generic;
using ASTLib;
using ASTLib.Exceptions;
using ASTLib.Exceptions.NotMatching;
using ASTLib.Interfaces;
using ASTLib.Nodes;
using ASTLib.Nodes.ExpressionNodes;
using ASTLib.Nodes.ExpressionNodes.CastExpressionNodes;
using ASTLib.Nodes.ExpressionNodes.CommonOperationNodes.ElementAndSetOperations;
using ASTLib.Nodes.ExpressionNodes.NumberOperationNodes;
using ASTLib.Nodes.ExpressionNodes.OperationNodes;
using ASTLib.Nodes.TypeNodes;
using TypeCheckerLib.Interfaces;
using InvalidCastException = ASTLib.Exceptions.Invalid.InvalidCastException;

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

            if ( (left.Type == TypeEnum.Integer && right.Type == TypeEnum.Real) || (left.Type == TypeEnum.Real && right.Type == TypeEnum.Integer) )
            {
                CastToReal(n, left, 0);
                CastToReal(n, right, 1);
                return new TypeNode(TypeEnum.Real, 0, 0);
            }
            else if (left.Type == TypeEnum.Integer && right.Type == TypeEnum.Integer)
            {
                return new TypeNode(TypeEnum.Integer, 0, 0);
            }
            else if (left.Type == TypeEnum.Real && right.Type == TypeEnum.Real)
            {
                return new TypeNode(TypeEnum.Real, 0, 0);
            }
            else if ( (left.Type == TypeEnum.String && IsAddableType(right.Type)) || IsAddableType(left.Type) && right.Type == TypeEnum.String )
            {
                CastToString(n, left, 0);
                CastToString(n, right, 1);
                return new TypeNode(TypeEnum.String, 0, 0);
            }
            throw new UnmatchableTypesException(n, left.Type, right.Type, "number or string");
        }

        private bool IsAddableType(TypeEnum t)
        {
            return t == TypeEnum.Integer || t == TypeEnum.Real || t == TypeEnum.String || t == TypeEnum.Boolean;
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
            else if (left.Type == TypeEnum.Real && right.Type == TypeEnum.Real)
            {
                return new TypeNode(TypeEnum.Real, 0, 0);
            }
            else if (left.Type == TypeEnum.Integer && right.Type == TypeEnum.Integer)
                return new TypeNode(left.Type, 0, 0);
            else if (left.Type == TypeEnum.Set && right.Type == TypeEnum.Set)
                return new TypeNode(TypeEnum.Set, 0, 0);
            else
                throw new UnmatchableTypesException(n, left.Type, right.Type, "Unmatchable types for subtraction");
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
                InsertCastNode(binaryNode, child, TypeEnum.Integer);
            }
        }

        private void CastToString(IExpressionNode binaryNode, TypeNode nodeType, int child)
        {
            if (nodeType.Type != TypeEnum.String)
            {
                if (nodeType.Type == TypeEnum.Integer)
                {
                    InsertCastNode(binaryNode, child, TypeEnum.Integer);
                }
                else if (nodeType.Type == TypeEnum.Real)
                {
                    InsertCastNode(binaryNode, child, TypeEnum.Real);
                }
                else if (nodeType.Type == TypeEnum.Boolean)
                {
                    InsertCastNode(binaryNode, child, TypeEnum.Boolean);
                }
            }
        }
        
        private void InsertCastNode(IExpressionNode binaryNode, int child, TypeEnum castFrom)
        {
            switch (castFrom)
            {
                case TypeEnum.Integer:
                    CastFromIntegerExpression cast1 = new CastFromIntegerExpression(binaryNode.Children[child], 0, 0);
                    binaryNode.Children[child] = cast1;
                    break;
                case TypeEnum.Real:
                    CastFromRealExpression cast2 = new CastFromRealExpression(binaryNode.Children[child], 0, 0);
                    binaryNode.Children[child] = cast2;
                    break;
                case TypeEnum.Boolean:
                    CastFromBooleanExpression cast3 = new CastFromBooleanExpression(binaryNode.Children[child], 0, 0);
                    binaryNode.Children[child] = cast3;
                    break;
                default:
                    throw new Exception("Invalid castFrom");
            }
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

        public TypeNode VisitGraph(GraphExpression node, List<TypeNode> parameterTypes)
        {
            TypeNode vertexSet = GetType(node.Children[0], parameterTypes);
            TypeNode edgeSet = GetType(node.Children[1], parameterTypes);
            FunctionTypeNode src = (FunctionTypeNode) GetType(node.Children[2], parameterTypes);
            FunctionTypeNode dst = (FunctionTypeNode) GetType(node.Children[3], parameterTypes);

            if (vertexSet.Type != TypeEnum.Set)
                throw new UnmatchableTypesException(node, vertexSet.Type, TypeEnum.Set);
            if (edgeSet.Type != TypeEnum.Set)
                throw new UnmatchableTypesException(node, edgeSet.Type, TypeEnum.Set);
            CheckEdgeFunction(src, nameof(src), node);
            CheckEdgeFunction(dst, nameof(dst), node);

            return new TypeNode(TypeEnum.Graph, 0, 0);
        }

        private void CheckEdgeFunction(FunctionTypeNode funcTypeNode, string name, GraphExpression parent)
        {
            if (funcTypeNode.Type != TypeEnum.Function)
                throw new UnmatchableTypesException(parent, funcTypeNode.Type, TypeEnum.Function);
            if (funcTypeNode.ParameterTypes.Count != 1)
                throw new Exception($"{name} function must take a single parameter");
            if (funcTypeNode.ParameterTypes[0].Type != TypeEnum.Element)
                throw new UnmatchableTypesException(parent, funcTypeNode.ParameterTypes[0].Type, TypeEnum.Element);
            if (funcTypeNode.ReturnType.Type != TypeEnum.Element)
                throw new UnmatchableTypesException(parent, funcTypeNode.ReturnType.Type, TypeEnum.Element);
        }
        
        public TypeNode VisitElement(ElementExpression n, List<TypeNode> parameterTypes)
        {
            foreach (var c in n.Children)
            {
                var type = _getType(c, parameterTypes).Type;
                if (type != TypeEnum.Integer)
                    throw new UnmatchableTypesException(n, TypeEnum.Integer, type);
            }
            return new TypeNode(TypeEnum.Element, 0, 0);
        }
        
        public TypeNode VisitISetGraphField(ISetGraphField node, List<TypeNode> parameterTypes)
        {
            TypeNode typeNode = GetType(node.Children[0], parameterTypes);
            if (typeNode.Type != TypeEnum.Graph)
                throw new UnmatchableTypesException(node.Children[0], typeNode.Type, TypeEnum.Graph);
            
            return new TypeNode(TypeEnum.Set, 0, 0);
        }
        
        public TypeNode VisitIFunctionGraphField(IFunctionGraphField node, List<TypeNode> parameterTypes)
        {
            TypeNode typeNode = GetType(node.Children[0], parameterTypes);
            if (typeNode.Type != TypeEnum.Graph)
                throw new UnmatchableTypesException(node.Children[0], typeNode.Type, TypeEnum.Graph);
            
            return new TypeNode(TypeEnum.Function, 0, 0);
        }
    }
}