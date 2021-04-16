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

namespace InterpreterLib.Tests
{
    [TestClass]
    public class Interpreter_Integer_Tests
    {

        #region DispatchInt


        #region DispatchInt_AdditionExpr
        [TestMethod]
        public void DispatchInteger_AdditionAndObjectList_CorrectListPassed()
        {
            List<Object> expected = new List<Object>() { 23, 2.334, null };
            AdditionExpression input1 = new AdditionExpression(null, null, 0, 0);
            IIntegerHelper ihelper = Substitute.For<IIntegerHelper>();
            Interpreter interpreter = Utilities.GetIntepreterOnlyWith(ihelper);
            List<Object> res = null;
            ihelper.AdditionInteger(Arg.Any<AdditionExpression>(), Arg.Do<List<Object>>(x => res = x));

            interpreter.DispatchInt(input1, expected);

            res.Should().BeEquivalentTo(expected);
        }

        [TestMethod]
        public void DispatchInteger_AdditionAndObjectList_CorrectAdditionExprPassed()
        {
            AdditionExpression expected = new AdditionExpression(null, null, 0, 0);
            AdditionExpression input1 = expected;
            List<Object> input2 = new List<Object>() { 23, 2.334, null };
            IIntegerHelper ihelper = Substitute.For<IIntegerHelper>();
            Interpreter interpreter = Utilities.GetIntepreterOnlyWith(ihelper);
            AdditionExpression res = null;
            ihelper.AdditionInteger(Arg.Do<AdditionExpression>(x => res = x), Arg.Any<List<Object>>());

            interpreter.DispatchInt(input1, input2);

            res.Should().BeEquivalentTo(expected);
        }

        [TestMethod]
        public void DispatchInteger_AdditionAndObjectList_CorrectValueReturned()
        {
            int expected = 17;
            AdditionExpression input1 = new AdditionExpression(null, null, 0, 0);
            List<Object> input2 = new List<Object>() { 23, 2.334, null };
            IIntegerHelper ihelper = Substitute.For<IIntegerHelper>();
            Interpreter interpreter = Utilities.GetIntepreterOnlyWith(ihelper);
            ihelper.AdditionInteger(Arg.Any<AdditionExpression>(), Arg.Any<List<Object>>()).Returns(expected);

            int res = interpreter.DispatchInt(input1, input2);

            Assert.AreEqual(expected, res);
        }
        #endregion

        #region DispatchInt_SubtractionExpr
        [TestMethod]
        public void DispatchInteger_SubtractionAndObjectList_CorrectListPassed()
        {
            List<Object> expected = new List<Object>() { 23, 2.334, null };
            SubtractionExpression input1 = new SubtractionExpression(null, null, 0, 0);
            IIntegerHelper ihelper = Substitute.For<IIntegerHelper>();
            Interpreter interpreter = Utilities.GetIntepreterOnlyWith(ihelper);
            List<Object> res = null;
            ihelper.SubtractionInteger(Arg.Any<SubtractionExpression>(), Arg.Do<List<Object>>(x => res = x));

            interpreter.DispatchInt(input1, expected);

            res.Should().BeEquivalentTo(expected);
        }

        [TestMethod]
        public void DispatchInteger_SubtractionAndObjectList_CorrectSubtractionExprPassed()
        {
            SubtractionExpression expected = new SubtractionExpression(null, null, 0, 0);
            SubtractionExpression input1 = expected;
            List<Object> input2 = new List<Object>() { 23, 2.334, null };
            IIntegerHelper ihelper = Substitute.For<IIntegerHelper>();
            Interpreter interpreter = Utilities.GetIntepreterOnlyWith(ihelper);
            SubtractionExpression res = null;
            ihelper.SubtractionInteger(Arg.Do<SubtractionExpression>(x => res = x), Arg.Any<List<Object>>());

            interpreter.DispatchInt(input1, input2);

            res.Should().BeEquivalentTo(expected);
        }

