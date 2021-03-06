using System;
using System.Collections.Generic;
using ASTLib;
using ASTLib.Interfaces;
using ASTLib.Nodes;
using ASTLib.Nodes.ExpressionNodes;
using ASTLib.Nodes.ExpressionNodes.OperationNodes;
using ASTLib.Nodes.TypeNodes;
using TypeCheckerLib.Interfaces;
using ASTLib.Exceptions;
using ASTLib.Exceptions.NotMatching;
using ASTLib.Nodes.ExpressionNodes.CommonOperationNodes;
using ASTLib.Nodes.ExpressionNodes.NumberOperationNodes;
using ASTLib.Nodes.ExpressionNodes.CastExpressionNodes;

namespace TypeCheckerLib.Helpers
{
    public class NumberHelper : INumberHelper
    {
        private List<FunctionNode> _functions;
        private Func<ExpressionNode, List<TypeNode>, TypeNode> _getType;

        public void Initialize(AST root, Func<ExpressionNode, List<TypeNode>, TypeNode> dispatcher)
        {
            _functions = root.Functions;
            _getType = dispatcher;
        }

        public TypeNode VisitBinaryNumOp(IBinaryNumberOperator binaryNode, List<TypeNode> parameterTypes)
        {
            TypeNode left = _getType(binaryNode.Children[0], parameterTypes);
            TypeNode right = _getType(binaryNode.Children[1], parameterTypes);
            
            if (!IsNumberType(left.Type) || !IsNumberType(right.Type))
                throw new UnmatchableTypesException((Node) binaryNode, left.Type, right.Type, "number");

            if (left.Type != right.Type)
            {
                CastToReal(binaryNode, left, 0);
                CastToReal(binaryNode, right, 1);
                return new TypeNode(TypeEnum.Real, 0, 0);
            }
            return new TypeNode(left.Type, 0, 0);
        }

        public TypeNode VisitNegative(NegativeExpression node, List<TypeNode> parameterTypes)
        {
            TypeNode childNodeType = _getType(node.Children[0], parameterTypes);
            if (!IsNumber(childNodeType.Type))
                throw new UnableToNegateTermException(node.Children[0], childNodeType.ToString());
            return childNodeType;
        }

        private bool IsNumber(TypeEnum t)
        {
            return t == TypeEnum.Integer || t == TypeEnum.Real;
        }
        
        private bool IsNumberType(TypeEnum t)
        {
            return t == TypeEnum.Integer || t == TypeEnum.Real;
        }

        private void CastToReal(IExpressionNode binaryNode, TypeNode nodeType, int child)
        {
            if (nodeType.Type != TypeEnum.Real)
                InsertCastNode(binaryNode, child);
        }
        
        private void InsertCastNode(IExpressionNode binaryNode, int child)
        {
            CastFromIntegerExpression cast = new CastFromIntegerExpression(binaryNode.Children[child], 0, 0);
            binaryNode.Children[child] = cast;
        }
    }
}