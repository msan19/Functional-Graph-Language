using System;
using System.Collections.Generic;
using System.Linq;
using ASTLib;
using ASTLib.Exceptions;
using ASTLib.Nodes;
using ASTLib.Nodes.ExpressionNodes;
using ASTLib.Nodes.ExpressionNodes.OperationNodes;
using ASTLib.Nodes.TypeNodes;
using TypeCheckerLib.Interfaces;
using InvalidCastException = ASTLib.Exceptions.InvalidCastException;

namespace TypeCheckerLib.Helpers
{
    public class DeclarationHelper : IDeclarationHelper
    {
        private List<FunctionNode> _functions;
        private Func<ExpressionNode, List<TypeNode>, TypeNode> _getType;

        public void Initialize(AST root, Func<ExpressionNode, List<TypeNode>, TypeNode> dispatcher)
        {
            _functions = root.Functions;
            _getType = dispatcher;
        }

        public void VisitExport(ExportNode exportNode)
        {
            TypeEnum type = _getType(exportNode.ExportValue, new List<TypeNode>()).Type;
            if (type == TypeEnum.Real)
                return;
            else if (type == TypeEnum.Integer)
                InsertCastNode(exportNode);
            else
                throw new InvalidCastException(exportNode, type, TypeEnum.Real);
        }

        private void InsertCastNode(ExportNode node)
        {
            CastFromIntegerExpression cast = new CastFromIntegerExpression(node.ExportValue, 0, 0);
            node.ExportValue = cast;
        }

        public void VisitFunction(FunctionNode functionNode)
        {
            List<TypeNode> parameterTypes = functionNode.FunctionType.ParameterTypes;

            foreach (ConditionNode condition in functionNode.Conditions)
            {
                CheckConditionNode(functionNode.FunctionType.ReturnType.Type, condition, parameterTypes);
            }
        }

        private void CheckConditionNode(TypeEnum rhsType, ConditionNode condition, List<TypeNode> parameterTypes)
        {
            TypeEnum type = _getType(condition.ReturnExpression, parameterTypes).Type;
            if (type != rhsType)
            {
                if (type == TypeEnum.Integer && rhsType == TypeEnum.Real)
                    InsertCastNode(condition);
                else
                    throw new InvalidCastException(condition, type, rhsType);
            }
        }

        private void InsertCastNode(ConditionNode conditionNode)
        {
            CastFromIntegerExpression cast = new CastFromIntegerExpression(conditionNode.ReturnExpression, 0, 0);
            conditionNode.ReturnExpression = cast;
        }
        
        public TypeNode VisitFunctionCall(FunctionCallExpression funcCallExpNode, List<TypeNode> parameterTypes)
        {
            TypeNode res;
            if (IsLocalReferenceAMatch(funcCallExpNode, parameterTypes))
            {
                funcCallExpNode.GlobalReferences = new List<int>();
                FunctionTypeNode funcDeclType = (FunctionTypeNode)parameterTypes[funcCallExpNode.LocalReference];
                res = funcDeclType.ReturnType;
            }
            else
            {
                List<int> matchingRefs = GetMatches(funcCallExpNode.Children, funcCallExpNode.GlobalReferences, parameterTypes);
                if (matchingRefs.Count > 1)
                    throw new OverloadException(funcCallExpNode, GetFunctions(matchingRefs));
                else if (matchingRefs.Count == 0)
                    throw new NoMatchingFunctionFoundException(funcCallExpNode);

                funcCallExpNode.LocalReference = FunctionCallExpression.NO_LOCAL_REF;
                funcCallExpNode.GlobalReferences = matchingRefs;
                res = _functions[matchingRefs.First()].FunctionType.ReturnType;
            }
            
            return res;
        }

        private List<FunctionNode> GetFunctions(List<int> matchingRefs)
        {
            var res = new List<FunctionNode>();
            foreach (var i in matchingRefs)
                res.Add(_functions[i]);
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
                var child = _getType(children[i], parameterTypes);
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
                var child = _getType(funcCallExpNode.Children[i], parameterTypes);
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

        
    }
}