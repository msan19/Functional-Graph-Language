using System;
using System.Collections.Generic;
using ASTLib;
using ASTLib.Nodes;
using ASTLib.Nodes.ExpressionNodes;
using ASTLib.Nodes.ExpressionNodes.OperationNodes;
using ASTLib.Nodes.TypeNodes;
using FluentAssertions;
using InterpreterLib.Helpers;
using InterpreterLib.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;

namespace InterpreterLib.Tests
{
    [TestClass]
    public class IntegerlHelperTests
    {

        #region FunctionInteger
        [DataRow(2, 2)]
        [TestMethod]
        public void ConditionInteger_Integer_ReturnsCorrectResult(int input, int expected)
        {
            IntegerLiteralExpression intLit = new IntegerLiteralExpression(input.ToString(), 1, 1);
            ConditionNode conditionNode = new ConditionNode(intLit, 1, 1);
            IInterpreter parent = Substitute.For<IInterpreter>();
            parent.DispatchInt(intLit, Arg.Any<List<object>>()).Returns(input);
            IntegerHelper integerHelper = new IntegerHelper()
            {
                Interpreter = parent
            };

            int res = integerHelper.ConditionInteger(conditionNode, new List<object>());

            Assert.AreEqual(expected, res);
        }
        #endregion

        #region AdditionInteger
        [DataRow(2, 2, 4)]
        [TestMethod]
        public void AdditionInteger_TwoIntegers_ReturnsCorrectResultOfAddition(int input1, int input2, int expected)
        {
            IntegerLiteralExpression intLit1 = new IntegerLiteralExpression(input1.ToString(), 1, 1);
            IntegerLiteralExpression intLit2 = new IntegerLiteralExpression(input2.ToString(), 2, 2);
            AdditionExpression addExpr = new AdditionExpression(intLit1, intLit2, 1, 1);
            IInterpreter parent = Substitute.For<IInterpreter>();
            parent.DispatchInt(intLit1, Arg.Any<List<object>>()).Returns(input1);
            parent.DispatchInt(intLit2, Arg.Any<List<object>>()).Returns(input2);
            IntegerHelper integerHelper = new IntegerHelper()
            {
                Interpreter = parent
            };

            int res = integerHelper.AdditionInteger(addExpr, new List<object>());

            Assert.AreEqual(expected, res);
        }
        #endregion

        #region SubtractionInteger
        [DataRow(2, 2, 0)]
        [DataRow(10, 8, 2)]
        [DataRow(8, 10, -2)]
        [TestMethod]
        public void SubtractionInteger_TwoIntegers_ReturnsCorrectResultOfSubtraction(int input1, int input2, int expected)
        {
            IntegerLiteralExpression intLit1 = new IntegerLiteralExpression(input1.ToString(), 1, 1);
            IntegerLiteralExpression intLit2 = new IntegerLiteralExpression(input2.ToString(), 2, 2);
            SubtractionExpression subExpr = new SubtractionExpression(intLit1, intLit2, 1, 1);
            IInterpreter parent = Substitute.For<IInterpreter>();
            parent.DispatchInt(intLit1, Arg.Any<List<object>>()).Returns(input1);
            parent.DispatchInt(intLit2, Arg.Any<List<object>>()).Returns(input2);
            IntegerHelper integerHelper = new IntegerHelper()
            {
                Interpreter = parent
            };

            int res = integerHelper.SubtractionInteger(subExpr, new List<object>());

            Assert.AreEqual(expected, res);
        }
        #endregion

        #region MultiplicationInteger
        [DataRow(2, 2, 4)]
        [DataRow(10, 8, 80)]
        [DataRow(-2, 2, -4)]
        [TestMethod]
        public void MultiplicationInteger_TwoIntegers_ReturnsCorrectResultOfMultiplication(int input1, int input2, int expected)
        {
            IntegerLiteralExpression intLit1 = new IntegerLiteralExpression(input1.ToString(), 1, 1);
            IntegerLiteralExpression intLit2 = new IntegerLiteralExpression(input2.ToString(), 2, 2);
            MultiplicationExpression multExpr = new MultiplicationExpression(intLit1, intLit2, 1, 1);
            IInterpreter parent = Substitute.For<IInterpreter>();
            parent.DispatchInt(intLit1, Arg.Any<List<object>>()).Returns(input1);
            parent.DispatchInt(intLit2, Arg.Any<List<object>>()).Returns(input2);
            IntegerHelper integerHelper = new IntegerHelper()
            {
                Interpreter = parent
            };

            int res = integerHelper.MultiplicationInteger(multExpr, new List<object>());

            Assert.AreEqual(expected, res);
        }
        #endregion

