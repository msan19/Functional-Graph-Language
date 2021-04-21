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

        object Dispatch(ExpressionNode node, List<object> parameters, TypeEnum type);
    }
}
