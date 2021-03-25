using System;
using System.Collections.Generic;
using System.Text;
using ASTLib;
using ASTLib.Interfaces;
using ASTLib.Nodes;
using ASTLib.Nodes.ExpressionNodes;
using ASTLib.Nodes.TypeNodes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using FluentAssertions;

namespace ReferenceHandlerLib.Tests
{
    [TestClass]
    public class ReferenceHelperTests
    {
        #region VisitExport
        [TestMethod]
        public void VisitExport_IntegerLiteralExpression_Correct()
        {
            System.Type expected = typeof(IntegerLiteralExpression);
            IntegerLiteralExpression integerLiteralExpression = new IntegerLiteralExpression("2",1,1);
            ExportNode input = new ExportNode(integerLiteralExpression,3,3);
            IReferenceHandler parent = Substitute.For<IReferenceHandler>();
            ReferenceHelper referenceHelper = new ReferenceHelper() { ReferenceHandler = parent };

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
            ReferenceHelper referenceHelper = new ReferenceHelper() { ReferenceHandler = parent };

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
            ConditionNode conditionNode = new ConditionNode(integerLiteralExpression, 2,2);
            List<string> parameterIdentifiers = new List<string> { "a", "b" };
            TypeNode typeNode = new TypeNode(TypeEnum.Integer, 1,1);
            List<TypeNode> parameterTypes = new List<TypeNode>() { typeNode, typeNode };
            FunctionTypeNode functionType = new FunctionTypeNode(typeNode, parameterTypes, 3,3);
            FunctionNode input = new FunctionNode("func1", 1, conditionNode, parameterIdentifiers, functionType, 4,4);
            IReferenceHandler parent = Substitute.For<IReferenceHandler>();
            ReferenceHelper referenceHelper = new ReferenceHelper() { ReferenceHandler = parent };

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
            FunctionNode input = new FunctionNode("func1", 1, conditionNode, parameterIdentifiers, functionType, 4, 4);
            IReferenceHandler parent = Substitute.For<IReferenceHandler>();
            ReferenceHelper referenceHelper = new ReferenceHelper() { ReferenceHandler = parent };

            System.Type result = null;
            parent.Dispatch(Arg.Do<RealLiteralExpression>(x => result = x.GetType()), Arg.Any<List<string>>());
            referenceHelper.VisitFunction(input);

            Assert.AreEqual(expected, result);
        }
        #endregion

        #region BuildTable
        [TestMethod]
        public void VisitFunctionCall_1FunctionNode_Correct()
        {
            // Setup for BuildTable
            IntegerLiteralExpression integerLiteralExpression = new IntegerLiteralExpression("2", 1, 1);
            ConditionNode conditionNode = new ConditionNode(integerLiteralExpression, 2, 2);
            List<string> parameterIdentifiers = new List<string> { "a", "b" };
            TypeNode typeNode = new TypeNode(TypeEnum.Integer, 1, 1);
            List<TypeNode> parameterTypes = new List<TypeNode>() { typeNode, typeNode };
            FunctionTypeNode functionType = new FunctionTypeNode(typeNode, parameterTypes, 3, 3);
            FunctionNode functionNode1 = new FunctionNode("func", 1, conditionNode, parameterIdentifiers, functionType, 17, 17);
            List<FunctionNode> functions = new List<FunctionNode>() { functionNode1 };

            // Setup for VisitFunctionCall
            IntegerLiteralExpression integerLiteralExpression1 = new IntegerLiteralExpression("1", 1, 1);
            IntegerLiteralExpression integerLiteralExpression2 = new IntegerLiteralExpression("2", 1, 1);
            List<ExpressionNode> children = new List<ExpressionNode>() { integerLiteralExpression1, integerLiteralExpression2 };
            FunctionCallExpression input1 = new FunctionCallExpression("func", children, 1, 1);
            List<string> input2 = new List<string>() { "a", "b" };

            IReferenceHandler parent = Substitute.For<IReferenceHandler>();
            ReferenceHelper referenceHelper = new ReferenceHelper() { ReferenceHandler = parent };

            referenceHelper.BuildTables(functions);
            referenceHelper.VisitFunctionCall(input1, input2);

            List<int> expected = new List<int>() { 0 };
            expected.Should().BeEquivalentTo(input1.GlobalReferences);
        }

