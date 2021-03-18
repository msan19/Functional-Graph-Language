using System.Collections.Generic;
using ASTLib.Nodes.ExpressionNodes;
using ASTLib.Nodes.ExpressionNodes.OperationNodes;
using InterpreterLib.Helpers;
using InterpreterLib.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;

namespace InterpreterLib.Tests
{
    [TestClass]
    public class RealHelperTests
    {
        #region PowerReal
        [DataRow(2.0,2.0, 4.0)]
        [DataRow(2.0,3.0, 8.0)]
        [DataRow(2.0,-3.0, 0.125)]
        [TestMethod]
        public void PowerReal_TwoReals_Correct(double input1, double input2, double expected)
        {
            RealLiteralExpression intLit1 = new RealLiteralExpression(input1.ToString(), 1, 1);
            RealLiteralExpression intLit2 = new RealLiteralExpression(input2.ToString(), 2, 2);
            PowerExpression powExpr = new PowerExpression(intLit1, intLit2, 1, 1);
            IInterpreter parent = Substitute.For<IInterpreter>();
            parent.DispatchReal(intLit1, Arg.Any<List<object>>()).Returns(input1);
            parent.DispatchReal(intLit2, Arg.Any<List<object>>()).Returns(input2);
            RealHelper realHelper = new RealHelper()
            {
                Interpreter = parent
            };

            double res = realHelper.PowerReal(powExpr, new List<object>());

            Assert.AreEqual(expected, res);
        }
        #endregion
    }
}