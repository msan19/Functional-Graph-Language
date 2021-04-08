using System;
using System.Collections.Generic;
using ASTLib.Nodes.ExpressionNodes;
using ASTLib.Nodes.ExpressionNodes.BooleanOperationNodes;
using ASTLib.Nodes.ExpressionNodes.CommonOperationNodes;
using ASTLib.Nodes.ExpressionNodes.CommonOperationNodes.RelationalOperationNodes;
using ASTLib.Nodes.TypeNodes;
using InterpreterLib.Helpers;
using InterpreterLib.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;

namespace InterpreterLib.Tests
{
    [TestClass]
    public class BooleanHelperTests
    {
        #region ConditionBoolean
        #endregion

        #region FunctionCallBoolean

        #endregion

        #region IdentifierBoolean
        [DataRow("true", true)]
        [DataRow("false", false)]
        [TestMethod]
        public void Identifier_Index0_CorrectValuesReturned(string input, bool expected)
        {
            var expression = GetIdentifier(0);
            var parameters = new List<object>() { input };
            IInterpreterBoolean parent = Substitute.For<IInterpreterBoolean>();

            BooleanHelper booleanHelper = Utilities.GetBooleanHelper(parent);

            bool res = booleanHelper.IdentifierBoolean(expression, parameters);

            Assert.AreEqual(expected, res);
        }

        [DataRow("true", true)]
        [DataRow("false", false)]
        [TestMethod]
        public void Identifier_Index1_CorrectValuesReturned(string input, bool expected)
        {
            var expression = GetIdentifier(1);
            var parameters = new List<object>() { "testing", input };
            IInterpreterBoolean parent = Substitute.For<IInterpreterBoolean>();

            BooleanHelper booleanHelper = Utilities.GetBooleanHelper(parent);

            bool res = booleanHelper.IdentifierBoolean(expression, parameters);

            Assert.AreEqual(expected, res);
        }

        private IdentifierExpression GetIdentifier(int reference)
        {
            return new IdentifierExpression("", 0, 0) { Reference = reference };
        }
        #endregion

        #region NotBoolean
        [DataRow(false, true)]
        [DataRow(true, false)]
        [TestMethod]
        public void NotBoolean_CorrectValuesReturned(bool input, bool expected)
        {
            BooleanLiteralExpression lhs = new BooleanLiteralExpression(input, 0, 0);
            
            var expr = new NotExpression(lhs, 0, 0);
            IInterpreterBoolean parent = Substitute.For<IInterpreterBoolean>();
            parent.DispatchBoolean(lhs, Arg.Any<List<object>>()).Returns(input);

            BooleanHelper booleanHelper = Utilities.GetBooleanHelper(parent);

            bool res = booleanHelper.NotBoolean(expr, new List<object>());

            Assert.AreEqual(expected, res);
        }
        #endregion

        #region AndBoolean
        [DataRow(true, true, true)]
        [DataRow(true, false, false)]
        [DataRow(false, true, false)]
        [DataRow(false, false, false)]
        [TestMethod]
        public void AndBoolean__CorrectValuesReturned(bool lhsValue, bool rhsValue, bool expected)
        {
            BooleanLiteralExpression lhs = new BooleanLiteralExpression(lhsValue, 0, 0);
            BooleanLiteralExpression rhs = new BooleanLiteralExpression(rhsValue, 0, 0);

            AndExpression andExpr = new AndExpression(lhs, rhs, 0, 0);
            IInterpreterBoolean parent = Substitute.For<IInterpreterBoolean>();
            parent.DispatchBoolean(lhs, Arg.Any<List<object>>()).Returns(lhsValue);
            parent.DispatchBoolean(rhs, Arg.Any<List<object>>()).Returns(rhsValue);

            BooleanHelper booleanHelper = Utilities.GetBooleanHelper(parent);

            bool res = booleanHelper.AndBoolean(andExpr, new List<object>());

            Assert.AreEqual(expected, res);
        }
        #endregion
        
