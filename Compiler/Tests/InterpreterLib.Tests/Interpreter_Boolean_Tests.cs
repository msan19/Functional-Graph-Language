﻿using ASTLib.Nodes;
using ASTLib.Nodes.ExpressionNodes;
using ASTLib.Nodes.ExpressionNodes.BooleanOperationNodes;
using ASTLib.Nodes.ExpressionNodes.CommonOperationNodes;
using ASTLib.Nodes.ExpressionNodes.CommonOperationNodes.RelationalOperationNodes;
using ASTLib.Nodes.TypeNodes;
using FluentAssertions;
using InterpreterLib.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InterpreterLib.Tests
{
    [TestClass]
    public class Interpreter_Boolean_Tests
    {
        #region Helper Get Expression
        private GreaterExpression GetGreaterExp() => new GreaterExpression(null, null, 0, 0);
        private LessExpression GetLessExp() => new LessExpression(null, null, 0, 0);
        private GreaterEqualExpression GetGreaterEqualExp() => new GreaterEqualExpression(null, null, 0, 0);
        private LessEqualExpression GetLessEqualExp() => new LessEqualExpression(null, null, 0, 0);
        private EqualExpression GetEqualExp() => new EqualExpression(null, null, 0, 0);
        private NotEqualExpression GetNotEqualExp() => new NotEqualExpression(null, null, 0, 0);
        private NotExpression GetNotExp() => new NotExpression(null, 0, 0);
        private AndExpression GetAndExp() => new AndExpression(null, null, 0, 0);
        private OrExpression GetOrExp() => new OrExpression(null, null, 0, 0);
        #endregion
        private List<object> GetParameterList() => new List<object>();
        private IBooleanHelper GetBooleanHelper() => Substitute.For<IBooleanHelper>();


        #region DispatchBoolean
        #region Returns Correct
        [DataRow(true)]
        [DataRow(false)]
        [TestMethod]
        public void DispatchBool_ReturnsCorrect_Greater(bool expected)
        {
            var node = GetGreaterExp();
            IBooleanHelper boolHelper = GetBooleanHelper();
            boolHelper.GreaterBoolean(Arg.Any<GreaterExpression>(), Arg.Any<List<object>>())
                .Returns(expected);

            DispatchBool_ReturnsCorrect(node, boolHelper, expected);
        }
        [DataRow(true)]
        [DataRow(false)]
        [TestMethod]
        public void DispatchBool_ReturnsCorrect_Less(bool expected)
        {
            var node = GetLessExp();
            IBooleanHelper boolHelper = GetBooleanHelper();
            boolHelper.LessBoolean(Arg.Any<LessExpression>(), Arg.Any<List<object>>())
                .Returns(expected);

            DispatchBool_ReturnsCorrect(node, boolHelper, expected);
        }
        [DataRow(true)]
        [DataRow(false)]
        [TestMethod]
        public void DispatchBool_ReturnsCorrect_GreaterEqual(bool expected)
        {
            var node = GetGreaterEqualExp();
            IBooleanHelper boolHelper = GetBooleanHelper();
            boolHelper.GreaterEqualBoolean(Arg.Any<GreaterEqualExpression>(), Arg.Any<List<object>>())
                .Returns(expected);

            DispatchBool_ReturnsCorrect(node, boolHelper, expected);
        }
        [DataRow(true)]
        [DataRow(false)]
        [TestMethod]
        public void DispatchBool_ReturnsCorrect_LessEqual(bool expected)
        {
            var node = GetLessEqualExp();
            IBooleanHelper boolHelper = GetBooleanHelper();
            boolHelper.LessEqualBoolean(Arg.Any<LessEqualExpression>(), Arg.Any<List<object>>())
                .Returns(expected);

            DispatchBool_ReturnsCorrect(node, boolHelper, expected);
        }
        [DataRow(true)]
        [DataRow(false)]
        [TestMethod]
        public void DispatchBool_ReturnsCorrect_Equal(bool expected)
        {
            var node = GetEqualExp();
            IBooleanHelper boolHelper = GetBooleanHelper();
            boolHelper.EqualBoolean(Arg.Any<EqualExpression>(), Arg.Any<List<object>>())
                .Returns(expected);

            DispatchBool_ReturnsCorrect(node, boolHelper, expected);
        }
        [DataRow(true)]
        [DataRow(false)]
        [TestMethod]
        public void DispatchBool_ReturnsCorrect_NotEqual(bool expected)
        {
            var node = GetNotEqualExp();
            IBooleanHelper boolHelper = GetBooleanHelper();
            boolHelper.NotEqualBoolean(Arg.Any<NotEqualExpression>(), Arg.Any<List<object>>())
                .Returns(expected);

            DispatchBool_ReturnsCorrect(node, boolHelper, expected);
        }
        [DataRow(true)]
        [DataRow(false)]
        [TestMethod]
        public void DispatchBool_ReturnsCorrect_Not(bool expected)
        {
            var node = GetNotExp();
            IBooleanHelper boolHelper = GetBooleanHelper();
            boolHelper.NotBoolean(Arg.Any<NotExpression>(), Arg.Any<List<object>>())
                .Returns(expected);

            DispatchBool_ReturnsCorrect(node, boolHelper, expected);
        }
        [DataRow(true)]
        [DataRow(false)]
        [TestMethod]
        public void DispatchBool_ReturnsCorrect_And(bool expected)
        {
            var node = GetAndExp();
            IBooleanHelper boolHelper = GetBooleanHelper();
            boolHelper.AndBoolean(Arg.Any<AndExpression>(), Arg.Any<List<object>>())
                .Returns(expected);

            DispatchBool_ReturnsCorrect(node, boolHelper, expected);
        }
        [DataRow(true)]
        [DataRow(false)]
        [TestMethod]
        public void DispatchBool_ReturnsCorrect_Or(bool expected)
        {
            var node = GetOrExp();
            IBooleanHelper boolHelper = GetBooleanHelper();
            boolHelper.OrBoolean(Arg.Any<OrExpression>(), Arg.Any<List<object>>())
                .Returns(expected);

            DispatchBool_ReturnsCorrect(node, boolHelper, expected);
        }

        public void DispatchBool_ReturnsCorrect(ExpressionNode node, IBooleanHelper boolHelper, bool expected)
        {
            var parameters = GetParameterList();
            var interpreter = Utilities.GetIntepretorOnlyWith(boolHelper);

            var res = interpreter.DispatchBoolean(node, parameters);

            Assert.AreEqual(expected, res);
        }
        #endregion

        #region And
        [TestMethod]
        public void DispatchBool_And_CorrectVisitMethodCalled()
        {
            var parameters = GetParameterList();
            var node = GetAndExp();

            var boolHelper = GetBooleanHelper();
            bool res = false;
            boolHelper.AndBoolean(Arg.Any<AndExpression>(), Arg.Do<List<object>>(x => res = true))
                .Returns(true);

            var interpreter = Utilities.GetIntepretorOnlyWith(boolHelper);
            interpreter.DispatchBoolean(node, parameters);

            Assert.AreEqual(true, res);
        }

        [TestMethod]
        public void DispatchBool_And_PassNodeDown()
        {
            var parameters = GetParameterList();
            var node = GetAndExp();
            var expected = node;

            var boolHelper = GetBooleanHelper();
            Node res = null;
            boolHelper.AndBoolean(Arg.Do<AndExpression>(x => res = x), Arg.Any<List<object>>())
                .Returns(true);

            var interpreter = Utilities.GetIntepretorOnlyWith(boolHelper);
            interpreter.DispatchBoolean(node, parameters);

            Assert.AreEqual(expected, res);
        }
        [DataRow(TypeEnum.Boolean)]
        [DataRow(1)]
        [TestMethod]
        public void DispatchBool_And_PassParametersDown(object o)
        {
            var parameters = GetParameterList();
            var node = GetAndExp();

            IBooleanHelper boolHelper = GetBooleanHelper();
            List<object> res = null;
            boolHelper.AndBoolean(Arg.Any<AndExpression>(), Arg.Do<List<object>>(x => res = x))
                .Returns(true);

            var interpreter = Utilities.GetIntepretorOnlyWith(boolHelper);
            interpreter.DispatchBoolean(node, parameters.ToList());

            res.Should().BeEquivalentTo(parameters);
        }
        #endregion
        #endregion
    }
}
