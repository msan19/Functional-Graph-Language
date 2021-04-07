using System;
using System.Collections.Generic;
using ASTLib.Nodes.ExpressionNodes;
using ASTLib.Nodes.ExpressionNodes.BooleanOperationNodes;
using ASTLib.Nodes.ExpressionNodes.CommonOperationNodes;
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
        #region SetASTRoot
        #endregion
        
        #region SetUpFuncs
        #endregion
        
        #region ConditionBoolean
        #endregion
        
        #region IdentifierBoolean
        #endregion

        #region NotBoolean
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
        #endregion
        
        #region GreaterEqualBoolean
        #endregion
        
        #region GreaterBoolean
        #endregion
        
        #region LessEqualBoolean
        #endregion
        
        #region LessBoolean
        #endregion
        
        #region FunctionCallBoolean
        #endregion
    }
}