using System;
using System.Collections.Generic;
using System.Linq;
using ASTLib;
using ASTLib.Exceptions;
using ASTLib.Nodes;
using ASTLib.Nodes.ExpressionNodes;
using ASTLib.Nodes.ExpressionNodes.CommonOperationNodes;
using ASTLib.Nodes.ExpressionNodes.NumberOperationNodes;
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
    public class RealHelperTests
    {
        private RealHelper SetUpHelper(IInterpreterReal parent)
        {
            RealHelper realHelper = new RealHelper();
            realHelper.SetInterpreter(parent);
            return realHelper;
        }

        #region ExportReal
        [DataRow(1.0, 1.0)]
        [DataRow(-1.0, -1.0)]
        [DataRow(0.0, 0.0)]
        [TestMethod]
        public void ExportReal_Real_ReturnsCorrectResult(double input, double expected)
        {
            RealLiteralExpression realLit = new RealLiteralExpression(input, 1, 1);
            ExportNode exportNode = new ExportNode(realLit, 1, 1);
            IInterpreterReal parent = Substitute.For<IInterpreterReal>();
            parent.DispatchReal(realLit, Arg.Any<List<object>>()).Returns(input);
            RealHelper realHelper = SetUpHelper(parent);

            double res = realHelper.ExportReal(exportNode, new List<object>());

            Assert.AreEqual(expected, res);
        }
        #endregion

        #region AdditionReal
        [DataRow(2.0, 3.0, 5.0)]
        [DataRow(2.5, 3.0, 5.5)]
        [DataRow(2.0, -3.0, -1.0)]
        [DataRow(-2.0, 3.0, 1.0)]
        [DataRow(-2.0, -3.0, -5.0)]
        [TestMethod]
        public void AdditionReal_TwoReals_ReturnsCorrectResult(double input1, double input2, double expected)
        {
            RealLiteralExpression realLit1 = new RealLiteralExpression(input1, 1, 1);
            RealLiteralExpression realLit2 = new RealLiteralExpression(input2, 2, 2);
            AdditionExpression additionExpr = new AdditionExpression(realLit1, realLit2, 1, 1);
            IInterpreterReal parent = Substitute.For<IInterpreterReal>();
            parent.DispatchReal(realLit1, Arg.Any<List<object>>()).Returns(input1);
            parent.DispatchReal(realLit2, Arg.Any<List<object>>()).Returns(input2);
            RealHelper realHelper = SetUpHelper(parent);

            double res = realHelper.AdditionReal(additionExpr, new List<object>());

            Assert.AreEqual(expected, res);
        }
        #endregion

        #region SubtractionReal
        [DataRow(2.0, 3.0, -1.0)]
        [DataRow(2.5, 3.0, -0.5)]
        [DataRow(2.0, -3.0, 5.0)]
        [DataRow(-2.0, 3.0, -5.0)]
        [DataRow(-2.0, -3.0, 1.0)]
        [TestMethod]
        public void SubtractionReal_TwoReals_ReturnsCorrectResult(double input1, double input2, double expected)
        {
            RealLiteralExpression realLit1 = new RealLiteralExpression(input1, 1, 1);
            RealLiteralExpression realLit2 = new RealLiteralExpression(input2, 2, 2);
            SubtractionExpression subtractionExpr = new SubtractionExpression(realLit1, realLit2, 1, 1);
            IInterpreterReal parent = Substitute.For<IInterpreterReal>();
            parent.DispatchReal(realLit1, Arg.Any<List<object>>()).Returns(input1);
            parent.DispatchReal(realLit2, Arg.Any<List<object>>()).Returns(input2);
            RealHelper realHelper = SetUpHelper(parent);

            double res = realHelper.SubtractionReal(subtractionExpr, new List<object>());

            Assert.AreEqual(expected, res);
        }
        #endregion

        #region MultiplicationReal
        [DataRow(2.0, 3.0, 6.0)]
        [DataRow(2.5, 3.0, 7.5)]
        [DataRow(2.0, -3.0, -6.0)]
        [DataRow(-2.0, 3.0, -6.0)]
        [DataRow(-2.0, -3.0, 6.0)]
        [TestMethod]
        public void MultiplicationReal_TwoReals_ReturnsCorrectResult(double input1, double input2, double expected)
        {
            RealLiteralExpression realLit1 = new RealLiteralExpression(input1, 1, 1);
            RealLiteralExpression realLit2 = new RealLiteralExpression(input2, 2, 2);
            MultiplicationExpression multiplicationExpr = new MultiplicationExpression(realLit1, realLit2, 1, 1);
            IInterpreterReal parent = Substitute.For<IInterpreterReal>();
            parent.DispatchReal(realLit1, Arg.Any<List<object>>()).Returns(input1);
            parent.DispatchReal(realLit2, Arg.Any<List<object>>()).Returns(input2);
            RealHelper realHelper = SetUpHelper(parent);

            double res = realHelper.MultiplicationReal(multiplicationExpr, new List<object>());

            Assert.AreEqual(expected, res);
        }
        #endregion

        #region DivisionReal
        [DataRow(3.0, 2.0, 1.5)]
        [DataRow(3.5, 2.0, 1.75)]
        [DataRow(3.0, -2.0, -1.5)]
        [DataRow(-3.0, 2.0, -1.5)]
        [DataRow(-3.0, -2.0, 1.5)]
        [TestMethod]
        public void DivisionReal_TwoReals_ReturnsCorrectResult(double input1, double input2, double expected)
        {
            RealLiteralExpression realLit1 = new RealLiteralExpression(input1, 1, 1);
            RealLiteralExpression realLit2 = new RealLiteralExpression(input2, 2, 2);
            DivisionExpression divisionExpr = new DivisionExpression(realLit1, realLit2, 1, 1);
            IInterpreterReal parent = Substitute.For<IInterpreterReal>();
            parent.DispatchReal(realLit1, Arg.Any<List<object>>()).Returns(input1);
            parent.DispatchReal(realLit2, Arg.Any<List<object>>()).Returns(input2);
            RealHelper realHelper = SetUpHelper(parent);

            double res = realHelper.DivisionReal(divisionExpr, new List<object>());

            Assert.AreEqual(expected, res);
        }

        [DataRow(3.0, 0.0)]
        [TestMethod]
        public void DivisionReal_DivisorIsZero_ThrowsException(double input1, double input2)
        {
            RealLiteralExpression realLit1 = new RealLiteralExpression(input1, 1, 1);
            RealLiteralExpression realLit2 = new RealLiteralExpression(input2, 2, 2);
            DivisionExpression divisionExpr = new DivisionExpression(realLit1, realLit2, 1, 1);
            IInterpreterReal parent = Substitute.For<IInterpreterReal>();
            parent.DispatchReal(realLit1, Arg.Any<List<object>>()).Returns(input1);
            parent.DispatchReal(realLit2, Arg.Any<List<object>>()).Returns(input2);
            RealHelper realHelper = SetUpHelper(parent);

            Assert.ThrowsException<DivisionByZeroException>(() => realHelper.DivisionReal(divisionExpr, new List<object>()));
        }
        #endregion

        #region ModuloReal
        [DataRow(3.0, 2.0, 1.0)]
        [DataRow(3.5, 2.0, 1.5)]
        [DataRow(3.0, -2.0, -1.0)]
        [DataRow(-3.0, 2.0, 1.0)]
        [DataRow(-3.0, -2.0, -1.0)]
        [TestMethod]
        public void ModuloReal_TwoReals_ReturnsCorrectResult(double input1, double input2, double expected)
        {
            RealLiteralExpression realLit1 = new RealLiteralExpression(input1, 1, 1);
            RealLiteralExpression realLit2 = new RealLiteralExpression(input2, 2, 2);
            ModuloExpression moduloExpr = new ModuloExpression(realLit1, realLit2, 1, 1);
            IInterpreterReal parent = Substitute.For<IInterpreterReal>();
            parent.DispatchReal(realLit1, Arg.Any<List<object>>()).Returns(input1);
            parent.DispatchReal(realLit2, Arg.Any<List<object>>()).Returns(input2);
            RealHelper realHelper = SetUpHelper(parent);

            double res = realHelper.ModuloReal(moduloExpr, new List<object>());

            Assert.AreEqual(expected, res);
        }

        [DataRow(3.0, 0.0)]
        [TestMethod]
        public void ModuloReal_DivisorIsZero_ThrowsException(double input1, double input2)
        {
            RealLiteralExpression realLit1 = new RealLiteralExpression(input1, 1, 1);
            RealLiteralExpression realLit2 = new RealLiteralExpression(input2, 2, 2);
            ModuloExpression moduloExpr = new ModuloExpression(realLit1, realLit2, 1, 1);
            IInterpreterReal parent = Substitute.For<IInterpreterReal>();
            parent.DispatchReal(realLit1, Arg.Any<List<object>>()).Returns(input1);
            parent.DispatchReal(realLit2, Arg.Any<List<object>>()).Returns(input2);
            RealHelper realHelper = SetUpHelper(parent);

            Assert.ThrowsException<DivisionByZeroException>(() => realHelper.ModuloReal(moduloExpr, new List<object>()));
        }
        #endregion

        #region AbsoluteReal
        [DataRow(3.0, 3.0)]
        [DataRow(-3.0, 3.0)]
        [DataRow(3.5, 3.5)]
        [DataRow(0.0, 0.0)]
        [TestMethod]
        public void AbsoluteReal_Real_ReturnsCorrectResult(double input, double expected)
        {
            RealLiteralExpression realLit = new RealLiteralExpression(input, 1, 1);
            AbsoluteValueExpression absoluteExpr = new AbsoluteValueExpression(realLit, 1, 1);
            IInterpreterReal parent = Substitute.For<IInterpreterReal>();
            parent.DispatchReal(realLit, Arg.Any<List<object>>()).Returns(input);
            RealHelper realHelper = SetUpHelper(parent);

            double res = realHelper.AbsoluteReal(absoluteExpr, new List<object>());

            Assert.AreEqual(expected, res);
        }
        #endregion

        #region PowerReal
        [DataRow(2.0, 3.0, 8.0)]
        [DataRow(2.5, 3.0, 15.625)]
        [DataRow(2.0, -3.0, 0.125)]
        [DataRow(-2.0, 3.0, -8)]
        [DataRow(-2.0, -3.0, -0.125)]
        [TestMethod]
        public void PowerReal_TwoReals_ReturnsCorrectResult(double input1, double input2, double expected)
        {
            RealLiteralExpression realLit1 = new RealLiteralExpression(input1, 1, 1);
            RealLiteralExpression realLit2 = new RealLiteralExpression(input2, 2, 2);
            PowerExpression powExpr = new PowerExpression(realLit1, realLit2, 1, 1);
            IInterpreterReal parent = Substitute.For<IInterpreterReal>();
            parent.DispatchReal(realLit1, Arg.Any<List<object>>()).Returns(input1);
            parent.DispatchReal(realLit2, Arg.Any<List<object>>()).Returns(input2);
            RealHelper realHelper = SetUpHelper(parent);

            double res = realHelper.PowerReal(powExpr, new List<object>());

            Assert.AreEqual(expected, res);
        }
        #endregion

        #region LiteralReal
        [DataRow(1.0, 1.0)]
        [DataRow(-1.0, -1.0)]
        [DataRow(0.0, 0.0)]
        [TestMethod]
        public void LiteralReal_Real_ReturnsCorrectResult(double input, double expected)
        {
            RealLiteralExpression realLit = new RealLiteralExpression(input, 1, 1);
            RealHelper realHelper = new RealHelper();

            double res = realHelper.LiteralReal(realLit, new List<object>());

            Assert.AreEqual(expected, res);
        }
        #endregion

        #region CastIntegerToReal
        [DataRow(1, 1.0)]
        [DataRow(-1, -1.0)]
        [DataRow(0, 0.0)]
        [TestMethod]
        public void CastIntegerToReal_Int_ReturnsCorrectResult(int input, double expected)
        {
            RealLiteralExpression realLit = new RealLiteralExpression(input, 1, 1);
            CastFromIntegerExpression castExpr = new CastFromIntegerExpression(realLit, 1, 1);
            IInterpreterReal parent = Substitute.For<IInterpreterReal>();
            parent.DispatchInt(realLit, Arg.Any<List<object>>()).Returns(input);
            RealHelper realHelper = SetUpHelper(parent);

            double res = realHelper.CastIntegerToReal(castExpr, new List<object>());

            Assert.AreEqual(expected, res);
        }
        #endregion

        #region NegativeReal
        [DataRow(0.0, -0.0)]
        [DataRow(-0.0, 0.0)]
        [DataRow(-1.0, 1.0)]
        [DataRow(1.0, -1.0)]
        [DataRow(3.1425132, -3.1425132)]
        [TestMethod]
        public void NegativeReal_Real_CorrectNegativeValue(double input, double expected)
        {
            RealLiteralExpression realLitExpr = new RealLiteralExpression(input, 0, 0);

            NegativeExpression negExpr = new NegativeExpression(new List<ExpressionNode>() {realLitExpr}, 0, 0);
            
            IInterpreterReal parent = Substitute.For<IInterpreterReal>();
            parent.DispatchReal(realLitExpr, Arg.Any<List<object>>()).Returns(input);
            RealHelper realHelper = SetUpHelper(parent);

            double res = realHelper.NegativeReal(negExpr, new List<object>());

            Assert.AreEqual(expected, res);
        }
        #endregion
    }
}