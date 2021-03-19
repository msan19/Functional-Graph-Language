using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using ASTLib;
using ASTLib.Interfaces;
using ASTLib.Nodes;
using ASTLib.Nodes.ExpressionNodes;
using ASTLib.Nodes.ExpressionNodes.OperationNodes;
using ASTLib.Nodes.TypeNodes;
using FluentAssertions;
using NSubstitute;

namespace TypeCheckerLib.Tests
{
    [TestClass]
    public class TypeCheckerTests
    {
        
        # region CheckTypes
        [TestMethod]
        public void CheckTypes_TwoExportNodesAndNoFunctionNodes_CalledCorrectNumberOfTimes()
        {
            int actualNumVisitExportCalls = 0;
            int expectedNumVisitExportCalls = 2;
            
            // Mock type helper
            ITypeHelper typeHelper = Substitute.For<ITypeHelper>();
            typeHelper.VisitExport(Arg.Do<ExportNode>(exp => actualNumVisitExportCalls++));

            ITypeChecker typeChecker = new TypeChecker(typeHelper);
            
            // Create AST with TwoExportNodesAndNoFunctionNodes
            FunctionCallExpression funcCallExpr = new FunctionCallExpression("f", new List<ExpressionNode>(), 1, 1);
            List<ExportNode> exportNodes = new List<ExportNode>()
            {
                new ExportNode(funcCallExpr, 1, 1),
                new ExportNode(funcCallExpr, 2, 2)
            };
            AST ast = new AST(new List<FunctionNode>(), exportNodes, 1, 1);

            typeChecker.CheckTypes(ast);

            Assert.AreEqual(expectedNumVisitExportCalls, actualNumVisitExportCalls);
        }
        
        [TestMethod]
        public void CheckTypes_NoExportNodesAndThreeFunctionNodes_CalledCorrectNumberOfTimes()
        {
            int actualNumVisitFunctionCalls = 0;
            int expectedNumVisitFunctionCalls = 3;
            
            // Mock type helper
            ITypeHelper typeHelper = Substitute.For<ITypeHelper>();
            typeHelper.VisitFunction(Arg.Do<FunctionNode>(exp => actualNumVisitFunctionCalls++));

            ITypeChecker typeChecker = new TypeChecker(typeHelper);
            
            // Create AST with NoExportNodesAndThreeFunctionNodes
            FunctionCallExpression funcCallExpr = new FunctionCallExpression("g", new List<ExpressionNode>(), 1, 1);
            FunctionTypeNode funcTypeNode =
                new FunctionTypeNode(new TypeNode(TypeEnum.Integer, 1, 1), new List<TypeNode>(), 1, 1);
            
            ConditionNode conditionNode = new ConditionNode(funcCallExpr, 1, 1); 
            
            List<FunctionNode> funcNodes = new List<FunctionNode>()
            {
                new FunctionNode("f1", 1, conditionNode, new List<string>(), funcTypeNode, 1, 1),
                new FunctionNode("f2", 2, conditionNode, new List<string>(), funcTypeNode, 1, 1),
                new FunctionNode("f2", 3, conditionNode, new List<string>(), funcTypeNode, 1, 1),
                
            };
            AST ast = new AST(funcNodes, new List<ExportNode>(), 1, 1);

            typeChecker.CheckTypes(ast);

            Assert.AreEqual(expectedNumVisitFunctionCalls, actualNumVisitFunctionCalls);
        }
        # endregion
        
        # region Dispatch

            # region IBinaryNumberOperator
        [TestMethod]
        public void Dispatch_ExpressionNodeIsIBinaryNumberOperator_CorrectVisitMethodCalled()
        {
            IntegerLiteralExpression n1 = new IntegerLiteralExpression("1", 1, 1);
            IntegerLiteralExpression n2 = new IntegerLiteralExpression("2", 1, 5);
            MultiplicationExpression multExpNode = new MultiplicationExpression(n1, n2, 1, 3);
            bool visitBinaryNumOpWasCalled = false;
            ITypeHelper typeHelper = Substitute.For<ITypeHelper>();
            typeHelper.VisitBinaryNumOp(Arg.Do<IBinaryNumberOperator>(exp => visitBinaryNumOpWasCalled = true));
            ITypeChecker typeChecker = new TypeChecker(typeHelper);
            
            typeChecker.Dispatch(multExpNode); 
            
            Assert.IsTrue(visitBinaryNumOpWasCalled);
        }
        
