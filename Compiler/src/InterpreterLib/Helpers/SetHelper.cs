using ASTLib.Nodes;
using ASTLib.Nodes.ExpressionNodes;
using ASTLib.Nodes.ExpressionNodes.SetOperationNodes;
using ASTLib.Objects;
using InterpreterLib.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using ASTLib.Nodes.ExpressionNodes.OperationNodes;
using ASTLib.Nodes.ExpressionNodes.CommonOperationNodes.GraphFields;


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
            if (!node.IsSetBuilder)
                return GetManualSet(node, parameters);

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

        private Set GetManualSet(SetExpression node, List<object> parameters)
        {
            List<Element> elements = new List<Element>();
            foreach (ExpressionNode n in node.Children)
                elements.Add(_interpreter.DispatchElement(n, parameters));
            return new Set(elements);
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
                Element element = new Element(indices.ToList());
                parameters.Add(element);
                parameters.AddRange(indices.ConvertAll(x => (Object) x));
                if (_interpreter.DispatchBoolean(condition, parameters))
                    set.Elements.Add(element);
                parameters.RemoveRange(parameters.Count - indices.Count - 1, indices.Count + 1);
            }
        }

        public Set SubtractionSet(SubtractionExpression node, List<Object> parameters)
        {
            Set leftSet = _interpreter.DispatchSet(node.Children[0], parameters);
            Set rightSet = _interpreter.DispatchSet(node.Children[1], parameters);

            HashSet<Element> set = leftSet.SetCopy;
            set.ExceptWith(rightSet.Elements);
            return new Set(set);
        }

        public Set IntersectionSet(IntersectionExpression node, List<object> parameters)
        {
            Set leftSet = _interpreter.DispatchSet(node.Children[0], parameters);
            Set rightSet = _interpreter.DispatchSet(node.Children[1], parameters);

            HashSet<Element> set = leftSet.SetCopy;
            set.IntersectWith(rightSet.Elements);
            return new Set(set);
        }
        
        public Set UnionSet(UnionExpression node, List<object> parameters)
        {
            Set leftSet = _interpreter.DispatchSet(node.Children[0], parameters);
            Set rightSet = _interpreter.DispatchSet(node.Children[1], parameters);

            HashSet<Element> set = leftSet.SetCopy;
            set.UnionWith(rightSet.Elements);
            return new Set(set);
        }

        public Set VerticesField(VerticesGraphField node, List<Object> parameters)
        {
            Graph graph = _interpreter.DispatchGraph(node.Children[0], parameters);

            return graph.Vertices;
        }

        public Set EdgesField(EdgesGraphField node, List<Object> parameters)
        {
            Graph graph = _interpreter.DispatchGraph(node.Children[0], parameters);

            return graph.Edges;
        }

        public Set EmptySetLiteral(EmptySetLiteralExpression e, List<object> parameters)
        {
            return e.Value;
        }
    }
}
