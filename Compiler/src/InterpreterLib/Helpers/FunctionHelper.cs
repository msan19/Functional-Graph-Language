using ASTLib;
using ASTLib.Nodes;
using ASTLib.Nodes.ExpressionNodes;
using System;
using System.Collections.Generic;
using InterpreterLib.Interfaces;
using ASTLib.Nodes.TypeNodes;

namespace InterpreterLib.Helpers
{
    public class FunctionHelper : IFunctionHelper
    {
        private Func<ExpressionNode, List<Object>, int> _dispatchFunction;
        private Func<ExpressionNode, List<object>, TypeEnum, Object> _dispatch;
        private Func<FunctionNode, List<Object>, int> _functionFunction;
        private List<FunctionNode> _functions;

        public void SetASTRoot(AST root)
        {
            _functions = root.Functions;
        }

        public void SetUpFuncs(Func<ExpressionNode, List<Object>, int> dispatchFunction,
                               Func<ExpressionNode, List<object>, TypeEnum, Object> dispatch,
                               Func<FunctionNode, List<Object>, int> functionFunction)
        {
            _dispatchFunction = dispatchFunction;
            _dispatch = dispatch;
            _functionFunction = functionFunction;
        }

        public int ConditionFunction(ConditionNode node, List<Object> parameters)
        {
            return _dispatchFunction(node.ReturnExpression, parameters);         
        }

        public int IdentifierFunction(IdentifierExpression node, List<Object> parameters)
        {
            return node.IsLocal ? (int) parameters[node.Reference] : node.Reference;
        }

        public int FunctionCallFunction(FunctionCallExpression node, List<Object> parameters)
        {
            List<object> funcParameterValues = new List<object>();

            FunctionNode funcNode;
            if (node.GlobalReferences.Count >= 1)
                funcNode = _functions[node.GlobalReferences[0]];
            else
                funcNode = _functions[(int)parameters[node.LocalReference]];

            for (int i = 0; i < node.Children.Count; i++)
            {
                TypeEnum parameterType = funcNode.FunctionType.ParameterTypes[i].Type;
                funcParameterValues.Add(_dispatch(node.Children[i], parameters, parameterType));
            }

            return _functionFunction(funcNode, funcParameterValues);
        }

    }
}