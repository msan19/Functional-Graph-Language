using System;
using System.Collections.Generic;
using ASTLib;
using ASTLib.Nodes;
using ASTLib.Nodes.ExpressionNodes;
using ASTLib.Nodes.ExpressionNodes.BooleanOperationNodes;
using ASTLib.Nodes.ExpressionNodes.CommonOperationNodes;
using ASTLib.Nodes.ExpressionNodes.CommonOperationNodes.RelationalOperationNodes;
using ASTLib.Nodes.TypeNodes;
using InterpreterLib.Interfaces;
using InterpreterLib.MatchPair;

namespace InterpreterLib.Helpers
{
    public class GenericHelper : IGenericHelper
    {
        private IInterpreterGeneric _interpreter;
        private AST _root;
        private List<FunctionNode> _functions => _root.Functions;

        public void SetInterpreter(IInterpreterGeneric interpreter)
        {
            _interpreter = interpreter;
        }

        public void SetASTRoot(AST root)
        {
            _root = root;
        }

        public T FunctionCall<T>(FunctionCallExpression node, List<object> parameters)
        {
            FunctionNode funcNode = GetFuncNode(node, parameters);
            return GetResult<T>(node, funcNode, parameters);
        }

        private T GetResult<T>(FunctionCallExpression node, FunctionNode funcNode, List<object> parameters)
        {
            List<object> funcParameters = new List<object>();
            for (int i = 0; i < node.Children.Count; i++)
            {
                TypeEnum type = funcNode.FunctionType.ParameterTypes[i].Type;
                funcParameters.Add(_interpreter.Dispatch(node.Children[i], parameters, type));
            }
            return _interpreter.Function<T>(funcNode, funcParameters);
        }

        private FunctionNode GetFuncNode(FunctionCallExpression node, List<object> parameters)
        {
            if (node.LocalReference == FunctionCallExpression.NO_LOCAL_REF)
                return _functions[node.GlobalReferences[0]];
            else
                return _functions[(int)parameters[node.LocalReference]];
        }

        public MatchPair<T> Condition<T>(ConditionNode node, List<object> parameters)
        {
            if (node.Condition == null || (bool)_interpreter.Dispatch(node.Condition, parameters, TypeEnum.Boolean))
                return new MatchPair<T>(true, (T) _interpreter.Dispatch(node.ReturnExpression, parameters, GetType(typeof(T))));
            return new MatchPair<T>(false, default);
        }

        private TypeEnum GetType(Type t)
        {
            if (t == typeof(int))
                return TypeEnum.Integer;
            else if (t == typeof(double))
                return TypeEnum.Real;
            else if (t == typeof(bool))
                return TypeEnum.Boolean;
            else if (t == typeof(long))
                return TypeEnum.Function;
            else
                throw new Exception($"Unimplemented type {t}");
        }


    }
}
