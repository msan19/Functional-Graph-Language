using ASTLib.Exceptions;
using ASTLib.Nodes;
using ASTLib.Nodes.ExpressionNodes;
using FluentAssertions;
using InterpreterLib.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Text;

namespace InterpreterLib.Tests
{
    [TestClass]
    public class Interpreter_Function_Tests
    {

        #region DispatchFunction

        #region DispatchFunction_Identifier   
        [TestMethod]
        public void DispatchFunction_IndentifierAndObjectList_CorrectListPassed()
        {
            List<Object> expected = new List<Object>() { 23, 2.334, null };
            IdentifierExpression input1 = new IdentifierExpression("", 0, 0);
            IFunctionHelper fhelper = Substitute.For<IFunctionHelper>();
            Interpreter interpreter = Utilities.GetIntepretorOnlyWith(fhelper);
            List<Object> res = null;
            fhelper.IdentifierFunction(Arg.Any<IdentifierExpression>(), Arg.Do<List<Object>>(x => res = x));

            interpreter.DispatchFunction(input1, expected);

            res.Should().BeEquivalentTo(expected);
        }

        [TestMethod]
        public void DispatchFunction_IdentifierAndObjectList_CorrectFunctionCallExprPassed()
        {
            IdentifierExpression expected = new IdentifierExpression("", 0, 0);
            IdentifierExpression input1 = expected;
            List<Object> input2 = new List<Object>() { 23, 2.334, null };
            IFunctionHelper fhelper = Substitute.For<IFunctionHelper>();
            Interpreter interpreter = Utilities.GetIntepretorOnlyWith(fhelper);
            IdentifierExpression res = null;
            fhelper.IdentifierFunction(Arg.Do<IdentifierExpression>(x => res = x), Arg.Any<List<Object>>());

            interpreter.DispatchFunction(input1, input2);

            res.Should().BeEquivalentTo(expected);
        }

        [TestMethod]
        public void DispatchFunction_IdentifierAndObjectList_CorrectValueReturned()
        {
            int expected = 17;
            IdentifierExpression input1 = new IdentifierExpression("", 0, 0);
            List<Object> input2 = new List<Object>() { 23, 2.334, null };
            IFunctionHelper fhelper = Substitute.For<IFunctionHelper>();
            Interpreter interpreter = Utilities.GetIntepretorOnlyWith(fhelper);
            fhelper.IdentifierFunction(Arg.Any<IdentifierExpression>(), Arg.Any<List<Object>>()).Returns(expected);

            int res = interpreter.DispatchFunction(input1, input2);

            Assert.AreEqual(expected, res);
        }
        #endregion

        #endregion

    }
}
