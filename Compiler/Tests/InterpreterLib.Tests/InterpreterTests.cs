using ASTLib;
using ASTLib.Nodes;
using ASTLib.Nodes.ExpressionNodes;
using ASTLib.Nodes.ExpressionNodes.OperationNodes;
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
    public class InterpreterTests
    {
        #region DispatchFunction
        #region DispatchFunction_FunctionCall   
        [TestMethod]
        public void DispatchFunction_FunctionCallAndObjectList_CorrectListPassed()
        {
            List<Object> expected = new List<Object>() { 23, 2.334, null };
            FunctionCallExpression input1 = new FunctionCallExpression("", null, 0, 0);
            IFunctionHelper fhelper = Substitute.For<IFunctionHelper>();
            IIntegerHelper ihelper = Substitute.For<IIntegerHelper>();
            IRealHelper rhelper = Substitute.For<IRealHelper>();
            Interpreter refHandler = new Interpreter(fhelper, ihelper, rhelper);
            List<Object> res = null;
            fhelper.FunctionCallFunction(Arg.Any<FunctionCallExpression>(), Arg.Do<List<Object>>(x => res = x));

            refHandler.DispatchFunction(input1, expected);

            res.Should().BeEquivalentTo(expected);
        }

        [TestMethod]
        public void DispatchFunction_FunctionCallAndObjectList_CorrectFunctionCallExprPassed()
        {
            FunctionCallExpression expected = new FunctionCallExpression("", null, 0, 0);
            FunctionCallExpression input1 = expected;
            List<Object> input2 = new List<Object>() { 23, 2.334, null };
            IFunctionHelper fhelper = Substitute.For<IFunctionHelper>();
            IIntegerHelper ihelper = Substitute.For<IIntegerHelper>();
            IRealHelper rhelper = Substitute.For<IRealHelper>();
            Interpreter refHandler = new Interpreter(fhelper, ihelper, rhelper);
            FunctionCallExpression res = null;
            fhelper.FunctionCallFunction(Arg.Do<FunctionCallExpression>(x => res = x), Arg.Any<List<Object>>());

            refHandler.DispatchFunction(input1, input2);

            res.Should().BeEquivalentTo(expected);
        }

        [TestMethod]
        public void DispatchFunction_FunctionCallAndObjectList_CorrectValueReturned()
        {
            int expected = 17;
            FunctionCallExpression input1 = new FunctionCallExpression("", null, 0, 0);
            List<Object> input2 = new List<Object>() { 23, 2.334, null };
            IFunctionHelper fhelper = Substitute.For<IFunctionHelper>();
            IIntegerHelper ihelper = Substitute.For<IIntegerHelper>();
            IRealHelper rhelper = Substitute.For<IRealHelper>();
            Interpreter refHandler = new Interpreter(fhelper, ihelper, rhelper);
            fhelper.FunctionCallFunction(Arg.Any<FunctionCallExpression>(), Arg.Any<List<Object>>()).Returns(expected);

            int res = refHandler.DispatchFunction(input1, input2);

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
            IIntegerHelper ihelper = Substitute.For<IIntegerHelper>();
            IRealHelper rhelper = Substitute.For<IRealHelper>();
            Interpreter refHandler = new Interpreter(fhelper, ihelper, rhelper);
            List<Object> res = null;
            fhelper.IdentifierFunction(Arg.Any<IdentifierExpression>(), Arg.Do<List<Object>>(x => res = x));

            refHandler.DispatchFunction(input1, expected);

            res.Should().BeEquivalentTo(expected);
        }

        [TestMethod]
        public void DispatchFunction_IdentifierAndObjectList_CorrectFunctionCallExprPassed()
        {
            IdentifierExpression expected = new IdentifierExpression("", 0, 0);
            IdentifierExpression input1 = expected;
            List<Object> input2 = new List<Object>() { 23, 2.334, null };
            IFunctionHelper fhelper = Substitute.For<IFunctionHelper>();
            IIntegerHelper ihelper = Substitute.For<IIntegerHelper>();
            IRealHelper rhelper = Substitute.For<IRealHelper>();
            Interpreter refHandler = new Interpreter(fhelper, ihelper, rhelper);
            IdentifierExpression res = null;
            fhelper.IdentifierFunction(Arg.Do<IdentifierExpression>(x => res = x), Arg.Any<List<Object>>());

            refHandler.DispatchFunction(input1, input2);

            res.Should().BeEquivalentTo(expected);
        }

        [TestMethod]
        public void DispatchFunction_IdentifierAndObjectList_CorrectValueReturned()
        {
            int expected = 17;
            IdentifierExpression input1 = new IdentifierExpression("", 0, 0);
            List<Object> input2 = new List<Object>() { 23, 2.334, null };
            IFunctionHelper fhelper = Substitute.For<IFunctionHelper>();
            IIntegerHelper ihelper = Substitute.For<IIntegerHelper>();
            IRealHelper rhelper = Substitute.For<IRealHelper>();
            Interpreter refHandler = new Interpreter(fhelper, ihelper, rhelper);
            fhelper.IdentifierFunction(Arg.Any<IdentifierExpression>(), Arg.Any<List<Object>>()).Returns(expected);

            int res = refHandler.DispatchFunction(input1, input2);

            Assert.AreEqual(expected, res);
        }
        #endregion

        #region Interpret
        [TestMethod]
        public void Interpret_AST_CorrectNumberOfCallsToExportReal()
        {
            List<ExportNode> exports = new List<ExportNode> { new ExportNode(null,0,0),
                                                              new ExportNode(null,0,0),
                                                              new ExportNode(null,0,0)};
            AST input1 = new AST(null, exports, 0, 0);
            IFunctionHelper fhelper = Substitute.For<IFunctionHelper>();
            IIntegerHelper ihelper = Substitute.For<IIntegerHelper>();
            IRealHelper rhelper = Substitute.For<IRealHelper>();
            Interpreter interpreter = new Interpreter(fhelper, ihelper, rhelper);

            interpreter.Interpret(input1);

            rhelper.Received(3).ExportReal(Arg.Any<ExportNode>(), Arg.Any<List<Object>>());
        }

        [TestMethod]
        public void Interpret_AST_CorrectListReturned()
        {
            List<double> expected = new List<double> { 0.1, 3.3, 7.0 };
            List<ExportNode> exports = new List<ExportNode> { new ExportNode(null,0,0),
                                                              new ExportNode(null,0,0),
                                                              new ExportNode(null,0,0)};
            AST input1 = new AST(null, exports, 0, 0);
            IFunctionHelper fhelper = Substitute.For<IFunctionHelper>();
            IIntegerHelper ihelper = Substitute.For<IIntegerHelper>();
            IRealHelper rhelper = Substitute.For<IRealHelper>();
            Interpreter interpreter = new Interpreter(fhelper, ihelper, rhelper);
            rhelper.ExportReal(exports[0], Arg.Any<List<Object>>()).Returns(0.1);
            rhelper.ExportReal(exports[1], Arg.Any<List<Object>>()).Returns(3.3);
            rhelper.ExportReal(exports[2], Arg.Any<List<Object>>()).Returns(7.0);

            List<double> res = interpreter.Interpret(input1);

            res.Should().BeEquivalentTo(expected);
        }

        #endregion
        #endregion

        #region DispatchInt
        #region DispatchInt_functionCallExpr
        [TestMethod]
        public void DispatchInteger_FunctionCallAndObjectList_CorrectListPassed()
        {
            List<Object> expected = new List<Object>() { 23, 2.334, null };
            FunctionCallExpression input1 = new FunctionCallExpression("", null, 0, 0);
            IFunctionHelper fhelper = Substitute.For<IFunctionHelper>();
            IIntegerHelper ihelper = Substitute.For<IIntegerHelper>();
            IRealHelper rhelper = Substitute.For<IRealHelper>();
            Interpreter interpreter = new Interpreter(fhelper, ihelper, rhelper);
            List<Object> res = null;
            ihelper.FunctionCallInteger(Arg.Any<FunctionCallExpression>(), Arg.Do<List<Object>>(x => res = x));

            interpreter.DispatchInt(input1, expected);

            res.Should().BeEquivalentTo(expected);
        }

        [TestMethod]
        public void DispatchInteger_FunctionCallAndObjectList_CorrectFunctionCallExprPassed()
        {
            FunctionCallExpression expected = new FunctionCallExpression("", null, 0, 0);
            FunctionCallExpression input1 = expected;
            List<Object> input2 = new List<Object>() { 23, 2.334, null };
            IFunctionHelper fhelper = Substitute.For<IFunctionHelper>();
            IIntegerHelper ihelper = Substitute.For<IIntegerHelper>();
            IRealHelper rhelper = Substitute.For<IRealHelper>();
            Interpreter interpreter = new Interpreter(fhelper, ihelper, rhelper);
            FunctionCallExpression res = null;
            ihelper.FunctionCallInteger(Arg.Do<FunctionCallExpression>(x => res = x), Arg.Any<List<Object>>());

            interpreter.DispatchInt(input1, input2);

            res.Should().BeEquivalentTo(expected);
        }

        [TestMethod]
        public void DispatchInteger_FunctionCallAndObjectList_CorrectValueReturned()
        {
            int expected = 17;
            FunctionCallExpression input1 = new FunctionCallExpression("", null, 0, 0);
            List<Object> input2 = new List<Object>() { 23, 2.334, null };
            IFunctionHelper fhelper = Substitute.For<IFunctionHelper>();
            IIntegerHelper ihelper = Substitute.For<IIntegerHelper>();
            IRealHelper rhelper = Substitute.For<IRealHelper>();
            Interpreter interpreter = new Interpreter(fhelper, ihelper, rhelper);
            ihelper.FunctionCallInteger(Arg.Any<FunctionCallExpression>(), Arg.Any<List<Object>>()).Returns(expected);

            int res = interpreter.DispatchInt(input1, input2);

            Assert.AreEqual(expected, res);
        }
        #endregion

        #region DispatchInt_AdditionExpr
        [TestMethod]
        public void DispatchInteger_AdditionAndObjectList_CorrectListPassed()
        {
            List<Object> expected = new List<Object>() { 23, 2.334, null };
            AdditionExpression input1 = new AdditionExpression(null, null, 0, 0);
            IFunctionHelper fhelper = Substitute.For<IFunctionHelper>();
            IIntegerHelper ihelper = Substitute.For<IIntegerHelper>();
            IRealHelper rhelper = Substitute.For<IRealHelper>();
            Interpreter interpreter = new Interpreter(fhelper, ihelper, rhelper);
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
            IFunctionHelper fhelper = Substitute.For<IFunctionHelper>();
            IIntegerHelper ihelper = Substitute.For<IIntegerHelper>();
            IRealHelper rhelper = Substitute.For<IRealHelper>();
            Interpreter interpreter = new Interpreter(fhelper, ihelper, rhelper);
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
            IFunctionHelper fhelper = Substitute.For<IFunctionHelper>();
            IIntegerHelper ihelper = Substitute.For<IIntegerHelper>();
            IRealHelper rhelper = Substitute.For<IRealHelper>();
            Interpreter interpreter = new Interpreter(fhelper, ihelper, rhelper);
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
            IFunctionHelper fhelper = Substitute.For<IFunctionHelper>();
            IIntegerHelper ihelper = Substitute.For<IIntegerHelper>();
            IRealHelper rhelper = Substitute.For<IRealHelper>();
            Interpreter interpreter = new Interpreter(fhelper, ihelper, rhelper);
            List<Object> res = null;
            ihelper.SubtractionInteger(Arg.Any <SubtractionExpression > (), Arg.Do<List<Object>>(x => res = x));

            interpreter.DispatchInt(input1, expected);

            res.Should().BeEquivalentTo(expected);
        }

        [TestMethod]
        public void DispatchInteger_SubtractionAndObjectList_CorrectSubtractionExprPassed()
        {
           SubtractionExpression expected = new SubtractionExpression(null, null, 0, 0);
           SubtractionExpression input1 = expected;
            List<Object> input2 = new List<Object>() { 23, 2.334, null };
            IFunctionHelper fhelper = Substitute.For<IFunctionHelper>();
            IIntegerHelper ihelper = Substitute.For<IIntegerHelper>();
            IRealHelper rhelper = Substitute.For<IRealHelper>();
            Interpreter interpreter = new Interpreter(fhelper, ihelper, rhelper);
           SubtractionExpression res = null;
            ihelper.SubtractionInteger(Arg.Do <SubtractionExpression > (x => res = x), Arg.Any<List<Object>>());

            interpreter.DispatchInt(input1, input2);

            res.Should().BeEquivalentTo(expected);
        }

        [TestMethod]
        public void DispatchInteger_SubtractionAndObjectList_CorrectValueReturned()
        {
            int expected = 17;
           SubtractionExpression input1 = new SubtractionExpression(null, null, 0, 0);
            List<Object> input2 = new List<Object>() { 23, 2.334, null };
            IFunctionHelper fhelper = Substitute.For<IFunctionHelper>();
            IIntegerHelper ihelper = Substitute.For<IIntegerHelper>();
            IRealHelper rhelper = Substitute.For<IRealHelper>();
            Interpreter interpreter = new Interpreter(fhelper, ihelper, rhelper);
            ihelper.SubtractionInteger(Arg.Any <SubtractionExpression > (), Arg.Any<List<Object>>()).Returns(expected);

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
            IFunctionHelper fhelper = Substitute.For<IFunctionHelper>();
            IIntegerHelper ihelper = Substitute.For<IIntegerHelper>();
            IRealHelper rhelper = Substitute.For<IRealHelper>();
            Interpreter interpreter = new Interpreter(fhelper, ihelper, rhelper);
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
            IFunctionHelper fhelper = Substitute.For<IFunctionHelper>();
            IIntegerHelper ihelper = Substitute.For<IIntegerHelper>();
            IRealHelper rhelper = Substitute.For<IRealHelper>();
            Interpreter interpreter = new Interpreter(fhelper, ihelper, rhelper);
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
            IFunctionHelper fhelper = Substitute.For<IFunctionHelper>();
            IIntegerHelper ihelper = Substitute.For<IIntegerHelper>();
            IRealHelper rhelper = Substitute.For<IRealHelper>();
            Interpreter interpreter = new Interpreter(fhelper, ihelper, rhelper);
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
            IFunctionHelper fhelper = Substitute.For<IFunctionHelper>();
            IIntegerHelper ihelper = Substitute.For<IIntegerHelper>();
            IRealHelper rhelper = Substitute.For<IRealHelper>();
            Interpreter interpreter = new Interpreter(fhelper, ihelper, rhelper);
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
            IFunctionHelper fhelper = Substitute.For<IFunctionHelper>();
            IIntegerHelper ihelper = Substitute.For<IIntegerHelper>();
            IRealHelper rhelper = Substitute.For<IRealHelper>();
            Interpreter interpreter = new Interpreter(fhelper, ihelper, rhelper);
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
            IFunctionHelper fhelper = Substitute.For<IFunctionHelper>();
            IIntegerHelper ihelper = Substitute.For<IIntegerHelper>();
            IRealHelper rhelper = Substitute.For<IRealHelper>();
            Interpreter interpreter = new Interpreter(fhelper, ihelper, rhelper);
            ihelper.DivisionInteger(Arg.Any<DivisionExpression>(), Arg.Any<List<Object>>()).Returns(expected);

            int res = interpreter.DispatchInt(input1, input2);

            Assert.AreEqual(expected, res);
        }
        #endregion

        #endregion
    }
}

