using ASTLib.Exceptions;
using ASTLib.Nodes;
using ASTLib.Nodes.ExpressionNodes;
using ASTLib.Nodes.ExpressionNodes.OperationNodes;
using FluentAssertions;
using InterpreterLib.Interfaces;
using InterpreterLib.MatchPair;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.Collections.Generic;

namespace InterpreterLib.Tests
{
    [TestClass]
    public class Interpreter_Generic_Tests
    {
        #region FunctionInteger
        [TestMethod]
        public void FunctionInteger_FunctionNodeAndObjectList_CorrectListPassed()
        {
            List<Object> expected = new List<Object>() { 23, 2.334, null };
            FunctionNode input1 = new FunctionNode("", new ConditionNode(null, 0, 0), null, null, 0, 0);
            IGenericHelper ihelper = Substitute.For<IGenericHelper>();
            Interpreter interpreter = Utilities.GetIntepretorOnlyWith(ihelper);
            List<Object> res = null;
            ihelper.Condition<int>(Arg.Any<ConditionNode>(), 
                                   Arg.Do<List<Object>>(x => res = x)).
                                   Returns(new MatchPair<int>(true, 1));

            interpreter.Function<int>(input1, expected);

            res.Should().BeEquivalentTo(expected);
        }

        [TestMethod]
        public void FunctionInteger_FunctionNodeAndObjectList_CorrectIntegerLiteralExprPassed()
        {
            ConditionNode expected = new ConditionNode(null, 0, 0);
            FunctionNode input1 = new FunctionNode("", expected, null, null, 0, 0);
            List<Object> input2 = new List<Object>() { 23, 2.334, null };
            IGenericHelper ihelper = Substitute.For<IGenericHelper>();
            Interpreter interpreter = Utilities.GetIntepretorOnlyWith(ihelper);
            ConditionNode res = null;
            ihelper.Condition<int>(Arg.Do<ConditionNode>(x => res = x), 
                                   Arg.Any<List<Object>>()).
                                   Returns(new MatchPair<int>(true, 1));

            interpreter.Function<int>(input1, input2);

            res.Should().BeEquivalentTo(expected);
        }

        [TestMethod]
        public void FunctionInteger_FunctionNodeAndObjectList_CorrectValueReturned()
        {
            int expected = 17;
            FunctionNode input1 = new FunctionNode("", new ConditionNode(null, 0, 0), null, null, 0, 0);
            List<Object> input2 = new List<Object>() { 23, 2.334, null };
            IGenericHelper ihelper = Substitute.For<IGenericHelper>();
            Interpreter interpreter = Utilities.GetIntepretorOnlyWith(ihelper);
            ihelper.Condition<int>(Arg.Any<ConditionNode>(), Arg.Any<List<Object>>()).Returns( new MatchPair<int>(true, expected));

            int res = (int)interpreter.Function<int>(input1, input2);

            Assert.AreEqual(expected, res);
        }

        [DataRow(true, 17, true, 13, 17)]
        [DataRow(false, 0, true, 13, 13)]
        [DataRow(true, 17, false, 0, 17)]
        [TestMethod]
        public void FunctionInteger_FunctionNodeAndTwoConditionsOneDefaultCaseAndObjectList_CorrectValueReturned(bool aValid, int a, bool bValid, int b, int expected)
        {
            ConditionNode cn1 = new ConditionNode(new BooleanLiteralExpression(true, 0, 0), null, 0, 0);
            ConditionNode cn2 = new ConditionNode(null, 0, 0);
            FunctionNode input1 = new FunctionNode(new List<ConditionNode> { cn1, cn2 }, "", null, null, 0, 0);
            List<Object> input2 = new List<Object>() { 23, 2.334, null };
            IGenericHelper fhelper = Substitute.For<IGenericHelper>();
            Interpreter interpreter = Utilities.GetIntepretorOnlyWith(fhelper);
            fhelper.Condition<int>(cn1, Arg.Any<List<Object>>()).Returns(new MatchPair<int>(aValid, a));
            fhelper.Condition<int>(cn2, Arg.Any<List<Object>>()).Returns(new MatchPair<int>(bValid, b));

            int res = (int)interpreter.Function<int>(input1, input2);

            Assert.AreEqual(expected, res);
        }

