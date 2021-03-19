using System;
using System.Collections.Generic;
using System.Linq;
using ASTLib.Nodes;
using ASTLib.Nodes.ExpressionNodes;
using ASTLib.Nodes.ExpressionNodes.OperationNodes;
using ASTLib.Nodes.TypeNodes;
using InterpreterLib.Helpers;
using InterpreterLib.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;

namespace InterpreterLib.Tests
{
    [TestClass]
    public class RealHelperTests
    {
        #region ExportReal
        [DataRow(1.0, 1.0)]
        [DataRow(-1.0, -1.0)]
        [DataRow(0.0, 0.0)]
        [TestMethod]
        public void ExportReal_Real_ReturnsCorrectResult(double input, double expected)
        {
            RealLiteralExpression realLit = new RealLiteralExpression(input.ToString(), 1, 1);
            ExportNode exportNode = new ExportNode(realLit, 1, 1);
            IInterpreter parent = Substitute.For<IInterpreter>();
            parent.DispatchReal(realLit, Arg.Any<List<object>>()).Returns(input);
            RealHelper realHelper = new RealHelper()
            {
                Interpreter = parent
            };

            double res = realHelper.ExportReal(exportNode, new List<object>());

            Assert.AreEqual(expected, res);
        }
        #endregion

        #region FunctionReal
        [DataRow(1.0, 1.0)]
        [DataRow(-1.0, -1.0)]
        [DataRow(0.0, 0.0)]
        [TestMethod]
        public void FunctionReal_Real_ReturnsCorrectResult(double input, double expected)
        {
            RealLiteralExpression realLit = new RealLiteralExpression(input.ToString(), 1, 1);
            ConditionNode conditionNode = new ConditionNode(realLit, 1, 1);
            FunctionNode functionNode = new FunctionNode("test", 1, conditionNode, new List<string>(), new FunctionTypeNode(new TypeNode(TypeEnum.Real, 1, 1), new List<TypeNode>(), 1, 1), 1, 1);
            IInterpreter parent = Substitute.For<IInterpreter>();
            parent.DispatchReal(realLit, Arg.Any<List<object>>()).Returns(input);
            RealHelper realHelper = new RealHelper()
            {
                Interpreter = parent
            };

            double res = realHelper.FunctionReal(functionNode, new List<object>());

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
            RealLiteralExpression realLit1 = new RealLiteralExpression(input1.ToString(), 1, 1);
            RealLiteralExpression realLit2 = new RealLiteralExpression(input2.ToString(), 2, 2);
            AdditionExpression additionExpr = new AdditionExpression(realLit1, realLit2, 1, 1);
            IInterpreter parent = Substitute.For<IInterpreter>();
            parent.DispatchReal(realLit1, Arg.Any<List<object>>()).Returns(input1);
            parent.DispatchReal(realLit2, Arg.Any<List<object>>()).Returns(input2);
            RealHelper realHelper = new RealHelper(new AST(null, null, 1, 1))
            {
                Interpreter = parent
            };

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
            RealLiteralExpression realLit1 = new RealLiteralExpression(input1.ToString(), 1, 1);
            RealLiteralExpression realLit2 = new RealLiteralExpression(input2.ToString(), 2, 2);
            SubtractionExpression subtractionExpr = new SubtractionExpression(realLit1, realLit2, 1, 1);
            IInterpreter parent = Substitute.For<IInterpreter>();
            parent.DispatchReal(realLit1, Arg.Any<List<object>>()).Returns(input1);
            parent.DispatchReal(realLit2, Arg.Any<List<object>>()).Returns(input2);
            RealHelper realHelper = new RealHelper()
            {
                Interpreter = parent
            };

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
            RealLiteralExpression realLit1 = new RealLiteralExpression(input1.ToString(), 1, 1);
            RealLiteralExpression realLit2 = new RealLiteralExpression(input2.ToString(), 2, 2);
            MultiplicationExpression multiplicationExpr = new MultiplicationExpression(realLit1, realLit2, 1, 1);
            IInterpreter parent = Substitute.For<IInterpreter>();
            parent.DispatchReal(realLit1, Arg.Any<List<object>>()).Returns(input1);
            parent.DispatchReal(realLit2, Arg.Any<List<object>>()).Returns(input2);
            RealHelper realHelper = new RealHelper()
            {
                Interpreter = parent
            };

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
            RealLiteralExpression realLit1 = new RealLiteralExpression(input1.ToString(), 1, 1);
            RealLiteralExpression realLit2 = new RealLiteralExpression(input2.ToString(), 2, 2);
            DivisionExpression divisionExpr = new DivisionExpression(realLit1, realLit2, 1, 1);
            IInterpreter parent = Substitute.For<IInterpreter>();
            parent.DispatchReal(realLit1, Arg.Any<List<object>>()).Returns(input1);
            parent.DispatchReal(realLit2, Arg.Any<List<object>>()).Returns(input2);
            RealHelper realHelper = new RealHelper()
            {
                Interpreter = parent
            };

            double res = realHelper.DivisionReal(divisionExpr, new List<object>());

            Assert.AreEqual(expected, res);
        }

