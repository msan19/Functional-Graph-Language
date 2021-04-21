using ASTLib.Nodes.ExpressionNodes;
using ASTLib.Objects;
using System;
using System.Collections.Generic;
using System.Text;

namespace InterpreterLib.Interfaces
{
    public interface IGraphHelper : IInterpreterHelper
    {
        void SetInterpreter(IInterpreterGraph interpreter);
        Graph GraphExpression(GraphExpression node, List<Object> parameters);
    }
}
