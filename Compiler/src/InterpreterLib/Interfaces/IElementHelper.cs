using ASTLib.Nodes.ExpressionNodes;
using ASTLib.Objects;
using System.Collections.Generic;

namespace InterpreterLib.Interfaces
{
    public interface IElementHelper : IInterpreterHelper
    {
        void SetInterpreter(IInterpreterElement interpreter);

        Element DispatchElement(ElementExpression node, List<object> parameters);
    }
}