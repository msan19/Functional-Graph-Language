using System;
using System.Collections.Generic;
using System.Linq;
using ASTLib;
using ASTLib.Exceptions;
using ASTLib.Exceptions.Invalid;
using ASTLib.Exceptions.NotMatching;
using ASTLib.Nodes;
using ASTLib.Nodes.ExpressionNodes;
using ASTLib.Nodes.ExpressionNodes.CastExpressionNodes;
using ASTLib.Nodes.ExpressionNodes.NumberOperationNodes;
using ASTLib.Nodes.ExpressionNodes.OperationNodes;
using ASTLib.Nodes.TypeNodes;
using TypeCheckerLib.Interfaces;
using InvalidCastException = ASTLib.Exceptions.Invalid.InvalidCastException;

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
            CheckForGraph(exportNode.ExportValue);
            CheckForString(exportNode.FileName);
            CheckForAttributeFunctions(exportNode.VertexLabels);
            CheckForAttributeFunctions(exportNode.EdgeLabels);
        }

        private void CheckForGraph(ExpressionNode node)
        {
            var parameters = new List<TypeNode>();
            var type = _getType(node, parameters).Type;
            if (type != TypeEnum.Graph)
                throw new UnmatchableTypesException(node, type, TypeEnum.Graph);
        }

        private void CheckForString(ExpressionNode node)
        {
            var parameters = new List<TypeNode>();
            var type = _getType(node, parameters).Type;
            if (type != TypeEnum.String)
                throw new UnmatchableTypesException(node, type, TypeEnum.String);
        }

        private void CheckForAttributeFunctions(List<ExpressionNode> nodes)
        {
            foreach (var node in nodes)
                CheckForAttributeFunction(node);
        }

        private void CheckForAttributeFunction(ExpressionNode node)
        {
            var parameters = new List<TypeNode>();
            var typeNode = _getType(node, parameters);
            if (typeNode.Type != TypeEnum.Function)
                throw new UnmatchableTypesException(node, typeNode.Type, TypeEnum.Function);

            var funcType = (FunctionTypeNode)typeNode;
            if (funcType.ReturnType.Type != TypeEnum.String)
                throw new UnmatchableTypesException(node, funcType.ReturnType.Type, TypeEnum.String);
            if (funcType.ParameterTypes.Count != 1)
                throw new UnmatchableTypesException(node, funcType.ParameterTypes.ConvertAll(x => x.Type), TypeEnum.Element);
            if (funcType.ParameterTypes[0].Type != TypeEnum.Element)
                throw new UnmatchableTypesException(node, funcType.ParameterTypes.ConvertAll(x => x.Type), TypeEnum.Element);
        }

        public void VisitFunction(FunctionNode functionNode)
        {
            List<TypeNode> parameterTypes = functionNode.FunctionType.ParameterTypes;

            foreach (ConditionNode condition in functionNode.Conditions)
            {
                CheckConditionNode(functionNode.FunctionType.ReturnType, condition, parameterTypes);
            }
        }
        
        public void CheckConditionNode(TypeNode expectedType, ConditionNode condition, List<TypeNode> parameterTypes)
        {
            CheckElements(condition.Elements, parameterTypes);
            var newParams = GetNewParameters(parameterTypes, condition);
            CheckCondition(condition, newParams);
            CheckReturnExpr(condition, newParams, expectedType);
        }

        private void CheckElements(List<ElementNode> elements, List<TypeNode> parameterTypes)
        {
            for (int i = 0; i < elements.Count; i++)
            {
                if (parameterTypes[i].Type != TypeEnum.Element)
                    throw new Exception();
            }
        }

        private List<TypeNode> GetNewParameters(List<TypeNode> parameterTypes, ConditionNode condition)
        {
            var res = parameterTypes.ToList();
            foreach (var element in condition.Elements)
            {
                for (int i = 0; i < element.IndexIdentifiers.Count; i++)
                {
                    res.Add(new TypeNode(TypeEnum.Integer, 0, 0));
                }
            }
            return res;
        }

        private void CheckCondition(ConditionNode condition, List<TypeNode> newParams)
        {
            if (!condition.IsDefaultCase && condition.Condition != null)
            {
                TypeEnum conditionType = _getType(condition.Condition, newParams).Type;
                if (conditionType != TypeEnum.Boolean)
                    throw new InvalidCastException(condition, conditionType, TypeEnum.Boolean);
            }
        }

        private void CheckReturnExpr(ConditionNode condition, List<TypeNode> newParams, TypeNode expectedType)
        {
            TypeNode returnType = _getType(condition.ReturnExpression, newParams);
            if (!AreTypesEqual(returnType, expectedType))
            {
                if (CanMakeCast(returnType, expectedType))
                    InsertCastNode(condition, expectedType);
                else
                    throw new InvalidCastException(condition, returnType.Type, expectedType.Type);
            }
        }

        private bool CanMakeCast(TypeNode returnType, TypeNode expectedType)
        {
            return returnType.Type == TypeEnum.Integer && expectedType.Type == TypeEnum.Real ||
                   returnType.Type == TypeEnum.Integer && expectedType.Type == TypeEnum.String ||
                   returnType.Type == TypeEnum.Real && expectedType.Type == TypeEnum.String ||
                   returnType.Type == TypeEnum.Boolean && expectedType.Type == TypeEnum.String;
        }

        private void InsertCastNode(ConditionNode conditionNode, TypeNode expectedType)
        {
            switch (expectedType.Type)
            {
                case TypeEnum.Real:
                    CastToReal(conditionNode);
                    break;
                case TypeEnum.String:
                    CastToString(conditionNode, expectedType);
                    break;
                default:
                    throw new Exception("Invalid castFrom");
            }
            CastFromIntegerExpression cast = new CastFromIntegerExpression(conditionNode.ReturnExpression, 0, 0);
            conditionNode.ReturnExpression = cast;
        }

        private void CastToString(ConditionNode node, TypeNode expectedType)
        {
            if (expectedType.Type == TypeEnum.Integer)
            {
                CastFromIntegerExpression cast1 = new CastFromIntegerExpression(node.ReturnExpression, 0, 0);
                node.ReturnExpression = cast1;
            }
            else if (expectedType.Type == TypeEnum.Real)
            {
                CastFromRealExpression cast2 = new CastFromRealExpression(node.ReturnExpression, 0, 0);
                node.ReturnExpression = cast2;
            }
            else if (expectedType.Type == TypeEnum.Boolean)
            {
                CastFromBooleanExpression cast3 = new CastFromBooleanExpression(node.ReturnExpression, 0, 0);
                node.ReturnExpression = cast3;
            }
        }

        private void CastToReal(ConditionNode node)
        {
            CastFromIntegerExpression cast1 = new CastFromIntegerExpression(node.ReturnExpression, 0, 0);
            node.ReturnExpression = cast1;
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
                List<int> matchingRefs = funcCallExpNode.GlobalReferences;
                CheckMatches(funcCallExpNode.Children, matchingRefs, parameterTypes);
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
            List<FunctionNode> res = new List<FunctionNode>();
            foreach (int i in matchingRefs)
                res.Add(_functions[i]);
            return res;
        }

        private void CheckMatches(List<ExpressionNode> children, List<int> references,
                                     List<TypeNode> parameterTypes)
        {
            List<TypeNode> childrenTypes = children.ConvertAll(child => _getType(child, parameterTypes));

            for (int i = references.Count - 1; i >= 0; i--)
            {
                FunctionNode func = _functions[references[i]];
                if (!ChildrenMatchesFunctionInputParams(childrenTypes, func.FunctionType.ParameterTypes))
                    references.Remove(i);
            }
        }

        private bool ChildrenMatchesFunctionInputParams(List<TypeNode> children,
                                                        List<TypeNode> functionTypeParameterTypes)
        {
            for (int i = 0; i < children.Count; i++)
            {
                if (!AreTypesEqual(children[i], functionTypeParameterTypes[i]))
                    return false;
            }
            return true;
        }

        private bool IsLocalReferenceAMatch(FunctionCallExpression funcCallExpNode, List<TypeNode> parameterTypes)
        {
            if (funcCallExpNode.LocalReference == FunctionCallExpression.NO_LOCAL_REF)
                return false;
            
            FunctionTypeNode funcDeclType = (FunctionTypeNode)parameterTypes[funcCallExpNode.LocalReference];
            for (int i = 0; i < funcCallExpNode.Children.Count; i++)
            {
                TypeNode child = _getType(funcCallExpNode.Children[i], parameterTypes);
                TypeNode expected = funcDeclType.ParameterTypes[i];
                if (!AreTypesEqual(child, expected))
                    return false;
            }
            return true;
        }
        
        private bool AreTypesEqual(TypeNode a, TypeNode b)
        {
            if (a.Type == TypeEnum.Function)
            {
                if (b.Type == TypeEnum.Function)
                    return IsFunctionTypesEqual((FunctionTypeNode)a, (FunctionTypeNode)b);
                else
                    return false;
            }

            return a.Type == b.Type;
        }

        private bool IsFunctionTypesEqual(FunctionTypeNode a, FunctionTypeNode b)
        {
            if (!AreTypesEqual(a.ReturnType, b.ReturnType))
                return false;
            if (a.ParameterTypes.Count != b.ParameterTypes.Count)
                return false;

            for (int i = 0; i < a.ParameterTypes.Count; i++)
            {
                if (!AreTypesEqual(a.ParameterTypes[i], b.ParameterTypes[i]))
                    return false;
            }
            return true;
        }

        public TypeNode VisitIdentifier(IdentifierExpression idExpressionNode, List<TypeNode> parameterTypes)
        {
            if (idExpressionNode.IsLocal)
                return parameterTypes[idExpressionNode.Reference];
            else
                return _functions[idExpressionNode.Reference].FunctionType;
        }

        public TypeNode VisitAnonymousFunction(AnonymousFunctionExpression node, List<TypeNode> parameterTypes)
        {
            List<TypeNode> newScope = new List<TypeNode>();
            newScope.AddRange(parameterTypes);
            newScope.AddRange(node.Types);

            TypeNode returnType = _getType(node.ReturnValue, newScope);

            FunctionTypeNode functionType = new FunctionTypeNode(returnType, newScope, 0, 0);

            int line = node.LineNumber;
            int letter = node.LineNumber;
            ConditionNode condition = new ConditionNode(node.ReturnValue, line, letter);
            FunctionNode function = new FunctionNode(functionType.ToString(), condition, 
                                                     node.Identifiers, functionType, line, letter);
            node.Reference = _functions.Count;
            _functions.Add(function);
            return new FunctionTypeNode(returnType, node.Types, 0,0);
        }

        public TypeNode VisitIntegerLiteral()
        {
            return new TypeNode(TypeEnum.Integer, 0, 0);
        }

        public TypeNode VisitRealLiteral()
        {
            return new TypeNode(TypeEnum.Real, 0, 0);
        }

        public TypeNode VisitBooleanLiteral()
        {
            return new TypeNode(TypeEnum.Boolean, 0, 0);
        }

        public TypeNode VisitStringLiteral()
        {
            return new TypeNode(TypeEnum.String, 0, 0);
        }

        public TypeNode VisitEmptySetLiteral()
        {
            return new TypeNode(TypeEnum.Set, 0, 0);
        }

    }
}