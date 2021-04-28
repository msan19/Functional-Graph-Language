using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using ASTLib.Objects;

namespace AST.Tests
{
    [TestClass]
    public class ElementTests
    {

        [DataRow(new int[] { 0 }, new int[] { 0 }, true)]
        [DataRow(new int[] { 17 }, new int[] { 0 }, false)]
        [DataRow(new int[] { 9, 5 }, new int[] { 9, 5 }, true)]
        [DataRow(new int[] { 17, 8 }, new int[] { 0 }, false)]
        [DataRow(new int[] { 8 }, new int[] { 0, 1 }, false)]
        [TestMethod]
        public void Equals_DataRows_CorrectEquivalence(int[] aIndices, int[] bIndices, bool expected)
        {
            Element a = new Element(aIndices.ToList());
            Element b = new Element(bIndices.ToList());

            bool result = a.Equals(b);

            Assert.AreEqual(expected, result);
        }

        [DataRow(new int[] { 0 }, new int[] { 0 }, 0)]
        [DataRow(new int[] { 17 }, new int[] { 0 }, 1)]
        [DataRow(new int[] { 9, 5 }, new int[] { 9, 5 }, 0)]
        [DataRow(new int[] { 17, 8 }, new int[] { 0 }, 1)]
        [DataRow(new int[] { 8 }, new int[] { 0, 1 }, -1)]
        [TestMethod]
        public void CompareTO_DataRows_CorrectEquivalence(int[] aIndices, int[] bIndices, int expected)
        {
            Element a = new Element(aIndices.ToList());
            Element b = new Element(bIndices.ToList());

            int result = a.CompareTo(b);

            Assert.AreEqual(expected, result);
        }

    }
}
