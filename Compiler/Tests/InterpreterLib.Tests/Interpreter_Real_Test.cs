using ASTLib.Nodes;
using ASTLib.Nodes.ExpressionNodes;
using ASTLib.Nodes.ExpressionNodes.OperationNodes;
using FluentAssertions;
using InterpreterLib.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.Collections.Generic;

namespace InterpreterLib.Tests
{
    [TestClass]
    public class Interpreter_Real_Test
    {


        #region FunctionReal
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

            Assert.ThrowsException<Exception>(() => interpreter.FunctionReal(input1, input2));
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

            Assert.ThrowsException<Exception>(() => interpreter.FunctionReal(input1, input2));
        }

        #endregion

        #region DispatchReal
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
        #endregion

        #region DispatchReal_AdditionExpr
        [TestMethod]
        public void DispatchReal_AdditionAndObjectList_CorrectListPassed()
        {
            List<Object> expected = new List<Object>() { 23, 2.334, null };
            AdditionExpression input1 = new AdditionExpression(null, null, 0, 0);
            IRealHelper rhelper = Substitute.For<IRealHelper>();
            Interpreter interpreter = Utilities.GetIntepretorOnlyWith(rhelper);
            List<Object> res = null;
            rhelper.AdditionReal(Arg.Any<AdditionExpression>(), Arg.Do<List<Object>>(x => res = x));

            interpreter.DispatchReal(input1, expected);

            res.Should().BeEquivalentTo(expected);
        }

        [TestMethod]
        public void DispatchReal_AdditionAndObjectList_CorrectAdditionExprPassed()
        {
            AdditionExpression expected = new AdditionExpression(null, null, 0, 0);
            AdditionExpression input1 = expected;
            List<Object> input2 = new List<Object>() { 23, 2.334, null };
            IRealHelper rhelper = Substitute.For<IRealHelper>();
            Interpreter interpreter = Utilities.GetIntepretorOnlyWith(rhelper);
            AdditionExpression res = null;
            rhelper.AdditionReal(Arg.Do<AdditionExpression>(x => res = x), Arg.Any<List<Object>>());

            interpreter.DispatchReal(input1, input2);

            res.Should().BeEquivalentTo(expected);
        }

        [TestMethod]
        public void DispatchReal_AdditionAndObjectList_CorrectValueReturned()
        {
            double expected = 17;
            AdditionExpression input1 = new AdditionExpression(null, null, 0, 0);
            List<Object> input2 = new List<Object>() { 23, 2.334, null };
            IRealHelper rhelper = Substitute.For<IRealHelper>();
            Interpreter interpreter = Utilities.GetIntepretorOnlyWith(rhelper);
            rhelper.AdditionReal(Arg.Any<AdditionExpression>(), Arg.Any<List<Object>>()).Returns(expected);

            double res = interpreter.DispatchReal(input1, input2);

            Assert.AreEqual(expected, res);
        }
        #endregion

        #region DispatchReal_SubtractionExpr
        [TestMethod]
        public void DispatchReal_SubtractionAndObjectList_CorrectListPassed()
        {
            List<Object> expected = new List<Object>() { 23, 2.334, null };
            SubtractionExpression input1 = new SubtractionExpression(null, null, 0, 0);
            IRealHelper rhelper = Substitute.For<IRealHelper>();
            Interpreter interpreter = Utilities.GetIntepretorOnlyWith(rhelper);
            List<Object> res = null;
            rhelper.SubtractionReal(Arg.Any<SubtractionExpression>(), Arg.Do<List<Object>>(x => res = x));

            interpreter.DispatchReal(input1, expected);

            res.Should().BeEquivalentTo(expected);
        }

        [TestMethod]
        public void DispatchReal_SubtractionAndObjectList_CorrectSubtractionExprPassed()
        {
            SubtractionExpression expected = new SubtractionExpression(null, null, 0, 0);
            SubtractionExpression input1 = expected;
            List<Object> input2 = new List<Object>() { 23, 2.334, null };
            IRealHelper rhelper = Substitute.For<IRealHelper>();
            Interpreter interpreter = Utilities.GetIntepretorOnlyWith(rhelper);
            SubtractionExpression res = null;
            rhelper.SubtractionReal(Arg.Do<SubtractionExpression>(x => res = x), Arg.Any<List<Object>>());

            interpreter.DispatchReal(input1, input2);

            res.Should().BeEquivalentTo(expected);
        }