        [TestMethod]
        public void VisitFunctionCall_2FunctionNodesSameFunctionNamesAndSameParameterCount_Correct()
        {
            // Setup for BuildTable
            IntegerLiteralExpression integerLiteralExpression = new IntegerLiteralExpression("2", 1, 1);
            ConditionNode conditionNode = new ConditionNode(integerLiteralExpression, 2, 2);
            List<string> parameterIdentifiers = new List<string> { "a", "b" };
            TypeNode typeNode = new TypeNode(TypeEnum.Integer, 1, 1);
            List<TypeNode> parameterTypes = new List<TypeNode>() { typeNode, typeNode };
            FunctionTypeNode functionType = new FunctionTypeNode(typeNode, parameterTypes, 3, 3);
            FunctionNode functionNode1 = new FunctionNode("func", 1, conditionNode, parameterIdentifiers, functionType, 17, 17);
            FunctionNode functionNode2 = new FunctionNode("func", 1, conditionNode, parameterIdentifiers, functionType, 17, 17);
            List<FunctionNode> functions = new List<FunctionNode>() { functionNode1, functionNode2 };

            // Setup for VisitFunctionCall
            IntegerLiteralExpression integerLiteralExpression1 = new IntegerLiteralExpression("1", 1, 1);
            IntegerLiteralExpression integerLiteralExpression2 = new IntegerLiteralExpression("2", 1, 1);
            List<ExpressionNode> children = new List<ExpressionNode>() { integerLiteralExpression1, integerLiteralExpression2 };
            FunctionCallExpression input1 = new FunctionCallExpression("func", children, 1, 1);
            List<string> input2 = new List<string>() { "a", "b" };

            IReferenceHandler parent = Substitute.For<IReferenceHandler>();
            ReferenceHelper referenceHelper = new ReferenceHelper() { ReferenceHandler = parent };

            referenceHelper.BuildTables(functions);
            referenceHelper.VisitFunctionCall(input1, input2);

            List<int> expected = new List<int>() { 0 };
            expected.Should().BeEquivalentTo(input1.GlobalReferences);
        }

        [TestMethod]
        public void VisitFunctionCall_2FunctionNodesDifferentFunctionNamesAndSameParameterCountCheckFunc1_Correct()
        {
            // Setup for BuildTable
            IntegerLiteralExpression integerLiteralExpression = new IntegerLiteralExpression("2", 1, 1);
            ConditionNode conditionNode = new ConditionNode(integerLiteralExpression, 2, 2);
            List<string> parameterIdentifiers = new List<string> { "a", "b" };
            TypeNode typeNode = new TypeNode(TypeEnum.Integer, 1, 1);
            List<TypeNode> parameterTypes = new List<TypeNode>() { typeNode, typeNode };
            FunctionTypeNode functionType = new FunctionTypeNode(typeNode, parameterTypes, 3, 3);
            FunctionNode functionNode1 = new FunctionNode("func1", 1, conditionNode, parameterIdentifiers, functionType, 17, 17);
            FunctionNode functionNode2 = new FunctionNode("func2", 1, conditionNode, parameterIdentifiers, functionType, 17, 17);
            List<FunctionNode> functions = new List<FunctionNode>() { functionNode1, functionNode2 };

            // Setup for VisitFunctionCall
            IntegerLiteralExpression integerLiteralExpression1 = new IntegerLiteralExpression("1", 1, 1);
            IntegerLiteralExpression integerLiteralExpression2 = new IntegerLiteralExpression("2", 1, 1);
            List<ExpressionNode> children = new List<ExpressionNode>() { integerLiteralExpression1, integerLiteralExpression2 };
            FunctionCallExpression input1 = new FunctionCallExpression("func1", children, 1, 1);
            List<string> input2 = new List<string>() { "a", "b" };

            IReferenceHandler parent = Substitute.For<IReferenceHandler>();
            ReferenceHelper referenceHelper = new ReferenceHelper() { ReferenceHandler = parent };

            referenceHelper.BuildTables(functions);
            referenceHelper.VisitFunctionCall(input1, input2);

            List<int> expected = new List<int>() { 0 };
            expected.Should().BeEquivalentTo(input1.GlobalReferences);
        }