        #region OrBoolean
        [DataRow(true, true, true)]
        [DataRow(true, false, true)]
        [DataRow(false, true, true)]
        [DataRow(false, false, false)]
        [TestMethod]
        public void OrBoolean__CorrectValuesReturned(bool lhsValue, bool rhsValue, bool expected)
        {
            BooleanLiteralExpression lhs = new BooleanLiteralExpression(lhsValue, 0, 0);
            BooleanLiteralExpression rhs = new BooleanLiteralExpression(rhsValue, 0, 0);

            OrExpression orExpr = new OrExpression(lhs, rhs, 0, 0);
            IInterpreterBoolean parent = Substitute.For<IInterpreterBoolean>();
            parent.DispatchBoolean(lhs, Arg.Any<List<object>>()).Returns(lhsValue);
            parent.DispatchBoolean(rhs, Arg.Any<List<object>>()).Returns(rhsValue);

            BooleanHelper booleanHelper = Utilities.GetBooleanHelper(parent);

            bool res = booleanHelper.OrBoolean(orExpr, new List<object>());

            Assert.AreEqual(expected, res);
        }
        #endregion
        
        
        #region EqualBoolean
        [DataRow(0, 1, false)]
        [DataRow(1, 0, false)]
        [DataRow(0, 0, true)]
        [TestMethod]
        public void EqualBoolean_Int_CorrectValuesReturned(int lhsValue, int rhsValue, bool expected)
        {
            var lhs = Utilities.GetIntLitExpression();
            var rhs = Utilities.GetIntLitExpression();

            EqualExpression expression = new EqualExpression(lhs, rhs, 0, 0);
            expression.Type = TypeEnum.Integer;
            IInterpreterBoolean parent = Substitute.For<IInterpreterBoolean>();
            parent.DispatchInt(lhs, Arg.Any<List<object>>()).Returns(lhsValue);
            parent.DispatchInt(rhs, Arg.Any<List<object>>()).Returns(rhsValue);

            BooleanHelper booleanHelper = Utilities.GetBooleanHelper(parent);

            bool res = booleanHelper.EqualBoolean(expression, new List<object>());
            
            Assert.AreEqual(expected, res);
        }

        [DataRow(0, 1, false)]
        [DataRow(1, 0, false)]
        [DataRow(0, 0, true)]
        [TestMethod]
        public void EqualBoolean_Function_CorrectValuesReturned(int lhsValue, int rhsValue, bool expected)
        {
            var lhs = Utilities.GetIntLitExpression();
            var rhs = Utilities.GetIntLitExpression();

            EqualExpression expression = new EqualExpression(lhs, rhs, 0, 0);
            expression.Type = TypeEnum.Function;
            IInterpreterBoolean parent = Substitute.For<IInterpreterBoolean>();
            parent.DispatchFunction(lhs, Arg.Any<List<object>>()).Returns(lhsValue);
            parent.DispatchFunction(rhs, Arg.Any<List<object>>()).Returns(rhsValue);

            BooleanHelper booleanHelper = Utilities.GetBooleanHelper(parent);

            bool res = booleanHelper.EqualBoolean(expression, new List<object>());

            Assert.AreEqual(expected, res);
        }

        [DataRow(0.0, 1.0, false)]
        [DataRow(1.0, 0.0, false)]
        [DataRow(0.0, 0.0, true)]
        [TestMethod]
        public void EqualBoolean_Real_CorrectValuesReturned(double lhsValue, double rhsValue, bool expected)
        {
            var lhs = Utilities.GetRealLitExpression();
            var rhs = Utilities.GetRealLitExpression();

            EqualExpression expression = new EqualExpression(lhs, rhs, 0, 0);
            expression.Type = TypeEnum.Real;
            IInterpreterBoolean parent = Substitute.For<IInterpreterBoolean>();
            parent.DispatchReal(lhs, Arg.Any<List<object>>()).Returns(lhsValue);
            parent.DispatchReal(rhs, Arg.Any<List<object>>()).Returns(rhsValue);

            BooleanHelper booleanHelper = Utilities.GetBooleanHelper(parent);

            bool res = booleanHelper.EqualBoolean(expression, new List<object>());
            
            Assert.AreEqual(expected, res);
        }
        
        [DataRow(true, true, true)]
        [DataRow(true, false, false)]
        [DataRow(false, true, false)]
        [DataRow(false, false, true)]
        [TestMethod]
        public void EqualBoolean_Bool_CorrectValuesReturned(bool lhsValue, bool rhsValue, bool expected)
        {
            var lhs = Utilities.GetRealLitExpression();
            var rhs = Utilities.GetRealLitExpression();

            EqualExpression expression = new EqualExpression(lhs, rhs, 0, 0);
            expression.Type = TypeEnum.Boolean;
            IInterpreterBoolean parent = Substitute.For<IInterpreterBoolean>();
            parent.DispatchBoolean(lhs, Arg.Any<List<object>>()).Returns(lhsValue);
            parent.DispatchBoolean(rhs, Arg.Any<List<object>>()).Returns(rhsValue);

            BooleanHelper booleanHelper = Utilities.GetBooleanHelper(parent);

            bool res = booleanHelper.EqualBoolean(expression, new List<object>());
            
            Assert.AreEqual(expected, res);
        }
        #endregion

