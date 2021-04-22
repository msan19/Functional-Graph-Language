using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using ASTLib.Objects;

namespace FileGeneratorLib.Tests
{
    [TestClass]
    public class FileGeneratorTests
    {
        [TestMethod]
        public void HandleLabelGraph_WithoutAdditionalLabels_DstSrcEvenNumber_()
        {
            List<int> srcList = new List<int>() { 1, 2 };
            List<int> dstList = new List<int>() { 3, 4 };
            string[,] vertexLabels = new string[,] { };
            string[,] edgeLabels = new string[,] { };
            LabelGraph labelGraph = new LabelGraph("test", srcList, dstList, vertexLabels, edgeLabels, 4);

            FileGenerator fileGenerator = new FileGenerator(new FileHelper());

            List<LabelGraph> labelGraphs = new List<LabelGraph>() { labelGraph };
            fileGenerator.Export(labelGraphs, false);
        }

        [TestMethod]
        public void HandleLabelGraph_WithAdditionalLabels_()
        {
            List<int> srcList = new List<int>() { 1, 2 };
            List<int> dstList = new List<int>() { 3, 4 };
            string[,] vertexLabels = new string[,]
            {
                {"1", "2", "3", "4"}, 
                { "someLabelV: 1", "someLabelV: 2", "", ""}
            };
            string[,] edgeLabels = new string[,]
            {
                {"1, 3", "2, 4"}, 
                { "someLabelE: 1", "someLabelE: 2" }
            };
            LabelGraph labelGraph = new LabelGraph("test", srcList, dstList, vertexLabels, edgeLabels, 4);

            FileGenerator fileGenerator = new FileGenerator(new FileHelper());

            List<LabelGraph> labelGraphs = new List<LabelGraph>() { labelGraph };
            fileGenerator.Export(labelGraphs, false);
        }
    }
}
