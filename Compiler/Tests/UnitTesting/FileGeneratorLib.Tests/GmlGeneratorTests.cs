using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using ASTLib.Objects;
using FileUtilities;
using FluentAssertions;

namespace FileGeneratorLib.Tests
{
    [TestClass]
    public class GmlGeneratorTests
    {
        ExpectedGmlStrings expectedGmlStrings = new ExpectedGmlStrings();
        
        [TestMethod]
        public void Generate_SingleWithoutAdditionalLabels_DstSrcEvenNumber_()
        {
            LabelGraph labelGraph = GetStr1LabelGraph("test1");
            GmlGenerator gmlGenerator = new GmlGenerator();
            string expected = expectedGmlStrings.Str1;

            string actual = gmlGenerator.Generate(labelGraph).Replace("\r", "");
            
            actual.Should().BeEquivalentTo(expected);
        }

        [TestMethod]
        public void Generate_SingleWithAdditionalLabels_()
        {
            List<int> srcList = new List<int>() { 1, 2 };
            List<int> dstList = new List<int>() { 3, 4 };
            string[,] vertexLabels = new string[,]
            {
                {"nodeValue: 1", "nodeValue: 2", "nodeValue: 3", "nodeValue: 4"}, 
                { "someLabelV: 1", "someLabelV: 2", "", ""}
            };
            string[,] edgeLabels = new string[,]
            {
                { "someLabelE: 1", "someLabelE: 2" }
            };
            LabelGraph labelGraph = new LabelGraph("test2", srcList, dstList, vertexLabels, edgeLabels, 4);
            
            GmlGenerator gmlGenerator = new GmlGenerator();
            string expected = expectedGmlStrings.Str2;

            string actual = gmlGenerator.Generate(labelGraph).Replace("\r", "");
            
            expected.Should().BeEquivalentTo(actual);
        }
        
        [TestMethod]
        public void Generate_MultipleWithOne_()
        {
            LabelGraph l1 = GetStr1LabelGraph("l1");
            List<LabelGraph> labelGraphs = new List<LabelGraph>() {l1};
            GmlGenerator gmlGenerator = new GmlGenerator();
            string expected = expectedGmlStrings.Str1;

            List<ExtensionalGraph> extensionalGraphs = gmlGenerator.Generate(labelGraphs);
            
            expected.Should().BeEquivalentTo(extensionalGraphs[0].GraphString);
        }
        
        [TestMethod]
        public void Generate_MultipleWithOne_AssertCorrectCount()
        {
            LabelGraph l1 = GetStr1LabelGraph("l1");
            List<LabelGraph> labelGraphs = new List<LabelGraph>() {l1};
            GmlGenerator gmlGenerator = new GmlGenerator();
            int expectedCount = 1;

            List<ExtensionalGraph> extensionalGraphs = gmlGenerator.Generate(labelGraphs);
            
            Assert.AreEqual(expectedCount, extensionalGraphs.Count);
        }
        
        [TestMethod]
        public void Generate_MultipleWithTwo_()
        {
            LabelGraph l1 = GetStr1LabelGraph("l1");
            LabelGraph l2 = GetStr1LabelGraph("l2");
            List<LabelGraph> labelGraphs = new List<LabelGraph>() {l1, l2};
            GmlGenerator gmlGenerator = new GmlGenerator();
            string expected = expectedGmlStrings.Str1;

            List<ExtensionalGraph> extensionalGraphs = gmlGenerator.Generate(labelGraphs);
            
            expected.Should().BeEquivalentTo(extensionalGraphs[0].GraphString);
            expected.Should().BeEquivalentTo(extensionalGraphs[1].GraphString);
        }
        
        [TestMethod]
        public void Generate_MultipleWithTwo_AssertCorrectCount()
        {
            LabelGraph l1 = GetStr1LabelGraph("l1");
            LabelGraph l2 = GetStr1LabelGraph("l2");
            List<LabelGraph> labelGraphs = new List<LabelGraph>() {l1, l2};
            GmlGenerator gmlGenerator = new GmlGenerator();
            int expectedCount = 2;

            List<ExtensionalGraph> extensionalGraphs = gmlGenerator.Generate(labelGraphs);
            
            Assert.AreEqual(expectedCount, extensionalGraphs.Count);
        }

        private LabelGraph GetStr1LabelGraph(string fileName)
        {
            List<int> srcList = new List<int>() { 1, 2 };
            List<int> dstList = new List<int>() { 3, 4 };
            string[,] vertexLabels = new string[,] { };
            string[,] edgeLabels = new string[,] { };
            return new LabelGraph(fileName, srcList, dstList, vertexLabels, edgeLabels, 4);
        }
    }
}
