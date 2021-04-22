using ASTLib.Nodes.ExpressionNodes;
using System;
using System.Collections.Generic;
using System.Text;

namespace ASTLib.Nodes
{
    public class ExportNode : Node
    {
        public ExpressionNode ExportValue { get; set; }
        public ExpressionNode FileName { get; }
        public List<ExpressionNode> VertexLabels { get; } = new List<ExpressionNode>();
        public List<ExpressionNode> EdgeLabels { get; } = new List<ExpressionNode>();

        public ExportNode(ExpressionNode exportValue, int line, int letter) : base(line, letter)
        {
            ExportValue = exportValue;
            VertexLabels = new List<ExpressionNode>();
            EdgeLabels = new List<ExpressionNode>();
        }

        public ExportNode(ExpressionNode exportValue, ExpressionNode fileName,
                          List<ExpressionNode> vertexLabels, List<ExpressionNode> edgeLabels, 
                          int line, int letter) : base(line, letter)
        {
            ExportValue = exportValue;
            FileName = fileName;
            VertexLabels = vertexLabels;
            EdgeLabels = edgeLabels;
        }
    }
}
