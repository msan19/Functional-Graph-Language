using System;
using System.Collections.Generic;
using ASTLib.Nodes;
using ASTLib.Nodes.ExpressionNodes;
using ASTLib.Nodes.TypeNodes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using FluentAssertions;
using ASTLib.Exceptions;
using System.Linq;
using ASTLib.Nodes.ExpressionNodes.CommonOperationNodes.RelationalOperationNodes;

namespace ReferenceHandlerLib.Tests
{
    [TestClass]
    public class ReferenceHelperTests
    {
        private ReferenceHelper BuildHelper(IReferenceHandler referenceHandler)
        {
            ReferenceHelper referenceHelper = new ReferenceHelper();
            referenceHelper.SetDispatch(referenceHandler.Dispatch);
            return referenceHelper;
        }

        #region VisitExport
        [TestMethod]
        public void VisitExport_IntegerLiteralExpression_Correct()
        {
            System.Type expected = typeof(IntegerLiteralExpression);
            IntegerLiteralExpression integerLiteralExpression = new IntegerLiteralExpression("2", 1, 1);
            ExportNode input = new ExportNode(integerLiteralExpression, 3, 3);
            IReferenceHandler parent = Substitute.For<IReferenceHandler>();
            ReferenceHelper referenceHelper = BuildHelper(parent);

            System.Type result = null;
            parent.Dispatch(Arg.Do<IntegerLiteralExpression>(x => result = x.GetType()), Arg.Any<List<string>>());
            referenceHelper.VisitExport(input);

            Assert.AreEqual(expected, result);
        }
        [TestMethod]
        public void VisitExport_RealLiteralExpression_Correct()
        {
            System.Type expected = typeof(RealLiteralExpression);
            RealLiteralExpression realLiteralExpression = new RealLiteralExpression("2", 1, 1);
            ExportNode input = new ExportNode(realLiteralExpression, 3, 3);
            IReferenceHandler parent = Substitute.For<IReferenceHandler>();
            ReferenceHelper referenceHelper = BuildHelper(parent);

            System.Type result = null;
            parent.Dispatch(Arg.Do<RealLiteralExpression>(x => result = x.GetType()), Arg.Any<List<string>>());
            referenceHelper.VisitExport(input);

            Assert.AreEqual(expected, result);
        }
        #endregion

