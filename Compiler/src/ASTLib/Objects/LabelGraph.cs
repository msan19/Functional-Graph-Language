using System;
using System.Collections.Generic;
using System.Text;

namespace ASTLib.Objects
{
    public class LabelGraph
    {
        public string FileName { get; }
        public List<int> SrcList { get; }
        public List<int> DstList { get; }
        public string[,] VertexLabels { get; }
        public string[,] EdgeLabels { get; }

        public LabelGraph(string fileName, List<int> srcList, List<int> dstList, string[,] vertexLabels, string[,] edgeLabels)
        {
            FileName = fileName;
            SrcList = srcList;
            DstList = dstList;
            VertexLabels = vertexLabels;
            EdgeLabels   = edgeLabels;
        }


    }
}
