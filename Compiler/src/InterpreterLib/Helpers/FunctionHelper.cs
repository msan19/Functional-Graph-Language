using ASTLib;
using ASTLib.Nodes;
using ASTLib.Nodes.ExpressionNodes;
using System;
using System.Collections.Generic;
using InterpreterLib.Interfaces;

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
            throw new NotImplementedException();
        }

    }
}