        #region VisitFunction
        [TestMethod]
        public void VisitFunction_IntegerReturnType_Correct()
        {
            System.Type expected = typeof(IntegerLiteralExpression);
            IntegerLiteralExpression integerLiteralExpression = new IntegerLiteralExpression("2", 1, 1);
            ConditionNode conditionNode = new ConditionNode(integerLiteralExpression, 2, 2);
            List<string> parameterIdentifiers = new List<string> { "a", "b" };
            TypeNode typeNode = new TypeNode(TypeEnum.Integer, 1, 1);
            List<TypeNode> parameterTypes = new List<TypeNode>() { typeNode, typeNode };
            FunctionTypeNode functionType = new FunctionTypeNode(typeNode, parameterTypes, 3, 3);
            FunctionNode input = new FunctionNode("func1", conditionNode, parameterIdentifiers, functionType, 4, 4);
            IReferenceHandler parent = Substitute.For<IReferenceHandler>();
            ReferenceHelper referenceHelper = BuildHelper(parent);

            System.Type result = null;
            parent.Dispatch(Arg.Do<IntegerLiteralExpression>(x => result = x.GetType()), Arg.Any<List<string>>());
            referenceHelper.VisitFunction(input);

            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void VisitFunction_RealReturnType_Correct()
        {
            System.Type expected = typeof(RealLiteralExpression);
            RealLiteralExpression realLiteralExpression = new RealLiteralExpression("2", 1, 1);
            ConditionNode conditionNode = new ConditionNode(realLiteralExpression, 2, 2);
            List<string> parameterIdentifiers = new List<string> { "a", "b" };
            TypeNode typeNode = new TypeNode(TypeEnum.Real, 1, 1);
            List<TypeNode> parameterTypes = new List<TypeNode>() { typeNode, typeNode };
            FunctionTypeNode functionType = new FunctionTypeNode(typeNode, parameterTypes, 3, 3);
            FunctionNode input = new FunctionNode("func1", conditionNode, parameterIdentifiers, functionType, 4, 4);
            IReferenceHandler parent = Substitute.For<IReferenceHandler>();
            ReferenceHelper referenceHelper = BuildHelper(parent);

            System.Type result = null;
            parent.Dispatch(Arg.Do<RealLiteralExpression>(x => result = x.GetType()), Arg.Any<List<string>>());
            referenceHelper.VisitFunction(input);

            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void VisitFunction_MultipleConditions_DispatchesCorrectExpressions()
        {
            List<Type> expected = new List<Type> { typeof(IntegerLiteralExpression), typeof(IntegerLiteralExpression) };
            ConditionNode firstCondition = new ConditionNode(new IntegerLiteralExpression("1", 1, 1), 1, 1);
            ConditionNode secondCondition = new ConditionNode(new IntegerLiteralExpression("2", 1, 1), 1, 1);
            List<ConditionNode> conditions = new List<ConditionNode> { firstCondition, secondCondition };
            TypeNode typeNode = new TypeNode(TypeEnum.Integer, 1, 1);
            List<string> parameterIdentifiers = new List<string> { "a", "b" };
            FunctionNode input = new FunctionNode(conditions, "func1", parameterIdentifiers, new FunctionTypeNode(typeNode, new List<TypeNode> { typeNode, typeNode }, 1, 1), 1, 1);
            IReferenceHandler parent = Substitute.For<IReferenceHandler>();
            ReferenceHelper referenceHelper = BuildHelper(parent);

            List<Type> result = new List<Type>();
            parent.Dispatch(Arg.Do<IntegerLiteralExpression>(x => result.Add(x.GetType())), Arg.Any<List<string>>());
            referenceHelper.VisitFunction(input);

            CollectionAssert.AreEqual(expected, result);
        }

        [TestMethod]
        public void VisitFunction_FunctionWithConditionExpression_DispatchesConditionExpression()
        {
            Type expected = typeof(BooleanLiteralExpression);
            BooleanLiteralExpression expressionNode = new BooleanLiteralExpression(true, 1, 1);
            List<ConditionNode> conditions = new List<ConditionNode> { new ConditionNode(expressionNode, new IntegerLiteralExpression("1", 1, 1), 1, 1) };
            TypeNode typeNode = new TypeNode(TypeEnum.Integer, 1, 1);
            List<string> parameterIdentifiers = new List<string> { "a", "b" };
            FunctionNode input = new FunctionNode(conditions, "func1", parameterIdentifiers, new FunctionTypeNode(typeNode, new List<TypeNode> { typeNode, typeNode }, 1, 1), 1, 1);
            IReferenceHandler parent = Substitute.For<IReferenceHandler>();
            ReferenceHelper referenceHelper = BuildHelper(parent);

            Type result = null;
            parent.Dispatch(Arg.Do<BooleanLiteralExpression>(x => result = x.GetType()), Arg.Any<List<string>>());
            referenceHelper.VisitFunction(input);

            Assert.AreEqual(expected, result);
        }

        #endregion

        #region VisitFunctionCall
        [TestMethod]
        public void VisitFunctionCall_1FunctionNode_Correct()
        {
            // Setup for BuildTables
            IntegerLiteralExpression integerLiteralExpression = new IntegerLiteralExpression("2", 1, 1);
            ConditionNode conditionNode = new ConditionNode(integerLiteralExpression, 2, 2);
            List<string> parameterIdentifiers = new List<string> { "a", "b" };
            TypeNode typeNode = new TypeNode(TypeEnum.Integer, 1, 1);
            List<TypeNode> parameterTypes = new List<TypeNode>() { typeNode, typeNode };
            FunctionTypeNode functionType = new FunctionTypeNode(typeNode, parameterTypes, 3, 3);
            FunctionNode functionNode1 = new FunctionNode("func", conditionNode, parameterIdentifiers, functionType, 17, 17);
            List<FunctionNode> functions = new List<FunctionNode>() { functionNode1 };

            // Setup for VisitFunctionCall
            IntegerLiteralExpression integerLiteralExpression1 = new IntegerLiteralExpression("1", 1, 1);
            IntegerLiteralExpression integerLiteralExpression2 = new IntegerLiteralExpression("2", 1, 1);
            List<ExpressionNode> children = new List<ExpressionNode>() { integerLiteralExpression1, integerLiteralExpression2 };
            FunctionCallExpression input1 = new FunctionCallExpression("func", children, 1, 1);
            List<string> input2 = new List<string>() { "a", "b" };


            IReferenceHandler parent = Substitute.For<IReferenceHandler>();
            ReferenceHelper referenceHelper = BuildHelper(parent);

            referenceHelper.BuildTables(functions);
            referenceHelper.VisitFunctionCall(input1, input2);

            List<int> expected = new List<int>() { 0 };
            expected.Should().BeEquivalentTo(input1.GlobalReferences);
        }

        [TestMethod]
        public void VisitFunctionCall_2FunctionNodesSameFunctionNamesAndSameParameterCount_Correct()
        {
            // Setup for BuildTables
            IntegerLiteralExpression integerLiteralExpression = new IntegerLiteralExpression("2", 1, 1);
            ConditionNode conditionNode = new ConditionNode(integerLiteralExpression, 2, 2);
            List<string> parameterIdentifiers = new List<string> { "a", "b" };
            TypeNode typeNode = new TypeNode(TypeEnum.Integer, 1, 1);
            List<TypeNode> parameterTypes = new List<TypeNode>() { typeNode, typeNode };
            FunctionTypeNode functionType = new FunctionTypeNode(typeNode, parameterTypes, 3, 3);
            FunctionNode functionNode1 = new FunctionNode("func", conditionNode, parameterIdentifiers, functionType, 17, 17);
            FunctionNode functionNode2 = new FunctionNode("func", conditionNode, parameterIdentifiers, functionType, 17, 17);
            List<FunctionNode> functions = new List<FunctionNode>() { functionNode1, functionNode2 };

            // Setup for VisitFunctionCall
            IntegerLiteralExpression integerLiteralExpression1 = new IntegerLiteralExpression("1", 1, 1);
            IntegerLiteralExpression integerLiteralExpression2 = new IntegerLiteralExpression("2", 1, 1);
            List<ExpressionNode> children = new List<ExpressionNode>() { integerLiteralExpression1, integerLiteralExpression2 };
            FunctionCallExpression input1 = new FunctionCallExpression("func", children, 1, 1);
            List<string> input2 = new List<string>() { "a", "b" };

            IReferenceHandler parent = Substitute.For<IReferenceHandler>();
            ReferenceHelper referenceHelper = BuildHelper(parent);

            referenceHelper.BuildTables(functions);
            referenceHelper.VisitFunctionCall(input1, input2);

            List<int> expected = new List<int>() { 0, 1 };
            expected.Should().BeEquivalentTo(input1.GlobalReferences);
        }

        [TestMethod]
        public void VisitFunctionCall_2FunctionNodesDifferentFunctionNamesAndSameParameterCountCheckFunc1_Correct()
        {
            // Setup for BuildTables
            IntegerLiteralExpression integerLiteralExpression = new IntegerLiteralExpression("2", 1, 1);
            ConditionNode conditionNode = new ConditionNode(integerLiteralExpression, 2, 2);
            List<string> parameterIdentifiers = new List<string> { "a", "b" };
            TypeNode typeNode = new TypeNode(TypeEnum.Integer, 1, 1);
            List<TypeNode> parameterTypes = new List<TypeNode>() { typeNode, typeNode };
            FunctionTypeNode functionType = new FunctionTypeNode(typeNode, parameterTypes, 3, 3);
            FunctionNode functionNode1 = new FunctionNode("func1", conditionNode, parameterIdentifiers, functionType, 17, 17);
            FunctionNode functionNode2 = new FunctionNode("func2", conditionNode, parameterIdentifiers, functionType, 17, 17);
            List<FunctionNode> functions = new List<FunctionNode>() { functionNode1, functionNode2 };

            // Setup for VisitFunctionCall
            IntegerLiteralExpression integerLiteralExpression1 = new IntegerLiteralExpression("1", 1, 1);
            IntegerLiteralExpression integerLiteralExpression2 = new IntegerLiteralExpression("2", 1, 1);
            List<ExpressionNode> children = new List<ExpressionNode>() { integerLiteralExpression1, integerLiteralExpression2 };
            FunctionCallExpression input1 = new FunctionCallExpression("func1", children, 1, 1);
            List<string> input2 = new List<string>() { "a", "b" };

            IReferenceHandler parent = Substitute.For<IReferenceHandler>();
            ReferenceHelper referenceHelper = BuildHelper(parent);

            referenceHelper.BuildTables(functions);
            referenceHelper.VisitFunctionCall(input1, input2);

            List<int> expected = new List<int>() { 0 };
            expected.Should().BeEquivalentTo(input1.GlobalReferences);
        }

        [TestMethod]
        public void VisitFunctionCall_2FunctionNodesDifferentFunctionNamesAndSameParameterCountCheckFunc2_Correct()
        {
            // Setup for BuildTables
            IntegerLiteralExpression integerLiteralExpression = new IntegerLiteralExpression("2", 1, 1);
            ConditionNode conditionNode = new ConditionNode(integerLiteralExpression, 2, 2);
            List<string> parameterIdentifiers = new List<string> { "a", "b" };
            TypeNode typeNode = new TypeNode(TypeEnum.Integer, 1, 1);
            List<TypeNode> parameterTypes = new List<TypeNode>() { typeNode, typeNode };
            FunctionTypeNode functionType = new FunctionTypeNode(typeNode, parameterTypes, 3, 3);
            FunctionNode functionNode1 = new FunctionNode("func1", conditionNode, parameterIdentifiers, functionType, 17, 17);
            FunctionNode functionNode2 = new FunctionNode("func2", conditionNode, parameterIdentifiers, functionType, 17, 17);
            List<FunctionNode> functions = new List<FunctionNode>() { functionNode1, functionNode2 };

            // Setup for VisitFunctionCall
            IntegerLiteralExpression integerLiteralExpression1 = new IntegerLiteralExpression("1", 1, 1);
            IntegerLiteralExpression integerLiteralExpression2 = new IntegerLiteralExpression("2", 1, 1);
            List<ExpressionNode> children = new List<ExpressionNode>() { integerLiteralExpression1, integerLiteralExpression2 };
            FunctionCallExpression input1 = new FunctionCallExpression("func2", children, 1, 1);
            List<string> input2 = new List<string>() { "a", "b" };

            IReferenceHandler parent = Substitute.For<IReferenceHandler>();
            ReferenceHelper referenceHelper = BuildHelper(parent);

            referenceHelper.BuildTables(functions);
            referenceHelper.VisitFunctionCall(input1, input2);

            List<int> expected = new List<int>() { 1 };
            expected.Should().BeEquivalentTo(input1.GlobalReferences);
        }

        [TestMethod]
        public void VisitFunctionCall_2FunctionNodesSameFunctionNamesAndDifferentParameterCountCheckFuncWithTwoParams_Correct()
        {
            // Setup for BuildTables
            IntegerLiteralExpression integerLiteralExpression = new IntegerLiteralExpression("2", 1, 1);
            ConditionNode conditionNode = new ConditionNode(integerLiteralExpression, 2, 2);
            List<string> parameterIdentifiers1 = new List<string> { "a", "b" };
            List<string> parameterIdentifiers2 = new List<string> { "a", "b", "c" };
            TypeNode typeNode = new TypeNode(TypeEnum.Integer, 1, 1);
            List<TypeNode> parameterTypes1 = new List<TypeNode>() { typeNode, typeNode };
            List<TypeNode> parameterTypes2 = new List<TypeNode>() { typeNode, typeNode, typeNode };
            FunctionTypeNode functionType1 = new FunctionTypeNode(typeNode, parameterTypes1, 3, 3);
            FunctionTypeNode functionType2 = new FunctionTypeNode(typeNode, parameterTypes2, 3, 3);
            FunctionNode functionNode1 = new FunctionNode("func", conditionNode, parameterIdentifiers1, functionType1, 17, 17);
            FunctionNode functionNode2 = new FunctionNode("func", conditionNode, parameterIdentifiers2, functionType2, 17, 17);
            List<FunctionNode> functions = new List<FunctionNode>() { functionNode1, functionNode2 };

            // Setup for VisitFunctionCall
            IntegerLiteralExpression integerLiteralExpression1 = new IntegerLiteralExpression("1", 1, 1);
            IntegerLiteralExpression integerLiteralExpression2 = new IntegerLiteralExpression("2", 1, 1);
            List<ExpressionNode> children = new List<ExpressionNode>() { integerLiteralExpression1, integerLiteralExpression2 };
            FunctionCallExpression input1 = new FunctionCallExpression("func", children, 1, 1);
            List<string> input2 = new List<string>() { "a", "b" };

            IReferenceHandler parent = Substitute.For<IReferenceHandler>();
            ReferenceHelper referenceHelper = BuildHelper(parent);

            referenceHelper.BuildTables(functions);
            referenceHelper.VisitFunctionCall(input1, input2);

            List<int> expected = new List<int>() { 0 };
            expected.Should().BeEquivalentTo(input1.GlobalReferences);
        }

        [TestMethod]
        public void VisitFunctionCall_2FunctionNodesSameFunctionNamesAndDifferentParameterCountCheckFuncWithThreeParams_Correct()
        {
            // Setup for BuildTables
            IntegerLiteralExpression integerLiteralExpression = new IntegerLiteralExpression("2", 1, 1);
            ConditionNode conditionNode = new ConditionNode(integerLiteralExpression, 2, 2);
            List<string> parameterIdentifiers1 = new List<string> { "a", "b" };
            List<string> parameterIdentifiers2 = new List<string> { "a", "b", "c" };
            TypeNode typeNode = new TypeNode(TypeEnum.Integer, 1, 1);
            List<TypeNode> parameterTypes1 = new List<TypeNode>() { typeNode, typeNode };
            List<TypeNode> parameterTypes2 = new List<TypeNode>() { typeNode, typeNode, typeNode };
            FunctionTypeNode functionType1 = new FunctionTypeNode(typeNode, parameterTypes1, 3, 3);
            FunctionTypeNode functionType2 = new FunctionTypeNode(typeNode, parameterTypes2, 3, 3);
            FunctionNode functionNode1 = new FunctionNode("func", conditionNode, parameterIdentifiers1, functionType1, 17, 17);
            FunctionNode functionNode2 = new FunctionNode("func", conditionNode, parameterIdentifiers2, functionType2, 17, 17);
            List<FunctionNode> functions = new List<FunctionNode>() { functionNode1, functionNode2 };

            // Setup for VisitFunctionCall
            IntegerLiteralExpression integerLiteralExpression1 = new IntegerLiteralExpression("1", 1, 1);
            IntegerLiteralExpression integerLiteralExpression2 = new IntegerLiteralExpression("2", 1, 1);
            IntegerLiteralExpression integerLiteralExpression3 = new IntegerLiteralExpression("3", 1, 1);
            List<ExpressionNode> children = new List<ExpressionNode>() { integerLiteralExpression1, integerLiteralExpression2, integerLiteralExpression3 };
            FunctionCallExpression input1 = new FunctionCallExpression("func", children, 1, 1);
            List<string> input2 = new List<string>() { "a", "b", "c" };

            IReferenceHandler parent = Substitute.For<IReferenceHandler>();
            ReferenceHelper referenceHelper = BuildHelper(parent);

            referenceHelper.BuildTables(functions);
            referenceHelper.VisitFunctionCall(input1, input2);

            List<int> expected = new List<int>() { 1 };
            expected.Should().BeEquivalentTo(input1.GlobalReferences);
        }

        [TestMethod]
        public void VisitFunctionCall_2FunctionNodesDifferentFunctionNamesAndDifferentParameterCount_Correct()
        {
            // Setup for BuildTables
            IntegerLiteralExpression integerLiteralExpression = new IntegerLiteralExpression("2", 1, 1);
            ConditionNode conditionNode = new ConditionNode(integerLiteralExpression, 2, 2);
            List<string> parameterIdentifiers1 = new List<string> { "a", "b" };
            List<string> parameterIdentifiers2 = new List<string> { "a", "b", "c" };
            TypeNode typeNode = new TypeNode(TypeEnum.Integer, 1, 1);
            List<TypeNode> parameterTypes1 = new List<TypeNode>() { typeNode, typeNode };
            List<TypeNode> parameterTypes2 = new List<TypeNode>() { typeNode, typeNode, typeNode };
            FunctionTypeNode functionType1 = new FunctionTypeNode(typeNode, parameterTypes1, 3, 3);
            FunctionTypeNode functionType2 = new FunctionTypeNode(typeNode, parameterTypes2, 3, 3);
            FunctionNode functionNode1 = new FunctionNode("func1", conditionNode, parameterIdentifiers1, functionType1, 17, 17);
            FunctionNode functionNode2 = new FunctionNode("func2", conditionNode, parameterIdentifiers2, functionType2, 17, 17);
            List<FunctionNode> functions = new List<FunctionNode>() { functionNode1, functionNode2 };

            // Setup for VisitFunctionCall
            IntegerLiteralExpression integerLiteralExpression1 = new IntegerLiteralExpression("1", 1, 1);
            IntegerLiteralExpression integerLiteralExpression2 = new IntegerLiteralExpression("2", 1, 1);
            List<ExpressionNode> children = new List<ExpressionNode>() { integerLiteralExpression1, integerLiteralExpression2 };
            FunctionCallExpression input1 = new FunctionCallExpression("func1", children, 1, 1);
            List<string> input2 = new List<string>() { "a", "b" };

            IReferenceHandler parent = Substitute.For<IReferenceHandler>();
            ReferenceHelper referenceHelper = BuildHelper(parent);

            referenceHelper.BuildTables(functions);
            referenceHelper.VisitFunctionCall(input1, input2);

            List<int> expected = new List<int>() { 0 };
            expected.Should().BeEquivalentTo(input1.GlobalReferences);
        }

        [TestMethod]
        public void VisitFunctionCall_2FunctionCallExpressions_GlobalReferencesListsAreSameValuesButDifferentReferences()
        {
            // Setup for BuildTables
            IntegerLiteralExpression integerLiteralExpression = new IntegerLiteralExpression("2", 1, 1);
            ConditionNode conditionNode = new ConditionNode(integerLiteralExpression, 2, 2);
            List<string> parameterIdentifiers1 = new List<string> { "a", "b" };
            List<string> parameterIdentifiers2 = new List<string> { "a", "b", "c" };
            TypeNode typeNode = new TypeNode(TypeEnum.Integer, 1, 1);
            List<TypeNode> parameterTypes1 = new List<TypeNode>() { typeNode, typeNode };
            List<TypeNode> parameterTypes2 = new List<TypeNode>() { typeNode, typeNode, typeNode };
            FunctionTypeNode functionType1 = new FunctionTypeNode(typeNode, parameterTypes1, 3, 3);
            FunctionTypeNode functionType2 = new FunctionTypeNode(typeNode, parameterTypes2, 3, 3);
            FunctionNode functionNode1 = new FunctionNode("func1", conditionNode, parameterIdentifiers1, functionType1, 17, 17);
            FunctionNode functionNode2 = new FunctionNode("func2", conditionNode, parameterIdentifiers2, functionType2, 17, 17);
            List<FunctionNode> functions = new List<FunctionNode>() { functionNode1, functionNode2 };

            // Setup for VisitFunctionCall
            IntegerLiteralExpression integerLiteralExpression1 = new IntegerLiteralExpression("1", 1, 1);
            IntegerLiteralExpression integerLiteralExpression2 = new IntegerLiteralExpression("2", 1, 1);
            List<ExpressionNode> children1 = new List<ExpressionNode>() { integerLiteralExpression1, integerLiteralExpression2 };
            FunctionCallExpression input1 = new FunctionCallExpression("func1", children1, 1, 1);
            List<string> input2 = new List<string>() { "a", "b" };

            IntegerLiteralExpression integerLiteralExpression3 = new IntegerLiteralExpression("1", 1, 1);
            IntegerLiteralExpression integerLiteralExpression4 = new IntegerLiteralExpression("2", 1, 1);
            List<ExpressionNode> children2 = new List<ExpressionNode>() { integerLiteralExpression1, integerLiteralExpression2 };
            FunctionCallExpression input3 = new FunctionCallExpression("func1", children2, 1, 1);
            List<string> input4 = new List<string>() { "a", "b" };

            IReferenceHandler parent = Substitute.For<IReferenceHandler>();
            ReferenceHelper referenceHelper = BuildHelper(parent);

            referenceHelper.BuildTables(functions);
            referenceHelper.VisitFunctionCall(input1, input2);
            referenceHelper.VisitFunctionCall(input3, input4);

            bool res = input1.GlobalReferences == input3.GlobalReferences;
            bool expected = false;

            Assert.AreEqual(expected, res);
        }

        [TestMethod]
        public void VisitFunctionCall_LocalWithTwoIntParameters_DispatchesCorrectChildren()
        {
            IntegerLiteralExpression expr1 = new IntegerLiteralExpression("1", 1, 1);
            IntegerLiteralExpression expr2 = new IntegerLiteralExpression("2", 1, 1);
            FunctionCallExpression funcCallExpr = new FunctionCallExpression("func1", new List<ExpressionNode> { expr1, expr2 }, 1, 1);
            IReferenceHandler parent = Substitute.For<IReferenceHandler>();
            ReferenceHelper referenceHelper = BuildHelper(parent);
            referenceHelper.BuildTables(new List<FunctionNode>());
            List<ExpressionNode> result = new List<ExpressionNode>();
            parent.Dispatch(Arg.Do<ExpressionNode>(x => result.Add(x)), Arg.Any<List<string>>());

            referenceHelper.VisitFunctionCall(funcCallExpr, new List<string> { "func1" });

            CollectionAssert.AreEqual(new List<ExpressionNode> { expr1, expr2 }, result);
        }

        [TestMethod]
        public void VisitFunctionCall_ReferenceIsGlobal_CorrectLocalReference()
        {
            IntegerLiteralExpression expr1 = new IntegerLiteralExpression("1", 1, 1);
            IntegerLiteralExpression expr2 = new IntegerLiteralExpression("2", 1, 1);
            FunctionCallExpression funcCallExpr = new FunctionCallExpression("func1", new List<ExpressionNode> { expr1, expr2 }, 1, 1);
            IReferenceHandler parent = Substitute.For<IReferenceHandler>();
            ReferenceHelper referenceHelper = BuildHelper(parent);
            TypeNode typeNode = new TypeNode(TypeEnum.Integer, 1, 1);
            FunctionTypeNode funcTypeNode = new FunctionTypeNode(typeNode, new List<TypeNode> { typeNode, typeNode }, 1, 1);
            FunctionNode func = new FunctionNode("func1", null, new List<string> { "param1", "param2" }, funcTypeNode, 1, 1);
            List<FunctionNode> funcs = new List<FunctionNode> { func };
            referenceHelper.BuildTables(funcs);

            referenceHelper.VisitFunctionCall(funcCallExpr, new List<string>());

            Assert.AreEqual(-1, funcCallExpr.LocalReference);
        }

        [TestMethod]
        public void VisitFunctionCall_ReferenceIsGlobal_CorrectGlobalReferences()
        {
            IntegerLiteralExpression expr1 = new IntegerLiteralExpression("1", 1, 1);
            IntegerLiteralExpression expr2 = new IntegerLiteralExpression("2", 1, 1);
            FunctionCallExpression funcCallExpr = new FunctionCallExpression("func1", new List<ExpressionNode> { expr1, expr2 }, 1, 1);
            IReferenceHandler parent = Substitute.For<IReferenceHandler>();
            ReferenceHelper referenceHelper = BuildHelper(parent);
            TypeNode typeNode = new TypeNode(TypeEnum.Integer, 1, 1);
            FunctionTypeNode funcTypeNode = new FunctionTypeNode(typeNode, new List<TypeNode> { typeNode, typeNode }, 1, 1);
            FunctionNode func = new FunctionNode("func1", null, new List<string> { "param1", "param2" }, funcTypeNode, 1, 1);
            List<FunctionNode> funcs = new List<FunctionNode> { func };
            referenceHelper.BuildTables(funcs);

            referenceHelper.VisitFunctionCall(funcCallExpr, new List<string>());

            CollectionAssert.AreEqual(new List<int> { 0 }, funcCallExpr.GlobalReferences);
        }

        [TestMethod]
        public void VisitFunctionCall_ReferenceIsLocal_CorrectLocalReference()
        {
            IntegerLiteralExpression expr1 = new IntegerLiteralExpression("1", 1, 1);
            IntegerLiteralExpression expr2 = new IntegerLiteralExpression("2", 1, 1);
            FunctionCallExpression funcCallExpr = new FunctionCallExpression("func1", new List<ExpressionNode> { expr1, expr2 }, 1, 1);
            IReferenceHandler parent = Substitute.For<IReferenceHandler>();
            ReferenceHelper referenceHelper = BuildHelper(parent);
            TypeNode typeNode = new TypeNode(TypeEnum.Integer, 1, 1);
            FunctionTypeNode funcTypeNode = new FunctionTypeNode(typeNode, new List<TypeNode> { typeNode, typeNode }, 1, 1);
            FunctionNode func = new FunctionNode("func2", null, new List<string> { "param1", "param2" }, funcTypeNode, 1, 1);
            List<FunctionNode> funcs = new List<FunctionNode> { func };
            referenceHelper.BuildTables(funcs);

            referenceHelper.VisitFunctionCall(funcCallExpr, new List<string> { "func1" });

            Assert.AreEqual(0, funcCallExpr.LocalReference);
        }

        [TestMethod]
        public void VisitFunctionCall_ReferenceIsLocal_CorrectGlobalReference()
        {
            IntegerLiteralExpression expr1 = new IntegerLiteralExpression("1", 1, 1);
            IntegerLiteralExpression expr2 = new IntegerLiteralExpression("2", 1, 1);
            FunctionCallExpression funcCallExpr = new FunctionCallExpression("func1", new List<ExpressionNode> { expr1, expr2 }, 1, 1);
            IReferenceHandler parent = Substitute.For<IReferenceHandler>();
            ReferenceHelper referenceHelper = BuildHelper(parent);
            TypeNode typeNode = new TypeNode(TypeEnum.Integer, 1, 1);
            FunctionTypeNode funcTypeNode = new FunctionTypeNode(typeNode, new List<TypeNode> { typeNode, typeNode }, 1, 1);
            FunctionNode func = new FunctionNode("func2", null, new List<string> { "param1", "param2" }, funcTypeNode, 1, 1);
            List<FunctionNode> funcs = new List<FunctionNode> { func };
            referenceHelper.BuildTables(funcs);

            referenceHelper.VisitFunctionCall(funcCallExpr, new List<string> { "func1" });

            Assert.AreEqual(0, funcCallExpr.GlobalReferences.Count);
        }

        #endregion

        #region VisitIdentifier
        [TestMethod]
        public void VisitIdentifier_IdentifierPresentInList_Correct()
        {
            // Setup for VisitIdentifier
            IdentifierExpression input1 = new IdentifierExpression("b", 1, 1);
            List<string> input2 = new List<string>() { "a", "b", "c" };
            IReferenceHandler parent = Substitute.For<IReferenceHandler>();
            ReferenceHelper referenceHelper = BuildHelper(parent);

            referenceHelper.VisitIdentifier(input1, input2);

            int expected = 1;
            Assert.AreEqual(expected, input1.Reference);
        }

        [TestMethod]
        public void VisitIdentifier_TwoOfSameIdentifier_Correct()
        {
            // Setup for BuildTables
            IntegerLiteralExpression integerLiteralExpression = new IntegerLiteralExpression("2", 1, 1);
            ConditionNode conditionNode = new ConditionNode(integerLiteralExpression, 2, 2);
            List<string> parameterIdentifiers1 = new List<string> { "a", "b" };
            List<string> parameterIdentifiers2 = new List<string> { "a", "b", "c" };
            TypeNode typeNode = new TypeNode(TypeEnum.Integer, 1, 1);
            List<TypeNode> parameterTypes1 = new List<TypeNode>() { typeNode, typeNode };
            List<TypeNode> parameterTypes2 = new List<TypeNode>() { typeNode, typeNode, typeNode };
            FunctionTypeNode functionType1 = new FunctionTypeNode(typeNode, parameterTypes1, 3, 3);
            FunctionTypeNode functionType2 = new FunctionTypeNode(typeNode, parameterTypes2, 3, 3);
            FunctionNode functionNode1 = new FunctionNode("func", conditionNode, parameterIdentifiers1, functionType1, 17, 17);
            FunctionNode functionNode2 = new FunctionNode("func", conditionNode, parameterIdentifiers2, functionType2, 17, 17);
            List<FunctionNode> functions = new List<FunctionNode>() { functionNode1, functionNode2 };

            // Setup for VisitIdentifier
            IdentifierExpression input1 = new IdentifierExpression("func", 1, 1);
            List<string> input2 = new List<string>() { "a", "b", "c", "func" };
            IReferenceHandler parent = Substitute.For<IReferenceHandler>();
            ReferenceHelper referenceHelper = BuildHelper(parent);

            referenceHelper.BuildTables(functions);
            referenceHelper.VisitIdentifier(input1, input2);
        }

        [TestMethod]
        [ExpectedException(typeof(OverloadedFunctionIdentifierException), "Not a valid identifier.")]
        public void VisitIdentifier_OverloadedFunctionIdentifier_Correct()
        {
            // Setup for BuildTables
            IntegerLiteralExpression integerLiteralExpression = new IntegerLiteralExpression("2", 1, 1);
            ConditionNode conditionNode = new ConditionNode(integerLiteralExpression, 2, 2);
            List<string> parameterIdentifiers1 = new List<string> { "a", "b" };
            List<string> parameterIdentifiers2 = new List<string> { "a", "b" };
            TypeNode typeNode = new TypeNode(TypeEnum.Integer, 1, 1);
            List<TypeNode> parameterTypes1 = new List<TypeNode>() { typeNode, typeNode };
            List<TypeNode> parameterTypes2 = new List<TypeNode>() { typeNode, typeNode };
            FunctionTypeNode functionType1 = new FunctionTypeNode(typeNode, parameterTypes1, 3, 3);
            FunctionTypeNode functionType2 = new FunctionTypeNode(typeNode, parameterTypes2, 3, 3);
            FunctionNode functionNode1 = new FunctionNode("func", conditionNode, parameterIdentifiers1, functionType1, 17, 17);
            FunctionNode functionNode2 = new FunctionNode("func", conditionNode, parameterIdentifiers2, functionType2, 17, 17);
            List<FunctionNode> functions = new List<FunctionNode>() { functionNode1, functionNode2 };

            // Setup for VisitIdentifier
            IdentifierExpression input1 = new IdentifierExpression("func", 1, 1);
            List<string> input2 = new List<string>() { "a", "b", "c", "func1", "func1" };
            IReferenceHandler parent = Substitute.For<IReferenceHandler>();
            ReferenceHelper referenceHelper = BuildHelper(parent);

            referenceHelper.BuildTables(functions);
            referenceHelper.VisitIdentifier(input1, input2);
        }

        #endregion

        #region VisitSet
        // TODO:
        //  Indecies names correct
        //  Predicate is discpatced with more parameters 


        [DataRow(new string[] { "a", "b" }, "a")]
        [DataRow(new string[] { "abc", "b" }, "b")]
        [DataRow(new string[] { "abc", "error", "b" }, "error")]
        [TestMethod]
        [ExpectedException(typeof(IdenticalParameterIdentifiersException))]
        public void VisitSet_ElementHideIdentifier_Exception(string[] parameters, string identifier)
        {
            var indicies = new List<string>();
            var element = GetElementNode(identifier, indicies);
            var bounds = new List<BoundNode>();
            var predicate = GetIntLit();
            var input = GetSetExpr(element, bounds, predicate);

            var parent = GetReferenceHandler();
            var referenceHelper = BuildHelper(parent);

            referenceHelper.VisitSet(input, parameters.ToList());
        }

        [DataRow(new string[] { "a", "b" }, "a")]
        [DataRow(new string[] { "abc", "b" }, "b")]
        [DataRow(new string[] { "abc", "error", "b" }, "error")]
        [TestMethod]
        [ExpectedException(typeof(IdenticalParameterIdentifiersException))]
        public void VisitSet_IndexHideIdentifier_Exception(string[] parameters, string identifier)
        {
            var indicies = GetStringList(identifier);

            var element = GetElementNode("this string should not be included in parameters", indicies);
            var bounds = new List<BoundNode>();
            var predicate = GetIntLit();
            var input = GetSetExpr(element, bounds, predicate);

            var parent = GetReferenceHandler();
            var referenceHelper = BuildHelper(parent);

            referenceHelper.VisitSet(input, parameters.ToList());
        }

        [DataRow(new string[] { "i", "j" }, 1)]
        [DataRow(new string[] { "i", "j", "k" }, 2)]
        [TestMethod]
        [ExpectedException(typeof(BoundException))]
        public void VisitSet_XBoundsYIndicies_Exception(string[] boundIds, int indexCount)
        {
            var indicies = GetStringList(boundIds.Take(indexCount));
            var bounds = GetBounds(boundIds);
            var element = GetElementNode("this string should not be included in parameters", indicies);
            var predicate = GetIntLit();
            var input = GetSetExpr(element, bounds, predicate);

            var parent = GetReferenceHandler();
            var referenceHelper = BuildHelper(parent);

            referenceHelper.VisitSet(input, GetStringList());
        }

        [DataRow(new string[] { "i", "j" }, 1)]
        [DataRow(new string[] { "i", "j", "k" }, 2)]
        [TestMethod]
        [ExpectedException(typeof(BoundException))]
        public void VisitSet_YBoundsXIndicies_Exception(string[] boundIds, int boundCount)
        {
            var indicies = GetStringList(boundIds);
            var bounds = GetBounds(boundIds.Take(boundCount));
            var element = GetElementNode("this string should not be included in parameters", indicies);
            var predicate = GetIntLit();
            var input = GetSetExpr(element, bounds, predicate);

            var parent = GetReferenceHandler();
            var referenceHelper = BuildHelper(parent);

            referenceHelper.VisitSet(input, GetStringList());
        }

        [DataRow(new string[] { "i", "j" })]
        [DataRow(new string[] { "i", "j", "k" })]
        [TestMethod]
        public void VisitSet_XBoundsXIndicies_DispatchBoundMin(string[] ids)
        {
            int expected = ids.Length;
            int res = 0;
            var parameters = GetStringList();

            var indicies = GetStringList(ids);
            var bounds = GetBounds(ids);
            var element = GetElementNode("this string should not be included in parameters", indicies);
            var predicate = GetIntLit();
            var input = GetSetExpr(element, bounds, predicate);

            var parent = GetReferenceHandler();
            AddDispache(bounds.ConvertAll(x => x.MinValue), () => res++, parent);
            var referenceHelper = BuildHelper(parent);

            referenceHelper.VisitSet(input, parameters);

            Assert.AreEqual(expected, res);
        }

        [DataRow(new string[] { "i", "j" })]
        [DataRow(new string[] { "i", "j", "k" })]
        [TestMethod]
        public void VisitSet_XBoundsXIndicies_DispatchBoundMax(string[] ids)
        {
            int expected = ids.Length;
            int res = 0;
            var parameters = GetStringList();

            var indicies = GetStringList(ids);
            var bounds = GetBounds(ids);
            var element = GetElementNode("this string should not be included in parameters", indicies);
            var predicate = GetIntLit();
            var input = GetSetExpr(element, bounds, predicate);

            var parent = GetReferenceHandler();
            AddDispache(bounds.ConvertAll(x => x.MaxValue), () => res++, parent);
            var referenceHelper = BuildHelper(parent);

            referenceHelper.VisitSet(input, parameters);

            Assert.AreEqual(expected, res);
        }

        [TestMethod]
        public void VisitSet_Predicate_DispatchPredicate()
        {
            var ids = new string[] { "i", "j" };
            int expected = 1;
            int res = 0;
            var parameters = GetStringList();

            var indicies = GetStringList(ids);
            var bounds = GetBounds(ids);
            var element = GetElementNode("this string should not be included in parameters", indicies);
            var predicate = GetIntLit();
            var input = GetSetExpr(element, bounds, predicate);

            var parent = GetReferenceHandler();
            AddDispache(predicate, () => res++, parent);
            var referenceHelper = BuildHelper(parent);

            referenceHelper.VisitSet(input, parameters);

            Assert.AreEqual(expected, res);
        }

        private void AddDispache(List<ExpressionNode> expressions, Func<int> func, IReferenceHandler parent)
        {
            foreach (var expr in expressions)
            {
                parent.Dispatch(expr, Arg.Do<List<string>>(x => func()));
            }
        }

        private void AddDispache(ExpressionNode expression, Func<int> func, IReferenceHandler parent)
        {
            parent.Dispatch(expression, Arg.Do<List<string>>(x => func()));
        }

        private List<string> GetStringList()
        {
            return new List<string>();
        }

        private List<BoundNode> GetBounds(IEnumerable<string> parameters)
        {
            var res = new List<BoundNode>();
            foreach (var id in parameters)
                res.Add(GetBoundnode(id));
            return res;
        }

        private BoundNode GetBoundnode(string id)
        {
            return new BoundNode(id, GetIntLit(0), GetIntLit(1), 0, 0);
        }

        private List<string> GetStringList(IEnumerable<string> enumerable)
        {
            return enumerable.ToList();
        }

        private List<string> GetStringList(string identifier)
        {
            return new List<string>() { identifier };
        }

        private IReferenceHandler GetReferenceHandler()
        {
            return Substitute.For<IReferenceHandler>();
        }

        private SetExpression GetSetExpr(ElementNode element, List<BoundNode> bounds, IntegerLiteralExpression predicate)
        {
            return new SetExpression(element, bounds, predicate, 0, 0);
        }
        #endregion

        private ElementNode GetElementNode(string identifier, List<string> indicies)
        {
            return new ElementNode(identifier, indicies, 0, 0);
        }

        private IntegerLiteralExpression GetIntLit()
        {
            return new IntegerLiteralExpression(0, 0, 0);
        }

        private IntegerLiteralExpression GetIntLit(int n)
        {
            return new IntegerLiteralExpression(n, 0, 0);
        }

        
        #region VisitCondition

        [ExpectedException(typeof(DuplicateElementIndexException))]
        [TestMethod]
        public void VisitCondition_MultipleElements_CausesDuplicateElementIndexException()
        {
	        string elementName = "e";
	        List<string> indexIdentifiers1 = new List<string>() {"i", "j"};
	        ElementNode elementNode1 = Utilities.GetElementNode(elementName, indexIdentifiers1);
	        
	        List<string> indexIdentifiers2 = new List<string>() {"i", "g"};
	        ElementNode elementNode2 = Utilities.GetElementNode(elementName, indexIdentifiers2);
	        
	        List<ElementNode> elementNodes = new List<ElementNode>() {elementNode1, elementNode2};
	        ConditionNode conditionNode = Utilities.GetConditionNodeOnlyWith(elementNodes);

	        ReferenceHelper referenceHelper = new ReferenceHelper();
	        
	        referenceHelper.VisitCondition(conditionNode, new List<string>());
        }
        
        
        // 2. Set element reference
        
        // OneParameterOneElement
        [TestMethod]
        public void VisitCondition_OneParameterOneElement_ExpectElementHasCorrectReferenceToParamList()
        {
	        List<string> indexIdentifiers = new List<string>() {"i", "j"};
	        ElementNode elementNode = Utilities.GetElementNode("e", indexIdentifiers); 
	        List<ElementNode> elementNodes = new List<ElementNode>() {elementNode};
	        ConditionNode conditionNode = Utilities.GetConditionNodeOnlyWith(elementNodes);

	        int expectedReference = 0;

            IReferenceHandler parent = Substitute.For<IReferenceHandler>();
            ReferenceHelper referenceHelper = BuildHelper(parent);

	        List<string> parameterIdentifiers = new List<string>() {"e"};
	        referenceHelper.VisitCondition(conditionNode, parameterIdentifiers);
	        
	        Assert.AreEqual(expectedReference, conditionNode.Elements[0].Reference);
        }
        
        // TwoParametersOneElement
        [TestMethod]
        public void VisitCondition_TwoParametersOneElement_ExpectElementHasCorrectReferenceToParamList()
        {
	        List<string> indexIdentifiers = new List<string>() {"i", "j"};
	        ElementNode elementNode = Utilities.GetElementNode("e", indexIdentifiers); 
	        List<ElementNode> elementNodes = new List<ElementNode>() {elementNode};
	        ConditionNode conditionNode = Utilities.GetConditionNodeOnlyWith(elementNodes);

	        int expectedReference = 1;

            IReferenceHandler parent = Substitute.For<IReferenceHandler>();
            ReferenceHelper referenceHelper = BuildHelper(parent);

	        List<string> parameterIdentifiers = new List<string>() {"v", "e"};
	        referenceHelper.VisitCondition(conditionNode, parameterIdentifiers);
	        
	        Assert.AreEqual(expectedReference, conditionNode.Elements[0].Reference);
        }
        
        // ElementId does not appear in list of parameterIdentifiers 
        [ExpectedException(typeof(NoMatchingIdentifierFoundException))]
        [TestMethod]
        public void VisitCondition_NoMatching_CausesNoMatchingIdentifierFoundException()
        {
	        List<string> indexIdentifiers = new List<string>() {"i", "j"};
	        ElementNode elementNode = Utilities.GetElementNode("e", indexIdentifiers); 
	        List<ElementNode> elementNodes = new List<ElementNode>() {elementNode};
	        ConditionNode conditionNode = Utilities.GetConditionNodeOnlyWith(elementNodes);

            IReferenceHandler parent = Substitute.For<IReferenceHandler>();
            ReferenceHelper referenceHelper = BuildHelper(parent);

	        List<string> parameterIdentifiers = new List<string>() {"a", "b", "c"};
	        referenceHelper.VisitCondition(conditionNode, parameterIdentifiers);
        }
        
        [TestMethod]
        public void VisitCondition__EnsureIndexIdentifiersAreAddedBeforeDispatchCondition()
        {
            List<string> indexIdentifiers = new List<string>() {"i", "j"};
            ElementNode elementNode = Utilities.GetElementNode("e", indexIdentifiers); 
            List<ElementNode> elementNodes = new List<ElementNode>() {elementNode};
            GreaterExpression greaterExpression = GetGreaterExpression();
            ConditionNode conditionNode = new ConditionNode(elementNodes, greaterExpression, null, 0, 0);

            IReferenceHandler parent = Substitute.For<IReferenceHandler>();
            List<string> res = new List<string>();
            parent.Dispatch(greaterExpression, Arg.Do<List<string>>(x => res = x));
            ReferenceHelper referenceHelper = BuildHelper(parent);

            List<string> parameterIdentifiers = new List<string>() {"e"};
            List<string> expectedIdentiferList = parameterIdentifiers.ToList();
            expectedIdentiferList.AddRange(indexIdentifiers);
            referenceHelper.VisitCondition(conditionNode, parameterIdentifiers);

            res.Should().BeEquivalentTo(expectedIdentiferList);
        }

        [TestMethod]
        public void VisitCondition__EnsureIndexIdentifiersAreAddedBeforeDispatchReturnExpression()
        {
            List<string> indexIdentifiers = new List<string>() {"i", "j"};
            ElementNode elementNode = Utilities.GetElementNode("e", indexIdentifiers); 
            List<ElementNode> elementNodes = new List<ElementNode>() {elementNode};
            GreaterExpression greaterExpression = GetGreaterExpression();
            ConditionNode conditionNode = new ConditionNode(elementNodes, null, greaterExpression, 0, 0);

            IReferenceHandler parent = Substitute.For<IReferenceHandler>();
            List<string> res = new List<string>();
            parent.Dispatch(greaterExpression, Arg.Do<List<string>>(x => res = x));
            ReferenceHelper referenceHelper = BuildHelper(parent);

            List<string> parameterIdentifiers = new List<string>() {"e"};
            List<string> expectedIdentiferList = parameterIdentifiers.ToList();
            expectedIdentiferList.AddRange(indexIdentifiers);
            referenceHelper.VisitCondition(conditionNode, parameterIdentifiers);

            res.Should().BeEquivalentTo(expectedIdentiferList);
        }

        [TestMethod]
        public void VisitCondition__EnsureIndexIdentifiersAreRemoved()
        {
            List<string> indexIdentifiers = new List<string>() {"i", "j"};
            ElementNode elementNode = Utilities.GetElementNode("e", indexIdentifiers); 
            List<ElementNode> elementNodes = new List<ElementNode>() {elementNode};
            GreaterExpression greaterExpression = GetGreaterExpression();
            ConditionNode conditionNode = new ConditionNode(elementNodes, null, greaterExpression, 0, 0);

            IReferenceHandler parent = Substitute.For<IReferenceHandler>();
            ReferenceHelper referenceHelper = BuildHelper(parent);
            
            List<string> parameterIdentifiers = new List<string>() {"e"};
            List<string> expectedIdentiferList = parameterIdentifiers.ToList(); // A copy!
            referenceHelper.VisitCondition(conditionNode, parameterIdentifiers);

            parameterIdentifiers.Should().BeEquivalentTo(expectedIdentiferList);
        }

        private GreaterExpression GetGreaterExpression()
        {
            ExpressionNode lhs = new IntegerLiteralExpression("1", 0, 0);
            ExpressionNode rhs = new IntegerLiteralExpression("3", 0, 0);
            return new GreaterExpression(lhs, rhs, 0, 0);
        }
        
        #endregion
    }


}