        [TestMethod]
        public void FunctionInteger_FunctionNodeAndTwoConditionAndObjectList_CorrectExceptionThrown()
        {
            ConditionNode cn1 = new ConditionNode(new BooleanLiteralExpression(true, 0, 0), null, 0, 0);
            ConditionNode cn2 = new ConditionNode(new BooleanLiteralExpression(true, 0, 0), null, 0, 0);
            FunctionNode input1 = new FunctionNode(new List<ConditionNode> { cn1, cn2 }, "", null, null, 0, 0);
            List<Object> input2 = new List<Object>() { 23, 2.334, null };
            IGenericHelper fhelper = Substitute.For<IGenericHelper>();
            Interpreter interpreter = Utilities.GetIntepretorOnlyWith(fhelper);
            fhelper.Condition<int>(cn1, Arg.Any<List<Object>>()).Returns(new MatchPair<int>(true, 17));
            fhelper.Condition<int>(cn2, Arg.Any<List<Object>>()).Returns(new MatchPair<int>(true, 18));

            Assert.ThrowsException<UnacceptedConditionsException>(() => interpreter.Function<int>(input1, input2));
        }

        [TestMethod]
        public void FunctionInteger_FunctionNodeAndTwoConditionsOneDefaultCaseAndObjectList_CorrectExceptionThrown()
        {
            ConditionNode cn1 = new ConditionNode(new BooleanLiteralExpression(true, 0, 0), null, 0, 0);
            ConditionNode cn2 = new ConditionNode(new BooleanLiteralExpression(true, 0, 0), null, 0, 0);
            FunctionNode input1 = new FunctionNode(new List<ConditionNode> { cn1, cn2 }, "", null, null, 0, 0);
            List<Object> input2 = new List<Object>() { 23, 2.334, null };
            IGenericHelper fhelper = Substitute.For<IGenericHelper>();
            Interpreter interpreter = Utilities.GetIntepretorOnlyWith(fhelper);
            fhelper.Condition<int>(cn1, Arg.Any<List<Object>>()).
                                   Returns(new MatchPair<int>(false, 1));
            fhelper.Condition<int>(cn2, Arg.Any<List<Object>>()).
                                   Returns(new MatchPair<int>(false, 1));

            Assert.ThrowsException<UnacceptedConditionsException>(() => interpreter.Function<int>(input1, input2));
        }

        #endregion

        #region DispatchInt_functionCallExpr
        [TestMethod]
        public void DispatchInteger_FunctionCallAndObjectList_CorrectListPassed()
        {
            List<Object> expected = new List<Object>() { 23, 2.334, null };
            FunctionCallExpression input1 = new FunctionCallExpression("", null, 0, 0);
            IGenericHelper ihelper = Substitute.For<IGenericHelper>();
            Interpreter interpreter = Utilities.GetIntepretorOnlyWith(ihelper);
            List<Object> res = null;
            ihelper.FunctionCall<int>(Arg.Any<FunctionCallExpression>(), Arg.Do<List<Object>>(x => res = x));

            interpreter.DispatchInt(input1, expected);

            res.Should().BeEquivalentTo(expected);
        }

        [TestMethod]
        public void DispatchInteger_FunctionCallAndObjectList_CorrectFunctionCallExprPassed()
        {
            FunctionCallExpression expected = new FunctionCallExpression("", null, 0, 0);
            FunctionCallExpression input1 = expected;
            List<Object> input2 = new List<Object>() { 23, 2.334, null };
            IGenericHelper ihelper = Substitute.For<IGenericHelper>();
            Interpreter interpreter = Utilities.GetIntepretorOnlyWith(ihelper);
            FunctionCallExpression res = null;
            ihelper.FunctionCall<int>(Arg.Do<FunctionCallExpression>(x => res = x), Arg.Any<List<Object>>());

            interpreter.DispatchInt(input1, input2);

            res.Should().BeEquivalentTo(expected);
        }

        [TestMethod]
        public void DispatchInteger_FunctionCallAndObjectList_CorrectValueReturned()
        {
            int expected = 17;
            FunctionCallExpression input1 = new FunctionCallExpression("", null, 0, 0);
            List<Object> input2 = new List<Object>() { 23, 2.334, null };
            IGenericHelper ihelper = Substitute.For<IGenericHelper>();
            Interpreter interpreter = Utilities.GetIntepretorOnlyWith(ihelper);
            ihelper.FunctionCall<int>(Arg.Any<FunctionCallExpression>(), Arg.Any<List<Object>>()).Returns(expected);

            int res = interpreter.DispatchInt(input1, input2);

            Assert.AreEqual(expected, res);
        }
        #endregion

