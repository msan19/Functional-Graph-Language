using System;
using System.Collections.Generic;
using System.Linq;
using ASTLib;
using ASTLib.Nodes;
using ASTLib.Nodes.ExpressionNodes;
using ASTLib.Nodes.ExpressionNodes.BooleanOperationNodes;
using ASTLib.Nodes.ExpressionNodes.CommonOperationNodes;
using ASTLib.Nodes.ExpressionNodes.CommonOperationNodes.RelationalOperationNodes;
using ASTLib.Nodes.TypeNodes;
using FluentAssertions;
using InterpreterLib.Helpers;
using InterpreterLib.Interfaces;
using InterpreterLib.MatchPair;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;

namespace InterpreterLib.Tests
{
    [TestClass]
    public class GenericHelperTests
    {
        #region ConditionBoolean
        #endregion

        #region FunctionCallBoolean
        // Global
        // Local

        // x input varaibles


        [DataRow(0, 1, true)]
        [DataRow(0, 1, false)]
        [DataRow(2, 4, false)]
        [DataRow(2, 4, true)]
        [TestMethod]
        public void FunctionCallBoolean_GlobalRef_(int index, int funcCount, bool expected)
        {
            var parameters = new List<object>();
            var children = new List<ExpressionNode>();
            var expr = new FunctionCallExpression("", children, 0, 0);
            expr.GlobalReferences.Add(index);

            AST ast = Utilities.GetAst(funcCount);
            var targetFunc = ast.Functions[index];

            IInterpreterGeneric parent = Substitute.For<IInterpreterGeneric>();
            parent.Function<bool>(targetFunc, Arg.Any<List<object>>()).Returns(expected);
            GenericHelper booleanHelper = Utilities.GetGenericHelper(parent, ast);

            bool res = booleanHelper.FunctionCall<bool>(expr, parameters);

            Assert.AreEqual(expected, res);
        }
        [DataRow(0, 1, 0, 1, true)]
        [DataRow(0, 1, 0, 1, false)]
        [DataRow(2, 4, 3, 5, false)]
        [DataRow(2, 4, 3, 5, true)]
        [TestMethod]
        public void FunctionCallBoolean_LocalRef_(
            int paramIndex, int paramCount, 
            int funcIndex, int funcCount, 
            bool expected)
        {
            var parameters = Utilities.GetParameterList(paramCount);
            parameters[paramIndex] = funcIndex;

            var children = new List<ExpressionNode>();
            var expr = new FunctionCallExpression("", children, 0, 0);
            expr.LocalReference = paramIndex;

            AST ast = Utilities.GetAst(funcCount);
            var targetFunc = ast.Functions[funcIndex];

            IInterpreterGeneric parent = Substitute.For<IInterpreterGeneric>();
            parent.Function<bool>(targetFunc, Arg.Any<List<object>>()).Returns(expected);
            GenericHelper booleanHelper = Utilities.GetGenericHelper(parent, ast);

            bool res = booleanHelper.FunctionCall<bool>(expr, parameters);

            Assert.AreEqual(expected, res);
        }
        [DataRow(0)]
        [DataRow(1)]
        [DataRow(10)]
        [TestMethod]
        public void FunctionCallBoolean_GlobalRef_ParameterCount(int expectedElementCount)
        {
            int funcIndex = 0;
            var parameters = new List<object>();

            var children = Utilities.GetIntLitExprNodes(expectedElementCount);
            var expr = new FunctionCallExpression("", children, 0, 0);
            expr.GlobalReferences.Add(funcIndex);

            var funcType = Utilities.GetFunctionTypeNode(expectedElementCount, TypeEnum.Integer);
            var func = Utilities.GetFunction(funcType);
            AST ast = Utilities.GetAst(func);

            IInterpreterGeneric parent = Substitute.For<IInterpreterGeneric>();
            var res = new List<object>();
            parent.Function<bool>(Arg.Any<FunctionNode>(), Arg.Do<List<object>>(x => res = x));
            GenericHelper booleanHelper = Utilities.GetGenericHelper(parent, ast);

            booleanHelper.FunctionCall<bool>(expr, parameters);

            Assert.AreEqual(expectedElementCount, res.Count);
        }
        
