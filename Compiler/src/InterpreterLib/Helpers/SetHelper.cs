using ASTLib.Nodes;
using ASTLib.Nodes.ExpressionNodes;
using ASTLib.Objects;
using InterpreterLib.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace InterpreterLib.Helpers
{
    public class SetHelper : ISetHelper
    {
        private IInterpreterSet _interpreter;

        public void SetInterpreter(IInterpreterSet interpreter)
        {
            _interpreter = interpreter;
        }

        public Set SetExpression(SetExpression node, List<Object> parameters)
        {
            Set set = new Set();
            List<int> minValues = new List<int>();
            List<int> maxValues = new List<int>();
            List<int> indices = new List<int>();

            for (int i = 0; i < node.Bounds.Count; i++)
            {
                minValues.Add(_interpreter.DispatchInt(node.Bounds[i].MinValue, parameters));
                maxValues.Add(_interpreter.DispatchInt(node.Bounds[i].MaxValue, parameters));
                indices.Add(0);
            }

            EvaluateSet(minValues, maxValues, indices, node.Predicate, 0, set, parameters);

            return set;
        }

        private void EvaluateSet(List<int> minValues, List<int> maxValues, List<int> indices, ExpressionNode condition, int recursionDepth, Set set, List<Object> parameters)
        {
            if (recursionDepth < minValues.Count)
            {
                for (int i = minValues[recursionDepth]; i <= maxValues[recursionDepth]; i++)
                {
                    indices[recursionDepth] = i;
                    EvaluateSet(minValues, maxValues, indices, condition, recursionDepth + 1, set, parameters);
                }
            }
            else
            {
                Element element = new Element(indices);
                parameters.Add(element);
                parameters.AddRange(indices.ConvertAll(x => (Object) x));
                if (_interpreter.DispatchBoolean(condition, parameters))
                    set.Elements.Add(element);
                parameters.RemoveRange(parameters.Count - indices.Count - 1, parameters.Count);
            }
        }
    }
}
