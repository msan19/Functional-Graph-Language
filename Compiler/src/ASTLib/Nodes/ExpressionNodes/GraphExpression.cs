using ASTLib.Interfaces;
using ASTLib.Nodes.TypeNodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ASTLib.Nodes.ExpressionNodes
{
    public class GraphExpression : ExpressionNode, INonIdentifierExpression
    {
        private const int VERTICES_INDEX = 0, EDGES_INDEX = 1, SRC_INDEX = 2, DST_INDEX = 3;

        public GraphExpression(ExpressionNode vertices, ExpressionNode edges,
                             ExpressionNode src, ExpressionNode dst, int line, int letter) :
                             base(new List<ExpressionNode>() { vertices, edges, src, dst }, line, letter)
        {
        }

        public ExpressionNode Vertices => Children[VERTICES_INDEX];

        public ExpressionNode Edges => Children[EDGES_INDEX];

        public ExpressionNode Src => Children[SRC_INDEX];

        public ExpressionNode Dst => Children[DST_INDEX];
    }
}