        [DataRow(1, TypeEnum.Integer)]
        [DataRow(1, TypeEnum.Real)]
        [DataRow(10, TypeEnum.Integer)]
        [DataRow(10, TypeEnum.Real)]
        [TestMethod]
        public void FunctionCallBoolean_GlobalRef_CorrectParameterTypes(int expectedElementCount, TypeEnum type)
        {
            int funcIndex = 0;
            var parameters = new List<object>();

            var children = Utilities.GetIntLitExprNodes(expectedElementCount);
            var expr = new FunctionCallExpression("", children, 0, 0);
            expr.GlobalReferences.Add(funcIndex);

            var funcType = Utilities.GetFunctionTypeNode(expectedElementCount, type);
            var func = Utilities.GetFunction(funcType);
            AST ast = Utilities.GetAst(func);

            IInterpreterGeneric parent = Substitute.For<IInterpreterGeneric>();
            var res = new List<object>();
            parent.Function<bool>(Arg.Any<FunctionNode>(), Arg.Do<List<object>>(x => res = x)).Returns(false);
            parent.Dispatch(Arg.Any<ExpressionNode>(), 
                            Arg.Any<List<object>>(), 
                            Arg.Is<TypeEnum>(x => x == type)).Returns(1);
            GenericHelper booleanHelper = Utilities.GetGenericHelper(parent, ast);

            booleanHelper.FunctionCall<bool>(expr, parameters);

            foreach (var elem in res)
                Assert.IsNotNull(elem);
        }
        #endregion

        private GenericHelper SetUpHelper(IInterpreterGeneric parent)
        {
            GenericHelper functionHelper = new GenericHelper();
            functionHelper.SetInterpreter(parent);
            return functionHelper;
        }

        #region ConditionFunction

        [TestMethod]
        public void ConditionFunction_ConditionNodeAndObjectList_ReturnsNull()
        {
            IdentifierExpression id = new IdentifierExpression("", 1, 1);
            ConditionNode conditionNode = new ConditionNode(id, id, 1, 1);
            IInterpreterGeneric parent = Substitute.For<IInterpreterGeneric>();
            parent.Dispatch(id, Arg.Any<List<Object>>(), Arg.Any<TypeEnum>()).Returns(false);
            GenericHelper functionHelper = SetUpHelper(parent);
            bool expected = false;

            bool res = functionHelper.Condition<long>(conditionNode, new List<Object>()).IsCalculated;

            Assert.AreEqual(expected, res);
        }

        [DataRow(1, 1)]
        [DataRow(3, 3)]
        [DataRow(4, 4)]
        [TestMethod]
        public void ConditionFunction_ConditionNodeAndObjectList_ReturnsCorrectResult(long input, int expected)
        {
            IdentifierExpression id = new IdentifierExpression("", 1, 1);
            ConditionNode conditionNode = new ConditionNode(id, 1, 1);
            IInterpreterGeneric parent = Substitute.For<IInterpreterGeneric>();
            parent.Dispatch(id, Arg.Any<List<Object>>(), Arg.Any<TypeEnum>()).Returns(input);
            GenericHelper functionHelper = SetUpHelper(parent);

            int res = (int) functionHelper.Condition<long>(conditionNode, new List<Object>()).Element;

            Assert.AreEqual(expected, res);
        }

