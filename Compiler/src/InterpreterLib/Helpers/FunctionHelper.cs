using ASTLib;
using ASTLib.Nodes;
using ASTLib.Nodes.ExpressionNodes;
using System;
using System.Collections.Generic;

namespace InterpreterLib.Helpers
{
    public class FunctionHelper : IFunctionHelper
    {

        public IInterpreter Interpreter { get; set; }

        public int FunctionFunction(FunctionNode node, List<Object> parameters)
        {
            throw new NotImplementedException();
        }

        private int ConditionFunction(ConditionNode node, List<Object> parameters)
        {
            throw new NotImplementedException();
        }

        public int IdentifierFunction(IdentifierExpression node, List<Object> parameters)
        {
            throw new NotImplementedException();
        }

        public int FunctionCallFunction(FunctionCallExpression node, List<Object> parameters)
        {
            throw new NotImplementedException();
        }

        public double FunctionCallReal(FunctionCallExpression node, List<Object> parameters)
        {
            throw new NotImplementedException();
        }

    }
}