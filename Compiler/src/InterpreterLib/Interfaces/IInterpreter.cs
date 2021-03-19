using System;
using System.Collections.Generic;
using ASTLib;
using ASTLib.Nodes;
using ASTLib.Nodes.ExpressionNodes;

namespace InterpreterLib.Interfaces
{
    public interface IInterpreter
    {
        int DispatchFunction(ExpressionNode node, List<object> parameters);

        int DispatchInt(ExpressionNode node, List<object> parameters);

        double DispatchReal(ExpressionNode node, List<object> parameters);

        List<double> Interpret(AST node);

        int FunctionInteger(FunctionNode node, List<Object> parameters);

        double FunctionReal(FunctionNode node, List<Object> parameters);

        int FunctionFunction(FunctionNode node, List<Object> parameters);
    }
}