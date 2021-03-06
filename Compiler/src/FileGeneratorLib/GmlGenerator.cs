using System.Collections.Generic;
using System.Text;
using ASTLib.Objects;
using FileGeneratorLib.Interfaces;

namespace FileGeneratorLib
{
    public class GmlGenerator : IOutputGenerator
    {
        public List<ExtensionalGraph> Generate(List<LabelGraph> graphs)
        {
            List<ExtensionalGraph> gmlGraphs = new List<ExtensionalGraph>();
            foreach (var graph in graphs)
            {
                string gmlString = Generate(graph);
                ExtensionalGraph extensionalGraph = new ExtensionalGraph(graph.FileName, gmlString);
                gmlGraphs.Add(extensionalGraph);
            }
            return gmlGraphs;
        }
        
        public string Generate(LabelGraph graph)
        {
            string s = "";
            s += GetVerticesAsString(graph);
            s += GetEdgesAsString(graph);

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("graph [ ");
            sb.AppendLine("\tdirected 1");
            sb.Append(s);
            sb.AppendLine("]");
            return sb.ToString();
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
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("\tnode [ ");
            sb.AppendLine($"\t\tid {i}");
            AddAdditionalVertexLabels(sb, graph, i);
            sb.AppendLine("\t]");
            return sb.ToString();
        }

        private void AddAdditionalVertexLabels(StringBuilder sb, LabelGraph graph, int i)
        {
            for (int row = 0; row < graph.VertexLabels.GetLength(0); row++)
            {
                string label = graph.VertexLabels[row, i];
                if (label != "")
                    sb.AppendLine($"\t\t{label}");
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
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("\tedge [ ");
            sb.AppendLine($"\t\tsource {graph.SrcList[i]}");
            sb.AppendLine($"\t\ttarget {graph.DstList[i]}");
            AddAdditionalEdgeLabels(sb, graph, i);
            sb.AppendLine("\t]");
            return sb.ToString();
        }

        private void AddAdditionalEdgeLabels(StringBuilder sb, LabelGraph graph, int i)
        {
            for (int row = 0; row < graph.EdgeLabels.GetLength(0); row++)
            {
                string label = graph.EdgeLabels[row, i];
                if (label != "")
                    sb.AppendLine($"\t\t{label}");
            }
        }
    }
}