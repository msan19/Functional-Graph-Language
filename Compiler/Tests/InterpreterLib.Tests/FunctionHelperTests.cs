using ASTLib;
using ASTLib.Nodes;
using ASTLib.Nodes.ExpressionNodes;
using ASTLib.Nodes.TypeNodes;
using ASTLib.Objects;
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
        [DataRow(false, 1, 0)]
        [DataRow(false, 10, 0)]
        [DataRow(true, 0, 0)]
        [DataRow(true, 4, 1)]
        [TestMethod]
        public void IdentifierFunction_IdentifierExpressionAndObjectList_ReturnsCorrectResult(bool isLocal, int reference, int parameters)
        {
            IdentifierExpression identifier = new IdentifierExpression("", 0, 0)
            {
                IsLocal = isLocal,
                Reference = reference
            };
            FunctionHelper fhelper = new FunctionHelper();
            List<Object> array = GetData(parameters);
            int expected = isLocal ? ((Function) array[reference]).Reference : reference;

            Function res = fhelper.IdentifierFunction(identifier, array);

            Assert.AreEqual(res.Reference, expected);
        }

        public List<Object> GetData(int i)
        {
            List<List<Object>> data = new List<List<Object>> { new List<Object>{ new Function(0), 18, "" }, 
                                                               new List<Object>{ 0, 18, "", 104, new Function(17) } };
            return data[i];
        }
        #endregion

    }
}