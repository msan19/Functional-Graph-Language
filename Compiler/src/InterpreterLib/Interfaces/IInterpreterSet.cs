using ASTLib.Nodes.ExpressionNodes;
using System;
using System.Collections.Generic;
using System.Text;
using ASTLib.Objects;

namespace InterpreterLib.Interfaces
{
    public interface IInterpreterSet
    {
        int DispatchInt(ExpressionNode node, List<object> parameters);
        bool DispatchBoolean(ExpressionNode node, List<object> parameters);
        Set DispatchSet(ExpressionNode node, List<Object> parameters);
    }
}