        [TestMethod]
        public void DispatchReal_SubtractionAndObjectList_CorrectValueReturned()
        {
            double expected = 17;
            SubtractionExpression input1 = new SubtractionExpression(null, null, 0, 0);
            List<Object> input2 = new List<Object>() { 23, 2.334, null };
            IRealHelper rhelper = Substitute.For<IRealHelper>();
            Interpreter interpreter = Utilities.GetIntepretorOnlyWith(rhelper);
            rhelper.SubtractionReal(Arg.Any<SubtractionExpression>(), Arg.Any<List<Object>>()).Returns(expected);

            double res = interpreter.DispatchReal(input1, input2);

            Assert.AreEqual(expected, res);
        }
        #endregion

        #region DispatchReal_MultiplicationExpr
        [TestMethod]
        public void DispatchReal_MultiplicationAndObjectList_CorrectListPassed()
        {
            List<Object> expected = new List<Object>() { 23, 2.334, null };
            MultiplicationExpression input1 = new MultiplicationExpression(null, null, 0, 0);
            IRealHelper rhelper = Substitute.For<IRealHelper>();
            Interpreter interpreter = Utilities.GetIntepretorOnlyWith(rhelper);
            List<Object> res = null;
            rhelper.MultiplicationReal(Arg.Any<MultiplicationExpression>(), Arg.Do<List<Object>>(x => res = x));

            interpreter.DispatchReal(input1, expected);

            res.Should().BeEquivalentTo(expected);
        }

        [TestMethod]
        public void DispatchReal_MultiplicationAndObjectList_CorrectMultiplicationExprPassed()
        {
            MultiplicationExpression expected = new MultiplicationExpression(null, null, 0, 0);
            MultiplicationExpression input1 = expected;
            List<Object> input2 = new List<Object>() { 23, 2.334, null };
            IRealHelper rhelper = Substitute.For<IRealHelper>();
            Interpreter interpreter = Utilities.GetIntepretorOnlyWith(rhelper);
            MultiplicationExpression res = null;
            rhelper.MultiplicationReal(Arg.Do<MultiplicationExpression>(x => res = x), Arg.Any<List<Object>>());

            interpreter.DispatchReal(input1, input2);

            res.Should().BeEquivalentTo(expected);
        }

        [TestMethod]
        public void DispatchReal_MultiplicationAndObjectList_CorrectValueReturned()
        {
            double expected = 17;
            MultiplicationExpression input1 = new MultiplicationExpression(null, null, 0, 0);
            List<Object> input2 = new List<Object>() { 23, 2.334, null };
            IRealHelper rhelper = Substitute.For<IRealHelper>();
            Interpreter interpreter = Utilities.GetIntepretorOnlyWith(rhelper);
            rhelper.MultiplicationReal(Arg.Any<MultiplicationExpression>(), Arg.Any<List<Object>>()).Returns(expected);

            double res = interpreter.DispatchReal(input1, input2);

            Assert.AreEqual(expected, res);
        }
        #endregion

        #region DispatchReal_PowerExpr
        [TestMethod]
        public void DispatchReal_PowerAndObjectList_CorrectListPassed()
        {
            List<Object> expected = new List<Object>() { 23, 2.334, null };
            PowerExpression input1 = new PowerExpression(null, null, 0, 0);
            IRealHelper rhelper = Substitute.For<IRealHelper>();
            Interpreter interpreter = Utilities.GetIntepretorOnlyWith(rhelper);
            List<Object> res = null;
            rhelper.PowerReal(Arg.Any<PowerExpression>(), Arg.Do<List<Object>>(x => res = x));

            interpreter.DispatchReal(input1, expected);

            res.Should().BeEquivalentTo(expected);
        }

        [TestMethod]
        public void DispatchReal_PowerAndObjectList_CorrectPowerExprPassed()
        {
            PowerExpression expected = new PowerExpression(null, null, 0, 0);
            PowerExpression input1 = expected;
            List<Object> input2 = new List<Object>() { 23, 2.334, null };
            IRealHelper rhelper = Substitute.For<IRealHelper>();
            Interpreter interpreter = Utilities.GetIntepretorOnlyWith(rhelper);
            PowerExpression res = null;
            rhelper.PowerReal(Arg.Do<PowerExpression>(x => res = x), Arg.Any<List<Object>>());

            interpreter.DispatchReal(input1, input2);

            res.Should().BeEquivalentTo(expected);
        }

        [TestMethod]
        public void DispatchReal_PowerAndObjectList_CorrectValueReturned()
        {
            double expected = 17;
            PowerExpression input1 = new PowerExpression(null, null, 0, 0);
            List<Object> input2 = new List<Object>() { 23, 2.334, null };
            IRealHelper rhelper = Substitute.For<IRealHelper>();
            Interpreter interpreter = Utilities.GetIntepretorOnlyWith(rhelper);
            rhelper.PowerReal(Arg.Any<PowerExpression>(), Arg.Any<List<Object>>()).Returns(expected);

            double res = interpreter.DispatchReal(input1, input2);

            Assert.AreEqual(expected, res);
        }
        #endregion