        [TestMethod]
        public void DispatchInteger_SubtractionAndObjectList_CorrectValueReturned()
        {
            int expected = 17;
            SubtractionExpression input1 = new SubtractionExpression(null, null, 0, 0);
            List<Object> input2 = new List<Object>() { 23, 2.334, null };
            IIntegerHelper ihelper = Substitute.For<IIntegerHelper>();
            Interpreter interpreter = Utilities.GetIntepreterOnlyWith(ihelper);
            ihelper.SubtractionInteger(Arg.Any<SubtractionExpression>(), Arg.Any<List<Object>>()).Returns(expected);

            int res = interpreter.DispatchInt(input1, input2);

            Assert.AreEqual(expected, res);
        }
        #endregion

        #region DispatchInt_MultiplicationExpr
        [TestMethod]
        public void DispatchInteger_MultiplicationAndObjectList_CorrectListPassed()
        {
            List<Object> expected = new List<Object>() { 23, 2.334, null };
            MultiplicationExpression input1 = new MultiplicationExpression(null, null, 0, 0);
            IIntegerHelper ihelper = Substitute.For<IIntegerHelper>();
            Interpreter interpreter = Utilities.GetIntepreterOnlyWith(ihelper);
            List<Object> res = null;
            ihelper.MultiplicationInteger(Arg.Any<MultiplicationExpression>(), Arg.Do<List<Object>>(x => res = x));

            interpreter.DispatchInt(input1, expected);

            res.Should().BeEquivalentTo(expected);
        }

        [TestMethod]
        public void DispatchInteger_MultiplicationAndObjectList_CorrectMultiplicationExprPassed()
        {
            MultiplicationExpression expected = new MultiplicationExpression(null, null, 0, 0);
            MultiplicationExpression input1 = expected;
            List<Object> input2 = new List<Object>() { 23, 2.334, null };
            IIntegerHelper ihelper = Substitute.For<IIntegerHelper>();
            Interpreter interpreter = Utilities.GetIntepreterOnlyWith(ihelper);
            MultiplicationExpression res = null;
            ihelper.MultiplicationInteger(Arg.Do<MultiplicationExpression>(x => res = x), Arg.Any<List<Object>>());

            interpreter.DispatchInt(input1, input2);

            res.Should().BeEquivalentTo(expected);
        }

        [TestMethod]
        public void DispatchInteger_MultiplicationAndObjectList_CorrectValueReturned()
        {
            int expected = 17;
            MultiplicationExpression input1 = new MultiplicationExpression(null, null, 0, 0);
            List<Object> input2 = new List<Object>() { 23, 2.334, null };
            IIntegerHelper ihelper = Substitute.For<IIntegerHelper>();
            Interpreter interpreter = Utilities.GetIntepreterOnlyWith(ihelper);
            ihelper.MultiplicationInteger(Arg.Any<MultiplicationExpression>(), Arg.Any<List<Object>>()).Returns(expected);

            int res = interpreter.DispatchInt(input1, input2);

            Assert.AreEqual(expected, res);
        }
        #endregion

        #region DispatchInt_DivisionExpr
        [TestMethod]
        public void DispatchInteger_DivisionAndObjectList_CorrectListPassed()
        {
            List<Object> expected = new List<Object>() { 23, 2.334, null };
            DivisionExpression input1 = new DivisionExpression(null, null, 0, 0);
            IIntegerHelper ihelper = Substitute.For<IIntegerHelper>();
            Interpreter interpreter = Utilities.GetIntepreterOnlyWith(ihelper);
            List<Object> res = null;
            ihelper.DivisionInteger(Arg.Any<DivisionExpression>(), Arg.Do<List<Object>>(x => res = x));

            interpreter.DispatchInt(input1, expected);

            res.Should().BeEquivalentTo(expected);
        }

