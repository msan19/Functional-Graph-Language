using ASTLib;
using ASTLib.Nodes;
using ASTLib.Nodes.ExpressionNodes;
using ASTLib.Nodes.TypeNodes;
using FluentAssertions;
using InterpreterLib.Helpers;
using InterpreterLib.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;

namespace InterpreterLib.Tests
{
    [TestClass]
    public class FunctionHelperTests
    {
        

        #region IdentifierFunction
        [DataRow(false, 1, new Object[] { 0, 18, "" })]
        [DataRow(false, 10, new Object[] { 0, 18, "" })]
        [DataRow(true, 0, new Object[] { 0, 18, "" })]
        [DataRow(true, 4, new Object[] { 0, 18, "", 104, 17})]
        [TestMethod]
        public void IdentifierFunction_IdentifierExpressionAndObjectList_ReturnsCorrectResult(bool isLocal, int reference, Object[] array)
        {
            IdentifierExpression identifier = new IdentifierExpression("", 0, 0)
            {
                IsLocal = isLocal,
                Reference = reference
            };
            FunctionHelper fhelper = new FunctionHelper();
            int expected = isLocal ? (int) array[reference] : reference;

            int res = fhelper.IdentifierFunction(identifier, array.ToList());

            Assert.AreEqual(res, expected);
        }
        #endregion

    }
}