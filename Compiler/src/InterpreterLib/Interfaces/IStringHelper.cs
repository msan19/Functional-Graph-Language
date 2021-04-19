using System;
using System.Collections.Generic;
using ASTLib.Nodes.ExpressionNodes.OperationNodes;

namespace InterpreterLib.Interfaces
{
    public interface IStringHelper : IInterpreterHelper
    {
        void SetInterpreter(IInterpreterString interpreter);

        string AdditionString(AdditionExpression node, List<Object> parameters);
    }
}