        [TestMethod]
        public void DispatchInteger_DivisionAndObjectList_CorrectDivisionExprPassed()
        {
            DivisionExpression expected = new DivisionExpression(null, null, 0, 0);
            DivisionExpression input1 = expected;
            List<Object> input2 = new List<Object>() { 23, 2.334, null };
            IIntegerHelper ihelper = Substitute.For<IIntegerHelper>();
            Interpreter interpreter = Utilities.GetIntepreterOnlyWith(ihelper);
            DivisionExpression res = null;
            ihelper.DivisionInteger(Arg.Do<DivisionExpression>(x => res = x), Arg.Any<List<Object>>());

            interpreter.DispatchInt(input1, input2);

            res.Should().BeEquivalentTo(expected);
        }

        [TestMethod]
        public void DispatchInteger_DivisionAndObjectList_CorrectValueReturned()
        {
            int expected = 17;
            DivisionExpression input1 = new DivisionExpression(null, null, 0, 0);
            List<Object> input2 = new List<Object>() { 23, 2.334, null };
            IIntegerHelper ihelper = Substitute.For<IIntegerHelper>();
            Interpreter interpreter = Utilities.GetIntepreterOnlyWith(ihelper);
            ihelper.DivisionInteger(Arg.Any<DivisionExpression>(), Arg.Any<List<Object>>()).Returns(expected);

            int res = interpreter.DispatchInt(input1, input2);

            Assert.AreEqual(expected, res);
        }
        #endregion

        #region DispatchInt_ModuloExpr
        [TestMethod]
        public void DispatchInteger_ModuloAndObjectList_CorrectListPassed()
        {
            List<Object> expected = new List<Object>() { 23, 2.334, null };
            ModuloExpression input1 = new ModuloExpression(null, null, 0, 0);
            IIntegerHelper ihelper = Substitute.For<IIntegerHelper>();
            Interpreter interpreter = Utilities.GetIntepreterOnlyWith(ihelper);
            List<Object> res = null;
            ihelper.ModuloInteger(Arg.Any<ModuloExpression>(), Arg.Do<List<Object>>(x => res = x));

            interpreter.DispatchInt(input1, expected);

            res.Should().BeEquivalentTo(expected);
        }

        [TestMethod]
        public void DispatchInteger_ModuloAndObjectList_CorrectModuloExprPassed()
        {
            ModuloExpression expected = new ModuloExpression(null, null, 0, 0);
            ModuloExpression input1 = expected;
            List<Object> input2 = new List<Object>() { 23, 2.334, null };
            IIntegerHelper ihelper = Substitute.For<IIntegerHelper>();
            Interpreter interpreter = Utilities.GetIntepreterOnlyWith(ihelper);
            ModuloExpression res = null;
            ihelper.ModuloInteger(Arg.Do<ModuloExpression>(x => res = x), Arg.Any<List<Object>>());

            interpreter.DispatchInt(input1, input2);

            res.Should().BeEquivalentTo(expected);
        }

        [TestMethod]
        public void DispatchInteger_ModuloAndObjectList_CorrectValueReturned()
        {
            int expected = 17;
            ModuloExpression input1 = new ModuloExpression(null, null, 0, 0);
            List<Object> input2 = new List<Object>() { 23, 2.334, null };
            IIntegerHelper ihelper = Substitute.For<IIntegerHelper>();
            Interpreter interpreter = Utilities.GetIntepreterOnlyWith(ihelper);
            ihelper.ModuloInteger(Arg.Any<ModuloExpression>(), Arg.Any<List<Object>>()).Returns(expected);

            int res = interpreter.DispatchInt(input1, input2);

            Assert.AreEqual(expected, res);
        }
        #endregion

        #region DispatchInt_AbsoluteValueExpr
        [TestMethod]
        public void DispatchInteger_AbsoluteValueAndObjectList_CorrectListPassed()
        {
            List<Object> expected = new List<Object>() { 23, 2.334, null };
            AbsoluteValueExpression input1 = new AbsoluteValueExpression(null, 0, 0);
            IIntegerHelper ihelper = Substitute.For<IIntegerHelper>();
            Interpreter interpreter = Utilities.GetIntepreterOnlyWith(ihelper);
            List<Object> res = null;
            ihelper.AbsoluteInteger(Arg.Any<AbsoluteValueExpression>(), Arg.Do<List<Object>>(x => res = x));

            interpreter.DispatchInt(input1, expected);

            res.Should().BeEquivalentTo(expected);
        }