/*
#region DispatchInt_INSERTExpr
        [TestMethod]
        public void DispatchInteger_INSERTAndObjectList_CorrectListPassed()
        {
            List<Object> expected = new List<Object>() { 23, 2.334, null };
            INSERTExpression input1 = new INSERTExpression(null, null, 0, 0);
            IFunctionHelper fhelper = Substitute.For<IFunctionHelper>();
            IIntegerHelper ihelper = Substitute.For<IIntegerHelper>();
            IRealHelper rhelper = Substitute.For<IRealHelper>();
            Interpreter interpreter = new Interpreter(fhelper, ihelper, rhelper);
            List<Object> res = null;
            ihelper.INSERTInteger(Arg.Any<INSERTExpression>(), Arg.Do<List<Object>>(x => res = x));

            interpreter.DispatchInt(input1, expected);

            res.Should().BeEquivalentTo(expected);
        }

        [TestMethod]
        public void DispatchInteger_INSERTAndObjectList_CorrectINSERTExprPassed()
        {
            INSERTExpression expected = new INSERTExpression(null, null, 0, 0);
            INSERTExpression input1 = expected;
            List<Object> input2 = new List<Object>() { 23, 2.334, null };
            IFunctionHelper fhelper = Substitute.For<IFunctionHelper>();
            IIntegerHelper ihelper = Substitute.For<IIntegerHelper>();
            IRealHelper rhelper = Substitute.For<IRealHelper>();
            Interpreter interpreter = new Interpreter(fhelper, ihelper, rhelper);
            INSERTExpression res = null;
            ihelper.INSERTInteger(Arg.Do<INSERTExpression>(x => res = x), Arg.Any<List<Object>>());

            interpreter.DispatchInt(input1, input2);

            res.Should().BeEquivalentTo(expected);
        }

        [TestMethod]
        public void DispatchInteger_INSERTAndObjectList_CorrectValueReturned()
        {
            int expected = 17;
            INSERTExpression input1 = new INSERTExpression(null, null, 0, 0);
            List<Object> input2 = new List<Object>() { 23, 2.334, null };
            IFunctionHelper fhelper = Substitute.For<IFunctionHelper>();
            IIntegerHelper ihelper = Substitute.For<IIntegerHelper>();
            IRealHelper rhelper = Substitute.For<IRealHelper>();
            Interpreter interpreter = new Interpreter(fhelper, ihelper, rhelper);
            ihelper.INSERTInteger(Arg.Any<INSERTExpression>(), Arg.Any<List<Object>>()).Returns(expected);

            int res = interpreter.DispatchInt(input1, input2);

            Assert.AreEqual(expected, res);
        }
        #endregion
*/


