using ASTLib.Nodes.ExpressionNodes;
using ASTLib.Objects;
using System;
using System.Collections.Generic;
using System.Text;

namespace InterpreterLib.Interfaces
{
    public interface IInterpreterGraph
    {
        Set DispatchSet(ExpressionNode node, List<Object> parameters);
        Function DispatchFunction(ExpressionNode node, List<object> parameters);
    }
}
