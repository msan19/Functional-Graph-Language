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
using ASTLib.Objects;
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

        #region DispatchReal_IdentifierExpr
        [TestMethod]
        public void DispatchReal_IdentifierAndObjectList_CorrectListPassed()
        {
            List<Object> expected = new List<Object>() { 23, 2.334, null };
            IdentifierExpression input1 = new IdentifierExpression(null, 0, 0);
            IGenericHelper rhelper = Substitute.For<IGenericHelper>();
            Interpreter interpreter = Utilities.GetIntepreterOnlyWith(rhelper);
            List<Object> res = null;
            rhelper.Identifier<double>(Arg.Any<IdentifierExpression>(), Arg.Do<List<Object>>(x => res = x));

            interpreter.DispatchReal(input1, expected);

            res.Should().BeEquivalentTo(expected);
        }

        [TestMethod]
        public void DispatchReal_IdentifierAndObjectList_CorrectIdentifierExprPassed()
        {
            IdentifierExpression expected = new IdentifierExpression(null, 0, 0);
            IdentifierExpression input1 = expected;
            List<Object> input2 = new List<Object>() { 23, 2.334, null };
            IGenericHelper rhelper = Substitute.For<IGenericHelper>();
            Interpreter interpreter = Utilities.GetIntepreterOnlyWith(rhelper);
            IdentifierExpression res = null;
            rhelper.Identifier<double>(Arg.Do<IdentifierExpression>(x => res = x), Arg.Any<List<Object>>());

            interpreter.DispatchReal(input1, input2);

            res.Should().BeEquivalentTo(expected);
        }

        [TestMethod]
        public void DispatchReal_IdentifierAndObjectList_CorrectValueReturned()
        {
            double expected = 17;
            IdentifierExpression input1 = new IdentifierExpression(null, 0, 0);
            List<Object> input2 = new List<Object>() { 23, 2.334, null };
            IGenericHelper rhelper = Substitute.For<IGenericHelper>();
            Interpreter interpreter = Utilities.GetIntepreterOnlyWith(rhelper);
            rhelper.Identifier<double>(Arg.Any<IdentifierExpression>(), Arg.Any<List<Object>>()).Returns(expected);

            double res = interpreter.DispatchReal(input1, input2);

            Assert.AreEqual(expected, res);
        }
        #endregion

        #region DispatchInt_IdentifierExpr
        [TestMethod]
        public void DispatchInteger_IdentifierAndObjectList_CorrectListPassed()
        {
            List<Object> expected = new List<Object>() { 23, 2.334, null };
            IdentifierExpression input1 = new IdentifierExpression(null, 0, 0);
            IGenericHelper ihelper = Substitute.For<IGenericHelper>();
            Interpreter interpreter = Utilities.GetIntepreterOnlyWith(ihelper);
            List<Object> res = null;
            ihelper.Identifier<int>(Arg.Any<IdentifierExpression>(), Arg.Do<List<Object>>(x => res = x));

            interpreter.DispatchInt(input1, expected);

            res.Should().BeEquivalentTo(expected);
        }

        [TestMethod]
        public void DispatchInteger_IdentifierAndObjectList_CorrectIdentifierExprPassed()
        {
            IdentifierExpression expected = new IdentifierExpression(null, 0, 0);
            IdentifierExpression input1 = expected;
            List<Object> input2 = new List<Object>() { 23, 2.334, null };
            IGenericHelper ihelper = Substitute.For<IGenericHelper>();
            Interpreter interpreter = Utilities.GetIntepreterOnlyWith(ihelper);
            IdentifierExpression res = null;
            ihelper.Identifier<int>(Arg.Do<IdentifierExpression>(x => res = x), Arg.Any<List<Object>>());

            interpreter.DispatchInt(input1, input2);

            res.Should().BeEquivalentTo(expected);
        }

        [TestMethod]
        public void DispatchInteger_IdentifierAndObjectList_CorrectValueReturned()
        {
            int expected = 17;
            IdentifierExpression input1 = new IdentifierExpression(null, 0, 0);
            List<Object> input2 = new List<Object>() { 23, 2.334, null };
            IGenericHelper ihelper = Substitute.For<IGenericHelper>();
            Interpreter interpreter = Utilities.GetIntepreterOnlyWith(ihelper);
            ihelper.Identifier<int>(Arg.Any<IdentifierExpression>(), Arg.Any<List<Object>>()).Returns(expected);

            int res = interpreter.DispatchInt(input1, input2);

            Assert.AreEqual(expected, res);
        }
        #endregion

        #region IdentifierInteger
        [TestMethod]
        public void IdentifierInteger_IdentifierNode_ReturnsCorrectResult()
        {
            IdentifierExpression identifierExpr = new IdentifierExpression("This is a test", 1, 1);
            identifierExpr.Reference = 0;
            List<object> parameters = new List<object> { 0 };
            int expected = (int)parameters[0];
            GenericHelper integerHelper = new GenericHelper();

            int res = integerHelper.Identifier<int>(identifierExpr, parameters);

            Assert.AreEqual(expected, res);
        }
        #endregion

        #region IdentifierBoolean
        [DataRow(true, true)]
        [DataRow(false, false)]
        [TestMethod]
        public void Identifier_Index0_CorrectValuesReturned(bool input, bool expected)
        {
            var expression = GetIdentifier(0);
            var parameters = new List<object>() { input };
            IInterpreterGeneric parent = Substitute.For<IInterpreterGeneric>();

            GenericHelper booleanHelper = Utilities.GetGenericHelper(parent);

            bool res = booleanHelper.Identifier<bool>(expression, parameters);

            Assert.AreEqual(expected, res);
        }

        [DataRow(true, true)]
        [DataRow(false, false)]
        [TestMethod]
        public void Identifier_Index1_CorrectValuesReturned(bool input, bool expected)
        {
            var expression = GetIdentifier(1);
            var parameters = new List<object>() { "testing", input };
            IInterpreterGeneric parent = Substitute.For<IInterpreterGeneric>();

            GenericHelper booleanHelper = Utilities.GetGenericHelper(parent);

            bool res = booleanHelper.Identifier<bool>(expression, parameters);

            Assert.AreEqual(expected, res);
        }

        private IdentifierExpression GetIdentifier(int reference)
        {
            return new IdentifierExpression("", 0, 0) { Reference = reference };
        }
        #endregion

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
            parameters[paramIndex] = new Function(funcIndex);

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

        #region IdentifierReal
        [DataRow(0, new object[] { 5.1 }, 5.1)]
        [DataRow(1, new object[] { 5.1, 3.0, 1.1 }, 3.0)]
        [TestMethod]
        public void IdentifierReal_ValidIdentifierExprAndParameters_ReturnsCorrectResult(int reference, object[] parameterArray, double expected)
        {
            IdentifierExpression identifierExpr = new IdentifierExpression("test", 1, 1);
            identifierExpr.Reference = reference;
            List<object> parameters = parameterArray.ToList();
            GenericHelper realHelper = new GenericHelper();

            double res = realHelper.Identifier<double>(identifierExpr, parameters);

            Assert.AreEqual(expected, res);
        }
        #endregion

        private GenericHelper SetUpHelper(IInterpreterGeneric parent)
        {
            GenericHelper functionHelper = new GenericHelper();
            functionHelper.SetInterpreter(parent);
            return functionHelper;
        }

        #region Condition
        [DataRow(1, new int[] { 2 }, new int[] { 2 }, 2, true)]
        [DataRow(1, new int[] { 1 }, new int[] { 2 }, 2, false)]
        [DataRow(1, new int[] { 2 }, new int[] { 1 }, 2, false)]
        [TestMethod]
        public void Condition_Elements_CheckElementCondition(int elemCount, int[] elemDims, int[] paramDims, int ps, bool expected)
        {
            List<ElementNode> elements = new List<ElementNode>();
            for (int i = 0; i < elemCount; i++)
                elements.AddRange(Utilities.GetElementNodess(elemCount, elemDims[i], ps));
            ExpressionNode conditionExpr = null;
            ExpressionNode returnExpr = Utilities.GetIntLitExpression();
            var node = Utilities.GetConditionNode(elements, conditionExpr, returnExpr);

            var parameters = Utilities.GetParameterList(ps);
            for (int i = 0; i < elemCount; i++)
                parameters.AddRange(Utilities.GetElements(elemCount, paramDims[i]));

            var parent = Utilities.GetGenericInterpreter();
            parent.Dispatch(Arg.Any<IntegerLiteralExpression>(), Arg.Any<List<object>>(), TypeEnum.Integer).Returns(1);
            var helper = Utilities.GetGenericHelper(parent);

            var res = helper.Condition<int>(node, parameters).IsCalculated;

            Assert.AreEqual(expected, res);
        }

        [DataRow(2, 2, 2)]
        [DataRow(2, 4, 3)]
        [DataRow(4, 1, 1)]
        [TestMethod]
        public void Condition_Elements_AddIdsToParametersBeforeCondition(int elemCount, int dims, int ps)
        {
            List<ElementNode> elements = Utilities.GetElementNodess(elemCount, dims, ps);
            ExpressionNode conditionExpr = Utilities.GetBoolLitExpression(true);
            ExpressionNode returnExpr = Utilities.GetIntLitExpression();
            var node = Utilities.GetConditionNode(elements, conditionExpr, returnExpr);

            var parameters = Utilities.GetParameterList(ps);
            parameters.AddRange(Utilities.GetElements(elemCount, dims));
            var expected = parameters.ToList();
            expected.AddRange(Utilities.ConvertElementNodesToInts(elements));

            var res = new List<object>();
            var parent = Utilities.GetGenericInterpreter();
            parent.DispatchBoolean(Arg.Any<BooleanLiteralExpression>(), Arg.Do<List<object>>(x => res = x)).Returns(true);
            parent.Dispatch(Arg.Any<IntegerLiteralExpression>(), Arg.Any<List<object>>(), TypeEnum.Integer).Returns(1);
            var helper = Utilities.GetGenericHelper(parent);

            helper.Condition<int>(node, parameters);

            res.Should().BeEquivalentTo(expected);
        }

        [DataRow(2, 2, 2)]
        [DataRow(2, 4, 3)]
        [DataRow(4, 1, 1)]
        [TestMethod]
        public void Condition_Elements_AddIdsToParametersBeforeReturnExpr(int elemCount, int dims, int ps)
        {
            List<ElementNode> elements = Utilities.GetElementNodess(elemCount, dims, ps);
            ExpressionNode conditionExpr = Utilities.GetBoolLitExpression(true);
            ExpressionNode returnExpr = Utilities.GetIntLitExpression();
            var node = Utilities.GetConditionNode(elements, conditionExpr, returnExpr);

            var parameters = Utilities.GetParameterList(ps);
            parameters.AddRange(Utilities.GetElements(elemCount, dims));
            var expected = parameters.ToList();
            expected.AddRange(Utilities.ConvertElementNodesToInts(elements));

            var res = new List<object>();
            var parent = Utilities.GetGenericInterpreter();
            parent.DispatchBoolean(Arg.Any<BooleanLiteralExpression>(), Arg.Do<List<object>>(x => res = x)).Returns(true);
            parent.Dispatch(Arg.Any<IntegerLiteralExpression>(), Arg.Do<List<object>>(x => res = x), TypeEnum.Integer).Returns(1);
            var helper = Utilities.GetGenericHelper(parent);

            helper.Condition<int>(node, parameters);

            res.Should().BeEquivalentTo(expected);
        }

        [DataRow(2, 2, 2)]
        [DataRow(2, 4, 3)]
        [DataRow(4, 1, 1)]
        [TestMethod]
        public void Condition_Elements_RemovedIdsFromParameters(int elemCount, int dims, int ps)
        {
            List<ElementNode> elements = Utilities.GetElementNodess(elemCount, dims, ps);
            ExpressionNode conditionExpr = Utilities.GetBoolLitExpression(true);
            ExpressionNode returnExpr = Utilities.GetIntLitExpression();
            var node = Utilities.GetConditionNode(elements, conditionExpr, returnExpr);

            var parameters = Utilities.GetParameterList(ps);
            parameters.AddRange(Utilities.GetElements(elemCount, dims));
            var expected = parameters.ToList();

            var parent = Utilities.GetGenericInterpreter();
            parent.Dispatch(Arg.Any<BooleanLiteralExpression>(), Arg.Any<List<object>>(), TypeEnum.Boolean).Returns(true);
            parent.Dispatch(Arg.Any<IntegerLiteralExpression>(), Arg.Any<List<object>>(), TypeEnum.Integer).Returns(1);
            var helper = Utilities.GetGenericHelper(parent);

            helper.Condition<int>(node, parameters);

            parameters.Should().BeEquivalentTo(expected);
        }
        #endregion

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

            bool res = functionHelper.Condition<Function>(conditionNode, new List<Object>()).IsCalculated;

            Assert.AreEqual(expected, res);
        }

        [DataRow(1, 1)]
        [DataRow(3, 3)]
        [DataRow(4, 4)]
        [TestMethod]
        public void ConditionFunction_ConditionNodeAndObjectList_ReturnsCorrectResult(int input, int expected)
        {
            IdentifierExpression id = new IdentifierExpression("", 1, 1);
            ConditionNode conditionNode = new ConditionNode(id, 1, 1);
            IInterpreterGeneric parent = Substitute.For<IInterpreterGeneric>();
            parent.Dispatch(id, Arg.Any<List<Object>>(), Arg.Any<TypeEnum>()).Returns(new Function(input));
            GenericHelper functionHelper = SetUpHelper(parent);

            int res = functionHelper.Condition<Function>(conditionNode, new List<Object>()).Element.Reference;

            Assert.AreEqual(expected, res);
        }

        [TestMethod]
        public void ConditionFunction_ConditionNodeAndObjectList_CorrectListPassed()
        {
            IdentifierExpression id = new IdentifierExpression("", 1, 1);
            ConditionNode conditionNode = new ConditionNode(id, 1, 1);
            IInterpreterGeneric parent = Substitute.For<IInterpreterGeneric>();
            List<Object> res = null;
            parent.Dispatch(id, Arg.Do<List<Object>>(x => res = x), Arg.Any<TypeEnum>()).Returns(new Function(0));
            List<Object> expected = new List<Object> { 1, 1.3, "" };
            GenericHelper functionHelper = SetUpHelper(parent);

            functionHelper.Condition<Function>(conditionNode, expected);

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
            parent.Dispatch(Arg.Do<ExpressionNode>(x => res = x), Arg.Any<List<Object>>(), Arg.Any<TypeEnum>())
                .Returns(new Function(0));
            List<Object> input2 = new List<Object> { 1, 1.3, "" };
            GenericHelper functionHelper = SetUpHelper(parent);

            functionHelper.Condition<Function>(conditionNode, input2);

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
            parent.Function<Function>(Arg.Do<FunctionNode>(x => res = x), Arg.Any<List<object>>());

            functionHelper.FunctionCall<Function>(funcCallExpr, new List<Object>());

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
            parent.Function<Function>(Arg.Do<FunctionNode>(x => res = x), Arg.Any<List<object>>());

            functionHelper.FunctionCall<Function>(funcCallExpr, new List<object> { new Function(0) });

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
            parent.Function<Function>(Arg.Any<FunctionNode>(), Arg.Do<List<object>>(x => res = x));

            functionHelper.FunctionCall<Function>(funcCallExpr, new List<Object>());

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