        [TestMethod]
        public void DispatchInteger_AbsoluteValueAndObjectList_CorrectAbsoluteValueExprPassed()
        {
            AbsoluteValueExpression expected = new AbsoluteValueExpression(null, 0, 0);
            AbsoluteValueExpression input1 = expected;
            List<Object> input2 = new List<Object>() { 23, 2.334, null };
            IIntegerHelper ihelper = Substitute.For<IIntegerHelper>();
            Interpreter interpreter = Utilities.GetIntepreterOnlyWith(ihelper);
            AbsoluteValueExpression res = null;
            ihelper.AbsoluteInteger(Arg.Do<AbsoluteValueExpression>(x => res = x), Arg.Any<List<Object>>());

            interpreter.DispatchInt(input1, input2);

            res.Should().BeEquivalentTo(expected);
        }

        [TestMethod]
        public void DispatchInteger_AbsoluteValueAndObjectList_CorrectValueReturned()
        {
            int expected = 17;
            AbsoluteValueExpression input1 = new AbsoluteValueExpression(null, 0, 0);
            List<Object> input2 = new List<Object>() { 23, 2.334, null };
            IIntegerHelper ihelper = Substitute.For<IIntegerHelper>();
            Interpreter interpreter = Utilities.GetIntepreterOnlyWith(ihelper);
            ihelper.AbsoluteInteger(Arg.Any<AbsoluteValueExpression>(), Arg.Any<List<Object>>()).Returns(expected);

            int res = interpreter.DispatchInt(input1, input2);

            Assert.AreEqual(expected, res);
        }
        #endregion

        #region DispatchInt_IntegerLiteralExpr
        [TestMethod]
        public void DispatchInteger_IntegerLiteralAndObjectList_CorrectListPassed()
        {
            List<Object> expected = new List<Object>() { 23, 2.334, null };
            IntegerLiteralExpression input1 = new IntegerLiteralExpression("", 0, 0);
            IIntegerHelper ihelper = Substitute.For<IIntegerHelper>();
            Interpreter interpreter = Utilities.GetIntepreterOnlyWith(ihelper);
            List<Object> res = null;
            ihelper.LiteralInteger(Arg.Any<IntegerLiteralExpression>(), Arg.Do<List<Object>>(x => res = x));

            interpreter.DispatchInt(input1, expected);

            res.Should().BeEquivalentTo(expected);
        }

        [TestMethod]
        public void DispatchInteger_IntegerLiteralAndObjectList_CorrectIntegerLiteralExprPassed()
        {
            IntegerLiteralExpression expected = new IntegerLiteralExpression("", 0, 0);
            IntegerLiteralExpression input1 = expected;
            List<Object> input2 = new List<Object>() { 23, 2.334, null };
            IIntegerHelper ihelper = Substitute.For<IIntegerHelper>();
            Interpreter interpreter = Utilities.GetIntepreterOnlyWith(ihelper);
            IntegerLiteralExpression res = null;
            ihelper.LiteralInteger(Arg.Do<IntegerLiteralExpression>(x => res = x), Arg.Any<List<Object>>());

            interpreter.DispatchInt(input1, input2);

            res.Should().BeEquivalentTo(expected);
        }

        [TestMethod]
        public void DispatchInteger_IntegerLiteralAndObjectList_CorrectValueReturned()
        {
            int expected = 17;
            IntegerLiteralExpression input1 = new IntegerLiteralExpression("", 0, 0);
            List<Object> input2 = new List<Object>() { 23, 2.334, null };
            IIntegerHelper ihelper = Substitute.For<IIntegerHelper>();
            Interpreter interpreter = Utilities.GetIntepreterOnlyWith(ihelper);
            ihelper.LiteralInteger(Arg.Any<IntegerLiteralExpression>(), Arg.Any<List<Object>>()).Returns(expected);

            int res = interpreter.DispatchInt(input1, input2);

            Assert.AreEqual(expected, res);
        }
        #endregion

        #endregion
    }
}
