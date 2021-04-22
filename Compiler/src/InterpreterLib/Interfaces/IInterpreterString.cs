using System;
using System.Collections.Generic;
using ASTLib.Nodes.ExpressionNodes;
using ASTLib.Nodes.TypeNodes;
using ASTLib.Objects;

namespace InterpreterLib.Interfaces
{
    public interface IInterpreterString
    {

        string DispatchString(ExpressionNode node, List<object> parameters);

        bool DispatchBoolean(ExpressionNode node, List<Object> parameters);
        double DispatchReal(ExpressionNode node, List<Object> parameters);
        int DispatchInt(ExpressionNode node, List<Object> parameters);

        object Dispatch(ExpressionNode node, List<object> parameters, TypeEnum type);
    }
}
