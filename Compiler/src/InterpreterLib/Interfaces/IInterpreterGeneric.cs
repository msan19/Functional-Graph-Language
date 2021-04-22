using System;
using System.Collections.Generic;
using ASTLib;
using ASTLib.Nodes;
using ASTLib.Nodes.ExpressionNodes;
using ASTLib.Nodes.TypeNodes;

namespace InterpreterLib.Interfaces
{
    public interface IInterpreterGeneric
    {

        object Dispatch(ExpressionNode node, List<object> parameters, TypeEnum type);

        bool DispatchBoolean(ExpressionNode node, List<Object> parameters);

        T Function<T>(FunctionNode node, List<Object> parameters);
    }
}