using System;
using System.Collections.Generic;
using ASTLib;
using ASTLib.Nodes;
using ASTLib.Nodes.ExpressionNodes;
using ASTLib.Nodes.TypeNodes;
using ASTLib.Objects;

namespace InterpreterLib.Interfaces
{
    public interface IInterpreterFunction
    {
        Function DispatchFunction(ExpressionNode node, List<object> parameters);

        object Dispatch(ExpressionNode node, List<object> parameters, TypeEnum type);

        bool DispatchBoolean(ExpressionNode node, List<object> parameters);

        Graph DispatchGraph(ExpressionNode node, List<Object> parameters);

    }
}