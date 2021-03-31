using System;
using System.Collections.Generic;
using ASTLib;
using ASTLib.Nodes;
using ASTLib.Nodes.ExpressionNodes;
using ASTLib.Nodes.TypeNodes;

namespace InterpreterLib.Interfaces
{
    public interface IInterpreterInteger
    {
        int DispatchInt(ExpressionNode node, List<object> parameters);

        object Dispatch(ExpressionNode node, List<object> parameters, TypeEnum type);

        int FunctionInteger(FunctionNode node, List<Object> parameters);

        bool DispatchBoolean(ExpressionNode node, List<object> parameters);
    }

}