        [TestMethod]
        public void Dispatch_ExpressionNodeIsIBinaryNumberOperator_CorrectNodePassedToVisit()
        {
            IntegerLiteralExpression n1 = new IntegerLiteralExpression("1", 1, 1);
            IntegerLiteralExpression n2 = new IntegerLiteralExpression("2", 1, 5);
            MultiplicationExpression multExpNode = new MultiplicationExpression(n1, n2, 1, 3);
            ITypeHelper typeHelper = Substitute.For<ITypeHelper>();
            IBinaryNumberOperator actualNode = null;
            typeHelper.VisitBinaryNumOp(Arg.Do<IBinaryNumberOperator>(expNode => actualNode = expNode));
            ITypeChecker typeChecker = new TypeChecker(typeHelper);
            
            typeChecker.Dispatch(multExpNode); 
            
            Assert.AreEqual(multExpNode, actualNode);
        }
        
        [TestMethod]
        public void Dispatch_ExpressionNodeIsIBinaryNumberOperator_ExpectToReturnSameTypeAsHelperMethod()
        {
            IntegerLiteralExpression n1 = new IntegerLiteralExpression("1", 1, 1);
            IntegerLiteralExpression n2 = new IntegerLiteralExpression("2", 1, 5);
            MultiplicationExpression multExpNode = new MultiplicationExpression(n1, n2, 1, 3);
            TypeNode expectedTypeNode = new TypeNode(TypeEnum.Real, 1, 3);
            ITypeHelper typeHelper = Substitute.For<ITypeHelper>();
            typeHelper.VisitBinaryNumOp(Arg.Any<IBinaryNumberOperator>()).Returns(expectedTypeNode);
            ITypeChecker typeChecker = new TypeChecker(typeHelper);
            
            TypeNode actualTypeNode = typeChecker.Dispatch(multExpNode); 
            
            Assert.AreEqual(expectedTypeNode, actualTypeNode);
        }
            # endregion
 
            
            # region FunctionCallExpression
        [TestMethod]
        public void Dispatch_ExpressionNodeIsFunctionCallExpression_CorrectVisitMethodCalled()
        {
            FunctionCallExpression funcCallExpr = new FunctionCallExpression("f", new List<ExpressionNode>(), 1, 1);
            bool visitFunctionCallWasCalled = false;
            ITypeHelper typeHelper = Substitute.For<ITypeHelper>();
            typeHelper.VisitFunctionCall(Arg.Do<FunctionCallExpression>(exp => visitFunctionCallWasCalled = true));
            ITypeChecker typeChecker = new TypeChecker(typeHelper);
            
            typeChecker.Dispatch(funcCallExpr); 
            
            Assert.IsTrue(visitFunctionCallWasCalled);
        }
        
        [TestMethod]
        public void Dispatch_ExpressionNodeIsFunctionCallExpression_CorrectNodePassedToVisit()
        {
            FunctionCallExpression funcCallExpr = new FunctionCallExpression("f", new List<ExpressionNode>(), 1, 1);
            ITypeHelper typeHelper = Substitute.For<ITypeHelper>();
            FunctionCallExpression actualNode = null;
            typeHelper.VisitFunctionCall(Arg.Do<FunctionCallExpression>(expNode => actualNode = expNode));
            ITypeChecker typeChecker = new TypeChecker(typeHelper);
            
            typeChecker.Dispatch(funcCallExpr); 
            
            funcCallExpr.Should().BeEquivalentTo(actualNode);
        }
        
        [TestMethod]
        public void Dispatch_ExpressionNodeIsFunctionCallExpression_ExpectToReturnSameTypeAsHelperMethod()
        {
            FunctionCallExpression funcCallExpr = new FunctionCallExpression("f", new List<ExpressionNode>(), 1, 1);
            ITypeHelper typeHelper = Substitute.For<ITypeHelper>();
            TypeNode expectedTypeNode = new TypeNode(TypeEnum.Function, 1, 1);
            typeHelper.VisitFunctionCall(Arg.Any<FunctionCallExpression>()).Returns(expectedTypeNode);
            ITypeChecker typeChecker = new TypeChecker(typeHelper);
            
            TypeNode actualTypeNode = typeChecker.Dispatch(funcCallExpr); 
            
            Assert.AreEqual(expectedTypeNode, actualTypeNode);        
        }
            # endregion

            
            # region IdentifierExpression
        [TestMethod]
        public void Dispatch_ExpressionNodeIsIdentifierExpression_CorrectVisitMethodCalled()
        {
            IdentifierExpression idExpressionNode = new IdentifierExpression("i", 1, 1);
            bool visitIdentifierWasCalled = false;
            ITypeHelper typeHelper = Substitute.For<ITypeHelper>();
            typeHelper.VisitIdentifier(Arg.Do<IdentifierExpression>(exp => visitIdentifierWasCalled = true));
            ITypeChecker typeChecker = new TypeChecker(typeHelper);
            
            typeChecker.Dispatch(idExpressionNode); 
            
            Assert.IsTrue(visitIdentifierWasCalled);
        }
        
