using System;
using System.Collections.Generic;
using System.Linq;
using ASTLib;
using ASTLib.Nodes;
using ASTLib.Nodes.ExpressionNodes;
using ASTLib.Nodes.ExpressionNodes.BooleanOperationNodes;
using ASTLib.Nodes.ExpressionNodes.CommonOperationNodes;
using ASTLib.Nodes.ExpressionNodes.CommonOperationNodes.RelationalOperationNodes;
using ASTLib.Nodes.TypeNodes;
using ASTLib.Objects;
using InterpreterLib.Interfaces;
using InterpreterLib.MatchPair;

namespace InterpreterLib.Helpers
{
    public class GenericHelper : IGenericHelper
    {
        private IInterpreterGeneric _interpreter;
        private List<FunctionNode> _functions;

        public void SetInterpreter(IInterpreterGeneric interpreter)
        {
            _interpreter = interpreter;
        }

        public void SetASTRoot(AST root)
        {
            _functions = root.Functions;
        }

        public T Identifier<T>(IdentifierExpression node, List<object> parameters)
        {
            return (T) parameters[node.Reference];
        }

        public T FunctionCall<T>(FunctionCallExpression node, List<object> parameters)
        {
            FunctionNode funcNode;
            List<object> scope;
            if (node.LocalReference == FunctionCallExpression.NO_LOCAL_REF)
            {
                scope = new List<object>();
                funcNode = _functions[node.GlobalReferences[0]];
            }
            else
            {
                Function f = (Function) parameters[node.LocalReference];
                scope = f.Scope.ToList();
                funcNode = _functions[f.Reference];
            }
                
            return GetResult<T>(node, parameters, funcNode, scope);
        }

        private T GetResult<T>(FunctionCallExpression node, List<object> parameters, 
                               FunctionNode funcNode, List<object> scope)
        {
            List<object> funcParameters = scope;
            for (int i = 0; i < node.Children.Count; i++)
            {
                TypeEnum type = funcNode.FunctionType.ParameterTypes[i].Type;
                funcParameters.Add(_interpreter.Dispatch(node.Children[i], parameters, type));
            }
            return _interpreter.Function<T>(funcNode, funcParameters);
        }

        public MatchPair<T> Condition<T>(ConditionNode node, List<object> parameters)
        {
            bool conditionIsPass = true;
            conditionIsPass &= IsElementsAMatch(node, parameters);
            var newParameters = GetNewParameters(parameters, node.Elements);
            conditionIsPass &= IsConditionAPass(node, newParameters);
            if (conditionIsPass)
                return GetReturnValue<T>(node, newParameters);
            else 
                return new MatchPair<T>(false, default);
        }

        private List<object> GetNewParameters(List<object> parameters, List<ElementNode> elements)
        {
            var res = parameters.ToList();
            foreach (var eNode in elements)
            {
                var e = (Element) parameters[eNode.Reference];
                foreach (var id in e.Indices)
                {
                    res.Add(id);
                }
            }
            return res;
        }

        private MatchPair<T> GetReturnValue<T>(ConditionNode node, List<object> parameters)
        {
            return new MatchPair<T>(true, (T) _interpreter.Dispatch(node.ReturnExpression, parameters, GetType(typeof(T))));
        }

        private bool IsConditionAPass(ConditionNode node, List<object> parameters)
        {
            if (node.IsDefaultCase)
                return true;
            if (node.Condition == null)
                return true;
            var v = _interpreter.DispatchBoolean(node.Condition, parameters);
            return (bool)v;
        }

        private bool IsElementsAMatch(ConditionNode node, List<object> parameters)
        {
            foreach (var eNode in node.Elements)
            {
                var e = (Element) parameters[eNode.Reference];
                if (e.Indices.Count != eNode.IndexIdentifiers.Count)
                    return false;
            }
            return true;
        }

        private TypeEnum GetType(Type t)
        {
            if (t == typeof(int))
                return TypeEnum.Integer;
            else if (t == typeof(double))
                return TypeEnum.Real;
            else if (t == typeof(bool))
                return TypeEnum.Boolean;
            else if (t == typeof(Function))
                return TypeEnum.Function;
            else if (t == typeof(Set))
                return TypeEnum.Set;
            else if (t == typeof(Element))
                return TypeEnum.Element;
            else if (t == typeof(Graph))
                return TypeEnum.Graph;
            else if (t == typeof(string))
                return TypeEnum.String;
            else
                throw new Exception($"Unimplemented type {t}");
        }


    }
}
