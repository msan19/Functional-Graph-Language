using ASTLib;
using ASTLib.Interfaces;
using ASTLib.Nodes;
using ASTLib.Nodes.ExpressionNodes;
using ASTLib.Nodes.ExpressionNodes.OperationNodes;
using ASTLib.Nodes.TypeNodes;
using System;

namespace TypeCheckerLib
{
    public class TypeHelper : ITypeHelper
    {
        public ITypeChecker TypeChecker { get; set; }
        
        public void VisitExport(ExportNode exportNode)
        {

        }

        public void VisitFunction(FunctionNode functionNode)
        {

        }

        public TypeNode VisitBinaryNumOp(IBinaryNumberOperator binaryNode)
        {
            var left = GetType(binaryNode.Children[0]);
            var right = GetType(binaryNode.Children[1]);

            if (left.Type == right.Type)
                return new TypeNode(left.Type, 0, 0);
            else
            {
                CastToReal(binaryNode, left, 0);
                CastToReal(binaryNode, right, 1);
                return new TypeNode(TypeEnum.Real, 0, 0);
            }
        }

        public TypeNode VisitFunctionCall(FunctionCallExpression funcCallExpNode)
        {
            return null;
        }

        public TypeNode VisitIdentifier(IdentifierExpression idExpressionNode)
        {
            return null;
        }

        public TypeNode VisitIntegerLiteral(IntegerLiteralExpression intLiteralExpressionNode)
        {
            return null;
        }

        public TypeNode VisitRealLiteral(RealLiteralExpression realLiteralExpressionNode)
        {
            return null;
        }

        /// 
        /// Private Helpers
        /// 

        private TypeNode GetType(ExpressionNode node)
        {
            return TypeChecker.Dispatch(node);
        }

        private void CastToReal(IBinaryNumberOperator binaryNode, TypeNode nodeType, int child)
        {
            if (nodeType.Type != TypeEnum.Real)
                InsertCastNode(binaryNode, child, TypeEnum.Real);
        }

        private void InsertCastNode(IBinaryNumberOperator binaryNode, int child, TypeEnum type)
        {
            var cast = new CastFromIntegerExpression(binaryNode.Children[child], 0, 0);
            binaryNode.Children[child] = cast;
        }
    }
}