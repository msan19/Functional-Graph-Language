using ASTLib;
using ASTLib.Nodes;
using ASTLib.Nodes.ExpressionNodes;
using ASTLib.Nodes.ExpressionNodes.BooleanOperationNodes;
using ASTLib.Nodes.ExpressionNodes.OperationNodes;
using ASTLib.Nodes.TypeNodes;
using FluentAssertions;
using InterpreterLib.Helpers;
using InterpreterLib.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ASTLib.Nodes.ExpressionNodes.NumberOperationNodes;

namespace InterpreterLib.Tests
{
    [TestClass]
    public class InterpreterTests
    {

        #region Interpret
        [TestMethod]
        public void Interpret_AST_CorrectNumberOfCallsToExportReal()
        {
            List<ExportNode> exports = new List<ExportNode> { new ExportNode(null,0,0),
                                                              new ExportNode(null,0,0),
                                                              new ExportNode(null,0,0)};
            AST input1 = new AST(null, exports, 0, 0);
            IRealHelper rhelper = Substitute.For<IRealHelper>();
            Interpreter interpreter = Utilities.GetIntepretorOnlyWith(rhelper);
            
            interpreter.Interpret(input1);

            rhelper.Received(3).ExportReal(Arg.Any<ExportNode>(), Arg.Any<List<Object>>());
        }

        [TestMethod]
        public void Interpret_AST_CorrectListReturned()
        {
            List<double> expected = new List<double> { 0.1, 3.3, 7.0 };
            List<ExportNode> exports = new List<ExportNode> { new ExportNode(null,0,0),
                                                              new ExportNode(null,0,0),
                                                              new ExportNode(null,0,0)};
            AST input1 = new AST(null, exports, 0, 0);
            IRealHelper rhelper = Substitute.For<IRealHelper>();
            Interpreter interpreter = Utilities.GetIntepretorOnlyWith(rhelper);
            rhelper.ExportReal(exports[0], Arg.Any<List<Object>>()).Returns(0.1);
            rhelper.ExportReal(exports[1], Arg.Any<List<Object>>()).Returns(3.3);
            rhelper.ExportReal(exports[2], Arg.Any<List<Object>>()).Returns(7.0);

            List<double> res = interpreter.Interpret(input1);

            res.Should().BeEquivalentTo(expected);
        }

        #endregion
        
        #region Dispatch
        #region Dispatch_IntegerLiteralExpr
        [TestMethod]
        public void Dispatch_IntegerLiteralAndObjectListAndIntegerType_CorrectListPassed()
        {
            List<Object> expected = new List<Object>() { 23, 2.334, null };
            IntegerLiteralExpression input1 = new IntegerLiteralExpression("", 0, 0);
                        IIntegerHelper ihelper = Substitute.For<IIntegerHelper>();
            Interpreter interpreter = Utilities.GetIntepretorOnlyWith(ihelper);
            List<Object> res = null;
            ihelper.LiteralInteger(Arg.Any<IntegerLiteralExpression>(), Arg.Do<List<Object>>(x => res = x));

            interpreter.Dispatch(input1, expected, TypeEnum.Integer);

            res.Should().BeEquivalentTo(expected);
        }

        [TestMethod]
        public void Dispatch_IntegerLiteralAndObjectListAndIntegerType_CorrectIntegerLiteralExprPassed()
        {
            IntegerLiteralExpression expected = new IntegerLiteralExpression("", 0, 0);
            IntegerLiteralExpression input1 = expected;
            List<Object> input2 = new List<Object>() { 23, 2.334, null };
                        IIntegerHelper ihelper = Substitute.For<IIntegerHelper>();
            Interpreter interpreter = Utilities.GetIntepretorOnlyWith(ihelper);
            IntegerLiteralExpression res = null;
            ihelper.LiteralInteger(Arg.Do<IntegerLiteralExpression>(x => res = x), Arg.Any<List<Object>>());

            interpreter.Dispatch(input1, input2, TypeEnum.Integer);

            res.Should().BeEquivalentTo(expected);
        }