        #region FunctionBoolean
        /*
        [DataRow(true, false, true)]
        [DataRow(null, false, false)]
        [DataRow(true, null, true)]
        [TestMethod]
        public void FunctionBoolean_FunctionNodeAndTwoConditionsOneDefaultCaseAndObjectList_CorrectValueReturned(bool? a, bool? b, bool? expected)
        {
            ConditionNode cn1 = new ConditionNode(new BooleanLiteralExpression(true, 0, 0), null, 0, 0);
            ConditionNode cn2 = new ConditionNode(null, 0, 0);
            FunctionNode input1 = new FunctionNode(new List<ConditionNode> { cn1, cn2 }, "", null, null, 0, 0);
            List<Object> input2 = new List<Object>() { 23, 2.334, null };
            IBooleanHelper fhelper = Substitute.For<IBooleanHelper>();
            Interpreter interpreter = Utilities.GetIntepretorOnlyWith(fhelper);
            fhelper.ConditionBoolean(cn1, Arg.Any<List<Object>>()).Returns(a);
            fhelper.ConditionBoolean(cn2, Arg.Any<List<Object>>()).Returns(b);

            bool res = (bool)interpreter.FunctionBoolean(input1, input2);

            Assert.AreEqual(expected, res);
        }

        [TestMethod]
        public void FunctionBoolean_FunctionNodeAndTwoConditionAndObjectList_CorrectExceptionThrown()
        {
            ConditionNode cn1 = new ConditionNode(new BooleanLiteralExpression(true, 0, 0), null, 0, 0);
            ConditionNode cn2 = new ConditionNode(new BooleanLiteralExpression(true, 0, 0), null, 0, 0);
            FunctionNode input1 = new FunctionNode(new List<ConditionNode> { cn1, cn2 }, "", null, null, 0, 0);
            List<Object> input2 = new List<Object>() { 23, 2.334, null };
            IBooleanHelper fhelper = Substitute.For<IBooleanHelper>();
            Interpreter interpreter = Utilities.GetIntepretorOnlyWith(fhelper);
            fhelper.ConditionBoolean(cn1, Arg.Any<List<Object>>()).Returns(true);
            fhelper.ConditionBoolean(cn2, Arg.Any<List<Object>>()).Returns(true);

            Assert.ThrowsException<UnacceptedConditionsException>(() => interpreter.FunctionBoolean(input1, input2));
        }

        [TestMethod]
        public void FunctionBoolean_FunctionNodeAndTwoConditionsOneDefaultCaseAndObjectList_CorrectExceptionThrown()
        {
            ConditionNode cn1 = new ConditionNode(new BooleanLiteralExpression(true, 0, 0), null, 0, 0);
            ConditionNode cn2 = new ConditionNode(new BooleanLiteralExpression(true, 0, 0), 0, 0);
            FunctionNode input1 = new FunctionNode(new List<ConditionNode> { cn1, cn2 }, "", null, null, 0, 0);
            List<Object> input2 = new List<Object>() { 23, 2.334, null };
            IBooleanHelper fhelper = Substitute.For<IBooleanHelper>();
            Interpreter interpreter = Utilities.GetIntepretorOnlyWith(fhelper);
            fhelper.ConditionBoolean(cn1, Arg.Any<List<Object>>());
            fhelper.ConditionBoolean(cn2, Arg.Any<List<Object>>());

            Assert.ThrowsException<UnacceptedConditionsException>(() => interpreter.FunctionBoolean(input1, input2));
        }
        */
        #endregion

