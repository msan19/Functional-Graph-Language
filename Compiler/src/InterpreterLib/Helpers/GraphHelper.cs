﻿using ASTLib;
using ASTLib.Nodes;
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
                src.Add(GetElementIndex(graph.Src, e, graph, comparer));
                dst.Add(GetElementIndex(graph.Dst, e, graph, comparer));
            }

            string[,] edgeLabels    = new string[graph.Edges.Elements.Count, node.EdgeLabels.Count];
            string[,] vertexLabels  = new string[graph.Edges.Elements.Count, node.EdgeLabels.Count];

            return new LabelGraph(fileName, src, dst, vertexLabels, edgeLabels);
        }

        private int GetElementIndex(Function function, Element input, Graph graph, ElementComparer comparer)
        {
            FunctionNode functionNode = _functions[function.Reference];
            List<Object> parameter = new List<Object> { input };
            Element element = _interpreter.Function<Element>(functionNode, parameter);
            int index = graph.Vertices.Elements.BinarySearch(element, comparer);
            if (index < 0)
                throw new Exception("The ");
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