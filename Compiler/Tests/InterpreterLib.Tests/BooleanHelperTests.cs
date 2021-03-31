using System.Collections.Generic;
using ASTLib.Nodes.ExpressionNodes;
using ASTLib.Nodes.ExpressionNodes.BooleanOperationNodes;
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
            IInterpreter parent = Substitute.For<IInterpreter>();
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
            IInterpreter parent = Substitute.For<IInterpreter>();
            parent.DispatchBoolean(lhs, Arg.Any<List<object>>()).Returns(lhsValue);
            parent.DispatchBoolean(rhs, Arg.Any<List<object>>()).Returns(rhsValue);

            BooleanHelper booleanHelper = Utilities.GetBooleanHelper(parent);

            bool res = booleanHelper.OrBoolean(orExpr, new List<object>());

            Assert.AreEqual(expected, res);
        }
        #endregion
        
        #region EqualBoolean
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