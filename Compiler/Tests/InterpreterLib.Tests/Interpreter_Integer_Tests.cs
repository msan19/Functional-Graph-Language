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
    public class Interpreter_Integer_Tests
    {


        #region FunctionInteger
        [TestMethod]
        public void FunctionInteger_FunctionNodeAndObjectList_CorrectListPassed()
        {
            List<Object> expected = new List<Object>() { 23, 2.334, null };
            FunctionNode input1 = new FunctionNode("", new ConditionNode(null, 0, 0), null, null, 0, 0);
            IIntegerHelper ihelper = Substitute.For<IIntegerHelper>();
            Interpreter interpreter = Utilities.GetIntepretorOnlyWith(ihelper);
            List<Object> res = null;
            ihelper.ConditionInteger(Arg.Any<ConditionNode>(), Arg.Do<List<Object>>(x => res = x)).Returns(1);

            interpreter.FunctionInteger(input1, expected);

            res.Should().BeEquivalentTo(expected);
        }

        [TestMethod]
        public void FunctionInteger_FunctionNodeAndObjectList_CorrectIntegerLiteralExprPassed()
        {
            ConditionNode expected = new ConditionNode(null, 0, 0);
            FunctionNode input1 = new FunctionNode("", expected, null, null, 0, 0);
            List<Object> input2 = new List<Object>() { 23, 2.334, null };
            IIntegerHelper ihelper = Substitute.For<IIntegerHelper>();
            Interpreter interpreter = Utilities.GetIntepretorOnlyWith(ihelper);
            ConditionNode res = null;
            ihelper.ConditionInteger(Arg.Do<ConditionNode>(x => res = x), Arg.Any<List<Object>>()).Returns(1);

            interpreter.FunctionInteger(input1, input2);

            res.Should().BeEquivalentTo(expected);
        }

        [TestMethod]
        public void FunctionInteger_FunctionNodeAndObjectList_CorrectValueReturned()
        {
            int expected = 17;
            FunctionNode input1 = new FunctionNode("", new ConditionNode(null, 0, 0), null, null, 0, 0);
            List<Object> input2 = new List<Object>() { 23, 2.334, null };
            IIntegerHelper ihelper = Substitute.For<IIntegerHelper>();
            Interpreter interpreter = Utilities.GetIntepretorOnlyWith(ihelper);
            ihelper.ConditionInteger(Arg.Any<ConditionNode>(), Arg.Any<List<Object>>()).Returns(expected);

            int res = (int)interpreter.FunctionInteger(input1, input2);

            Assert.AreEqual(expected, res);
        }

        [DataRow(17, 13, 17)]
        [DataRow(null, 13, 13)]
        [DataRow(17, null, 17)]
        [TestMethod]
        public void FunctionInteger_FunctionNodeAndTwoConditionsOneDefaultCaseAndObjectList_CorrectValueReturned(int? a, int? b, int? expected)
        {
            ConditionNode cn1 = new ConditionNode(new BooleanLiteralExpression(true, 0, 0), null, 0, 0);
            ConditionNode cn2 = new ConditionNode(null, 0, 0);
            FunctionNode input1 = new FunctionNode(new List<ConditionNode> { cn1, cn2 }, "", null, null, 0, 0);
            List<Object> input2 = new List<Object>() { 23, 2.334, null };
            IIntegerHelper fhelper = Substitute.For<IIntegerHelper>();
            Interpreter interpreter = Utilities.GetIntepretorOnlyWith(fhelper);
            fhelper.ConditionInteger(cn1, Arg.Any<List<Object>>()).Returns(a);
            fhelper.ConditionInteger(cn2, Arg.Any<List<Object>>()).Returns(b);

            int res = (int)interpreter.FunctionInteger(input1, input2);

            Assert.AreEqual(expected, res);
        }

        [TestMethod]
        public void FunctionInteger_FunctionNodeAndTwoConditionAndObjectList_CorrectExceptionThrown()
        {
            ConditionNode cn1 = new ConditionNode(new BooleanLiteralExpression(true, 0, 0), null, 0, 0);
            ConditionNode cn2 = new ConditionNode(new BooleanLiteralExpression(true, 0, 0), null, 0, 0);
            FunctionNode input1 = new FunctionNode(new List<ConditionNode> { cn1, cn2 }, "", null, null, 0, 0);
            List<Object> input2 = new List<Object>() { 23, 2.334, null };
            IIntegerHelper fhelper = Substitute.For<IIntegerHelper>();
            Interpreter interpreter = Utilities.GetIntepretorOnlyWith(fhelper);
            fhelper.ConditionInteger(cn1, Arg.Any<List<Object>>()).Returns(17);
            fhelper.ConditionInteger(cn2, Arg.Any<List<Object>>()).Returns(18);

            Assert.ThrowsException<Exception>(() => interpreter.FunctionInteger(input1, input2));
        }

        [TestMethod]
        public void FunctionInteger_FunctionNodeAndTwoConditionsOneDefaultCaseAndObjectList_CorrectExceptionThrown()
        {
            ConditionNode cn1 = new ConditionNode(new BooleanLiteralExpression(true, 0, 0), null, 0, 0);
            ConditionNode cn2 = new ConditionNode(new BooleanLiteralExpression(true, 0, 0), 0, 0);
            FunctionNode input1 = new FunctionNode(new List<ConditionNode> { cn1, cn2 }, "", null, null, 0, 0);
            List<Object> input2 = new List<Object>() { 23, 2.334, null };
            IIntegerHelper fhelper = Substitute.For<IIntegerHelper>();
            Interpreter interpreter = Utilities.GetIntepretorOnlyWith(fhelper);
            fhelper.ConditionInteger(cn1, Arg.Any<List<Object>>());
            fhelper.ConditionInteger(cn2, Arg.Any<List<Object>>());

            Assert.ThrowsException<Exception>(() => interpreter.FunctionInteger(input1, input2));
        }

        #endregion


        #region DispatchInt
        #region DispatchInt_functionCallExpr
        [TestMethod]
        public void DispatchInteger_FunctionCallAndObjectList_CorrectListPassed()
        {
            List<Object> expected = new List<Object>() { 23, 2.334, null };
            FunctionCallExpression input1 = new FunctionCallExpression("", null, 0, 0);
            IIntegerHelper ihelper = Substitute.For<IIntegerHelper>();
            Interpreter interpreter = Utilities.GetIntepretorOnlyWith(ihelper);
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
            IIntegerHelper ihelper = Substitute.For<IIntegerHelper>();
            Interpreter interpreter = Utilities.GetIntepretorOnlyWith(ihelper);
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
            IIntegerHelper ihelper = Substitute.For<IIntegerHelper>();
            Interpreter interpreter = Utilities.GetIntepretorOnlyWith(ihelper);
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
            IIntegerHelper ihelper = Substitute.For<IIntegerHelper>();
            Interpreter interpreter = Utilities.GetIntepretorOnlyWith(ihelper);
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
            Interpreter interpreter = Utilities.GetIntepretorOnlyWith(ihelper);
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
            Interpreter interpreter = Utilities.GetIntepretorOnlyWith(ihelper);
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
            Interpreter interpreter = Utilities.GetIntepretorOnlyWith(ihelper);
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
            Interpreter interpreter = Utilities.GetIntepretorOnlyWith(ihelper);
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
            Interpreter interpreter = Utilities.GetIntepretorOnlyWith(ihelper);
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
            Interpreter interpreter = Utilities.GetIntepretorOnlyWith(ihelper);
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
            Interpreter interpreter = Utilities.GetIntepretorOnlyWith(ihelper);
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
            Interpreter interpreter = Utilities.GetIntepretorOnlyWith(ihelper);
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
            Interpreter interpreter = Utilities.GetIntepretorOnlyWith(ihelper);
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
            Interpreter interpreter = Utilities.GetIntepretorOnlyWith(ihelper);
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
            Interpreter interpreter = Utilities.GetIntepretorOnlyWith(ihelper);
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
            Interpreter interpreter = Utilities.GetIntepretorOnlyWith(ihelper);
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
            Interpreter interpreter = Utilities.GetIntepretorOnlyWith(ihelper);
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
            Interpreter interpreter = Utilities.GetIntepretorOnlyWith(ihelper);
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
            Interpreter interpreter = Utilities.GetIntepretorOnlyWith(ihelper);
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
            Interpreter interpreter = Utilities.GetIntepretorOnlyWith(ihelper);
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
            Interpreter interpreter = Utilities.GetIntepretorOnlyWith(ihelper);
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
            IIntegerHelper ihelper = Substitute.For<IIntegerHelper>();
            Interpreter interpreter = Utilities.GetIntepretorOnlyWith(ihelper);
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
            IIntegerHelper ihelper = Substitute.For<IIntegerHelper>();
            Interpreter interpreter = Utilities.GetIntepretorOnlyWith(ihelper);
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
            IIntegerHelper ihelper = Substitute.For<IIntegerHelper>();
            Interpreter interpreter = Utilities.GetIntepretorOnlyWith(ihelper);
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
            IIntegerHelper ihelper = Substitute.For<IIntegerHelper>();
            Interpreter interpreter = Utilities.GetIntepretorOnlyWith(ihelper);
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
            Interpreter interpreter = Utilities.GetIntepretorOnlyWith(ihelper);
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
            Interpreter interpreter = Utilities.GetIntepretorOnlyWith(ihelper);
            ihelper.LiteralInteger(Arg.Any<IntegerLiteralExpression>(), Arg.Any<List<Object>>()).Returns(expected);

            int res = interpreter.DispatchInt(input1, input2);

            Assert.AreEqual(expected, res);
        }
        #endregion

        #endregion
    }
}