        #region NotEqualBoolean
        [DataRow(0, 1, true)]
        [DataRow(1, 0, true)]
        [DataRow(0, 0, false)]
        [TestMethod]
        public void NotEqualBoolean_Int_CorrectValuesReturned(int lhsValue, int rhsValue, bool expected)
        {
            var lhs = Utilities.GetIntLitExpression();
            var rhs = Utilities.GetIntLitExpression();

            var expression = new NotEqualExpression(lhs, rhs, 0, 0);
            expression.Type = TypeEnum.Integer;
            IInterpreterBoolean parent = Substitute.For<IInterpreterBoolean>();
            parent.DispatchInt(lhs, Arg.Any<List<object>>()).Returns(lhsValue);
            parent.DispatchInt(rhs, Arg.Any<List<object>>()).Returns(rhsValue);

            BooleanHelper booleanHelper = Utilities.GetBooleanHelper(parent);

            bool res = booleanHelper.NotEqualBoolean(expression, new List<object>());

            Assert.AreEqual(expected, res);
        }

        [DataRow(0, 1, true)]
        [DataRow(1, 0, true)]
        [DataRow(0, 0, false)]
        [TestMethod]
        public void NotEqualBoolean_Function_CorrectValuesReturned(int lhsValue, int rhsValue, bool expected)
        {
            var lhs = Utilities.GetIntLitExpression();
            var rhs = Utilities.GetIntLitExpression();

            var expression = new NotEqualExpression(lhs, rhs, 0, 0);
            expression.Type = TypeEnum.Function;
            IInterpreterBoolean parent = Substitute.For<IInterpreterBoolean>();
            parent.DispatchFunction(lhs, Arg.Any<List<object>>()).Returns(lhsValue);
            parent.DispatchFunction(rhs, Arg.Any<List<object>>()).Returns(rhsValue);

            BooleanHelper booleanHelper = Utilities.GetBooleanHelper(parent);

            bool res = booleanHelper.NotEqualBoolean(expression, new List<object>());

            Assert.AreEqual(expected, res);
        }

        [DataRow(0.0, 1.0, true)]
        [DataRow(1.0, 0.0, true)]
        [DataRow(0.0, 0.0, false)]
        [TestMethod]
        public void NotEqualBoolean_Real_CorrectValuesReturned(double lhsValue, double rhsValue, bool expected)
        {
            var lhs = Utilities.GetRealLitExpression();
            var rhs = Utilities.GetRealLitExpression();

            var expression = new NotEqualExpression(lhs, rhs, 0, 0);
            expression.Type = TypeEnum.Real;
            IInterpreterBoolean parent = Substitute.For<IInterpreterBoolean>();
            parent.DispatchReal(lhs, Arg.Any<List<object>>()).Returns(lhsValue);
            parent.DispatchReal(rhs, Arg.Any<List<object>>()).Returns(rhsValue);

            BooleanHelper booleanHelper = Utilities.GetBooleanHelper(parent);

            bool res = booleanHelper.NotEqualBoolean(expression, new List<object>());

            Assert.AreEqual(expected, res);
        }

        [DataRow(true, true, false)]
        [DataRow(true, false, true)]
        [DataRow(false, true, true)]
        [DataRow(false, false, false)]
        [TestMethod]
        public void NotEqualBoolean_Bool_CorrectValuesReturned(bool lhsValue, bool rhsValue, bool expected)
        {
            var lhs = Utilities.GetRealLitExpression();
            var rhs = Utilities.GetRealLitExpression();

            var expression = new NotEqualExpression(lhs, rhs, 0, 0);
            expression.Type = TypeEnum.Boolean;
            IInterpreterBoolean parent = Substitute.For<IInterpreterBoolean>();
            parent.DispatchBoolean(lhs, Arg.Any<List<object>>()).Returns(lhsValue);
            parent.DispatchBoolean(rhs, Arg.Any<List<object>>()).Returns(rhsValue);

            BooleanHelper booleanHelper = Utilities.GetBooleanHelper(parent);

            bool res = booleanHelper.NotEqualBoolean(expression, new List<object>());

            Assert.AreEqual(expected, res);
        }
        #endregion