        [TestMethod]
        public void ConditionFunction_ConditionNodeAndObjectList_CorrectListPassed()
        {
            IdentifierExpression id = new IdentifierExpression("", 1, 1);
            ConditionNode conditionNode = new ConditionNode(id, 1, 1);
            IInterpreterGeneric parent = Substitute.For<IInterpreterGeneric>();
            List<Object> res = null;
            parent.Dispatch(id, Arg.Do<List<Object>>(x => res = x), Arg.Any<TypeEnum>()).Returns(0L);
            List<Object> expected = new List<Object> { 1, 1.3, "" };
            GenericHelper functionHelper = SetUpHelper(parent);

            functionHelper.Condition<long>(conditionNode, expected);

            res.Should().BeEquivalentTo(expected);
        }

        [TestMethod]
        public void ConditionFunction_ConditionNodeAndObjectList_CorrectNodePassed()
        {
            IdentifierExpression id = new IdentifierExpression("", 1, 1);
            ExpressionNode expected = id;
            ConditionNode conditionNode = new ConditionNode(id, 1, 1);
            IInterpreterGeneric parent = Substitute.For<IInterpreterGeneric>();
            ExpressionNode res = null;
            parent.Dispatch(Arg.Do<ExpressionNode>(x => res = x), Arg.Any<List<Object>>(), Arg.Any<TypeEnum>()).Returns(0L);
            List<Object> input2 = new List<Object> { 1, 1.3, "" };
            GenericHelper functionHelper = SetUpHelper(parent);

            functionHelper.Condition<long>(conditionNode, input2);

            res.Should().BeEquivalentTo(expected);
        }
        #endregion

        #region FunctionCallFunction
        [TestMethod]
        public void FunctionCallFunction_UsingGlobalReferences_PassesCorrectFunctionNodeToFunctionFunction()
        {
            IntegerLiteralExpression functionLit = new IntegerLiteralExpression("1.0", 1, 1);
            List<ExpressionNode> funcParams = new List<ExpressionNode> { functionLit };
            FunctionCallExpression funcCallExpr = new FunctionCallExpression("test", funcParams, 1, 1);
            funcCallExpr.GlobalReferences = new List<int> { 0 };
            funcCallExpr.LocalReference = -1;
            IInterpreterGeneric parent = Substitute.For<IInterpreterGeneric>();
            parent.Dispatch(funcParams[0], Arg.Any<List<object>>(), TypeEnum.Function).Returns((Object)1);
            GenericHelper functionHelper = SetUpHelper(parent);
            List<TypeNode> typeNodes = new List<TypeNode> { new TypeNode(TypeEnum.Function, 1, 1) };
            FunctionTypeNode funcTypeNode = new FunctionTypeNode(null, typeNodes, 1, 1);
            FunctionNode funcNode = new FunctionNode("", null, null, funcTypeNode, 1, 1);
            AST ast = new AST(new List<FunctionNode> { funcNode }, null, 1, 1);
            functionHelper.SetASTRoot(ast);
            FunctionNode res = null;
            parent.Function<long>(Arg.Do<FunctionNode>(x => res = x), Arg.Any<List<object>>());

            functionHelper.FunctionCall<long>(funcCallExpr, new List<Object>());

            res.Should().BeEquivalentTo(funcNode);
        }

        [TestMethod]
        public void FunctionCallFunction_UsingLocalReference_PassesCorrectFunctionNodeToFunctionFunction()
        {
            IntegerLiteralExpression functionLit = new IntegerLiteralExpression("1.0", 1, 1);
            List<ExpressionNode> funcParams = new List<ExpressionNode> { functionLit };
            FunctionCallExpression funcCallExpr = new FunctionCallExpression("test", funcParams, 1, 1);
            funcCallExpr.LocalReference = 0;
            funcCallExpr.GlobalReferences = new List<int>();
            IInterpreterGeneric parent = Substitute.For<IInterpreterGeneric>();
            parent.Dispatch(funcParams[0], Arg.Any<List<object>>(), TypeEnum.Function).Returns((Object)1);
            GenericHelper functionHelper = SetUpHelper(parent);
            List<TypeNode> typeNodes = new List<TypeNode> { new TypeNode(TypeEnum.Function, 1, 1) };
            FunctionTypeNode funcTypeNode = new FunctionTypeNode(null, typeNodes, 1, 1);
            FunctionNode funcNode = new FunctionNode("", null, null, funcTypeNode, 1, 1);
            AST ast = new AST(new List<FunctionNode> { funcNode }, null, 1, 1);
            functionHelper.SetASTRoot(ast);
            FunctionNode res = null;
            parent.Function<long>(Arg.Do<FunctionNode>(x => res = x), Arg.Any<List<object>>());

            functionHelper.FunctionCall<long>(funcCallExpr, new List<object> { 0 });

            res.Should().BeEquivalentTo(funcNode);
        }

