using ASTLib.Nodes.ExpressionNodes;
using System;
using System.Collections.Generic;
using System.Text;

namespace ASTLib.Nodes
{
    public class ExportNode : Node
    {
        public ExpressionNode ExportValue { get; private set; }
        public ExpressionNode FileName { get; private set; }
        public List<ExpressionNode> VertexLabels { get; private set; }
        public List<ExpressionNode> EdgeLabels { get; private set; }

        public ExportNode(ExpressionNode exportValue)
        {
            ExportValue = exportValue;
        }

    }
}
