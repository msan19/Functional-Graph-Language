using System;
using System.Collections.Generic;
using ASTLib;
using ASTLib.Interfaces;
using ASTLib.Nodes;
using ASTLib.Nodes.ExpressionNodes;
using ASTLib.Nodes.ExpressionNodes.OperationNodes;
using ASTLib.Nodes.TypeNodes;
using TypeCheckerLib.Interfaces;

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
            TypeNode left = GetType(binaryNode.Children[0], parameterTypes);
            TypeNode right = GetType(binaryNode.Children[1], parameterTypes);
            
            if (left.Type == TypeEnum.Function || right.Type == TypeEnum.Function)
                throw new Exception("One of the arguments is of type Function.");

            if (left.Type != right.Type)
            {
                CastToReal(binaryNode, left, 0);
                CastToReal(binaryNode, right, 1);
                return new TypeNode(TypeEnum.Real, 0, 0);
            }
            return new TypeNode(left.Type, 0, 0);
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
        
        private TypeNode GetType(ExpressionNode node, List<TypeNode> parameterTypes)
        {
            return _getType(node, parameterTypes);
        }
    }
}