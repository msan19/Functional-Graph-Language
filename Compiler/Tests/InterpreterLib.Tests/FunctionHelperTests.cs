using ASTLib.Nodes;
using ASTLib.Nodes.ExpressionNodes;
using FluentAssertions;
using InterpreterLib.Helpers;
using InterpreterLib.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;

namespace InterpreterLib.Tests
{
    [TestClass]
    public class FunctionHelperTests
    {
        #region ConditionFunction
        [DataRow(1, 1)]
        [DataRow(3, 3)]
        [DataRow(4, 4)]
        [TestMethod]
        public void ConditionFunction_ConditionNodeAndObjectList_ReturnsCorrectResult(int input, int expected)
        {
            IdentifierExpression id = new IdentifierExpression("", 1, 1);
            ConditionNode conditionNode = new ConditionNode(id, 1, 1);
            IInterpreter parent = Substitute.For<IInterpreter>();
            parent.DispatchFunction(id, Arg.Any<List<Object>>()).Returns(input);
            FunctionHelper functionHelper = new FunctionHelper()
            {
                Interpreter = parent
            };

            int res = functionHelper.ConditionFunction(conditionNode, new List<Object>());

            Assert.AreEqual(expected, res);
        }

        [TestMethod]
        public void ConditionFunction_ConditionNodeAndObjectList_CorrectListPassed()
        {
            IdentifierExpression id = new IdentifierExpression("", 1, 1);
            ConditionNode conditionNode = new ConditionNode(id, 1, 1);
            IInterpreter parent = Substitute.For<IInterpreter>();
            List<Object> res = null;
            parent.DispatchFunction(id, Arg.Do<List<Object>>(x => res = x));
            List<Object> expected = new List<Object> { 1, 1.3, "" };
            FunctionHelper functionHelper = new FunctionHelper()
            {
                Interpreter = parent
            };

            functionHelper.ConditionFunction(conditionNode, expected);

            res.Should().BeEquivalentTo(expected);
        }

        [TestMethod]
        public void ConditionFunction_ConditionNodeAndObjectList_CorrectNodePassed()
        {
            IdentifierExpression id = new IdentifierExpression("", 1, 1);
            ExpressionNode expected = id;
            ConditionNode conditionNode = new ConditionNode(id, 1, 1);
            IInterpreter parent = Substitute.For<IInterpreter>();
            ExpressionNode res = null;
            parent.DispatchFunction(Arg.Do<ExpressionNode>(x => res = x), Arg.Any<List<Object>>());
            List<Object> input2 = new List<Object> { 1, 1.3, "" };
            FunctionHelper functionHelper = new FunctionHelper()
            {
                Interpreter = parent
            };

            functionHelper.ConditionFunction(conditionNode, input2);

            res.Should().BeEquivalentTo(expected);
        }
        #endregion

        #region IdentifierFunction
        [DataRow(false, 1, new Object[] { 0, 18, "" })]
        [DataRow(false, 10, new Object[] { 0, 18, "" })]
        [DataRow(true, 0, new Object[] { 0, 18, "" })]
        [DataRow(true, 4, new Object[] { 0, 18, "", 104, 17})]
        [TestMethod]
        public void IdentifierFunction_IdentifierExpressionAndObjectList_ReturnsCorrectResult(bool isLocal, int reference, Object[] array)
        {
            IdentifierExpression identifier = new IdentifierExpression("", 0, 0)
            {
                IsLocal = isLocal,
                Reference = reference
            };
            FunctionHelper fhelper = new FunctionHelper();
            int expected = isLocal ? (int) array[reference] : reference;

            int res = fhelper.IdentifierFunction(identifier, array.ToList());

            Assert.AreEqual(res, expected);
        }
        #endregion
    }
}