        // Assumption: Relational operators only works for real
        //             - TyperChecker have casted children from type Integer to type Real

        #region GreaterEqualBoolean
        // a >= b

        [DataRow(0.0, 1.0, false)]
        [DataRow(1.0, 0.0, true)]
        [DataRow(0.0, 0.0, true)]
        [TestMethod]
        public void GreaterEqualBoolean_Real_CorrectValuesReturned(double lhsValue, double rhsValue, bool expected)
        {
            var lhs = Utilities.GetRealLitExpression();
            var rhs = Utilities.GetRealLitExpression();

            var expression = new GreaterEqualExpression(lhs, rhs, 0, 0);
            IInterpreterBoolean parent = Substitute.For<IInterpreterBoolean>();
            parent.DispatchReal(lhs, Arg.Any<List<object>>()).Returns(lhsValue);
            parent.DispatchReal(rhs, Arg.Any<List<object>>()).Returns(rhsValue);

            BooleanHelper booleanHelper = Utilities.GetBooleanHelper(parent);

            bool res = booleanHelper.GreaterEqualBoolean(expression, new List<object>());
            
            Assert.AreEqual(expected, res);
        }
        #endregion
        
        #region GreaterBoolean
        // a > b
        
        [DataRow(0.0, 1.0, false)]
        [DataRow(1.0, 0.0, true)]
        [DataRow(0.0, 0.0, false)]
        [TestMethod]
        public void GreaterBoolean_Real_CorrectValuesReturned(double lhsValue, double rhsValue, bool expected)
        {
            var lhs = Utilities.GetRealLitExpression();
            var rhs = Utilities.GetRealLitExpression();

            var expression = new GreaterExpression(lhs, rhs, 0, 0);
            IInterpreterBoolean parent = Substitute.For<IInterpreterBoolean>();
            parent.DispatchReal(lhs, Arg.Any<List<object>>()).Returns(lhsValue);
            parent.DispatchReal(rhs, Arg.Any<List<object>>()).Returns(rhsValue);

            BooleanHelper booleanHelper = Utilities.GetBooleanHelper(parent);

            bool res = booleanHelper.GreaterBoolean(expression, new List<object>());
            
            Assert.AreEqual(expected, res);
        }
        #endregion
        
        #region LessEqualBoolean
        // a <= b
        
        [DataRow(0.0, 1.0, true)]
        [DataRow(1.0, 0.0, false)]
        [DataRow(0.0, 0.0, true)]
        [TestMethod]
        public void LessEqualBoolean_Real_CorrectValuesReturned(double lhsValue, double rhsValue, bool expected)
        {
            var lhs = Utilities.GetRealLitExpression();
            var rhs = Utilities.GetRealLitExpression();

            var expression = new LessEqualExpression(lhs, rhs, 0, 0);
            IInterpreterBoolean parent = Substitute.For<IInterpreterBoolean>();
            parent.DispatchReal(lhs, Arg.Any<List<object>>()).Returns(lhsValue);
            parent.DispatchReal(rhs, Arg.Any<List<object>>()).Returns(rhsValue);

            BooleanHelper booleanHelper = Utilities.GetBooleanHelper(parent);

            bool res = booleanHelper.LessEqualBoolean(expression, new List<object>());
            
            Assert.AreEqual(expected, res);
        }
        #endregion
        
        #region LessBoolean
        // a < b
        
        [DataRow(0.0, 1.0, true)]
        [DataRow(1.0, 0.0, false)]
        [DataRow(0.0, 0.0, false)]
        [TestMethod]
        public void LessBoolean_Real_CorrectValuesReturned(double lhsValue, double rhsValue, bool expected)
        {
            var lhs = Utilities.GetRealLitExpression();
            var rhs = Utilities.GetRealLitExpression();

            var expression = new LessExpression(lhs, rhs, 0, 0);
            IInterpreterBoolean parent = Substitute.For<IInterpreterBoolean>();
            parent.DispatchReal(lhs, Arg.Any<List<object>>()).Returns(lhsValue);
            parent.DispatchReal(rhs, Arg.Any<List<object>>()).Returns(rhsValue);

            BooleanHelper booleanHelper = Utilities.GetBooleanHelper(parent);

            bool res = booleanHelper.LessBoolean(expression, new List<object>());
            
            Assert.AreEqual(expected, res);
        }
        #endregion

    }
}