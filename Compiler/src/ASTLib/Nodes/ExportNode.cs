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
        public List<ExpressionNode> VertexLabels { get; }
        public List<ExpressionNode> EdgeLabels { get; }

        public ExportNode(ExpressionNode exportValue, int line, int letter) : base(line, letter)
        {
            ExportValue = exportValue;
        }
    }
}