        [DataRow(new Object[] { 1.0, 1 }, new TypeEnum[] { TypeEnum.Real, TypeEnum.Integer })]
        [DataRow(new Object[] { }, new TypeEnum[] { })]
        [DataRow(new Object[] { 17, 1 }, new TypeEnum[] { TypeEnum.Integer, TypeEnum.Integer })]
        [DataRow(new Object[] { 0 }, new TypeEnum[] { TypeEnum.Function })]
        [TestMethod]
        public void FunctionCallFunction_f_f(Object[] numbers, TypeEnum[] types)
        {
            List<Object> expected = numbers.ToList();
            List<TypeEnum> exTypes = types.ToList();
            List<ExpressionNode> funcParams = new List<ExpressionNode>();
            IInterpreterGeneric parent = Substitute.For<IInterpreterGeneric>();
            List<TypeNode> typeNodes = new List<TypeNode>();

            for (int i = 0; i < expected.Count; i++)
            {
                switch (expected[i])
                {
                    case int x:
                        funcParams.Add(new IntegerLiteralExpression(x.ToString(), 1, 1));
                        break;
                    case double x:
                        funcParams.Add(new RealLiteralExpression(x.ToString(), 1, 1));
                        break;
                    default:
                        throw new Exception("Unexpected shit");
                }
                parent.Dispatch(funcParams[i], Arg.Any<List<object>>(), exTypes[i]).Returns(expected[i]);
                typeNodes.Add(new TypeNode(exTypes[i], 1, 1));
            }



            FunctionCallExpression funcCallExpr = new FunctionCallExpression("test", funcParams, 1, 1);
            funcCallExpr.GlobalReferences = new List<int> { 0 };
            funcCallExpr.LocalReference = -1;
            GenericHelper functionHelper = SetUpHelper(parent);
            FunctionTypeNode funcTypeNode = new FunctionTypeNode(null, typeNodes, 1, 1);
            FunctionNode funcNode = new FunctionNode("", null, null, funcTypeNode, 1, 1);
            AST ast = new AST(new List<FunctionNode> { funcNode }, null, 1, 1);
            functionHelper.SetASTRoot(ast);
            List<object> res = new List<object>();
            parent.Function<long>(Arg.Any<FunctionNode>(), Arg.Do<List<object>>(x => res = x));

            functionHelper.FunctionCall<long>(funcCallExpr, new List<Object>());

            res.Should().BeEquivalentTo(expected);
        }
        #endregion

