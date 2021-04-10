using System;
using System.Collections.Generic;
using ASTLib;
using ASTLib.Nodes;
using ASTLib.Nodes.ExpressionNodes;
using ASTLib.Nodes.TypeNodes;

namespace InterpreterLib.Interfaces
{
    public interface IInterpreterReal
    {
        int DispatchInt(ExpressionNode node, List<object> parameters);

        double DispatchReal(ExpressionNode node, List<object> parameters);

        object Dispatch(ExpressionNode node, List<object> parameters, TypeEnum type);

        bool DispatchBoolean(ExpressionNode node, List<object> parameters);
    }
}