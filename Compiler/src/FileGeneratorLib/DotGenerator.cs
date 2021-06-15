using System.Collections.Generic;
using System.Text;
using ASTLib.Objects;
using FileGeneratorLib.Interfaces;

namespace FileGeneratorLib
{
    public class DotGenerator : IOutputGenerator
    {
        public List<ExtensionalGraph> Generate(List<LabelGraph> graphs)
        {
            List<ExtensionalGraph> gmlGraphs = new List<ExtensionalGraph>();
            foreach (var graph in graphs)
            {
                string dotString = Generate(graph);
                ExtensionalGraph extensionalGraph = new ExtensionalGraph(graph.FileName, dotString);
                gmlGraphs.Add(extensionalGraph);
            }
            return gmlGraphs;
        }
        
        public string Generate(LabelGraph graph)
        {
            string s = "";
            s += GetVerticesAsString(graph);
            s += "\n";
            s += GetEdgesAsString(graph);
            return "digraph " + graph.FileName + " { \n" + s + "}\n";
        }

        private string GetVerticesAsString(LabelGraph graph)
        {
            string s = "";
            for (int i = 0; i < graph.VertexCount; i++)
                s += GetVertexString(graph, i);
            return s;
        }

        private string GetVertexString(LabelGraph graph, int i)
        {
            StringBuilder sb = new StringBuilder($"\t {i} [label = \"");
            AddAdditionalLabels(sb, graph.VertexLabels, i);
            sb.Append("\"];\n");
            return sb.ToString();
        }

        private void AddAdditionalLabels(StringBuilder sb, string[,] labels, int i)
        {
            for (int row = 0; row < labels.GetLength(0); row++)
            {
                string label = labels[row, i];
                sb.AppendLine(label + "\n");
            }
        }

        private string GetEdgesAsString(LabelGraph graph)
        {
            string s = "";
            for (int i = 0; i < graph.SrcList.Count; i++)
                s += GetEdgeAsString(graph, i);
            return s;
        }

        private string GetEdgeAsString(LabelGraph graph, int i)
        {
            StringBuilder sb = new StringBuilder($"\t{graph.SrcList[i]} -> {graph.DstList[i]} [label = \"");
            AddAdditionalLabels(sb, graph.EdgeLabels, i);
            sb.Append("\"];\n");
            return sb.ToString();
        }
    }
}