        #region FunctionFunction
        /*
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
            FunctionNode input1 = new FunctionNode(new List<ConditionNode> { cn1, cn2 }, "", null, null, 0, 0);
            List<Object> input2 = new List<Object>() { 23, 2.334, null };
            IFunctionHelper fhelper = Substitute.For<IFunctionHelper>();
            Interpreter interpreter = Utilities.GetIntepretorOnlyWith(fhelper);
            fhelper.ConditionFunction(cn1, Arg.Any<List<Object>>()).Returns(a);
            fhelper.ConditionFunction(cn2, Arg.Any<List<Object>>()).Returns(b);

            int res = (int)interpreter.FunctionFunction(input1, input2);

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

            Assert.ThrowsException<UnacceptedConditionsException>(() => interpreter.FunctionFunction(input1, input2));
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

            Assert.ThrowsException<UnacceptedConditionsException>(() => interpreter.FunctionFunction(input1, input2));
        }

        #endregion

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
        */
        #endregion



        #region FunctionReal
        /*
        [TestMethod]
        public void FunctionReal_FunctionNodeAndObjectList_CorrectListPassed()
        {
            List<Object> expected = new List<Object>() { 23, 2.334, null };
            FunctionNode input1 = new FunctionNode("", new ConditionNode(null, 0, 0), null, null, 0, 0);
            IRealHelper rhelper = Substitute.For<IRealHelper>();
            Interpreter interpreter = Utilities.GetIntepretorOnlyWith(rhelper);
            List<Object> res = null;
            rhelper.ConditionReal(Arg.Any<ConditionNode>(), Arg.Do<List<Object>>(x => res = x)).Returns(1.0);

            interpreter.FunctionReal(input1, expected);

            res.Should().BeEquivalentTo(expected);
        }

        [TestMethod]
        public void FunctionReal_FunctionNodeAndObjectList_CorrectRealLiteralExprPassed()
        {
            ConditionNode expected = new ConditionNode(null, 0, 0);
            FunctionNode input1 = new FunctionNode("", expected, null, null, 0, 0);
            List<Object> input2 = new List<Object>() { 23, 2.334, null };
            IRealHelper rhelper = Substitute.For<IRealHelper>();
            Interpreter interpreter = Utilities.GetIntepretorOnlyWith(rhelper);
            ConditionNode res = null;
            rhelper.ConditionReal(Arg.Do<ConditionNode>(x => res = x), Arg.Any<List<Object>>()).Returns(1.0);

            interpreter.FunctionReal(input1, input2);

            res.Should().BeEquivalentTo(expected);
        }

        [TestMethod]
        public void FunctionReal_FunctionNodeAndObjectList_CorrectValueReturned()
        {
            double expected = 17;
            FunctionNode input1 = new FunctionNode("", new ConditionNode(null, 0, 0), null, null, 0, 0);
            List<Object> input2 = new List<Object>() { 23, 2.334, null };
            IRealHelper rhelper = Substitute.For<IRealHelper>();
            Interpreter interpreter = Utilities.GetIntepretorOnlyWith(rhelper);
            rhelper.ConditionReal(Arg.Any<ConditionNode>(), Arg.Any<List<Object>>()).Returns(expected);

            double res = (double)interpreter.FunctionReal(input1, input2);

            Assert.AreEqual(expected, res);
        }

        [DataRow(17.1, 13.2, 17.1)]
        [DataRow(null, 13.2, 13.2)]
        [DataRow(17.1, null, 17.1)]
        [TestMethod]
        public void FunctionReal_FunctionNodeAndTwoConditionsOneDefaultCaseAndObjectList_CorrectValueReturned(double? a, double? b, double? expected)
        {
            ConditionNode cn1 = new ConditionNode(new BooleanLiteralExpression(true, 0, 0), null, 0, 0);
            ConditionNode cn2 = new ConditionNode(null, 0, 0);
            FunctionNode input1 = new FunctionNode(new List<ConditionNode> { cn1, cn2 }, "", null, null, 0, 0);
            List<Object> input2 = new List<Object>() { 23, 2.334, null };
            IRealHelper fhelper = Substitute.For<IRealHelper>();
            Interpreter interpreter = Utilities.GetIntepretorOnlyWith(fhelper);
            fhelper.ConditionReal(cn1, Arg.Any<List<Object>>()).Returns(a);
            fhelper.ConditionReal(cn2, Arg.Any<List<Object>>()).Returns(b);

            double res = (double)interpreter.FunctionReal(input1, input2);

            Assert.AreEqual(expected, res);
        }

        [TestMethod]
        public void FunctionReal_FunctionNodeAndTwoConditionAndObjectList_CorrectExceptionThrown()
        {
            ConditionNode cn1 = new ConditionNode(new BooleanLiteralExpression(true, 0, 0), null, 0, 0);
            ConditionNode cn2 = new ConditionNode(new BooleanLiteralExpression(true, 0, 0), null, 0, 0);
            FunctionNode input1 = new FunctionNode(new List<ConditionNode> { cn1, cn2 }, "", null, null, 0, 0);
            List<Object> input2 = new List<Object>() { 23, 2.334, null };
            IRealHelper fhelper = Substitute.For<IRealHelper>();
            Interpreter interpreter = Utilities.GetIntepretorOnlyWith(fhelper);
            fhelper.ConditionReal(cn1, Arg.Any<List<Object>>()).Returns(17.1);
            fhelper.ConditionReal(cn2, Arg.Any<List<Object>>()).Returns(18.3);

            Assert.ThrowsException<UnacceptedConditionsException>(() => interpreter.FunctionReal(input1, input2));
        }

        [TestMethod]
        public void FunctionReal_FunctionNodeAndTwoConditionsOneDefaultCaseAndObjectList_CorrectExceptionThrown()
        {
            ConditionNode cn1 = new ConditionNode(new BooleanLiteralExpression(true, 0, 0), null, 0, 0);
            ConditionNode cn2 = new ConditionNode(new BooleanLiteralExpression(true, 0, 0), 0, 0);
            FunctionNode input1 = new FunctionNode(new List<ConditionNode> { cn1, cn2 }, "", null, null, 0, 0);
            List<Object> input2 = new List<Object>() { 23, 2.334, null };
            IRealHelper fhelper = Substitute.For<IRealHelper>();
            Interpreter interpreter = Utilities.GetIntepretorOnlyWith(fhelper);
            fhelper.ConditionReal(cn1, Arg.Any<List<Object>>());
            fhelper.ConditionReal(cn2, Arg.Any<List<Object>>());

            Assert.ThrowsException<UnacceptedConditionsException>(() => interpreter.FunctionReal(input1, input2));
        }

        #endregion


        #region DispatchReal_functionCallExpr
        [TestMethod]
        public void DispatchReal_FunctionCallAndObjectList_CorrectListPassed()
        {
            List<Object> expected = new List<Object>() { 23, 2.334, null };
            FunctionCallExpression input1 = new FunctionCallExpression("", null, 0, 0);
            IRealHelper rhelper = Substitute.For<IRealHelper>();
            Interpreter interpreter = Utilities.GetIntepretorOnlyWith(rhelper);
            List<Object> res = null;
            rhelper.FunctionCallReal(Arg.Any<FunctionCallExpression>(), Arg.Do<List<Object>>(x => res = x));

            interpreter.DispatchReal(input1, expected);

            res.Should().BeEquivalentTo(expected);
        }

        [TestMethod]
        public void DispatchReal_FunctionCallAndObjectList_CorrectFunctionCallExprPassed()
        {
            FunctionCallExpression expected = new FunctionCallExpression("", null, 0, 0);
            FunctionCallExpression input1 = expected;
            List<Object> input2 = new List<Object>() { 23, 2.334, null };
            IRealHelper rhelper = Substitute.For<IRealHelper>();
            Interpreter interpreter = Utilities.GetIntepretorOnlyWith(rhelper);
            FunctionCallExpression res = null;
            rhelper.FunctionCallReal(Arg.Do<FunctionCallExpression>(x => res = x), Arg.Any<List<Object>>());

            interpreter.DispatchReal(input1, input2);

            res.Should().BeEquivalentTo(expected);
        }

        [TestMethod]
        public void DispatchReal_FunctionCallAndObjectList_CorrectValueReturned()
        {
            double expected = 17;
            FunctionCallExpression input1 = new FunctionCallExpression("", null, 0, 0);
            List<Object> input2 = new List<Object>() { 23, 2.334, null };
            IRealHelper rhelper = Substitute.For<IRealHelper>();
            Interpreter interpreter = Utilities.GetIntepretorOnlyWith(rhelper);
            rhelper.FunctionCallReal(Arg.Any<FunctionCallExpression>(), Arg.Any<List<Object>>()).Returns(expected);

            double res = interpreter.DispatchReal(input1, input2);

            Assert.AreEqual(expected, res);
        }
        */
        #endregion

    }
}
