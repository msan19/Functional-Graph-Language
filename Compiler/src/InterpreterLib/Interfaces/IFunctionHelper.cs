using System;
using System.Collections.Generic;
using ASTLib;
using ASTLib.Nodes;
using ASTLib.Nodes.ExpressionNodes;

namespace InterpreterLib.Interfaces
{
    public interface IFunctionHelper
    {
        IInterpreter Interpreter { get; set; }

        void SetAST(AST root);

        int ConditionFunction(ConditionNode node, List<Object> parameters);

        int IdentifierFunction(IdentifierExpression node, List<Object> parameters);

        int FunctionCallFunction(FunctionCallExpression node, List<Object> parameters);

    }
}