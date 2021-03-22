﻿using ASTLib;
using ASTLib.Nodes;
using ASTLib.Nodes.ExpressionNodes;
using ASTLib.Nodes.ExpressionNodes.OperationNodes;
using ASTLib.Nodes.TypeNodes;
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

        #region DispatchInt_ModuloExpr
        [TestMethod]
        public void DispatchInteger_ModuloAndObjectList_CorrectListPassed()
        {
            List<Object> expected = new List<Object>() { 23, 2.334, null };
            ModuloExpression input1 = new ModuloExpression(null, null, 0, 0);
            IFunctionHelper fhelper = Substitute.For<IFunctionHelper>();
            IIntegerHelper ihelper = Substitute.For<IIntegerHelper>();
            IRealHelper rhelper = Substitute.For<IRealHelper>();
            Interpreter interpreter = new Interpreter(fhelper, ihelper, rhelper);
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
            IFunctionHelper fhelper = Substitute.For<IFunctionHelper>();
            IIntegerHelper ihelper = Substitute.For<IIntegerHelper>();
            IRealHelper rhelper = Substitute.For<IRealHelper>();
            Interpreter interpreter = new Interpreter(fhelper, ihelper, rhelper);
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
            IFunctionHelper fhelper = Substitute.For<IFunctionHelper>();
            IIntegerHelper ihelper = Substitute.For<IIntegerHelper>();
            IRealHelper rhelper = Substitute.For<IRealHelper>();
            Interpreter interpreter = new Interpreter(fhelper, ihelper, rhelper);
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
            IFunctionHelper fhelper = Substitute.For<IFunctionHelper>();
            IIntegerHelper ihelper = Substitute.For<IIntegerHelper>();
            IRealHelper rhelper = Substitute.For<IRealHelper>();
            Interpreter interpreter = new Interpreter(fhelper, ihelper, rhelper);
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
            IFunctionHelper fhelper = Substitute.For<IFunctionHelper>();
            IIntegerHelper ihelper = Substitute.For<IIntegerHelper>();
            IRealHelper rhelper = Substitute.For<IRealHelper>();
            Interpreter interpreter = new Interpreter(fhelper, ihelper, rhelper);
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
            IFunctionHelper fhelper = Substitute.For<IFunctionHelper>();
            IIntegerHelper ihelper = Substitute.For<IIntegerHelper>();
            IRealHelper rhelper = Substitute.For<IRealHelper>();
            Interpreter interpreter = new Interpreter(fhelper, ihelper, rhelper);
            ihelper.AbsoluteInteger(Arg.Any<AbsoluteValueExpression>(), Arg.Any<List<Object>>()).Returns(expected);

            int res = interpreter.DispatchInt(input1, input2);

            Assert.AreEqual(expected, res);
        }
        #endregion

        #region DispatchInt_IdentifierExpr
        [TestMethod]
        public void DispatchInteger_IdentifierAndObjectList_CorrectListPassed()
        {
            List<Object> expected = new List<Object>() { 23, 2.334, null };
            IdentifierExpression input1 = new IdentifierExpression(null, 0, 0);
            IFunctionHelper fhelper = Substitute.For<IFunctionHelper>();
            IIntegerHelper ihelper = Substitute.For<IIntegerHelper>();
            IRealHelper rhelper = Substitute.For<IRealHelper>();
            Interpreter interpreter = new Interpreter(fhelper, ihelper, rhelper);
            List<Object> res = null;
            ihelper.IdentifierInteger(Arg.Any<IdentifierExpression>(), Arg.Do<List<Object>>(x => res = x));

            interpreter.DispatchInt(input1, expected);

            res.Should().BeEquivalentTo(expected);
        }

        [TestMethod]
        public void DispatchInteger_IdentifierAndObjectList_CorrectIdentifierExprPassed()
        {
            IdentifierExpression expected = new IdentifierExpression(null, 0, 0);
            IdentifierExpression input1 = expected;
            List<Object> input2 = new List<Object>() { 23, 2.334, null };
            IFunctionHelper fhelper = Substitute.For<IFunctionHelper>();
            IIntegerHelper ihelper = Substitute.For<IIntegerHelper>();
            IRealHelper rhelper = Substitute.For<IRealHelper>();
            Interpreter interpreter = new Interpreter(fhelper, ihelper, rhelper);
            IdentifierExpression res = null;
            ihelper.IdentifierInteger(Arg.Do<IdentifierExpression>(x => res = x), Arg.Any<List<Object>>());

            interpreter.DispatchInt(input1, input2);

            res.Should().BeEquivalentTo(expected);
        }

        [TestMethod]
        public void DispatchInteger_IdentifierAndObjectList_CorrectValueReturned()
        {
            int expected = 17;
            IdentifierExpression input1 = new IdentifierExpression(null, 0, 0);
            List<Object> input2 = new List<Object>() { 23, 2.334, null };
            IFunctionHelper fhelper = Substitute.For<IFunctionHelper>();
            IIntegerHelper ihelper = Substitute.For<IIntegerHelper>();
            IRealHelper rhelper = Substitute.For<IRealHelper>();
            Interpreter interpreter = new Interpreter(fhelper, ihelper, rhelper);
            ihelper.IdentifierInteger(Arg.Any<IdentifierExpression>(), Arg.Any<List<Object>>()).Returns(expected);

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
            IFunctionHelper fhelper = Substitute.For<IFunctionHelper>();
            IIntegerHelper ihelper = Substitute.For<IIntegerHelper>();
            IRealHelper rhelper = Substitute.For<IRealHelper>();
            Interpreter interpreter = new Interpreter(fhelper, ihelper, rhelper);
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
            IFunctionHelper fhelper = Substitute.For<IFunctionHelper>();
            IIntegerHelper ihelper = Substitute.For<IIntegerHelper>();
            IRealHelper rhelper = Substitute.For<IRealHelper>();
            Interpreter interpreter = new Interpreter(fhelper, ihelper, rhelper);
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
            IFunctionHelper fhelper = Substitute.For<IFunctionHelper>();
            IIntegerHelper ihelper = Substitute.For<IIntegerHelper>();
            IRealHelper rhelper = Substitute.For<IRealHelper>();
            Interpreter interpreter = new Interpreter(fhelper, ihelper, rhelper);
            ihelper.LiteralInteger(Arg.Any<IntegerLiteralExpression>(), Arg.Any<List<Object>>()).Returns(expected);

            int res = interpreter.DispatchInt(input1, input2);

            Assert.AreEqual(expected, res);
        }
        #endregion

        #endregion

        #region DispatchReal
        #region DispatchReal_functionCallExpr
        [TestMethod]
        public void DispatchReal_FunctionCallAndObjectList_CorrectListPassed()
        {
            List<Object> expected = new List<Object>() { 23, 2.334, null };
            FunctionCallExpression input1 = new FunctionCallExpression("", null, 0, 0);
            IFunctionHelper fhelper = Substitute.For<IFunctionHelper>();
            IIntegerHelper ihelper = Substitute.For<IIntegerHelper>();
            IRealHelper rhelper = Substitute.For<IRealHelper>();
            Interpreter interpreter = new Interpreter(fhelper, ihelper, rhelper);
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
            IFunctionHelper fhelper = Substitute.For<IFunctionHelper>();
            IIntegerHelper ihelper = Substitute.For<IIntegerHelper>();
            IRealHelper rhelper = Substitute.For<IRealHelper>();
            Interpreter interpreter = new Interpreter(fhelper, ihelper, rhelper);
            FunctionCallExpression res = null;
            rhelper.FunctionCallReal(Arg.Do<FunctionCallExpression>(x => res = x), Arg.Any<List<Object>>());

            interpreter.DispatchReal(input1, input2);

            res.Should().BeEquivalentTo(expected);
        }

        [TestMethod]
        public void DispatchReal_FunctionCallAndObjectList_CorrectValueReturned()
        {
            int expected = 17;
            FunctionCallExpression input1 = new FunctionCallExpression("", null, 0, 0);
            List<Object> input2 = new List<Object>() { 23, 2.334, null };
            IFunctionHelper fhelper = Substitute.For<IFunctionHelper>();
            IIntegerHelper ihelper = Substitute.For<IIntegerHelper>();
            IRealHelper rhelper = Substitute.For<IRealHelper>();
            Interpreter interpreter = new Interpreter(fhelper, ihelper, rhelper);
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
            IFunctionHelper fhelper = Substitute.For<IFunctionHelper>();
            IIntegerHelper ihelper = Substitute.For<IIntegerHelper>();
            IRealHelper rhelper = Substitute.For<IRealHelper>();
            Interpreter interpreter = new Interpreter(fhelper, ihelper, rhelper);
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
            IFunctionHelper fhelper = Substitute.For<IFunctionHelper>();
            IIntegerHelper ihelper = Substitute.For<IIntegerHelper>();
            IRealHelper rhelper = Substitute.For<IRealHelper>();
            Interpreter interpreter = new Interpreter(fhelper, ihelper, rhelper);
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
            IFunctionHelper fhelper = Substitute.For<IFunctionHelper>();
            IIntegerHelper ihelper = Substitute.For<IIntegerHelper>();
            IRealHelper rhelper = Substitute.For<IRealHelper>();
            Interpreter interpreter = new Interpreter(fhelper, ihelper, rhelper);
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
            IFunctionHelper fhelper = Substitute.For<IFunctionHelper>();
            IIntegerHelper ihelper = Substitute.For<IIntegerHelper>();
            IRealHelper rhelper = Substitute.For<IRealHelper>();
            Interpreter interpreter = new Interpreter(fhelper, ihelper, rhelper);
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
            IFunctionHelper fhelper = Substitute.For<IFunctionHelper>();
            IIntegerHelper ihelper = Substitute.For<IIntegerHelper>();
            IRealHelper rhelper = Substitute.For<IRealHelper>();
            Interpreter interpreter = new Interpreter(fhelper, ihelper, rhelper);
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
            IFunctionHelper fhelper = Substitute.For<IFunctionHelper>();
            IIntegerHelper ihelper = Substitute.For<IIntegerHelper>();
            IRealHelper rhelper = Substitute.For<IRealHelper>();
            Interpreter interpreter = new Interpreter(fhelper, ihelper, rhelper);
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
            IFunctionHelper fhelper = Substitute.For<IFunctionHelper>();
            IIntegerHelper ihelper = Substitute.For<IIntegerHelper>();
            IRealHelper rhelper = Substitute.For<IRealHelper>();
            Interpreter interpreter = new Interpreter(fhelper, ihelper, rhelper);
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
            IFunctionHelper fhelper = Substitute.For<IFunctionHelper>();
            IIntegerHelper ihelper = Substitute.For<IIntegerHelper>();
            IRealHelper rhelper = Substitute.For<IRealHelper>();
            Interpreter interpreter = new Interpreter(fhelper, ihelper, rhelper);
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
            IFunctionHelper fhelper = Substitute.For<IFunctionHelper>();
            IIntegerHelper ihelper = Substitute.For<IIntegerHelper>();
            IRealHelper rhelper = Substitute.For<IRealHelper>();
            Interpreter interpreter = new Interpreter(fhelper, ihelper, rhelper);
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
            IFunctionHelper fhelper = Substitute.For<IFunctionHelper>();
            IIntegerHelper ihelper = Substitute.For<IIntegerHelper>();
            IRealHelper rhelper = Substitute.For<IRealHelper>();
            Interpreter interpreter = new Interpreter(fhelper, ihelper, rhelper);
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
            IFunctionHelper fhelper = Substitute.For<IFunctionHelper>();
            IIntegerHelper ihelper = Substitute.For<IIntegerHelper>();
            IRealHelper rhelper = Substitute.For<IRealHelper>();
            Interpreter interpreter = new Interpreter(fhelper, ihelper, rhelper);
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
            IFunctionHelper fhelper = Substitute.For<IFunctionHelper>();
            IIntegerHelper ihelper = Substitute.For<IIntegerHelper>();
            IRealHelper rhelper = Substitute.For<IRealHelper>();
            Interpreter interpreter = new Interpreter(fhelper, ihelper, rhelper);
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
            IFunctionHelper fhelper = Substitute.For<IFunctionHelper>();
            IIntegerHelper ihelper = Substitute.For<IIntegerHelper>();
            IRealHelper rhelper = Substitute.For<IRealHelper>();
            Interpreter interpreter = new Interpreter(fhelper, ihelper, rhelper);
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
            IFunctionHelper fhelper = Substitute.For<IFunctionHelper>();
            IIntegerHelper ihelper = Substitute.For<IIntegerHelper>();
            IRealHelper rhelper = Substitute.For<IRealHelper>();
            Interpreter interpreter = new Interpreter(fhelper, ihelper, rhelper);
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
            IFunctionHelper fhelper = Substitute.For<IFunctionHelper>();
            IIntegerHelper ihelper = Substitute.For<IIntegerHelper>();
            IRealHelper rhelper = Substitute.For<IRealHelper>();
            Interpreter interpreter = new Interpreter(fhelper, ihelper, rhelper);
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
            IFunctionHelper fhelper = Substitute.For<IFunctionHelper>();
            IIntegerHelper ihelper = Substitute.For<IIntegerHelper>();
            IRealHelper rhelper = Substitute.For<IRealHelper>();
            Interpreter interpreter = new Interpreter(fhelper, ihelper, rhelper);
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
            IFunctionHelper fhelper = Substitute.For<IFunctionHelper>();
            IIntegerHelper ihelper = Substitute.For<IIntegerHelper>();
            IRealHelper rhelper = Substitute.For<IRealHelper>();
            Interpreter interpreter = new Interpreter(fhelper, ihelper, rhelper);
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
            IFunctionHelper fhelper = Substitute.For<IFunctionHelper>();
            IIntegerHelper ihelper = Substitute.For<IIntegerHelper>();
            IRealHelper rhelper = Substitute.For<IRealHelper>();
            Interpreter interpreter = new Interpreter(fhelper, ihelper, rhelper);
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
            IFunctionHelper fhelper = Substitute.For<IFunctionHelper>();
            IIntegerHelper ihelper = Substitute.For<IIntegerHelper>();
            IRealHelper rhelper = Substitute.For<IRealHelper>();
            Interpreter interpreter = new Interpreter(fhelper, ihelper, rhelper);
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
            IFunctionHelper fhelper = Substitute.For<IFunctionHelper>();
            IIntegerHelper ihelper = Substitute.For<IIntegerHelper>();
            IRealHelper rhelper = Substitute.For<IRealHelper>();
            Interpreter interpreter = new Interpreter(fhelper, ihelper, rhelper);
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
            IFunctionHelper fhelper = Substitute.For<IFunctionHelper>();
            IIntegerHelper ihelper = Substitute.For<IIntegerHelper>();
            IRealHelper rhelper = Substitute.For<IRealHelper>();
            Interpreter interpreter = new Interpreter(fhelper, ihelper, rhelper);
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
            IFunctionHelper fhelper = Substitute.For<IFunctionHelper>();
            IIntegerHelper ihelper = Substitute.For<IIntegerHelper>();
            IRealHelper rhelper = Substitute.For<IRealHelper>();
            Interpreter interpreter = new Interpreter(fhelper, ihelper, rhelper);
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
            IFunctionHelper fhelper = Substitute.For<IFunctionHelper>();
            IIntegerHelper ihelper = Substitute.For<IIntegerHelper>();
            IRealHelper rhelper = Substitute.For<IRealHelper>();
            Interpreter interpreter = new Interpreter(fhelper, ihelper, rhelper);
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
            IFunctionHelper fhelper = Substitute.For<IFunctionHelper>();
            IIntegerHelper ihelper = Substitute.For<IIntegerHelper>();
            IRealHelper rhelper = Substitute.For<IRealHelper>();
            Interpreter interpreter = new Interpreter(fhelper, ihelper, rhelper);
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
            IFunctionHelper fhelper = Substitute.For<IFunctionHelper>();
            IIntegerHelper ihelper = Substitute.For<IIntegerHelper>();
            IRealHelper rhelper = Substitute.For<IRealHelper>();
            Interpreter interpreter = new Interpreter(fhelper, ihelper, rhelper);
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
            IFunctionHelper fhelper = Substitute.For<IFunctionHelper>();
            IIntegerHelper ihelper = Substitute.For<IIntegerHelper>();
            IRealHelper rhelper = Substitute.For<IRealHelper>();
            Interpreter interpreter = new Interpreter(fhelper, ihelper, rhelper);
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
            IFunctionHelper fhelper = Substitute.For<IFunctionHelper>();
            IIntegerHelper ihelper = Substitute.For<IIntegerHelper>();
            IRealHelper rhelper = Substitute.For<IRealHelper>();
            Interpreter interpreter = new Interpreter(fhelper, ihelper, rhelper);
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
            IFunctionHelper fhelper = Substitute.For<IFunctionHelper>();
            IIntegerHelper ihelper = Substitute.For<IIntegerHelper>();
            IRealHelper rhelper = Substitute.For<IRealHelper>();
            Interpreter interpreter = new Interpreter(fhelper, ihelper, rhelper);
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
            IFunctionHelper fhelper = Substitute.For<IFunctionHelper>();
            IIntegerHelper ihelper = Substitute.For<IIntegerHelper>();
            IRealHelper rhelper = Substitute.For<IRealHelper>();
            Interpreter interpreter = new Interpreter(fhelper, ihelper, rhelper);
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
            IFunctionHelper fhelper = Substitute.For<IFunctionHelper>();
            IIntegerHelper ihelper = Substitute.For<IIntegerHelper>();
            IRealHelper rhelper = Substitute.For<IRealHelper>();
            Interpreter interpreter = new Interpreter(fhelper, ihelper, rhelper);
            rhelper.LiteralReal(Arg.Any<RealLiteralExpression>(), Arg.Any<List<Object>>()).Returns(expected);

            double res = interpreter.DispatchReal(input1, input2);

            Assert.AreEqual(expected, res);
        }
        #endregion

        #endregion

        #region Dispatch
        #region Dispatch_IntegerLiteralExpr
        [TestMethod]
        public void Dispatch_IntegerLiteralAndObjectListAndIntegerType_CorrectListPassed()
        {
            List<Object> expected = new List<Object>() { 23, 2.334, null };
            IntegerLiteralExpression input1 = new IntegerLiteralExpression("", 0, 0);
            IFunctionHelper fhelper = Substitute.For<IFunctionHelper>();
            IIntegerHelper ihelper = Substitute.For<IIntegerHelper>();
            IRealHelper rhelper = Substitute.For<IRealHelper>();
            Interpreter interpreter = new Interpreter(fhelper, ihelper, rhelper);
            List<Object> res = null;
            ihelper.LiteralInteger(Arg.Any<IntegerLiteralExpression>(), Arg.Do<List<Object>>(x => res = x));

            interpreter.Dispatch(input1, expected, TypeEnum.Integer);

            res.Should().BeEquivalentTo(expected);
        }

        [TestMethod]
        public void Dispatch_IntegerLiteralAndObjectListAndIntegerType_CorrectIntegerLiteralExprPassed()
        {
            IntegerLiteralExpression expected = new IntegerLiteralExpression("", 0, 0);
            IntegerLiteralExpression input1 = expected;
            List<Object> input2 = new List<Object>() { 23, 2.334, null };
            IFunctionHelper fhelper = Substitute.For<IFunctionHelper>();
            IIntegerHelper ihelper = Substitute.For<IIntegerHelper>();
            IRealHelper rhelper = Substitute.For<IRealHelper>();
            Interpreter interpreter = new Interpreter(fhelper, ihelper, rhelper);
            IntegerLiteralExpression res = null;
            ihelper.LiteralInteger(Arg.Do<IntegerLiteralExpression>(x => res = x), Arg.Any<List<Object>>());

            interpreter.Dispatch(input1, input2, TypeEnum.Integer);

            res.Should().BeEquivalentTo(expected);
        }

        [TestMethod]
        public void Dispatch_IntegerLiteralAndObjectListAndIntegerType_CorrectValueReturned()
        {
            int expected = 17;
            IntegerLiteralExpression input1 = new IntegerLiteralExpression("", 0, 0);
            List<Object> input2 = new List<Object>() { 23, 2.334, null };
            IFunctionHelper fhelper = Substitute.For<IFunctionHelper>();
            IIntegerHelper ihelper = Substitute.For<IIntegerHelper>();
            IRealHelper rhelper = Substitute.For<IRealHelper>();
            Interpreter interpreter = new Interpreter(fhelper, ihelper, rhelper);
            ihelper.LiteralInteger(Arg.Any<IntegerLiteralExpression>(), Arg.Any<List<Object>>()).Returns(expected);

            int res = (int)interpreter.Dispatch(input1, input2, TypeEnum.Integer);

            Assert.AreEqual(expected, res);
        }
        #endregion

        #region Dispatch_RealAddition
        [TestMethod]
        public void Dispatch_AdditionAndObjectListAndRealType_CorrectListPassed()
        {
            List<Object> expected = new List<Object>() { 23, 2.334, null };
            AdditionExpression input1 = new AdditionExpression(null, null, 0, 0);
            IFunctionHelper fhelper = Substitute.For<IFunctionHelper>();
            IIntegerHelper ihelper = Substitute.For<IIntegerHelper>();
            IRealHelper rhelper = Substitute.For<IRealHelper>();
            Interpreter interpreter = new Interpreter(fhelper, ihelper, rhelper);
            List<Object> res = null;
            rhelper.AdditionReal(Arg.Any<AdditionExpression>(), Arg.Do<List<Object>>(x => res = x));

            interpreter.Dispatch(input1, expected, TypeEnum.Real);

            res.Should().BeEquivalentTo(expected);
        }

        [TestMethod]
        public void Dispatch_AdditionAndObjectListAndRealType_CorrectAdditionExprPassed()
        {
            AdditionExpression expected = new AdditionExpression(null, null, 0, 0);
            AdditionExpression input1 = expected;
            List<Object> input2 = new List<Object>() { 23, 2.334, null };
            IFunctionHelper fhelper = Substitute.For<IFunctionHelper>();
            IIntegerHelper ihelper = Substitute.For<IIntegerHelper>();
            IRealHelper rhelper = Substitute.For<IRealHelper>();
            Interpreter interpreter = new Interpreter(fhelper, ihelper, rhelper);
            AdditionExpression res = null;
            rhelper.AdditionReal(Arg.Do<AdditionExpression>(x => res = x), Arg.Any<List<Object>>());

            interpreter.Dispatch(input1, input2, TypeEnum.Real);

            res.Should().BeEquivalentTo(expected);
        }

        [TestMethod]
        public void Dispatch_AdditionAndObjectListAndRealType_CorrectValueReturned()
        {
            double expected = 17.2;
            AdditionExpression input1 = new AdditionExpression(null, null, 0, 0);
            List<Object> input2 = new List<Object>() { 23, 2.334, null };
            IFunctionHelper fhelper = Substitute.For<IFunctionHelper>();
            IIntegerHelper ihelper = Substitute.For<IIntegerHelper>();
            IRealHelper rhelper = Substitute.For<IRealHelper>();
            Interpreter interpreter = new Interpreter(fhelper, ihelper, rhelper);
            rhelper.AdditionReal(Arg.Any<AdditionExpression>(), Arg.Any<List<Object>>()).Returns(expected);

            double res = (double)interpreter.Dispatch(input1, input2, TypeEnum.Real);

            Assert.AreEqual(expected, res);
        }
        #endregion

        #region Dispatch_FunctionIdentifier
        [TestMethod]
        public void Dispatch_IdentifierAndObjectListAndFunctionType_CorrectListPassed()
        {
            List<Object> expected = new List<Object>() { 23, 2.334, null };
            IdentifierExpression input1 = new IdentifierExpression("", 0, 0);
            IFunctionHelper fhelper = Substitute.For<IFunctionHelper>();
            IIntegerHelper ihelper = Substitute.For<IIntegerHelper>();
            IRealHelper rhelper = Substitute.For<IRealHelper>();
            Interpreter interpreter = new Interpreter(fhelper, ihelper, rhelper);
            List<Object> res = null;
            fhelper.IdentifierFunction(Arg.Any<IdentifierExpression>(), Arg.Do<List<Object>>(x => res = x));

            interpreter.Dispatch(input1, expected, TypeEnum.Function);

            res.Should().BeEquivalentTo(expected);
        }

        [TestMethod]
        public void Dispatch_IdentifierAndObjectListAndFunctionType_CorrectIdentifierExprPassed()
        {
            IdentifierExpression expected = new IdentifierExpression("", 0, 0);
            IdentifierExpression input1 = expected;
            List<Object> input2 = new List<Object>() { 23, 2.334, null };
            IFunctionHelper fhelper = Substitute.For<IFunctionHelper>();
            IIntegerHelper ihelper = Substitute.For<IIntegerHelper>();
            IRealHelper rhelper = Substitute.For<IRealHelper>();
            Interpreter interpreter = new Interpreter(fhelper, ihelper, rhelper);
            IdentifierExpression res = null;
            fhelper.IdentifierFunction(Arg.Do<IdentifierExpression>(x => res = x), Arg.Any<List<Object>>());

            interpreter.Dispatch(input1, input2, TypeEnum.Function);

            res.Should().BeEquivalentTo(expected);
        }

        [TestMethod]
        public void Dispatch_IdentifierAndObjectListAndFunctionType_CorrectValueReturned()
        {
            int expected = 17;
            IdentifierExpression input1 = new IdentifierExpression("", 0, 0);
            List<Object> input2 = new List<Object>() { 23, 2.334, null };
            IFunctionHelper fhelper = Substitute.For<IFunctionHelper>();
            IIntegerHelper ihelper = Substitute.For<IIntegerHelper>();
            IRealHelper rhelper = Substitute.For<IRealHelper>();
            Interpreter interpreter = new Interpreter(fhelper, ihelper, rhelper);
            fhelper.IdentifierFunction(Arg.Any<IdentifierExpression>(), Arg.Any<List<Object>>()).Returns(expected);

            int res = (int)interpreter.Dispatch(input1, input2, TypeEnum.Function);

            Assert.AreEqual(expected, res);
        }
        #endregion

        #endregion

        #region FunctionFunction
        [TestMethod]
        public void FunctionFunction_FunctionNodeAndObjectList_CorrectListPassed()
        {
            List<Object> expected = new List<Object>() { 23, 2.334, null };
            FunctionNode input1 = new FunctionNode("", 0, new ConditionNode(null, 0, 0), null, null, 0, 0);
            IFunctionHelper fhelper = Substitute.For<IFunctionHelper>();
            IIntegerHelper ihelper = Substitute.For<IIntegerHelper>();
            IRealHelper rhelper = Substitute.For<IRealHelper>();
            Interpreter interpreter = new Interpreter(fhelper, ihelper, rhelper);
            List<Object> res = null;
            fhelper.ConditionFunction(Arg.Any<ConditionNode>(), Arg.Do<List<Object>>(x => res = x));

            interpreter.FunctionFunction(input1, expected);

            res.Should().BeEquivalentTo(expected);
        }

        [TestMethod]
        public void FunctionFunction_FunctionNodeAndObjectList_CorrectIntegerLiteralExprPassed()
        {
            ConditionNode expected = new ConditionNode(null, 0, 0);
            FunctionNode input1 = new FunctionNode("", 0, expected, null, null, 0, 0);
            List<Object> input2 = new List<Object>() { 23, 2.334, null };
            IFunctionHelper fhelper = Substitute.For<IFunctionHelper>();
            IIntegerHelper ihelper = Substitute.For<IIntegerHelper>();
            IRealHelper rhelper = Substitute.For<IRealHelper>();
            Interpreter interpreter = new Interpreter(fhelper, ihelper, rhelper);
            ConditionNode res = null;
            fhelper.ConditionFunction(Arg.Do<ConditionNode>(x => res = x), Arg.Any<List<Object>>());

            interpreter.FunctionFunction(input1, input2);

            res.Should().BeEquivalentTo(expected);
        }

        [TestMethod]
        public void FunctionFunction_FunctionNodeAndObjectList_CorrectValueReturned()
        {
            int expected = 17;
            FunctionNode input1 = new FunctionNode("", 0, new ConditionNode(null, 0, 0), null, null, 0, 0);
            List<Object> input2 = new List<Object>() { 23, 2.334, null };
            IFunctionHelper fhelper = Substitute.For<IFunctionHelper>();
            IIntegerHelper ihelper = Substitute.For<IIntegerHelper>();
            IRealHelper rhelper = Substitute.For<IRealHelper>();
            Interpreter interpreter = new Interpreter(fhelper, ihelper, rhelper);
            fhelper.ConditionFunction(Arg.Any<ConditionNode>(), Arg.Any<List<Object>>()).Returns(expected);

            int res = (int) interpreter.FunctionFunction(input1, input2);

            Assert.AreEqual(expected, res);
        }

        #endregion

        #region FunctionInteger
        [TestMethod]
        public void FunctionInteger_FunctionNodeAndObjectList_CorrectListPassed()
        {
            List<Object> expected = new List<Object>() { 23, 2.334, null };
            FunctionNode input1 = new FunctionNode("", 0, new ConditionNode(null, 0, 0), null, null, 0, 0);
            IFunctionHelper fhelper = Substitute.For<IFunctionHelper>();
            IIntegerHelper ihelper = Substitute.For<IIntegerHelper>();
            IRealHelper rhelper = Substitute.For<IRealHelper>();
            Interpreter interpreter = new Interpreter(fhelper, ihelper, rhelper);
            List<Object> res = null;
            ihelper.ConditionInteger(Arg.Any<ConditionNode>(), Arg.Do<List<Object>>(x => res = x));

            interpreter.FunctionInteger(input1, expected);

            res.Should().BeEquivalentTo(expected);
        }

        [TestMethod]
        public void FunctionInteger_FunctionNodeAndObjectList_CorrectIntegerLiteralExprPassed()
        {
            ConditionNode expected = new ConditionNode(null, 0, 0);
            FunctionNode input1 = new FunctionNode("", 0, expected, null, null, 0, 0);
            List<Object> input2 = new List<Object>() { 23, 2.334, null };
            IFunctionHelper fhelper = Substitute.For<IFunctionHelper>();
            IIntegerHelper ihelper = Substitute.For<IIntegerHelper>();
            IRealHelper rhelper = Substitute.For<IRealHelper>();
            Interpreter interpreter = new Interpreter(fhelper, ihelper, rhelper);
            ConditionNode res = null;
            ihelper.ConditionInteger(Arg.Do<ConditionNode>(x => res = x), Arg.Any<List<Object>>());

            interpreter.FunctionInteger(input1, input2);

            res.Should().BeEquivalentTo(expected);
        }

        [TestMethod]
        public void FunctionInteger_FunctionNodeAndObjectList_CorrectValueReturned()
        {
            int expected = 17;
            FunctionNode input1 = new FunctionNode("", 0, new ConditionNode(null, 0, 0), null, null, 0, 0);
            List<Object> input2 = new List<Object>() { 23, 2.334, null };
            IFunctionHelper fhelper = Substitute.For<IFunctionHelper>();
            IIntegerHelper ihelper = Substitute.For<IIntegerHelper>();
            IRealHelper rhelper = Substitute.For<IRealHelper>();
            Interpreter interpreter = new Interpreter(fhelper, ihelper, rhelper);
            ihelper.ConditionInteger(Arg.Any<ConditionNode>(), Arg.Any<List<Object>>()).Returns(expected);

            int res = (int) interpreter.FunctionInteger(input1, input2);

            Assert.AreEqual(expected, res);
        }

        #endregion

        #region FunctionReal
        [TestMethod]
        public void FunctionReal_FunctionNodeAndObjectList_CorrectListPassed()
        {
            List<Object> expected = new List<Object>() { 23, 2.334, null };
            FunctionNode input1 = new FunctionNode("", 0, new ConditionNode(null, 0, 0), null, null, 0, 0);
            IFunctionHelper fhelper = Substitute.For<IFunctionHelper>();
            IIntegerHelper ihelper = Substitute.For<IIntegerHelper>();
            IRealHelper rhelper = Substitute.For<IRealHelper>();
            Interpreter interpreter = new Interpreter(fhelper, ihelper, rhelper);
            List<Object> res = null;
            rhelper.ConditionReal(Arg.Any<ConditionNode>(), Arg.Do<List<Object>>(x => res = x));

            interpreter.FunctionReal(input1, expected);

            res.Should().BeEquivalentTo(expected);
        }

        [TestMethod]
        public void FunctionReal_FunctionNodeAndObjectList_CorrectRealLiteralExprPassed()
        {
            ConditionNode expected = new ConditionNode(null, 0, 0);
            FunctionNode input1 = new FunctionNode("", 0, expected, null, null, 0, 0);
            List<Object> input2 = new List<Object>() { 23, 2.334, null };
            IFunctionHelper fhelper = Substitute.For<IFunctionHelper>();
            IIntegerHelper ihelper = Substitute.For<IIntegerHelper>();
            IRealHelper rhelper = Substitute.For<IRealHelper>();
            Interpreter interpreter = new Interpreter(fhelper, ihelper, rhelper);
            ConditionNode res = null;
            rhelper.ConditionReal(Arg.Do<ConditionNode>(x => res = x), Arg.Any<List<Object>>());

            interpreter.FunctionReal(input1, input2);

            res.Should().BeEquivalentTo(expected);
        }

        [TestMethod]
        public void FunctionReal_FunctionNodeAndObjectList_CorrectValueReturned()
        {
            int expected = 17;
            FunctionNode input1 = new FunctionNode("", 0, new ConditionNode(null, 0, 0), null, null, 0, 0);
            List<Object> input2 = new List<Object>() { 23, 2.334, null };
            IFunctionHelper fhelper = Substitute.For<IFunctionHelper>();
            IIntegerHelper ihelper = Substitute.For<IIntegerHelper>();
            IRealHelper rhelper = Substitute.For<IRealHelper>();
            Interpreter interpreter = new Interpreter(fhelper, ihelper, rhelper);
            rhelper.ConditionReal(Arg.Any<ConditionNode>(), Arg.Any<List<Object>>()).Returns(expected);

            int res = (int)interpreter.FunctionReal(input1, input2);

            Assert.AreEqual(expected, res);
        }

        #endregion

    }
}