        #region FunctionCallInteger
        /*
        [TestMethod]
        public void FunctionCallInteger_LocalReference_CorrectFunctionNodeToFunctionInteger()
        {
            IntegerLiteralExpression intLit = new IntegerLiteralExpression("1", 1, 1);
            List<ExpressionNode> funcParams = new List<ExpressionNode> { intLit };
            FunctionCallExpression funcCallExpr = new FunctionCallExpression("1", funcParams, 1, 1);
            funcCallExpr.LocalReference = 0;
            funcCallExpr.GlobalReferences = new List<int>();
            IInterpreterInteger parent = Substitute.For<IInterpreterInteger>();
            IntegerHelper integerHelper = SetUpHelper(parent);
            parent.Dispatch(funcParams[0], Arg.Any<List<Object>>(), TypeEnum.Integer).Returns(1);
            FunctionNode functionNode = new FunctionNode("", null, null, new FunctionTypeNode(null, new List<TypeNode> { new TypeNode(TypeEnum.Integer, 1, 1) }, 1, 1), 1, 1);
            AST astRoot = new AST(new List<FunctionNode> { functionNode }, null, 1, 1);
            integerHelper.SetASTRoot(astRoot);

            FunctionNode res = null;
            parent.FunctionInteger(Arg.Do<FunctionNode>(x => res = x), Arg.Any<List<Object>>());
            integerHelper.FunctionCallInteger(funcCallExpr, new List<Object> { 0 });

            res.Should().BeEquivalentTo(functionNode);

        }

        [DataRow(new Object[] { 1.0, 1 }, new TypeEnum[] { TypeEnum.Real, TypeEnum.Integer })]
        [TestMethod]
        public void FunctionCallInteger_DifferentParameters_PassesCorrectParameterValuesToFunctionInteger(Object[] numbers, TypeEnum[] types)
        {
            List<Object> expectedList = numbers.ToList();
            List<TypeEnum> expectedTypes = types.ToList();
            List<ExpressionNode> funcParams = new List<ExpressionNode>();
            IInterpreterInteger parent = Substitute.For<IInterpreterInteger>();
            List<TypeNode> typeNodes = new List<TypeNode>();

            for (int i = 0; i < expectedList.Count; i++)
            {
                switch (expectedList[i])
                {
                    case int x:
                        funcParams.Add(new IntegerLiteralExpression(x.ToString(), 1, 1));
                        break;
                    case double x:
                        funcParams.Add(new RealLiteralExpression(x.ToString(), 1, 1));
                        break;
                    default:
                        throw new Exception("Unexpected shit");
                }
                parent.Dispatch(funcParams[i], Arg.Any<List<object>>(), expectedTypes[i]).Returns(expectedList[i]);
                typeNodes.Add(new TypeNode(expectedTypes[i], 1, 1));
            }

            FunctionCallExpression funcCallExpr = new FunctionCallExpression("test", funcParams, 1, 1);
            funcCallExpr.GlobalReferences = new List<int> { 0 };
            funcCallExpr.LocalReference = -1;
            IntegerHelper integerHelper = SetUpHelper(parent);
            FunctionTypeNode funcTypeNode = new FunctionTypeNode(null, typeNodes, 1, 1);
            FunctionNode funcNode = new FunctionNode("", null, null, funcTypeNode, 1, 1);
            AST ast = new AST(new List<FunctionNode> { funcNode }, null, 1, 1);
            integerHelper.SetASTRoot(ast);
            List<object> res = new List<object>();
            parent.FunctionInteger(Arg.Any<FunctionNode>(), Arg.Do<List<object>>(x => res = x));

            integerHelper.FunctionCallInteger(funcCallExpr, new List<Object>());

            res.Should().BeEquivalentTo(expectedList);
        }
        */
        #endregion

        #region FunctionInteger
        /*
        [TestMethod]
        public void ConditionInteger_ConditionNodeAndObjectList_ReturnsNull()
        {
            IdentifierExpression id = new IdentifierExpression("", 1, 1);
            ConditionNode conditionNode = new ConditionNode(id, id, 1, 1);
            IInterpreterInteger parent = Substitute.For<IInterpreterInteger>();
            parent.DispatchBoolean(id, Arg.Any<List<Object>>()).Returns(false);
            IntegerHelper integerHelper = SetUpHelper(parent);
            int? expected = null;

            int? res = integerHelper.ConditionInteger(conditionNode, new List<Object>());

            Assert.AreEqual(expected, res);
        }

        [DataRow(2, 2)]
        [TestMethod]
        public void ConditionInteger_Integer_ReturnsCorrectResult(int input, int expected)
        {
            IntegerLiteralExpression intLit = new IntegerLiteralExpression(input.ToString(), 1, 1);
            ConditionNode conditionNode = new ConditionNode(intLit, 1, 1);
            IInterpreterInteger parent = Substitute.For<IInterpreterInteger>();
            parent.DispatchInt(intLit, Arg.Any<List<object>>()).Returns(input);
            IntegerHelper integerHelper = SetUpHelper(parent);

            int res = (int)integerHelper.ConditionInteger(conditionNode, new List<object>());

            Assert.AreEqual(expected, res);
        }
        */
        #endregion