        #region DispatchReal_DivisionExpr
        [TestMethod]
        public void DispatchReal_DivisionAndObjectList_CorrectListPassed()
        {
            List<Object> expected = new List<Object>() { 23, 2.334, null };
            DivisionExpression input1 = new DivisionExpression(null, null, 0, 0);
            IRealHelper rhelper = Substitute.For<IRealHelper>();
            Interpreter interpreter = Utilities.GetIntepretorOnlyWith(rhelper);
            List<Object> res = null;
            rhelper.DivisionReal(Arg.Any<DivisionExpression>(), Arg.Do<List<Object>>(x => res = x));

            interpreter.DispatchReal(input1, expected);

            res.Should().BeEquivalentTo(expected);
        }

        [TestMethod]
        public void DispatchReal_DivisionAndObjectList_CorrectDivisionExprPassed()
        {
            DivisionExpression expected = new DivisionExpression(null, null, 0, 0);
            DivisionExpression input1 = expected;
            List<Object> input2 = new List<Object>() { 23, 2.334, null };
            IRealHelper rhelper = Substitute.For<IRealHelper>();
            Interpreter interpreter = Utilities.GetIntepretorOnlyWith(rhelper);
            DivisionExpression res = null;
            rhelper.DivisionReal(Arg.Do<DivisionExpression>(x => res = x), Arg.Any<List<Object>>());

            interpreter.DispatchReal(input1, input2);

            res.Should().BeEquivalentTo(expected);
        }

        [TestMethod]
        public void DispatchReal_DivisionAndObjectList_CorrectValueReturned()
        {
            double expected = 17;
            DivisionExpression input1 = new DivisionExpression(null, null, 0, 0);
            List<Object> input2 = new List<Object>() { 23, 2.334, null };
            IRealHelper rhelper = Substitute.For<IRealHelper>();
            Interpreter interpreter = Utilities.GetIntepretorOnlyWith(rhelper);
            rhelper.DivisionReal(Arg.Any<DivisionExpression>(), Arg.Any<List<Object>>()).Returns(expected);

            double res = interpreter.DispatchReal(input1, input2);

            Assert.AreEqual(expected, res);
        }
        #endregion

        #region DispatchReal_ModuloExpr
        [TestMethod]
        public void DispatchReal_ModuloAndObjectList_CorrectListPassed()
        {
            List<Object> expected = new List<Object>() { 23, 2.334, null };
            ModuloExpression input1 = new ModuloExpression(null, null, 0, 0);
            IRealHelper rhelper = Substitute.For<IRealHelper>();
            Interpreter interpreter = Utilities.GetIntepretorOnlyWith(rhelper);
            List<Object> res = null;
            rhelper.ModuloReal(Arg.Any<ModuloExpression>(), Arg.Do<List<Object>>(x => res = x));

            interpreter.DispatchReal(input1, expected);

            res.Should().BeEquivalentTo(expected);
        }

        [TestMethod]
        public void DispatchReal_ModuloAndObjectList_CorrectModuloExprPassed()
        {
            ModuloExpression expected = new ModuloExpression(null, null, 0, 0);
            ModuloExpression input1 = expected;
            List<Object> input2 = new List<Object>() { 23, 2.334, null };
            IRealHelper rhelper = Substitute.For<IRealHelper>();
            Interpreter interpreter = Utilities.GetIntepretorOnlyWith(rhelper);
            ModuloExpression res = null;
            rhelper.ModuloReal(Arg.Do<ModuloExpression>(x => res = x), Arg.Any<List<Object>>());

            interpreter.DispatchReal(input1, input2);

            res.Should().BeEquivalentTo(expected);
        }

        [TestMethod]
        public void DispatchReal_ModuloAndObjectList_CorrectValueReturned()
        {
            double expected = 17;
            ModuloExpression input1 = new ModuloExpression(null, null, 0, 0);
            List<Object> input2 = new List<Object>() { 23, 2.334, null };
            IRealHelper rhelper = Substitute.For<IRealHelper>();
            Interpreter interpreter = Utilities.GetIntepretorOnlyWith(rhelper);
            rhelper.ModuloReal(Arg.Any<ModuloExpression>(), Arg.Any<List<Object>>()).Returns(expected);

            double res = interpreter.DispatchReal(input1, input2);

            Assert.AreEqual(expected, res);
        }
        #endregion

