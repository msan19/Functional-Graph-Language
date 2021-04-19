using System;
using System.Collections.Generic;
using System.Text;

namespace ASTLib.Objects
{
    public class Graph
    {
        public Set Vertices { get; }
        public Set Edges { get; }
        public Function Src { get; }
        public Function Dst { get; }

        public Graph(Set vertices, Set edges, Function src, Function dst)
        {
            Vertices = vertices;
            Edges = edges;
            Src = src;
            Dst = dst;
        }

    }
}