        [TestMethod]
        public void Dispatch_IntegerLiteralAndObjectListAndIntegerType_CorrectValueReturned()
        {
            int expected = 17;
            IntegerLiteralExpression input1 = new IntegerLiteralExpression("", 0, 0);
            List<Object> input2 = new List<Object>() { 23, 2.334, null };
                        IIntegerHelper ihelper = Substitute.For<IIntegerHelper>();
            Interpreter interpreter = Utilities.GetIntepretorOnlyWith(ihelper);
            ihelper.LiteralInteger(Arg.Any<IntegerLiteralExpression>(), Arg.Any<List<Object>>()).Returns(expected);

            int res = (int)interpreter.Dispatch(input1, input2, TypeEnum.Integer);

            Assert.AreEqual(expected, res);
        }
        #endregion

        #region Dispatch_RealAddition
        [TestMethod]
        public void Dispatch_AdditionAndObjectListAndRealType_CorrectListPassed()
        {
            List<Object> expected = new List<Object>() { 23, 2.334, null };
            AdditionExpression input1 = new AdditionExpression(null, null, 0, 0);
            IRealHelper rhelper = Substitute.For<IRealHelper>();
            Interpreter interpreter = Utilities.GetIntepretorOnlyWith(rhelper);
            List<Object> res = null;
            rhelper.AdditionReal(Arg.Any<AdditionExpression>(), Arg.Do<List<Object>>(x => res = x));

            interpreter.Dispatch(input1, expected, TypeEnum.Real);

            res.Should().BeEquivalentTo(expected);
        }

        [TestMethod]
        public void Dispatch_AdditionAndObjectListAndRealType_CorrectAdditionExprPassed()
        {
            AdditionExpression expected = new AdditionExpression(null, null, 0, 0);
            AdditionExpression input1 = expected;
            List<Object> input2 = new List<Object>() { 23, 2.334, null };
            IRealHelper rhelper = Substitute.For<IRealHelper>();
            Interpreter interpreter = Utilities.GetIntepretorOnlyWith(rhelper);
            AdditionExpression res = null;
            rhelper.AdditionReal(Arg.Do<AdditionExpression>(x => res = x), Arg.Any<List<Object>>());

            interpreter.Dispatch(input1, input2, TypeEnum.Real);

            res.Should().BeEquivalentTo(expected);
        }

        [TestMethod]
        public void Dispatch_AdditionAndObjectListAndRealType_CorrectValueReturned()
        {
            double expected = 17.2;
            AdditionExpression input1 = new AdditionExpression(null, null, 0, 0);
            List<Object> input2 = new List<Object>() { 23, 2.334, null };
            IRealHelper rhelper = Substitute.For<IRealHelper>();
            Interpreter interpreter = Utilities.GetIntepretorOnlyWith(rhelper);
            rhelper.AdditionReal(Arg.Any<AdditionExpression>(), Arg.Any<List<Object>>()).Returns(expected);

            double res = (double)interpreter.Dispatch(input1, input2, TypeEnum.Real);

            Assert.AreEqual(expected, res);
        }
        #endregion

        #region Dispatch_FunctionIdentifier
        [TestMethod]
        public void Dispatch_IdentifierAndObjectListAndFunctionType_CorrectListPassed()
        {
            List<Object> expected = new List<Object>() { 23, 2.334, null };
            IdentifierExpression input1 = new IdentifierExpression("", 0, 0);
            IFunctionHelper fhelper = Substitute.For<IFunctionHelper>();
            Interpreter interpreter = Utilities.GetIntepretorOnlyWith(fhelper);
            List<Object> res = null;
            fhelper.IdentifierFunction(Arg.Any<IdentifierExpression>(), Arg.Do<List<Object>>(x => res = x));

            interpreter.Dispatch(input1, expected, TypeEnum.Function);

            res.Should().BeEquivalentTo(expected);
        }