        #region DispatchReal_AbsoluteValueExpr
        [TestMethod]
        public void DispatchReal_AbsoluteValueAndObjectList_CorrectListPassed()
        {
            List<Object> expected = new List<Object>() { 23, 2.334, null };
            AbsoluteValueExpression input1 = new AbsoluteValueExpression(null, 0, 0);
            IRealHelper rhelper = Substitute.For<IRealHelper>();
            Interpreter interpreter = Utilities.GetIntepretorOnlyWith(rhelper);
            List<Object> res = null;
            rhelper.AbsoluteReal(Arg.Any<AbsoluteValueExpression>(), Arg.Do<List<Object>>(x => res = x));

            interpreter.DispatchReal(input1, expected);

            res.Should().BeEquivalentTo(expected);
        }

        [TestMethod]
        public void DispatchReal_AbsoluteValueAndObjectList_CorrectAbsoluteValueExprPassed()
        {
            AbsoluteValueExpression expected = new AbsoluteValueExpression(null, 0, 0);
            AbsoluteValueExpression input1 = expected;
            List<Object> input2 = new List<Object>() { 23, 2.334, null };
            IRealHelper rhelper = Substitute.For<IRealHelper>();
            Interpreter interpreter = Utilities.GetIntepretorOnlyWith(rhelper);
            AbsoluteValueExpression res = null;
            rhelper.AbsoluteReal(Arg.Do<AbsoluteValueExpression>(x => res = x), Arg.Any<List<Object>>());

            interpreter.DispatchReal(input1, input2);

            res.Should().BeEquivalentTo(expected);
        }

        [TestMethod]
        public void DispatchReal_AbsoluteValueAndObjectList_CorrectValueReturned()
        {
            double expected = 17;
            AbsoluteValueExpression input1 = new AbsoluteValueExpression(null, 0, 0);
            List<Object> input2 = new List<Object>() { 23, 2.334, null };
            IRealHelper rhelper = Substitute.For<IRealHelper>();
            Interpreter interpreter = Utilities.GetIntepretorOnlyWith(rhelper);
            rhelper.AbsoluteReal(Arg.Any<AbsoluteValueExpression>(), Arg.Any<List<Object>>()).Returns(expected);

            double res = interpreter.DispatchReal(input1, input2);

            Assert.AreEqual(expected, res);
        }
        #endregion

        #region DispatchReal_CastFromIntegerExpr
        [TestMethod]
        public void DispatchReal_CastFromIntegerAndObjectList_CorrectListPassed()
        {
            List<Object> expected = new List<Object>() { 23, 2.334, null };
            CastFromIntegerExpression input1 = new CastFromIntegerExpression(null, 0, 0);
            IRealHelper rhelper = Substitute.For<IRealHelper>();
            Interpreter interpreter = Utilities.GetIntepretorOnlyWith(rhelper);
            List<Object> res = null;
            rhelper.CastIntegerToReal(Arg.Any<CastFromIntegerExpression>(), Arg.Do<List<Object>>(x => res = x));

            interpreter.DispatchReal(input1, expected);

            res.Should().BeEquivalentTo(expected);
        }

        [TestMethod]
        public void DispatchReal_CastFromIntegerAndObjectList_CorrectCastFromIntegerExprPassed()
        {
            CastFromIntegerExpression expected = new CastFromIntegerExpression(null, 0, 0);
            CastFromIntegerExpression input1 = expected;
            List<Object> input2 = new List<Object>() { 23, 2.334, null };
            IRealHelper rhelper = Substitute.For<IRealHelper>();
            Interpreter interpreter = Utilities.GetIntepretorOnlyWith(rhelper);
            CastFromIntegerExpression res = null;
            rhelper.CastIntegerToReal(Arg.Do<CastFromIntegerExpression>(x => res = x), Arg.Any<List<Object>>());

            interpreter.DispatchReal(input1, input2);

            res.Should().BeEquivalentTo(expected);
        }

        [TestMethod]
        public void DispatchReal_CastFromIntegerAndObjectList_CorrectValueReturned()
        {
            double expected = 17;
            CastFromIntegerExpression input1 = new CastFromIntegerExpression(null, 0, 0);
            List<Object> input2 = new List<Object>() { 23, 2.334, null };
            IRealHelper rhelper = Substitute.For<IRealHelper>();
            Interpreter interpreter = Utilities.GetIntepretorOnlyWith(rhelper);
            rhelper.CastIntegerToReal(Arg.Any<CastFromIntegerExpression>(), Arg.Any<List<Object>>()).Returns(expected);

            double res = interpreter.DispatchReal(input1, input2);

            Assert.AreEqual(expected, res);
        }
        #endregion

