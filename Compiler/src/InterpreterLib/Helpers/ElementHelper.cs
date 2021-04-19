using ASTLib.Nodes.ExpressionNodes;
using ASTLib.Objects;
using InterpreterLib.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public Element Element(ElementExpression node, List<object> parameters)
        {
            var ids = new List<int>();
            foreach (var n in node.Children)
                ids.Add(_interpreter.DispatchInt(n, parameters));
            return new Element(ids);
        }
    }
}