        [TestMethod]
        public void VisitFunctionCall_2FunctionNodesDifferentFunctionNamesAndSameParameterCountCheckFunc2_Correct()
        {
            // Setup for BuildTable
            IntegerLiteralExpression integerLiteralExpression = new IntegerLiteralExpression("2", 1, 1);
            ConditionNode conditionNode = new ConditionNode(integerLiteralExpression, 2, 2);
            List<string> parameterIdentifiers = new List<string> { "a", "b" };
            TypeNode typeNode = new TypeNode(TypeEnum.Integer, 1, 1);
            List<TypeNode> parameterTypes = new List<TypeNode>() { typeNode, typeNode };
            FunctionTypeNode functionType = new FunctionTypeNode(typeNode, parameterTypes, 3, 3);
            FunctionNode functionNode1 = new FunctionNode("func1", 1, conditionNode, parameterIdentifiers, functionType, 17, 17);
            FunctionNode functionNode2 = new FunctionNode("func2", 1, conditionNode, parameterIdentifiers, functionType, 17, 17);
            List<FunctionNode> functions = new List<FunctionNode>() { functionNode1, functionNode2 };

            // Setup for VisitFunctionCall
            IntegerLiteralExpression integerLiteralExpression1 = new IntegerLiteralExpression("1", 1, 1);
            IntegerLiteralExpression integerLiteralExpression2 = new IntegerLiteralExpression("2", 1, 1);
            List<ExpressionNode> children = new List<ExpressionNode>() { integerLiteralExpression1, integerLiteralExpression2 };
            FunctionCallExpression input1 = new FunctionCallExpression("func2", children, 1, 1);
            List<string> input2 = new List<string>() { "a", "b" };

            IReferenceHandler parent = Substitute.For<IReferenceHandler>();
            ReferenceHelper referenceHelper = new ReferenceHelper() { ReferenceHandler = parent };

            referenceHelper.BuildTables(functions);
            referenceHelper.VisitFunctionCall(input1, input2);

            List<int> expected = new List<int>() { 1 };
            expected.Should().BeEquivalentTo(input1.GlobalReferences);
        }

        [TestMethod]
        public void VisitFunctionCall_2FunctionNodesSameFunctionNamesAndDifferentParameterCountCheckFuncWithTwoParams_Correct()
        {
            // Setup for BuildTable
            IntegerLiteralExpression integerLiteralExpression = new IntegerLiteralExpression("2", 1, 1);
            ConditionNode conditionNode = new ConditionNode(integerLiteralExpression, 2, 2);
            List<string> parameterIdentifiers1 = new List<string> { "a", "b" };
            List<string> parameterIdentifiers2 = new List<string> { "a", "b", "c" };
            TypeNode typeNode = new TypeNode(TypeEnum.Integer, 1, 1);
            List<TypeNode> parameterTypes1 = new List<TypeNode>() { typeNode, typeNode };
            List<TypeNode> parameterTypes2 = new List<TypeNode>() { typeNode, typeNode, typeNode };
            FunctionTypeNode functionType1 = new FunctionTypeNode(typeNode, parameterTypes1, 3, 3);
            FunctionTypeNode functionType2 = new FunctionTypeNode(typeNode, parameterTypes2, 3, 3);
            FunctionNode functionNode1 = new FunctionNode("func", 1, conditionNode, parameterIdentifiers1, functionType1, 17, 17);
            FunctionNode functionNode2 = new FunctionNode("func", 1, conditionNode, parameterIdentifiers2, functionType2, 17, 17);
            List<FunctionNode> functions = new List<FunctionNode>() { functionNode1, functionNode2 };

            // Setup for VisitFunctionCall
            IntegerLiteralExpression integerLiteralExpression1 = new IntegerLiteralExpression("1", 1, 1);
            IntegerLiteralExpression integerLiteralExpression2 = new IntegerLiteralExpression("2", 1, 1);
            List<ExpressionNode> children = new List<ExpressionNode>() { integerLiteralExpression1, integerLiteralExpression2 };
            FunctionCallExpression input1 = new FunctionCallExpression("func", children, 1, 1);
            List<string> input2 = new List<string>() { "a", "b" };

            IReferenceHandler parent = Substitute.For<IReferenceHandler>();
            ReferenceHelper referenceHelper = new ReferenceHelper() { ReferenceHandler = parent };

            referenceHelper.BuildTables(functions);
            referenceHelper.VisitFunctionCall(input1, input2);

            List<int> expected = new List<int>() { 0 };
            expected.Should().BeEquivalentTo(input1.GlobalReferences);
        }