        #region ConditionReal
        /*
        [TestMethod]
        public void ConditionReal_ConditionNodeAndObjectList_ReturnsNull()
        {
            IdentifierExpression id = new IdentifierExpression("", 1, 1);
            ConditionNode conditionNode = new ConditionNode(id, id, 1, 1);
            IInterpreterReal parent = Substitute.For<IInterpreterReal>();
            parent.DispatchBoolean(id, Arg.Any<List<Object>>()).Returns(false);
            RealHelper realHelper = SetUpHelper(parent);
            double? expected = null;

            double? res = realHelper.ConditionReal(conditionNode, new List<Object>());

            Assert.AreEqual(expected, res);
        }

        [DataRow(1.0, 1.0)]
        [DataRow(-1.0, -1.0)]
        [DataRow(0.0, 0.0)]
        [TestMethod]
        public void ConditionReal_Real_ReturnsCorrectResult(double input, double expected)
        {
            IntegerLiteralExpression realLit = new IntegerLiteralExpression(input.ToString(), 1, 1);
            ConditionNode conditionNode = new ConditionNode(realLit, 1, 1);
            IInterpreterReal parent = Substitute.For<IInterpreterReal>();
            parent.DispatchReal(realLit, Arg.Any<List<object>>()).Returns(input);
            RealHelper realHelper = SetUpHelper(parent);

            double res = (double)realHelper.ConditionReal(conditionNode, new List<object>());

            Assert.AreEqual(expected, res);
        }
        */
        #endregion

