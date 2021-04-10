using System;
using System.Collections.Generic;
using ASTLib;
using ASTLib.Nodes;
using ASTLib.Nodes.ExpressionNodes;
using ASTLib.Nodes.TypeNodes;

namespace InterpreterLib.Interfaces
{
    public interface IInterpreterFunction
    {
        int DispatchFunction(ExpressionNode node, List<object> parameters);

        object Dispatch(ExpressionNode node, List<object> parameters, TypeEnum type);

        bool DispatchBoolean(ExpressionNode node, List<object> parameters);
    }
}