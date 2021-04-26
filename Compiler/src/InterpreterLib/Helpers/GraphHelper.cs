using ASTLib;
using ASTLib.Exceptions.Invalid;
using ASTLib.Nodes;
using ASTLib.Nodes.ExpressionNodes;
using ASTLib.Objects;
using InterpreterLib.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InterpreterLib.Helpers
{
    public class GraphHelper : IGraphHelper
    {
        private IInterpreterGraph _interpreter;
        private List<FunctionNode> _functions;

        public void SetASTRoot(AST root)
        {
            _functions = root.Functions;
        }

        public void SetInterpreter(IInterpreterGraph interpreter)
        {
            _interpreter = interpreter;
        }

        public LabelGraph ExportGraph(ExportNode node)
        {
            List<Object> emptyParameters = new List<Object>();
            Graph graph     = _interpreter.DispatchGraph(node.ExportValue, emptyParameters);
            string fileName = _interpreter.DispatchString(node.FileName, emptyParameters);
            List<int> src = new List<int>();
            List<int> dst = new List<int>();

            ElementComparer comparer = new ElementComparer();
            foreach (Element e in graph.Edges.Elements)
            {
                src.Add(GetElementIndex(node, graph.Src, e, graph, comparer));
                dst.Add(GetElementIndex(node, graph.Dst, e, graph, comparer));
            }

            string[,] edgeLabels = GetLabels(node.EdgeLabels, graph.Edges);
            string[,] vertexLabels  = GetLabels(node.VertexLabels, graph.Vertices);

            return new LabelGraph(fileName, src, dst, vertexLabels, edgeLabels, graph.Vertices.Elements.Count);
        }

        private string[,] GetLabels(List<ExpressionNode> functions, Set set)
        {
            List<Object> parameters = new List<Object>();
            List<FunctionNode> nodes = functions.ConvertAll(x => _interpreter.DispatchFunction(x, parameters)).
                                                 ConvertAll(x => _functions[x.Reference]);
            string[,] labels = new string[functions.Count, set.Elements.Count];
            for (int i = 0; i < functions.Count; i++)
                for (int ii = 0; ii < set.Elements.Count; ii++)
                    labels[i,ii] = _interpreter.Function<string>(nodes[i], new List<Object> { set.Elements[ii] });

            return labels;
        }

        private int GetElementIndex(ExportNode node, Function function, Element input, Graph graph, ElementComparer comparer)
        {
            FunctionNode functionNode = _functions[function.Reference];
            List<Object> parameters = function.Scope.ToList();
            parameters.Add(input);
            Element element = _interpreter.Function<Element>(functionNode, parameters);
            int index = graph.Vertices.Elements.BinarySearch(element, comparer);
            if (index < 0)
                throw new InvalidElementException(node, element);
            return index;
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
