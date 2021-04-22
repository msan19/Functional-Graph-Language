using ASTLib.Nodes.ExpressionNodes;
using ASTLib.Nodes.ExpressionNodes.SetOperationNodes;
using ASTLib.Objects;
using System;
using System.Collections.Generic;
using System.Text;

namespace InterpreterLib.Interfaces
{
    public interface IInterpreterSet
    {
        int DispatchInt(ExpressionNode node, List<object> parameters);
        bool DispatchBoolean(ExpressionNode node, List<object> parameters);
        Set DispatchSet(ExpressionNode node, List<Object> parameters);
        Graph DispatchGraph(ExpressionNode node, List<Object> parameters);
    }
}