        #region DivisionInteger
        [DataRow(2, 2, 1)]
        [DataRow(8, 2, 4)]
        [TestMethod]
        public void DivisionInteger_TwoIntegers_ReturnsCorrectResultOfDivision(int input1, int input2, int expected)
        {
            IntegerLiteralExpression intLit1 = new IntegerLiteralExpression(input1.ToString(), 1, 1);
            IntegerLiteralExpression intLit2 = new IntegerLiteralExpression(input2.ToString(), 2, 2);
            DivisionExpression divExpr = new DivisionExpression(intLit1, intLit2, 1, 1);
            IInterpreter parent = Substitute.For<IInterpreter>();
            parent.DispatchInt(intLit1, Arg.Any<List<object>>()).Returns(input1);
            parent.DispatchInt(intLit2, Arg.Any<List<object>>()).Returns(input2);
            IntegerHelper integerHelper = new IntegerHelper()
            {
                Interpreter = parent
            };

            int res = integerHelper.DivisionInteger(divExpr, new List<object>());

            Assert.AreEqual(expected, res);
        }
        #endregion

        #region ModuloInteger
        [DataRow(2, 2, 0)]
        //[DataRow(2, 8, 2)]
        [TestMethod]
        public void ModuloInteger_TwoIntegers_ReturnsCorrectResultOfModulo(int input1, int input2, int expected)
        {
            IntegerLiteralExpression intLit1 = new IntegerLiteralExpression(input1.ToString(), 1, 1);
            IntegerLiteralExpression intLit2 = new IntegerLiteralExpression(input2.ToString(), 2, 2);
            ModuloExpression modExpr = new ModuloExpression(intLit1, intLit2, 1, 1);
            IInterpreter parent = Substitute.For<IInterpreter>();
            parent.DispatchInt(intLit1, Arg.Any<List<object>>()).Returns(input1);
            parent.DispatchInt(intLit2, Arg.Any<List<object>>()).Returns(input2);
            IntegerHelper integerHelper = new IntegerHelper()
            {
                Interpreter = parent
            };

            int res = integerHelper.ModuloInteger(modExpr, new List<object>());

            Assert.AreEqual(expected, res);
        }
        #endregion
              
        #region AbsoluteInteger
        [DataRow(-2, 2)]
        [TestMethod]
        public void AbsoluteInteger_Integer_ReturnsAbsoluteValue(int input, int expected)
        {
            IntegerLiteralExpression intLit = new IntegerLiteralExpression(input.ToString(), 1, 1);
            AbsoluteValueExpression absExpr = new AbsoluteValueExpression(intLit, 1, 1);
            absExpr.Type = TypeEnum.Integer;
            IInterpreter parent = Substitute.For<IInterpreter>();
            parent.DispatchInt(intLit, Arg.Any<List<object>>()).Returns(input);
            IntegerHelper integerHelper = new IntegerHelper()
            {
                Interpreter = parent
            };

            int res = integerHelper.AbsoluteInteger(absExpr, new List<object>());

            Assert.AreEqual(expected, res);
        }
        #endregion

        #region LiteralInteger
        [DataRow(2, 2)]
        [TestMethod]
        public void LiteralInteger_GivenInteger_ReturnsIntegerLiteral(int input, int expected)
        {
            IntegerLiteralExpression intLit = new IntegerLiteralExpression(input.ToString(), 1, 1);
            IInterpreter parent = Substitute.For<IInterpreter>();
            parent.DispatchInt(intLit, Arg.Any<List<object>>()).Returns(input);
            IntegerHelper integerHelper = new IntegerHelper()
            {
                Interpreter = parent
            };

            int res = integerHelper.LiteralInteger(intLit, new List<object>());

            Assert.AreEqual(expected, res);
        }
        #endregion

        #region IdentifierInteger
        [TestMethod]
        public void IdentifierInteger_IdentifierNode_ReturnsCorrectResult()
        {
            IdentifierExpression identifierExpr = new IdentifierExpression("This is a test", 1, 1);
            identifierExpr.Reference = 0;
            List<object> parameters = new List<object> { 0 };
            int expected = (int)parameters[0];
            IntegerHelper integerHelper = new IntegerHelper();

            int res = integerHelper.IdentifierInteger(identifierExpr, parameters);

            Assert.AreEqual(expected, res);
        }
        #endregion

        #region FunctionCallInteger
        #endregion

    }
}