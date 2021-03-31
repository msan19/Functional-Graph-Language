using ASTLib;
using ASTLib.Nodes;
using ASTLib.Nodes.ExpressionNodes;
using ASTLib.Nodes.TypeNodes;
using FluentAssertions;
using InterpreterLib.Helpers;
using InterpreterLib.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;

namespace InterpreterLib.Tests
{
    [TestClass]
    public class FunctionHelperTests
    {
        private FunctionHelper SetUpHelper(IInterpreterFunction parent)
        {
            FunctionHelper functionHelper = new FunctionHelper();
            functionHelper.SetInterpreter(parent);
            return functionHelper;
        }

        #region ConditionFunction
        [DataRow(1, 1)]
        [DataRow(3, 3)]
        [DataRow(4, 4)]
        [TestMethod]
        public void ConditionFunction_ConditionNodeAndObjectList_ReturnsCorrectResult(int input, int expected)
        {
            IdentifierExpression id = new IdentifierExpression("", 1, 1);
            ConditionNode conditionNode = new ConditionNode(id, 1, 1);
            IInterpreterFunction parent = Substitute.For<IInterpreterFunction>();
            parent.DispatchFunction(id, Arg.Any<List<Object>>()).Returns(input);
            FunctionHelper functionHelper = SetUpHelper(parent);

            int res = (int) functionHelper.ConditionFunction(conditionNode, new List<Object>());

            Assert.AreEqual(expected, res);
        }

        [TestMethod]
        public void ConditionFunction_ConditionNodeAndObjectList_CorrectListPassed()
        {
            IdentifierExpression id = new IdentifierExpression("", 1, 1);
            ConditionNode conditionNode = new ConditionNode(id, 1, 1);
            IInterpreterFunction parent = Substitute.For<IInterpreterFunction>();
            List<Object> res = null;
            parent.DispatchFunction(id, Arg.Do<List<Object>>(x => res = x));
            List<Object> expected = new List<Object> { 1, 1.3, "" };
            FunctionHelper functionHelper = SetUpHelper(parent);

            functionHelper.ConditionFunction(conditionNode, expected);

            res.Should().BeEquivalentTo(expected);
        }

        [TestMethod]
        public void ConditionFunction_ConditionNodeAndObjectList_CorrectNodePassed()
        {
            IdentifierExpression id = new IdentifierExpression("", 1, 1);
            ExpressionNode expected = id;
            ConditionNode conditionNode = new ConditionNode(id, 1, 1);
            IInterpreterFunction parent = Substitute.For<IInterpreterFunction>();
            ExpressionNode res = null;
            parent.DispatchFunction(Arg.Do<ExpressionNode>(x => res = x), Arg.Any<List<Object>>());
            List<Object> input2 = new List<Object> { 1, 1.3, "" };
            FunctionHelper functionHelper = SetUpHelper(parent);

            functionHelper.ConditionFunction(conditionNode, input2);

            res.Should().BeEquivalentTo(expected);
        }
        #endregion

        #region IdentifierFunction
        [DataRow(false, 1, new Object[] { 0, 18, "" })]
        [DataRow(false, 10, new Object[] { 0, 18, "" })]
        [DataRow(true, 0, new Object[] { 0, 18, "" })]
        [DataRow(true, 4, new Object[] { 0, 18, "", 104, 17})]
        [TestMethod]
        public void IdentifierFunction_IdentifierExpressionAndObjectList_ReturnsCorrectResult(bool isLocal, int reference, Object[] array)
        {
            IdentifierExpression identifier = new IdentifierExpression("", 0, 0)
            {
                IsLocal = isLocal,
                Reference = reference
            };
            FunctionHelper fhelper = new FunctionHelper();
            int expected = isLocal ? (int) array[reference] : reference;

            int res = fhelper.IdentifierFunction(identifier, array.ToList());

            Assert.AreEqual(res, expected);
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
            IInterpreterFunction parent = Substitute.For<IInterpreterFunction>();
            parent.Dispatch(funcParams[0], Arg.Any<List<object>>(), TypeEnum.Function).Returns((Object)1);
            FunctionHelper functionHelper = SetUpHelper(parent);
            List<TypeNode> typeNodes = new List<TypeNode> { new TypeNode(TypeEnum.Function, 1, 1) };
            FunctionTypeNode funcTypeNode = new FunctionTypeNode(null, typeNodes, 1, 1);
            FunctionNode funcNode = new FunctionNode("", null, null, funcTypeNode, 1, 1);
            AST ast = new AST(new List<FunctionNode> { funcNode }, null, 1, 1);
            functionHelper.SetAST(ast);
            FunctionNode res = null;
            parent.FunctionFunction(Arg.Do<FunctionNode>(x => res = x), Arg.Any<List<object>>());

            functionHelper.FunctionCallFunction(funcCallExpr, new List<Object>());

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
            IInterpreterFunction parent = Substitute.For<IInterpreterFunction>();
            parent.Dispatch(funcParams[0], Arg.Any<List<object>>(), TypeEnum.Function).Returns((Object)1);
            FunctionHelper functionHelper = SetUpHelper(parent);
            List<TypeNode> typeNodes = new List<TypeNode> { new TypeNode(TypeEnum.Function, 1, 1) };
            FunctionTypeNode funcTypeNode = new FunctionTypeNode(null, typeNodes, 1, 1);
            FunctionNode funcNode = new FunctionNode("", null, null, funcTypeNode, 1, 1);
            AST ast = new AST(new List<FunctionNode> { funcNode }, null, 1, 1);
            functionHelper.SetAST(ast);
            FunctionNode res = null;
            parent.FunctionFunction(Arg.Do<FunctionNode>(x => res = x), Arg.Any<List<object>>());

            functionHelper.FunctionCallFunction(funcCallExpr, new List<object> { 0 });

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
            IInterpreterFunction parent = Substitute.For<IInterpreterFunction>();
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
            FunctionHelper functionHelper = SetUpHelper(parent);
            FunctionTypeNode funcTypeNode = new FunctionTypeNode(null, typeNodes, 1, 1);
            FunctionNode funcNode = new FunctionNode("", null, null, funcTypeNode, 1, 1);
            AST ast = new AST(new List<FunctionNode> { funcNode }, null, 1, 1);
            functionHelper.SetAST(ast);
            List<object> res = new List<object>();
            parent.FunctionFunction(Arg.Any<FunctionNode>(), Arg.Do<List<object>>(x => res = x));

            functionHelper.FunctionCallFunction(funcCallExpr, new List<Object>());

            res.Should().BeEquivalentTo(expected);
        }
        #endregion
    }
}