        [TestMethod]
        public void Dispatch_ExpressionNodeIsIdentifierExpression_CorrectNodePassedToVisit()
        {
            IdentifierExpression idExpressionNode = new IdentifierExpression("i", 1, 1);
            ITypeHelper typeHelper = Substitute.For<ITypeHelper>();
            IdentifierExpression actualNode = null;
            typeHelper.VisitIdentifier(Arg.Do<IdentifierExpression>(expNode => actualNode = expNode));
            ITypeChecker typeChecker = new TypeChecker(typeHelper);
            
            typeChecker.Dispatch(idExpressionNode); 
            
            Assert.AreEqual(idExpressionNode, actualNode);
        }
        
        [TestMethod]
        public void Dispatch_ExpressionNodeIsIdentifierExpression_ExpectToReturnSameTypeAsHelperMethod()
        {
            IdentifierExpression idExpressionNode = new IdentifierExpression("i", 1, 1);

            TypeNode expectedTypeNode = new TypeNode(TypeEnum.Real, 1, 3);
            ITypeHelper typeHelper = Substitute.For<ITypeHelper>();
            typeHelper.VisitIdentifier(Arg.Any<IdentifierExpression>()).Returns(expectedTypeNode);
            ITypeChecker typeChecker = new TypeChecker(typeHelper);
            
            TypeNode actualTypeNode = typeChecker.Dispatch(idExpressionNode); 
            
            Assert.AreEqual(expectedTypeNode, actualTypeNode);        
        }
            # endregion

            # region IntegerLiteralExpression
        [TestMethod]
        public void Dispatch_ExpressionNodeIsIntegerLiteralExpression_CorrectVisitMethodCalled()
        {
            IntegerLiteralExpression intLiteralExpression = new IntegerLiteralExpression("1", 1, 1);
            bool visitIntegerLiteralWasCalled = false;
            ITypeHelper typeHelper = Substitute.For<ITypeHelper>();
            typeHelper.VisitIntegerLiteral(Arg.Do<IntegerLiteralExpression>(exp => visitIntegerLiteralWasCalled = true));
            ITypeChecker typeChecker = new TypeChecker(typeHelper);
            
            typeChecker.Dispatch(intLiteralExpression); 
            
            Assert.IsTrue(visitIntegerLiteralWasCalled);
        }
        
        [TestMethod]
        public void Dispatch_ExpressionNodeIsIntegerLiteralExpression_CorrectNodePassedToVisit()
        {
            IntegerLiteralExpression intLiteralExpression = new IntegerLiteralExpression("1", 1, 1);
            ITypeHelper typeHelper = Substitute.For<ITypeHelper>();
            IntegerLiteralExpression actualNode = null;
            typeHelper.VisitIntegerLiteral(Arg.Do<IntegerLiteralExpression>(intLitNode => actualNode = intLitNode));
            ITypeChecker typeChecker = new TypeChecker(typeHelper);
            
            typeChecker.Dispatch(intLiteralExpression); 
            
            Assert.AreEqual(intLiteralExpression, actualNode);
        }
        
        [TestMethod]
        public void Dispatch_ExpressionNodeIsIntegerLiteralExpression_ExpectToReturnSameTypeAsHelperMethod()
        {
            IntegerLiteralExpression intLiteralExpression = new IntegerLiteralExpression("1", 1, 1);

            TypeNode expectedTypeNode = new TypeNode(TypeEnum.Real, 1, 3);
            ITypeHelper typeHelper = Substitute.For<ITypeHelper>();
            typeHelper.VisitIntegerLiteral(Arg.Any<IntegerLiteralExpression>()).Returns(expectedTypeNode);
            ITypeChecker typeChecker = new TypeChecker(typeHelper);
            
            TypeNode actualTypeNode = typeChecker.Dispatch(intLiteralExpression); 
            
            Assert.AreEqual(expectedTypeNode, actualTypeNode);
        }
            # endregion
            
            # region RealLiteralExpression
            # endregion
            
        # endregion

    }
}
