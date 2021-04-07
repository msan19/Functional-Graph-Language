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

        #region FunctionFunction
        [TestMethod]
        public void FunctionFunction_FunctionNodeAndObjectList_CorrectListPassed()
        {
            List<Object> expected = new List<Object>() { 23, 2.334, null };
            FunctionNode input1 = new FunctionNode("", new ConditionNode(null, 0, 0), null, null, 0, 0);
            IFunctionHelper fhelper = Substitute.For<IFunctionHelper>();
            Interpreter interpreter = Utilities.GetIntepretorOnlyWith(fhelper);
            List<Object> res = null;
            fhelper.ConditionFunction(Arg.Any<ConditionNode>(), Arg.Do<List<Object>>(x => res = x)).Returns(0);

            interpreter.FunctionFunction(input1, expected);

            res.Should().BeEquivalentTo(expected);
        }

        [TestMethod]
        public void FunctionFunction_FunctionNodeAndObjectList_CorrectIntegerLiteralExprPassed()
        {
            ConditionNode expected = new ConditionNode(null, 0, 0);
            FunctionNode input1 = new FunctionNode("", expected, null, null, 0, 0);
            List<Object> input2 = new List<Object>() { 23, 2.334, null };
            IFunctionHelper fhelper = Substitute.For<IFunctionHelper>();
            Interpreter interpreter = Utilities.GetIntepretorOnlyWith(fhelper);
            ConditionNode res = null;
            fhelper.ConditionFunction(Arg.Do<ConditionNode>(x => res = x), Arg.Any<List<Object>>()).Returns(0);

            interpreter.FunctionFunction(input1, input2);

            res.Should().BeEquivalentTo(expected);
        }

        [TestMethod]
        public void FunctionFunction_FunctionNodeAndObjectList_CorrectValueReturned()
        {
            int expected = 17;
            FunctionNode input1 = new FunctionNode("", new ConditionNode(null, 0, 0), null, null, 0, 0);
            List<Object> input2 = new List<Object>() { 23, 2.334, null };
            IFunctionHelper fhelper = Substitute.For<IFunctionHelper>();
            Interpreter interpreter = Utilities.GetIntepretorOnlyWith(fhelper);
            fhelper.ConditionFunction(Arg.Any<ConditionNode>(), Arg.Any<List<Object>>()).Returns(expected);

            int res = (int)interpreter.FunctionFunction(input1, input2);

            Assert.AreEqual(expected, res);
        }

        [DataRow(17, 13, 17)]
        [DataRow(null, 13, 13)]
        [DataRow(17, null, 17)]
        [TestMethod]
        public void FunctionFunction_FunctionNodeAndTwoConditionsOneDefaultCaseAndObjectList_CorrectValueReturned(int? a, int? b, int? expected)
        {
            ConditionNode cn1 = new ConditionNode(new BooleanLiteralExpression(true, 0, 0), null, 0, 0);
            ConditionNode cn2 = new ConditionNode(null, 0, 0);
            FunctionNode input1 = new FunctionNode(new List<ConditionNode> { cn1, cn2 }, "" , null, null, 0, 0);
            List<Object> input2 = new List<Object>() { 23, 2.334, null };
            IFunctionHelper fhelper = Substitute.For<IFunctionHelper>();
            Interpreter interpreter = Utilities.GetIntepretorOnlyWith(fhelper);
            fhelper.ConditionFunction(cn1, Arg.Any<List<Object>>()).Returns(a);
            fhelper.ConditionFunction(cn2, Arg.Any<List<Object>>()).Returns(b);

            int res = (int) interpreter.FunctionFunction(input1, input2);

            Assert.AreEqual(expected, res);
        }

        [TestMethod]
        public void FunctionFunction_FunctionNodeAndTwoConditionAndObjectList_CorrectExceptionThrown()
        {
            ConditionNode cn1 = new ConditionNode(new BooleanLiteralExpression(true, 0, 0), null, 0, 0);
            ConditionNode cn2 = new ConditionNode(new BooleanLiteralExpression(true, 0, 0), null, 0, 0);
            FunctionNode input1 = new FunctionNode(new List<ConditionNode> { cn1, cn2 }, "", null, null, 0, 0);
            List<Object> input2 = new List<Object>() { 23, 2.334, null };
            IFunctionHelper fhelper = Substitute.For<IFunctionHelper>();
            Interpreter interpreter = Utilities.GetIntepretorOnlyWith(fhelper);
            fhelper.ConditionFunction(cn1, Arg.Any<List<Object>>()).Returns(17);
            fhelper.ConditionFunction(cn2, Arg.Any<List<Object>>()).Returns(18);

            Assert.ThrowsException<Exception>(() => interpreter.FunctionFunction(input1, input2));
        }

        [TestMethod]
        public void FunctionFunction_FunctionNodeAndTwoConditionsOneDefaultCaseAndObjectList_CorrectExceptionThrown()
        {
            ConditionNode cn1 = new ConditionNode(new BooleanLiteralExpression(true, 0, 0), null, 0, 0);
            ConditionNode cn2 = new ConditionNode(new BooleanLiteralExpression(true, 0, 0), 0, 0);
            FunctionNode input1 = new FunctionNode(new List<ConditionNode> { cn1, cn2 }, "", null, null, 0, 0);
            List<Object> input2 = new List<Object>() { 23, 2.334, null };
            IFunctionHelper fhelper = Substitute.For<IFunctionHelper>();
            Interpreter interpreter = Utilities.GetIntepretorOnlyWith(fhelper);
            fhelper.ConditionFunction(cn1, Arg.Any<List<Object>>());
            fhelper.ConditionFunction(cn2, Arg.Any<List<Object>>());

            Assert.ThrowsException<Exception>(() => interpreter.FunctionFunction(input1, input2));
        }

        #endregion

        #region DispatchFunction
        #region DispatchFunction_FunctionCall   
        [TestMethod]
        public void DispatchFunction_FunctionCallAndObjectList_CorrectListPassed()
        {
            List<Object> expected = new List<Object>() { 23, 2.334, null };
            FunctionCallExpression input1 = new FunctionCallExpression("", null, 0, 0);
            IFunctionHelper fhelper = Substitute.For<IFunctionHelper>();
            Interpreter interpreter = Utilities.GetIntepretorOnlyWith(fhelper);

            List<Object> res = null;
            fhelper.FunctionCallFunction(Arg.Any<FunctionCallExpression>(), Arg.Do<List<Object>>(x => res = x));

            interpreter.DispatchFunction(input1, expected);

            res.Should().BeEquivalentTo(expected);
        }

        [TestMethod]
        public void DispatchFunction_FunctionCallAndObjectList_CorrectFunctionCallExprPassed()
        {
            FunctionCallExpression expected = new FunctionCallExpression("", null, 0, 0);
            FunctionCallExpression input1 = expected;
            List<Object> input2 = new List<Object>() { 23, 2.334, null };
            IFunctionHelper fhelper = Substitute.For<IFunctionHelper>();
            Interpreter interpreter = Utilities.GetIntepretorOnlyWith(fhelper);

            FunctionCallExpression res = null;
            fhelper.FunctionCallFunction(Arg.Do<FunctionCallExpression>(x => res = x), Arg.Any<List<Object>>());

            interpreter.DispatchFunction(input1, input2);

            res.Should().BeEquivalentTo(expected);
        }

        [TestMethod]
        public void DispatchFunction_FunctionCallAndObjectList_CorrectValueReturned()
        {
            int expected = 17;
            FunctionCallExpression input1 = new FunctionCallExpression("", null, 0, 0);
            List<Object> input2 = new List<Object>() { 23, 2.334, null };
            IFunctionHelper fhelper = Substitute.For<IFunctionHelper>();
            Interpreter interpreter = Utilities.GetIntepretorOnlyWith(fhelper);
            fhelper.FunctionCallFunction(Arg.Any<FunctionCallExpression>(), Arg.Any<List<Object>>()).Returns(expected);

            int res = interpreter.DispatchFunction(input1, input2);

            Assert.AreEqual(expected, res);
        }
        #endregion

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
