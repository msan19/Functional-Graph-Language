using ASTLib.Nodes.ExpressionNodes;
using ASTLib.Objects;
using System;
using System.Collections.Generic;
using System.Text;

namespace InterpreterLib.Interfaces
{
    public interface ISetHelper : IInterpreterHelper
    {
        public void SetInterpreter(IInterpreterSet interpreter);
        Set SetExpression(SetExpression node, List<Object> parameters);
    }
}
