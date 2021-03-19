using System;
using System.Collections.Generic;
using ASTLib.Nodes;
using ASTLib.Nodes.ExpressionNodes;

namespace InterpreterLib.Interfaces
{
    public interface IFunctionHelper
    {
        public IInterpreter Interpreter { get; set; }

        int FunctionFunction(FunctionNode node, List<Object> parameters);

        int IdentifierFunction(IdentifierExpression node, List<Object> parameters);

        int FunctionCallFunction(FunctionCallExpression node, List<Object> parameters);

    }
}