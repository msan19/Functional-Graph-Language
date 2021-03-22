using ASTLib;
using ASTLib.Interfaces;
using ASTLib.Nodes;
using ASTLib.Nodes.ExpressionNodes;
using ASTLib.Nodes.ExpressionNodes.OperationNodes;
using ASTLib.Nodes.TypeNodes;
using System;
using System.Collections.Generic;

namespace TypeCheckerLib
{
    public class TypeHelper : ITypeHelper
    {
        public ITypeChecker TypeChecker { get; set; }
        private List<FunctionNode> _functions;
        private int _currentFunction;

        public void SetAstRoot(AST root)
        {
            _functions = root.Functions;
        }

        public void VisitExport(ExportNode exportNode)
        {

        }

        public void VisitFunction(FunctionNode functionNode)
        {
            //_currentFunction = functionNode.Index;
        }

        public TypeNode VisitBinaryNumOp(IBinaryNumberOperator binaryNode)
        {
            TypeNode left = GetType(binaryNode.Children[0]);
            TypeNode right = GetType(binaryNode.Children[1]);

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
            var matches = GetMatchingFunctions(funcCallExpNode);
            if (matches.Count != 1)
                throw new Exception("No overload matched");

            return matches[0].FunctionType.ReturnType;
        }

        private List<FunctionNode> GetMatchingFunctions(FunctionCallExpression funcCallExpNode)
        {
            List<FunctionNode> matches = new List<FunctionNode>();
            foreach (var i in funcCallExpNode.GlobalReferences)
            {
                var func = _functions[i];
                if (FunctionIsMatch(func.FunctionType.ParameterTypes, funcCallExpNode))
                    matches.Add(func);
            }

            return matches;
        }

        private bool FunctionIsMatch(List<TypeNode> parameterTypes, FunctionCallExpression funcCallExpNode)
        {
            for (int i = 0; i < parameterTypes.Count; i++)
            {
                var typeNode = TypeChecker.Dispatch(funcCallExpNode.Children[i]);
                if(!TypesAreEqual(typeNode, parameterTypes[i]))
                    return false;
            }
            return true;
        }

        private bool TypesAreEqual(TypeNode a, TypeNode b)
        {
            if (a.GetType() == typeof(FunctionTypeNode))
            {
                if (b.GetType() == typeof(FunctionTypeNode))
                   return IsFunctionTypesEqual((FunctionTypeNode)a, (FunctionTypeNode)b);
                else
                    return false;
            }

            return a.Type == b.Type;
        }

        private bool IsFunctionTypesEqual(FunctionTypeNode a, FunctionTypeNode b)
        {
            if (!TypesAreEqual(a.ReturnType, b.ReturnType))
                return false;
            if (a.ParameterTypes.Count != b.ParameterTypes.Count)
                return false;

            for (int i = 0; i < a.ParameterTypes.Count; i++)
            {
                if (!TypesAreEqual(a.ParameterTypes[i], b.ParameterTypes[i]))
                    return false;
            }
            return true;
        }

        public TypeNode VisitIdentifier(IdentifierExpression idExpressionNode)
        {
            // Get current Function
            // Get Parameter index
            // Return Parameter type

            // TODO: This works, but plz remove :D
            var func = _functions[_currentFunction];
            var index = -1; ;
            for (int i = 0; i < func.ParameterIdentifiers.Count; i++)
            {
                string id = func.ParameterIdentifiers[i];
                if (id.Equals(idExpressionNode.Id))
                    index = i;
            }

            return func.FunctionType.ParameterTypes[index];
            return null;
        }

        public TypeNode VisitIntegerLiteral(IntegerLiteralExpression intLiteralExpressionNode)
        {
            return new TypeNode(TypeEnum.Integer, 0, 0);
        }

        public TypeNode VisitRealLiteral(RealLiteralExpression realLiteralExpressionNode)
        {
            return new TypeNode(TypeEnum.Real, 0, 0);
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
            CastFromIntegerExpression cast = new CastFromIntegerExpression(binaryNode.Children[child], 0, 0);
            binaryNode.Children[child] = cast;
        }
    }
}