using ASTLib.Nodes.ExpressionNodes;
using ASTLib.Objects;
using InterpreterLib.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace InterpreterLib.Helpers
{
    public class ElementHelper : IElementHelper
    {
        private IInterpreterElement _interpreter;

        public void SetInterpreter(IInterpreterElement interpreter)
        {
            _interpreter = interpreter;
        }

        public Element DispatchElement(ElementExpression node, List<object> parameters)
        {
            return default;
        }
    }
}
