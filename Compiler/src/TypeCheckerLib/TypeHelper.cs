using ASTLib;
using ASTLib.Interfaces;
using ASTLib.Nodes;
using ASTLib.Nodes.ExpressionNodes;
using ASTLib.Nodes.ExpressionNodes.OperationNodes;
using ASTLib.Nodes.TypeNodes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TypeCheckerLib
{
    public class TypeHelper : ITypeHelper
    {
        public ITypeChecker TypeChecker { get; set; }
        private List<FunctionNode> _functions;

        public void SetAstRoot(AST root)
        {
            _functions = root.Functions;
        }

        
        public void VisitExport(ExportNode exportNode)
        {
            var type = TypeChecker.Dispatch(exportNode.ExportValue, new List<TypeNode>()).Type;
            if (type == TypeEnum.Real)
                return;
            else if (type == TypeEnum.Integer)
                InsertCastNode(exportNode);
            else
                throw new Exception("Export recived " + type.ToString() + " instead of Integer or Double");
        }

        private void InsertCastNode(ExportNode node)
        {
            CastFromIntegerExpression cast = new CastFromIntegerExpression(node.ExportValue, 0, 0);
            node.ExportValue = cast;
        }

        public void VisitFunction(FunctionNode functionNode)
        {
            List<TypeNode> parameterTypes = functionNode.FunctionType.ParameterTypes;

            foreach (var condition in functionNode.Conditions)
            {
                CheckConditionNode(functionNode.FunctionType.ReturnType.Type, condition, parameterTypes);
            }
        }

        private void CheckConditionNode(TypeEnum rhsType, ConditionNode condition, List<TypeNode> parameterTypes)
        {
            var type = TypeChecker.Dispatch(condition.ReturnExpression, parameterTypes).Type;
            if (type != rhsType)
            {
                if (type == TypeEnum.Integer && rhsType == TypeEnum.Real)
                    InsertCastNode(condition);
                else
                    throw new Exception("Function body returns " + type.ToString()
                        + ", expected " + rhsType.ToString());
            }
        }

        private void InsertCastNode(ConditionNode conditionNode)
        {
            CastFromIntegerExpression cast = new CastFromIntegerExpression(conditionNode.ReturnExpression, 0, 0);
            conditionNode.ReturnExpression = cast;
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

        public TypeNode VisitFunctionCall(FunctionCallExpression funcCallExpNode, List<TypeNode> parameterTypes)
        {
            TypeNode res = null;
            if (IsLocalReferenceAMatch(funcCallExpNode, parameterTypes))
            {
                funcCallExpNode.GlobalReferences = new List<int>();
                FunctionTypeNode funcDeclType = (FunctionTypeNode)parameterTypes[funcCallExpNode.LocalReference];
                res = funcDeclType.ReturnType;
            }
            else
            {
                List<int> matchingRefs = GetMatches(funcCallExpNode.Children, funcCallExpNode.GlobalReferences, parameterTypes);
                if (matchingRefs.Count != 1)
                    throw new Exception("Found no or more than one match");

                funcCallExpNode.LocalReference = FunctionCallExpression.NO_LOCAL_REF;
                funcCallExpNode.GlobalReferences = matchingRefs;
                res = _functions[matchingRefs.First()].FunctionType.ReturnType;
            }
            
            return res;
        }

        private List<int> GetMatches(List<ExpressionNode> children, List<int> globalReferences,
            List<TypeNode> parameterTypes)
        {
            List<int> res = new List<int>();
            foreach (var globRef in globalReferences)
            {
                var func = _functions[globRef];
                if (ChildrenMatchesFunctionInputParams(children, func.FunctionType.ParameterTypes, parameterTypes))
                    res.Add(globRef);
            }
            return res;
        }

        private bool ChildrenMatchesFunctionInputParams(List<ExpressionNode> children,
            List<TypeNode> functionTypeParameterTypes, List<TypeNode> parameterTypes)
        {
            for (var i = 0; i < children.Count; i++)
            {
                var child = TypeChecker.Dispatch(children[i], parameterTypes);
                var expected = functionTypeParameterTypes[i];
                if (!TypesAreEqual(child, expected))
                    return false;
            }
            return true;
        }

        private bool IsLocalReferenceAMatch(FunctionCallExpression funcCallExpNode, List<TypeNode> parameterTypes)
        {
            if (funcCallExpNode.LocalReference == FunctionCallExpression.NO_LOCAL_REF)
                return false;
            
            FunctionTypeNode funcDeclType = (FunctionTypeNode)parameterTypes[funcCallExpNode.LocalReference];
            for (var i = 0; i < funcCallExpNode.Children.Count; i++)
            {
                var child = TypeChecker.Dispatch(funcCallExpNode.Children[i], parameterTypes);
                var expected = funcDeclType.ParameterTypes[i];
                if (!TypesAreEqual(child, expected))
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

        public TypeNode VisitIdentifier(IdentifierExpression idExpressionNode, List<TypeNode> parameterTypes)
        {
            if (idExpressionNode.IsLocal)
                return parameterTypes[idExpressionNode.Reference];
            else
                return _functions[idExpressionNode.Reference].FunctionType.ReturnType;
        }

        public TypeNode VisitIntegerLiteral(IntegerLiteralExpression intLiteralExpressionNode,
            List<TypeNode> parameterTypes)
        {
            return new TypeNode(TypeEnum.Integer, 0, 0);
        }

        public TypeNode VisitRealLiteral(RealLiteralExpression realLiteralExpressionNode, List<TypeNode> parameterTypes)
        {
            return new TypeNode(TypeEnum.Real, 0, 0);
        }

        /// 
        /// Private Helpers
        /// 

        private TypeNode GetType(ExpressionNode node, List<TypeNode> parameterTypes)
        {
            return TypeChecker.Dispatch(node, parameterTypes);
        }

        private void CastToReal(IBinaryNumberOperator binaryNode, TypeNode nodeType, int child)
        {
            if (nodeType.Type != TypeEnum.Real)
                InsertCastNode(binaryNode, child);
        }

        private void InsertCastNode(IBinaryNumberOperator binaryNode, int child)
        {
            CastFromIntegerExpression cast = new CastFromIntegerExpression(binaryNode.Children[child], 0, 0);
            binaryNode.Children[child] = cast;
        }
    }
}