using ASTLib.Nodes.ExpressionNodes;
using System;
using System.Collections.Generic;
using System.Text;

namespace InterpreterLib.Interfaces
{
    public interface IInterpreterSet
    {
        int DispatchInt(ExpressionNode node, List<object> parameters);
        bool DispatchBoolean(ExpressionNode node, List<object> parameters);
    }
}
