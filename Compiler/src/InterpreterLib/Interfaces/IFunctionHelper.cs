using ASTLib;
using ASTLib.Nodes;
using ASTLib.Nodes.ExpressionNodes;
using System;
using System.Collections.Generic;

namespace InterpreterLib.Helpers
{
    public interface IFunctionHelper
    {
        public IInterpreter Interpreter { get; set; }

        int FunctionFunction(FunctionNode node, List<Object> parameters);

        int IdentifierFunction(IdentifierExpression node, List<Object> parameters);

        int FunctionCallFunction(FunctionCallExpression node, List<Object> parameters);

        double FunctionCallReal(FunctionCallExpression node, List<Object> parameters);

    }
}