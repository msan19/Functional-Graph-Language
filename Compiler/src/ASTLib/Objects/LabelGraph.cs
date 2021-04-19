﻿using System;
using System.Collections.Generic;
using System.Text;

namespace ASTLib.Objects
{
    public class LabelGraph
    {
        public List<int> SrcList { get; }
        public List<int> DstList { get; }
        public string[][] VertexLabels { get; }
        public string[][] EdgeLabels { get; }

        public LabelGraph(List<int> srcList, List<int> dstList, string[][] vertexLabels, string[][] edgeLabels)
        {
            SrcList = srcList;
            DstList = dstList;
            VertexLabels = vertexLabels;
            EdgeLabels   = edgeLabels;
        }


    }
}
