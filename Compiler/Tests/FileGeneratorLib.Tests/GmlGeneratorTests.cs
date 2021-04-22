﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using ASTLib.Objects;

namespace FileGeneratorLib.Tests
{
    [TestClass]
    public class GmlGeneratorTests
    {
        [TestMethod]
        public void HandleLabelGraph_WithoutAdditionalLabels_DstSrcEvenNumber_()
        {
            List<int> srcList = new List<int>() { 1, 2 };
            List<int> dstList = new List<int>() { 3, 4 };
            string[,] vertexLabels = new string[,] { };
            string[,] edgeLabels = new string[,] { };
            LabelGraph labelGraph = new LabelGraph("test1", srcList, dstList, vertexLabels, edgeLabels, 4);

            GmlGenerator gmlGenerator = new GmlGenerator();
            string expected = ReadFile("expected1.gml");

            string actual = gmlGenerator.Generate(labelGraph);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void HandleLabelGraph_WithAdditionalLabels_()
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
            string expected = ReadFile("expected2.gml");

            string actual = gmlGenerator.Generate(labelGraph);
            
            Assert.AreEqual(expected, actual);
        }

        private string ReadFile(string fileName)
        {
            FileHelper fileHelper = new FileHelper();
            string currentPath = fileHelper.GetProjectDirectory();
            string fullPath = fileHelper.AppendStr(currentPath, fileName);
            return File.ReadAllText(fullPath);
        }
    }
}