        [TestMethod]
        public void Dispatch_IdentifierAndObjectListAndFunctionType_CorrectIdentifierExprPassed()
        {
            IdentifierExpression expected = new IdentifierExpression("", 0, 0);
            IdentifierExpression input1 = expected;
            List<Object> input2 = new List<Object>() { 23, 2.334, null };
            IFunctionHelper fhelper = Substitute.For<IFunctionHelper>();
            Interpreter interpreter = Utilities.GetIntepretorOnlyWith(fhelper);
            IdentifierExpression res = null;
            fhelper.IdentifierFunction(Arg.Do<IdentifierExpression>(x => res = x), Arg.Any<List<Object>>());

            interpreter.Dispatch(input1, input2, TypeEnum.Function);

            res.Should().BeEquivalentTo(expected);
        }

        [TestMethod]
        public void Dispatch_IdentifierAndObjectListAndFunctionType_CorrectValueReturned()
        {
            int expected = 17;
            IdentifierExpression input1 = new IdentifierExpression("", 0, 0);
            List<Object> input2 = new List<Object>() { 23, 2.334, null };
            IFunctionHelper fhelper = Substitute.For<IFunctionHelper>();
            Interpreter interpreter = Utilities.GetIntepretorOnlyWith(fhelper);
            fhelper.IdentifierFunction(Arg.Any<IdentifierExpression>(), Arg.Any<List<Object>>()).Returns(expected);

            int res = (int)interpreter.Dispatch(input1, input2, TypeEnum.Function);

            Assert.AreEqual(expected, res);
        }
        #endregion

        #endregion

        #region CompleteComponent
        [DataRow(1, 1.0, 1.0)]
        [DataRow(10, 0.017, 1.0399201658290593)]
        [DataRow(10, 0.5, 3.1622776601683795)]
        [TestMethod]
        public void Interpret_Unmocked_ASTWithXtoThePowerOfY_CorrectListReturned(int xValue, double yValue, double expected)
        {
            Interpreter interpreter = new Interpreter(new GenericHelper(), new FunctionHelper(), new IntegerHelper(), new RealHelper(), new BooleanHelper(), new SetHelper());
            IdentifierExpression x = new IdentifierExpression("x", 0, 0)
            {
                IsLocal = true,
                Reference = 0
            };
            IdentifierExpression y = new IdentifierExpression("y", 0, 0)
            {
                IsLocal = true,
                Reference = 1
            };
            CastFromIntegerExpression cast = new CastFromIntegerExpression(x, 0, 0);
            PowerExpression power = new PowerExpression(cast, y, 0, 0);
            ConditionNode condition = new ConditionNode(power, 0, 0);
            TypeNode integerType = new TypeNode(TypeEnum.Integer, 0, 0);
            TypeNode realType = new TypeNode(TypeEnum.Real, 0, 0);
            FunctionTypeNode functionType = new FunctionTypeNode(realType, new List<TypeNode> { integerType, realType }, 0, 0);
            FunctionNode function = new FunctionNode("func", condition, new List<string> { "x", "y" }, functionType, 0, 0);
            IntegerLiteralExpression integerLiteral = new IntegerLiteralExpression(xValue.ToString(), 0, 0);
            RealLiteralExpression realLiteral = new RealLiteralExpression(yValue.ToString(), 0, 0);
            FunctionCallExpression functionCall = new FunctionCallExpression("func",
                                                                             new List<ExpressionNode> { integerLiteral, realLiteral },
                                                                             0, 0)
            {
                GlobalReferences = new List<int> { 0 },
                LocalReference = -1
            };
            ExportNode export = new ExportNode(functionCall, 0, 0);
            AST ast = new AST(new List<FunctionNode> { function }, new List<ExportNode> { export }, 0, 0);

            List<double> res = interpreter.Interpret(ast);

            Assert.AreEqual(expected, res[0]);
        }
        #endregion
    }
}