using ASTLib.Exceptions;
using ASTLib.Nodes;
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

        #region FunctionBoolean
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
        #endregion

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
        #region Pass Node Down
        [TestMethod]
        public void DispatchBool_Greater_PassNodeDown()
        {
            var parameters = GetParameterList();
            var node = GetGreaterExp();
            var expected = node;

            var boolHelper = GetBooleanHelper();
            Node res = null;
            boolHelper.GreaterBoolean(Arg.Do<GreaterExpression>(x => res = x), Arg.Any<List<object>>())
                .Returns(true);

            var interpreter = Utilities.GetIntepretorOnlyWith(boolHelper);
            interpreter.DispatchBoolean(node, parameters);

            Assert.AreEqual(expected, res);
        }
        [TestMethod]
        public void DispatchBool_Less_PassNodeDown()
        {
            var parameters = GetParameterList();
            var node = GetLessExp();
            var expected = node;

            var boolHelper = GetBooleanHelper();
            Node res = null;
            boolHelper.LessBoolean(Arg.Do<LessExpression>(x => res = x), Arg.Any<List<object>>())
                .Returns(true);

            var interpreter = Utilities.GetIntepretorOnlyWith(boolHelper);
            interpreter.DispatchBoolean(node, parameters);

            Assert.AreEqual(expected, res);
        }
        [TestMethod]
        public void DispatchBool_GreaterEqual_PassNodeDown()
        {
            var parameters = GetParameterList();
            var node = GetGreaterEqualExp();
            var expected = node;

            var boolHelper = GetBooleanHelper();
            Node res = null;
            boolHelper.GreaterEqualBoolean(Arg.Do<GreaterEqualExpression>(x => res = x), Arg.Any<List<object>>())
                .Returns(true);

            var interpreter = Utilities.GetIntepretorOnlyWith(boolHelper);
            interpreter.DispatchBoolean(node, parameters);

            Assert.AreEqual(expected, res);
        }
        [TestMethod]
        public void DispatchBool_LessEqual_PassNodeDown()
        {
            var parameters = GetParameterList();
            var node = GetLessEqualExp();
            var expected = node;

            var boolHelper = GetBooleanHelper();
            Node res = null;
            boolHelper.LessEqualBoolean(Arg.Do<LessEqualExpression>(x => res = x), Arg.Any<List<object>>())
                .Returns(true);

            var interpreter = Utilities.GetIntepretorOnlyWith(boolHelper);
            interpreter.DispatchBoolean(node, parameters);

            Assert.AreEqual(expected, res);
        }
        [TestMethod]
        public void DispatchBool_Equal_PassNodeDown()
        {
            var parameters = GetParameterList();
            var node = GetEqualExp();
            var expected = node;

            var boolHelper = GetBooleanHelper();
            Node res = null;
            boolHelper.EqualBoolean(Arg.Do<EqualExpression>(x => res = x), Arg.Any<List<object>>())
                .Returns(true);

            var interpreter = Utilities.GetIntepretorOnlyWith(boolHelper);
            interpreter.DispatchBoolean(node, parameters);

            Assert.AreEqual(expected, res);
        }
        [TestMethod]
        public void DispatchBool_NotEqual_PassNodeDown()
        {
            var parameters = GetParameterList();
            var node = GetNotEqualExp();
            var expected = node;

            var boolHelper = GetBooleanHelper();
            Node res = null;
            boolHelper.NotEqualBoolean(Arg.Do<NotEqualExpression>(x => res = x), Arg.Any<List<object>>())
                .Returns(true);

            var interpreter = Utilities.GetIntepretorOnlyWith(boolHelper);
            interpreter.DispatchBoolean(node, parameters);

            Assert.AreEqual(expected, res);
        }
        [TestMethod]
        public void DispatchBool_Not_PassNodeDown()
        {
            var parameters = GetParameterList();
            var node = GetNotExp();
            var expected = node;

            var boolHelper = GetBooleanHelper();
            Node res = null;
            boolHelper.NotBoolean(Arg.Do<NotExpression>(x => res = x), Arg.Any<List<object>>())
                .Returns(true);

            var interpreter = Utilities.GetIntepretorOnlyWith(boolHelper);
            interpreter.DispatchBoolean(node, parameters);

            Assert.AreEqual(expected, res);
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
        [TestMethod]
        public void DispatchBool_Or_PassNodeDown()
        {
            var parameters = GetParameterList();
            var node = GetOrExp();
            var expected = node;

            var boolHelper = GetBooleanHelper();
            Node res = null;
            boolHelper.OrBoolean(Arg.Do<OrExpression>(x => res = x), Arg.Any<List<object>>())
                .Returns(true);

            var interpreter = Utilities.GetIntepretorOnlyWith(boolHelper);
            interpreter.DispatchBoolean(node, parameters);

            Assert.AreEqual(expected, res);
        }
        #endregion
        #region Pass Parameters Down
        [DataRow(TypeEnum.Boolean)]
        [DataRow(1)]
        [TestMethod]
        public void DispatchBool_Greater_PassParametersDown(object o)
        {
            var parameters = GetParameterList();
            var node = GetGreaterExp();

            IBooleanHelper boolHelper = GetBooleanHelper();
            List<object> res = null;
            boolHelper.GreaterBoolean(Arg.Any<GreaterExpression>(), Arg.Do<List<object>>(x => res = x))
                .Returns(true);

            var interpreter = Utilities.GetIntepretorOnlyWith(boolHelper);
            interpreter.DispatchBoolean(node, parameters.ToList());

            res.Should().BeEquivalentTo(parameters);
        }
        [DataRow(TypeEnum.Boolean)]
        [DataRow(1)]
        [TestMethod]
        public void DispatchBool_Less_PassParametersDown(object o)
        {
            var parameters = GetParameterList();
            var node = GetLessExp();

            IBooleanHelper boolHelper = GetBooleanHelper();
            List<object> res = null;
            boolHelper.LessBoolean(Arg.Any<LessExpression>(), Arg.Do<List<object>>(x => res = x))
                .Returns(true);

            var interpreter = Utilities.GetIntepretorOnlyWith(boolHelper);
            interpreter.DispatchBoolean(node, parameters.ToList());

            res.Should().BeEquivalentTo(parameters);
        }
        [DataRow(TypeEnum.Boolean)]
        [DataRow(1)]
        [TestMethod]
        public void DispatchBool_GreaterEqual_PassParametersDown(object o)
        {
            var parameters = GetParameterList();
            var node = GetGreaterEqualExp();

            IBooleanHelper boolHelper = GetBooleanHelper();
            List<object> res = null;
            boolHelper.GreaterEqualBoolean(Arg.Any<GreaterEqualExpression>(), Arg.Do<List<object>>(x => res = x))
                .Returns(true);

            var interpreter = Utilities.GetIntepretorOnlyWith(boolHelper);
            interpreter.DispatchBoolean(node, parameters.ToList());

            res.Should().BeEquivalentTo(parameters);
        }
        [DataRow(TypeEnum.Boolean)]
        [DataRow(1)]
        [TestMethod]
        public void DispatchBool_LessEqual_PassParametersDown(object o)
        {
            var parameters = GetParameterList();
            var node = GetLessEqualExp();

            IBooleanHelper boolHelper = GetBooleanHelper();
            List<object> res = null;
            boolHelper.LessEqualBoolean(Arg.Any<LessEqualExpression>(), Arg.Do<List<object>>(x => res = x))
                .Returns(true);

            var interpreter = Utilities.GetIntepretorOnlyWith(boolHelper);
            interpreter.DispatchBoolean(node, parameters.ToList());

            res.Should().BeEquivalentTo(parameters);
        }
        [DataRow(TypeEnum.Boolean)]
        [DataRow(1)]
        [TestMethod]
        public void DispatchBool_Equal_PassParametersDown(object o)
        {
            var parameters = GetParameterList();
            var node = GetEqualExp();

            IBooleanHelper boolHelper = GetBooleanHelper();
            List<object> res = null;
            boolHelper.EqualBoolean(Arg.Any<EqualExpression>(), Arg.Do<List<object>>(x => res = x))
                .Returns(true);

            var interpreter = Utilities.GetIntepretorOnlyWith(boolHelper);
            interpreter.DispatchBoolean(node, parameters.ToList());

            res.Should().BeEquivalentTo(parameters);
        }
        [DataRow(TypeEnum.Boolean)]
        [DataRow(1)]
        [TestMethod]
        public void DispatchBool_NotEqual_PassParametersDown(object o)
        {
            var parameters = GetParameterList();
            var node = GetNotEqualExp();

            IBooleanHelper boolHelper = GetBooleanHelper();
            List<object> res = null;
            boolHelper.NotEqualBoolean(Arg.Any<NotEqualExpression>(), Arg.Do<List<object>>(x => res = x))
                .Returns(true);

            var interpreter = Utilities.GetIntepretorOnlyWith(boolHelper);
            interpreter.DispatchBoolean(node, parameters.ToList());

            res.Should().BeEquivalentTo(parameters);
        }
        [DataRow(TypeEnum.Boolean)]
        [DataRow(1)]
        [TestMethod]
        public void DispatchBool_Not_PassParametersDown(object o)
        {
            var parameters = GetParameterList();
            var node = GetNotExp();

            IBooleanHelper boolHelper = GetBooleanHelper();
            List<object> res = null;
            boolHelper.NotBoolean(Arg.Any<NotExpression>(), Arg.Do<List<object>>(x => res = x))
                .Returns(true);

            var interpreter = Utilities.GetIntepretorOnlyWith(boolHelper);
            interpreter.DispatchBoolean(node, parameters.ToList());

            res.Should().BeEquivalentTo(parameters);
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
        [DataRow(TypeEnum.Boolean)]
        [DataRow(1)]
        [TestMethod]
        public void DispatchBool_Or_PassParametersDown(object o)
        {
            var parameters = GetParameterList();
            var node = GetOrExp();

            IBooleanHelper boolHelper = GetBooleanHelper();
            List<object> res = null;
            boolHelper.OrBoolean(Arg.Any<OrExpression>(), Arg.Do<List<object>>(x => res = x))
                .Returns(true);

            var interpreter = Utilities.GetIntepretorOnlyWith(boolHelper);
            interpreter.DispatchBoolean(node, parameters.ToList());

            res.Should().BeEquivalentTo(parameters);
        }
        #endregion
        #endregion
    }
}