        #region FunctionCallReal
        /*
        [TestMethod]
        public void FunctionCallReal_UsingGlobalReferences_PassesCorrectFunctionNodeToFunctionReal()
        {
            IntegerLiteralExpression realLit = new IntegerLiteralExpression("1.0", 1, 1);
            List<ExpressionNode> funcParams = new List<ExpressionNode> { realLit };
            FunctionCallExpression funcCallExpr = new FunctionCallExpression("test", funcParams, 1, 1);
            funcCallExpr.GlobalReferences = new List<int> { 0 };
            funcCallExpr.LocalReference = -1;
            IInterpreterReal parent = Substitute.For<IInterpreterReal>();
            parent.Dispatch(funcParams[0], Arg.Any<List<object>>(), TypeEnum.Real).Returns((Object)1.0);
            RealHelper realHelper = SetUpHelper(parent);
            List<TypeNode> typeNodes = new List<TypeNode> { new TypeNode(TypeEnum.Real, 1, 1) };
            FunctionTypeNode funcTypeNode = new FunctionTypeNode(null, typeNodes, 1, 1);
            FunctionNode funcNode = new FunctionNode("", null, null, funcTypeNode, 1, 1);
            AST ast = new AST(new List<FunctionNode> { funcNode }, null, 1, 1);
            realHelper.SetASTRoot(ast);
            FunctionNode res = null;
            parent.FunctionReal(Arg.Do<FunctionNode>(x => res = x), Arg.Any<List<object>>());

            realHelper.FunctionCallReal(funcCallExpr, new List<Object>());

            res.Should().BeEquivalentTo(funcNode);
        }

        [TestMethod]
        public void FunctionCallReal_UsingLocalReference_PassesCorrectFunctionNodeToFunctionReal()
        {
            IntegerLiteralExpression realLit = new IntegerLiteralExpression("1.0", 1, 1);
            List<ExpressionNode> funcParams = new List<ExpressionNode> { realLit };
            FunctionCallExpression funcCallExpr = new FunctionCallExpression("test", funcParams, 1, 1);
            funcCallExpr.LocalReference = 0;
            funcCallExpr.GlobalReferences = new List<int>();
            IInterpreterReal parent = Substitute.For<IInterpreterReal>();
            parent.Dispatch(funcParams[0], Arg.Any<List<object>>(), TypeEnum.Real).Returns((Object)1.0);
            RealHelper realHelper = SetUpHelper(parent);
            List<TypeNode> typeNodes = new List<TypeNode> { new TypeNode(TypeEnum.Real, 1, 1) };
            FunctionTypeNode funcTypeNode = new FunctionTypeNode(null, typeNodes, 1, 1);
            FunctionNode funcNode = new FunctionNode("", null, null, funcTypeNode, 1, 1);
            AST ast = new AST(new List<FunctionNode> { funcNode }, null, 1, 1);
            realHelper.SetASTRoot(ast);
            FunctionNode res = null;
            parent.FunctionReal(Arg.Do<FunctionNode>(x => res = x), Arg.Any<List<object>>());

            realHelper.FunctionCallReal(funcCallExpr, new List<object> { 0 });

            res.Should().BeEquivalentTo(funcNode);
        }

        [DataRow(new Object[] { 1.0, 1 }, new TypeEnum[] { TypeEnum.Real, TypeEnum.Integer })]
        [DataRow(new Object[] { 1.0, 2.0, 3.0 }, new TypeEnum[] { TypeEnum.Real, TypeEnum.Real, TypeEnum.Real })]
        [DataRow(new Object[] { 1, 2, 3 }, new TypeEnum[] { TypeEnum.Integer, TypeEnum.Integer, TypeEnum.Integer })]
        [DataRow(new Object[] { }, new TypeEnum[] { })]
        [TestMethod]
        public void FunctionCallReal_DifferentParameters_PassesCorrectParameterValuesToFunctionReal(Object[] numbers, TypeEnum[] types)
        {
            List<Object> expected = numbers.ToList();
            List<TypeEnum> exTypes = types.ToList();
            List<ExpressionNode> funcParams = new List<ExpressionNode>();
            IInterpreterReal parent = Substitute.For<IInterpreterReal>();
            List<TypeNode> typeNodes = new List<TypeNode>();
            for (int i = 0; i < expected.Count; i++)
            {
                switch (expected[i])
                {
                    case int x:
                        funcParams.Add(new IntegerLiteralExpression(x.ToString(), 1, 1));
                        break;
                    case double x:
                        funcParams.Add(new RealLiteralExpression(x.ToString(), 1, 1));
                        break;
                    default:
                        throw new Exception("Unexpected shit");
                }
                parent.Dispatch(funcParams[i], Arg.Any<List<object>>(), exTypes[i]).Returns(expected[i]);
                typeNodes.Add(new TypeNode(exTypes[i], 1, 1));
            }
            FunctionCallExpression funcCallExpr = new FunctionCallExpression("test", funcParams, 1, 1);
            funcCallExpr.GlobalReferences = new List<int> { 0 };
            funcCallExpr.LocalReference = -1;
            RealHelper realHelper = SetUpHelper(parent);
            FunctionTypeNode funcTypeNode = new FunctionTypeNode(null, typeNodes, 1, 1);
            FunctionNode funcNode = new FunctionNode("", null, null, funcTypeNode, 1, 1);
            AST ast = new AST(new List<FunctionNode> { funcNode }, null, 1, 1);
            realHelper.SetASTRoot(ast);
            List<object> res = new List<object>();
            parent.FunctionReal(Arg.Any<FunctionNode>(), Arg.Do<List<object>>(x => res = x));

            realHelper.FunctionCallReal(funcCallExpr, new List<Object>());

            res.Should().BeEquivalentTo(expected);
        }
        */
        #endregion
    }
}