        [TestMethod]
        public void VisitFunctionCall_2FunctionNodesSameFunctionNamesAndDifferentParameterCountCheckFuncWithThreeParams_Correct()
        {
            // Setup for BuildTable
            IntegerLiteralExpression integerLiteralExpression = new IntegerLiteralExpression("2", 1, 1);
            ConditionNode conditionNode = new ConditionNode(integerLiteralExpression, 2, 2);
            List<string> parameterIdentifiers1 = new List<string> { "a", "b" };
            List<string> parameterIdentifiers2 = new List<string> { "a", "b", "c" };
            TypeNode typeNode = new TypeNode(TypeEnum.Integer, 1, 1);
            List<TypeNode> parameterTypes1 = new List<TypeNode>() { typeNode, typeNode };
            List<TypeNode> parameterTypes2 = new List<TypeNode>() { typeNode, typeNode, typeNode };
            FunctionTypeNode functionType1 = new FunctionTypeNode(typeNode, parameterTypes1, 3, 3);
            FunctionTypeNode functionType2 = new FunctionTypeNode(typeNode, parameterTypes2, 3, 3);
            FunctionNode functionNode1 = new FunctionNode("func", 1, conditionNode, parameterIdentifiers1, functionType1, 17, 17);
            FunctionNode functionNode2 = new FunctionNode("func", 1, conditionNode, parameterIdentifiers2, functionType2, 17, 17);
            List<FunctionNode> functions = new List<FunctionNode>() { functionNode1, functionNode2 };

            // Setup for VisitFunctionCall
            IntegerLiteralExpression integerLiteralExpression1 = new IntegerLiteralExpression("1", 1, 1);
            IntegerLiteralExpression integerLiteralExpression2 = new IntegerLiteralExpression("2", 1, 1);
            IntegerLiteralExpression integerLiteralExpression3 = new IntegerLiteralExpression("3", 1, 1);
            List<ExpressionNode> children = new List<ExpressionNode>() { integerLiteralExpression1, integerLiteralExpression2, integerLiteralExpression3 };
            FunctionCallExpression input1 = new FunctionCallExpression("func", children, 1, 1);
            List<string> input2 = new List<string>() { "a", "b", "c" };

            IReferenceHandler parent = Substitute.For<IReferenceHandler>();
            ReferenceHelper referenceHelper = new ReferenceHelper() { ReferenceHandler = parent };

            referenceHelper.BuildTables(functions);
            referenceHelper.VisitFunctionCall(input1, input2);

            List<int> expected = new List<int>() { 1 };
            expected.Should().BeEquivalentTo(input1.GlobalReferences);
        }

        [TestMethod]
        public void VisitFunctionCall_2FunctionNodesDifferentFunctionNamesAndDifferentParameterCount_Correct()
        {
            // Setup for BuildTable
            IntegerLiteralExpression integerLiteralExpression = new IntegerLiteralExpression("2", 1, 1);
            ConditionNode conditionNode = new ConditionNode(integerLiteralExpression, 2, 2);
            List<string> parameterIdentifiers1 = new List<string> { "a", "b" };
            List<string> parameterIdentifiers2 = new List<string> { "a", "b", "c" };
            TypeNode typeNode = new TypeNode(TypeEnum.Integer, 1, 1);
            List<TypeNode> parameterTypes1 = new List<TypeNode>() { typeNode, typeNode };
            List<TypeNode> parameterTypes2 = new List<TypeNode>() { typeNode, typeNode, typeNode };
            FunctionTypeNode functionType1 = new FunctionTypeNode(typeNode, parameterTypes1, 3, 3);
            FunctionTypeNode functionType2 = new FunctionTypeNode(typeNode, parameterTypes2, 3, 3);
            FunctionNode functionNode1 = new FunctionNode("func1", 1, conditionNode, parameterIdentifiers1, functionType1, 17, 17);
            FunctionNode functionNode2 = new FunctionNode("func2", 1, conditionNode, parameterIdentifiers2, functionType2, 17, 17);
            List<FunctionNode> functions = new List<FunctionNode>() { functionNode1, functionNode2 };

            // Setup for VisitFunctionCall
            IntegerLiteralExpression integerLiteralExpression1 = new IntegerLiteralExpression("1", 1, 1);
            IntegerLiteralExpression integerLiteralExpression2 = new IntegerLiteralExpression("2", 1, 1);
            List<ExpressionNode> children = new List<ExpressionNode>() { integerLiteralExpression1, integerLiteralExpression2 };
            FunctionCallExpression input1 = new FunctionCallExpression("func1", children, 1, 1);
            List<string> input2 = new List<string>() { "a", "b" };

            IReferenceHandler parent = Substitute.For<IReferenceHandler>();
            ReferenceHelper referenceHelper = new ReferenceHelper() { ReferenceHandler = parent };

            referenceHelper.BuildTables(functions);
            referenceHelper.VisitFunctionCall(input1, input2);

            List<int> expected = new List<int>() { 0 };
            expected.Should().BeEquivalentTo(input1.GlobalReferences);
        }

        #endregion

        #region VisitIdentifier
        [TestMethod]
        public void VisitIdentifier()
        {
            System.Type expected = typeof(IdentifierExpression);
            IdentifierExpression input1 = new IdentifierExpression("a", 1, 1);
            List<string> input2 = new List<string>() { "a", "b" };
            IReferenceHandler parent = Substitute.For<IReferenceHandler>();
            ReferenceHelper referenceHelper = new ReferenceHelper() { ReferenceHandler = parent };

            System.Type result = null;
            parent.Dispatch(Arg.Do<IdentifierExpression>(x => result = x.GetType()), Arg.Any<List<string>>());
            referenceHelper.VisitIdentifier(input1, input2);

            Assert.AreEqual(expected, result);
        }
        #endregion

    }


}
