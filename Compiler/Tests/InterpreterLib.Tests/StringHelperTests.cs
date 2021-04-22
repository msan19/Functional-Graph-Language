using System;
using System.Collections.Generic;
using ASTLib.Nodes.ExpressionNodes;
using ASTLib.Nodes.ExpressionNodes.OperationNodes;
using ASTLib.Nodes.TypeNodes;
using InterpreterLib.Helpers;
using InterpreterLib.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;

namespace InterpreterLib.Tests
{
    [TestClass]
    public class StringHelperTests
    {
        private StringHelper SetUpHelper(IInterpreterString parent)
        {
            StringHelper stringHelper = new StringHelper();
            stringHelper.SetInterpreter(parent);
            return stringHelper;
        }

        #region AdditionString
        [DataRow("a", "b", "ab")]
        [DataRow("Label", "1", "Label1")]
        [DataRow("test", "", "test")]
        [TestMethod]
        public void AdditionString_StringAndString_ReturnsConcatenatedString(string input1, string input2, string expected)
        {
            StringLiteralExpression strLit1 = new StringLiteralExpression(input1.ToString(), 1, 1);
            StringLiteralExpression strLit2 = new StringLiteralExpression(input2.ToString(), 2, 2);
            AdditionExpression addExpr = new AdditionExpression(strLit1, strLit2, 1, 1);
            IInterpreterString parent = Substitute.For<IInterpreterString>();
            parent.DispatchString(strLit1, Arg.Any<List<object>>()).Returns(input1);
            parent.DispatchString(strLit2, Arg.Any<List<object>>()).Returns(input2);
            StringHelper stringHelper = SetUpHelper(parent);

            string res = stringHelper.AdditionString(addExpr, new List<object>());

            Assert.AreEqual(expected, res);
        }
        #endregion

        #region LiteralString

        [DataRow("test", "test")]
        [DataRow("", "")]
        [TestMethod]
        public void LiteralString_String_ReturnsCorrectResult(string input, string expected)
        {
            StringLiteralExpression stringLit = new StringLiteralExpression(input, 1, 1);
            StringHelper stringHelper = new StringHelper();

            string res = stringHelper.LiteralString(stringLit, new List<object>());

            Assert.AreEqual(expected, res);
        }

        #endregion

        #region CastIntegerToString
        [DataRow(1, "1")]
        [TestMethod]
        public void CastIntegerToString__Correct(int input, string expected)
        {
            var inputNode = Utilities.GetIntLitExpression();
            var castNode = Utilities.GetCastNode(inputNode);
            var parent = Utilities.GetStringInterpreter();
            parent.Dispatch(Arg.Any<IntegerLiteralExpression>(), Arg.Any<List<object>>(), Arg.Is<TypeEnum>(x => x == TypeEnum.Integer)).Returns(input);
            var stringHelper = SetUpHelper(parent);

            string res = stringHelper.CastIntegerToString(castNode, new List<object>());

            Assert.AreEqual(expected, res);
        }
        #endregion

        #region CastBooleanToString
        #endregion

        #region CastRealToString
        #endregion

    }
}
