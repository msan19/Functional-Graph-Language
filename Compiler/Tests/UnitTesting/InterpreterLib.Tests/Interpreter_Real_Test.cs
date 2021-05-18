using ASTLib.Exceptions;
using ASTLib.Nodes;
using ASTLib.Nodes.ExpressionNodes;
using ASTLib.Nodes.ExpressionNodes.OperationNodes;
using FluentAssertions;
using InterpreterLib.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.Collections.Generic;
using ASTLib.Nodes.ExpressionNodes.NumberOperationNodes;
using ASTLib.Nodes.ExpressionNodes.CastExpressionNodes;

namespace InterpreterLib.Tests
{
    [TestClass]
    public class Interpreter_Real_Test
    {

        #region DispatchReal

        #region DispatchReal_AdditionExpr
        [TestMethod]
        public void DispatchReal_AdditionAndObjectList_CorrectListPassed()
        {
            List<Object> expected = new List<Object>() { 23, 2.334, null };
            AdditionExpression input1 = new AdditionExpression(null, null, 0, 0);
            IRealHelper rhelper = Substitute.For<IRealHelper>();
            Interpreter interpreter = Utilities.GetIntepreterOnlyWith(rhelper);
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
            Interpreter interpreter = Utilities.GetIntepreterOnlyWith(rhelper);
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
            Interpreter interpreter = Utilities.GetIntepreterOnlyWith(rhelper);
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
            Interpreter interpreter = Utilities.GetIntepreterOnlyWith(rhelper);
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
            Interpreter interpreter = Utilities.GetIntepreterOnlyWith(rhelper);
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
            Interpreter interpreter = Utilities.GetIntepreterOnlyWith(rhelper);
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
            Interpreter interpreter = Utilities.GetIntepreterOnlyWith(rhelper);
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
            Interpreter interpreter = Utilities.GetIntepreterOnlyWith(rhelper);
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
            Interpreter interpreter = Utilities.GetIntepreterOnlyWith(rhelper);
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
            Interpreter interpreter = Utilities.GetIntepreterOnlyWith(rhelper);
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
            Interpreter interpreter = Utilities.GetIntepreterOnlyWith(rhelper);
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
            Interpreter interpreter = Utilities.GetIntepreterOnlyWith(rhelper);
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
            Interpreter interpreter = Utilities.GetIntepreterOnlyWith(rhelper);
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
            Interpreter interpreter = Utilities.GetIntepreterOnlyWith(rhelper);
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
            Interpreter interpreter = Utilities.GetIntepreterOnlyWith(rhelper);
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
            Interpreter interpreter = Utilities.GetIntepreterOnlyWith(rhelper);
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
            Interpreter interpreter = Utilities.GetIntepreterOnlyWith(rhelper);
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
            Interpreter interpreter = Utilities.GetIntepreterOnlyWith(rhelper);
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
            Interpreter interpreter = Utilities.GetIntepreterOnlyWith(rhelper);
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
            Interpreter interpreter = Utilities.GetIntepreterOnlyWith(rhelper);
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
            Interpreter interpreter = Utilities.GetIntepreterOnlyWith(rhelper);
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
            Interpreter interpreter = Utilities.GetIntepreterOnlyWith(rhelper);
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
            Interpreter interpreter = Utilities.GetIntepreterOnlyWith(rhelper);
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
            Interpreter interpreter = Utilities.GetIntepreterOnlyWith(rhelper);
            rhelper.CastIntegerToReal(Arg.Any<CastFromIntegerExpression>(), Arg.Any<List<Object>>()).Returns(expected);

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
            Interpreter interpreter = Utilities.GetIntepreterOnlyWith(rhelper);
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
            Interpreter interpreter = Utilities.GetIntepreterOnlyWith(rhelper);
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
            Interpreter interpreter = Utilities.GetIntepreterOnlyWith(rhelper);
            rhelper.LiteralReal(Arg.Any<RealLiteralExpression>(), Arg.Any<List<Object>>()).Returns(expected);

            double res = interpreter.DispatchReal(input1, input2);

            Assert.AreEqual(expected, res);
        }
        #endregion

        #endregion

    }
}