        [DataRow(3.0, 0.0)]
        [TestMethod]
        public void DivisionReal_DivisorIsZero_ThrowsException(double input1, double input2)
        {
            RealLiteralExpression realLit1 = new RealLiteralExpression(input1.ToString(), 1, 1);
            RealLiteralExpression realLit2 = new RealLiteralExpression(input2.ToString(), 2, 2);
            DivisionExpression divisionExpr = new DivisionExpression(realLit1, realLit2, 1, 1);
            IInterpreter parent = Substitute.For<IInterpreter>();
            parent.DispatchReal(realLit1, Arg.Any<List<object>>()).Returns(input1);
            parent.DispatchReal(realLit2, Arg.Any<List<object>>()).Returns(input2);
            RealHelper realHelper = new RealHelper()
            {
                Interpreter = parent
            };

            Assert.ThrowsException<Exception>(() => realHelper.DivisionReal(divisionExpr, new List<object>()));
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
            RealLiteralExpression realLit1 = new RealLiteralExpression(input1.ToString(), 1, 1);
            RealLiteralExpression realLit2 = new RealLiteralExpression(input2.ToString(), 2, 2);
            ModuloExpression moduloExpr = new ModuloExpression(realLit1, realLit2, 1, 1);
            IInterpreter parent = Substitute.For<IInterpreter>();
            parent.DispatchReal(realLit1, Arg.Any<List<object>>()).Returns(input1);
            parent.DispatchReal(realLit2, Arg.Any<List<object>>()).Returns(input2);
            RealHelper realHelper = new RealHelper()
            {
                Interpreter = parent
            };

            double res = realHelper.ModuloReal(moduloExpr, new List<object>());

            Assert.AreEqual(expected, res);
        }

        [DataRow(3.0, 0.0)]
        [TestMethod]
        public void ModuloReal_DivisorIsZero_ThrowsException(double input1, double input2)
        {
            RealLiteralExpression realLit1 = new RealLiteralExpression(input1.ToString(), 1, 1);
            RealLiteralExpression realLit2 = new RealLiteralExpression(input2.ToString(), 2, 2);
            ModuloExpression moduloExpr = new ModuloExpression(realLit1, realLit2, 1, 1);
            IInterpreter parent = Substitute.For<IInterpreter>();
            parent.DispatchReal(realLit1, Arg.Any<List<object>>()).Returns(input1);
            parent.DispatchReal(realLit2, Arg.Any<List<object>>()).Returns(input2);
            RealHelper realHelper = new RealHelper()
            {
                Interpreter = parent
            };

            Assert.ThrowsException<Exception>(() => realHelper.ModuloReal(moduloExpr, new List<object>()));
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
            RealLiteralExpression realLit = new RealLiteralExpression(input.ToString(), 1, 1);
            AbsoluteValueExpression absoluteExpr = new AbsoluteValueExpression(realLit, 1, 1);
            IInterpreter parent = Substitute.For<IInterpreter>();
            parent.DispatchReal(realLit, Arg.Any<List<object>>()).Returns(input);
            RealHelper realHelper = new RealHelper()
            {
                Interpreter = parent
            };

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
            RealLiteralExpression realLit1 = new RealLiteralExpression(input1.ToString(), 1, 1);
            RealLiteralExpression realLit2 = new RealLiteralExpression(input2.ToString(), 2, 2);
            PowerExpression powExpr = new PowerExpression(realLit1, realLit2, 1, 1);
            IInterpreter parent = Substitute.For<IInterpreter>();
            parent.DispatchReal(realLit1, Arg.Any<List<object>>()).Returns(input1);
            parent.DispatchReal(realLit2, Arg.Any<List<object>>()).Returns(input2);
            RealHelper realHelper = new RealHelper()
            {
                Interpreter = parent
            };

            double res = realHelper.PowerReal(powExpr, new List<object>());

            Assert.AreEqual(expected, res);
        }
        #endregion

        #region IdentifierReal
        [DataRow(0, new object[] { 5.1 }, 5.1)]
        [DataRow(1, new object[] { 5.1, 3.0, 1.1 }, 3.0)]
        [TestMethod]
        public void IdentifierReal_ValidIdentifierExprAndParameters_ReturnsCorrectResult(int reference, object[] parameterArray, double expected)
        {
            IdentifierExpression identifierExpr = new IdentifierExpression("test", 1, 1);
            identifierExpr.Reference = reference;
            List<object> parameters = parameterArray.ToList();
            RealHelper realHelper = new RealHelper();

            double res = realHelper.IdentifierReal(identifierExpr, parameters);

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
            RealLiteralExpression realLit = new RealLiteralExpression(input.ToString(), 1, 1);
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
        public void CastIntegerToReal_Real_ReturnsCorrectResult(int input, double expected)
        {
            IntegerLiteralExpression realLit = new IntegerLiteralExpression(input.ToString(), 1, 1);
            CastFromIntegerExpression castExpr = new CastFromIntegerExpression(realLit, 1, 1);
            IInterpreter parent = Substitute.For<IInterpreter>();
            parent.DispatchInt(realLit, Arg.Any<List<object>>()).Returns(input);
            RealHelper realHelper = new RealHelper()
            {
                Interpreter = parent
            };

            double res = realHelper.CastIntegerToReal(castExpr, new List<object>());

            Assert.AreEqual(expected, res);
        }
        #endregion
    }
}