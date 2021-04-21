using ASTLib.Nodes.ExpressionNodes;
using ASTLib.Objects;
using InterpreterLib.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace InterpreterLib.Helpers
{
    public class GraphHelper : IGraphHelper
    {
        private IInterpreterGraph _interpreter;

        public void SetInterpreter(IInterpreterGraph interpreter)
        {
            _interpreter = interpreter;
        }

        public Graph GraphExpression(GraphExpression node, List<Object> parameters)
        {
            Set vertices = _interpreter.DispatchSet(node.Vertices, parameters);
            Set edges = _interpreter.DispatchSet(node.Edges, parameters);
            Function src = _interpreter.DispatchFunction(node.Src, parameters);
            Function dst = _interpreter.DispatchFunction(node.Dst, parameters);
            return new Graph(vertices, edges, src, dst);
        }
    }
}
