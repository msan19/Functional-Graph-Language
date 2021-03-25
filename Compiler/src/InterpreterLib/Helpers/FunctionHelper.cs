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
        public IInterpreter Interpreter { get; set; }
        private List<FunctionNode> _functions;

        public void SetAST(AST root)
        {
            _functions = root.Functions;
        }

        public int ConditionFunction(ConditionNode node, List<Object> parameters)
        {
            return Interpreter.DispatchFunction(node.ReturnExpression, parameters);         
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
                funcParameterValues.Add(Interpreter.Dispatch(node.Children[i], parameters, parameterType));
            }

            return Interpreter.FunctionFunction(funcNode, funcParameterValues);
        }

    }
}