        #region DispatchReal_IdentifierExpr
        [TestMethod]
        public void DispatchReal_IdentifierAndObjectList_CorrectListPassed()
        {
            List<Object> expected = new List<Object>() { 23, 2.334, null };
            IdentifierExpression input1 = new IdentifierExpression(null, 0, 0);
            IRealHelper rhelper = Substitute.For<IRealHelper>();
            Interpreter interpreter = Utilities.GetIntepretorOnlyWith(rhelper);
            List<Object> res = null;
            rhelper.IdentifierReal(Arg.Any<IdentifierExpression>(), Arg.Do<List<Object>>(x => res = x));

            interpreter.DispatchReal(input1, expected);

            res.Should().BeEquivalentTo(expected);
        }

        [TestMethod]
        public void DispatchReal_IdentifierAndObjectList_CorrectIdentifierExprPassed()
        {
            IdentifierExpression expected = new IdentifierExpression(null, 0, 0);
            IdentifierExpression input1 = expected;
            List<Object> input2 = new List<Object>() { 23, 2.334, null };
            IRealHelper rhelper = Substitute.For<IRealHelper>();
            Interpreter interpreter = Utilities.GetIntepretorOnlyWith(rhelper);
            IdentifierExpression res = null;
            rhelper.IdentifierReal(Arg.Do<IdentifierExpression>(x => res = x), Arg.Any<List<Object>>());

            interpreter.DispatchReal(input1, input2);

            res.Should().BeEquivalentTo(expected);
        }

        [TestMethod]
        public void DispatchReal_IdentifierAndObjectList_CorrectValueReturned()
        {
            double expected = 17;
            IdentifierExpression input1 = new IdentifierExpression(null, 0, 0);
            List<Object> input2 = new List<Object>() { 23, 2.334, null };
            IRealHelper rhelper = Substitute.For<IRealHelper>();
            Interpreter interpreter = Utilities.GetIntepretorOnlyWith(rhelper);
            rhelper.IdentifierReal(Arg.Any<IdentifierExpression>(), Arg.Any<List<Object>>()).Returns(expected);

            double res = interpreter.DispatchReal(input1, input2);

            Assert.AreEqual(expected, res);
        }
        #endregion

        #region DispatchReal_RealLiteralExpr
        [TestMethod]
        public void DispatchReal_RealLiteralAndObjectList_CorrectListPassed()
        {
            List<Object> expected = new List<Object>() { 23, 2.334, null };
            RealLiteralExpression input1 = new RealLiteralExpression("", 0, 0);
            IRealHelper rhelper = Substitute.For<IRealHelper>();
            Interpreter interpreter = Utilities.GetIntepretorOnlyWith(rhelper);
            List<Object> res = null;
            rhelper.LiteralReal(Arg.Any<RealLiteralExpression>(), Arg.Do<List<Object>>(x => res = x));

            interpreter.DispatchReal(input1, expected);

            res.Should().BeEquivalentTo(expected);
        }

        [TestMethod]
        public void DispatchReal_RealLiteralAndObjectList_CorrectRealLiteralExprPassed()
        {
            RealLiteralExpression expected = new RealLiteralExpression("", 0, 0);
            RealLiteralExpression input1 = expected;
            List<Object> input2 = new List<Object>() { 23, 2.334, null };
            IRealHelper rhelper = Substitute.For<IRealHelper>();
            Interpreter interpreter = Utilities.GetIntepretorOnlyWith(rhelper);
            RealLiteralExpression res = null;
            rhelper.LiteralReal(Arg.Do<RealLiteralExpression>(x => res = x), Arg.Any<List<Object>>());

            interpreter.DispatchReal(input1, input2);

            res.Should().BeEquivalentTo(expected);
        }

        [TestMethod]
        public void DispatchReal_RealLiteralAndObjectList_CorrectValueReturned()
        {
            double expected = 17;
            RealLiteralExpression input1 = new RealLiteralExpression("", 0, 0);
            List<Object> input2 = new List<Object>() { 23, 2.334, null };
            IRealHelper rhelper = Substitute.For<IRealHelper>();
            Interpreter interpreter = Utilities.GetIntepretorOnlyWith(rhelper);
            rhelper.LiteralReal(Arg.Any<RealLiteralExpression>(), Arg.Any<List<Object>>()).Returns(expected);

            double res = interpreter.DispatchReal(input1, input2);

            Assert.AreEqual(expected, res);
        }
        #endregion

        #endregion

    }
}
