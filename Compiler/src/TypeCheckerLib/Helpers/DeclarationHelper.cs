using System;
using System.Collections.Generic;
using System.Linq;
using ASTLib;
using ASTLib.Exceptions;
using ASTLib.Nodes;
using ASTLib.Nodes.ExpressionNodes;
using ASTLib.Nodes.ExpressionNodes.NumberOperationNodes;
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
            if (type != TypeEnum.Set)
                throw new InvalidSetTypeException(exportNode);
            /*
            if (type == TypeEnum.Real)
                return;
            else if (type == TypeEnum.Integer)
                InsertCastNode(exportNode);
            else
                throw new InvalidCastException(exportNode, type, TypeEnum.Real);
            */
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
            if (!condition.IsDefaultCase)
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
                    InsertCastNode(condition);
                else
                    throw new InvalidCastException(condition, returnType.Type, expectedType.Type);
            }
        }

        private bool CanMakeCast(TypeNode returnType, TypeNode expectedType)
        {
            return returnType.Type == TypeEnum.Integer && expectedType.Type == TypeEnum.Real;
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

        private bool AreTypesEqual(TypeEnum a, TypeEnum b)
        {
            return a == b;
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

        
    }
}