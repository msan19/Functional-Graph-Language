using System.Text;
using ASTLib.Objects;

namespace FileGeneratorLib
{
    public class GmlGenerator : IGmlGenerator
    {
        public string Generate(LabelGraph graph)
        {
            string s = "";
            s += GetVerticesAsString(graph);
            s += GetEdgesAsString(graph);
            return "graph [ \n\tdirected 1\n" + s + "]\n";
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
            StringBuilder sb = new StringBuilder("\tnode [ ");
            sb.AppendLine($"\n\t    id {i + 1}");
            AddAdditionalVertexLabels(sb, graph, i);
            sb.Append("\t]\n");
            return sb.ToString();
        }

        private void AddAdditionalVertexLabels(StringBuilder sb, LabelGraph graph, int i)
        {
            for (int row = 0; row < graph.VertexLabels.GetLength(0); row++)
            {
                string label = graph.VertexLabels[row, i];
                if (label != "")
                    sb.AppendLine($"\t    {label}");
            }
        }

        private string GetEdgesAsString(LabelGraph graph)
        {
            string s = "";
            for (int i = 0; i < graph.SrcList.Count; i++)
            {
                s += GetEdgeAsString(graph, i);
            }
            return s;
        }

        private string GetEdgeAsString(LabelGraph graph, int i)
        {
            StringBuilder sb = new StringBuilder("\tedge [ ");
            sb.AppendLine($"\n\t    source {graph.SrcList[i]}");
            sb.AppendLine($"\t    target {graph.DstList[i]}");
            AddAdditionalEdgeLabels(sb, graph, i);
            sb.Append("\t]\n");
            return sb.ToString();
        }

        private void AddAdditionalEdgeLabels(StringBuilder sb, LabelGraph graph, int i)
        {
            for (int row = 0; row < graph.EdgeLabels.GetLength(0); row++)
            {
                string label = graph.EdgeLabels[row, i];
                if (label != "")
                    sb.AppendLine($"\t    {label}");
            }
        }
    }
}