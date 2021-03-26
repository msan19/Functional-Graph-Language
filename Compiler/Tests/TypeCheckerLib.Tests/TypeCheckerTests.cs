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
        public void CheckTypes_NoFunctionNodesAndTwoExportNodes_CalledCorrectNumberOfTimes()
        {
            int actualNumberOfCallToVisitExport = 0;
            int expectedNumberOfCallsToVisitExport = 2;
            ITypeHelper typeHelper = Substitute.For<ITypeHelper>();
            typeHelper.VisitExport(Arg.Do<ExportNode>(exp => actualNumberOfCallToVisitExport++));
            ITypeChecker typeChecker = new TypeChecker(typeHelper);
            AST ast = CreateAst(0, 2);

            typeChecker.CheckTypes(ast);

            Assert.AreEqual(expectedNumberOfCallsToVisitExport, actualNumberOfCallToVisitExport);
        }

        [TestMethod]
        public void CheckTypes_ThreeFunctionNodesAndNoExportNodes_CalledCorrectNumberOfTimes()
        {
            int actualNumberOfCallsToVisitFunction = 0;
            int expectedNumberOfCallsToVisitFunction = 3;
            ITypeHelper typeHelper = Substitute.For<ITypeHelper>();
            typeHelper.VisitFunction(Arg.Do<FunctionNode>(exp => actualNumberOfCallsToVisitFunction++));
            ITypeChecker typeChecker = new TypeChecker(typeHelper);
            AST ast = CreateAst(3, 0);

            typeChecker.CheckTypes(ast);

            Assert.AreEqual(expectedNumberOfCallsToVisitFunction, actualNumberOfCallsToVisitFunction);
        }
        
        // Utility methods  
        private AST CreateAst(int numberOfFunctionNodes, int numberOfExportNodes)
        {
            List<FunctionNode> functionNodes = new List<FunctionNode>();
            List<ExportNode> exportNodes = new List<ExportNode>();

            functionNodes = CreateFunctionNodes(numberOfFunctionNodes);
            exportNodes = CreateExportNodes(numberOfExportNodes);

            return new AST(functionNodes, exportNodes, 0, 0);
        }

        private List<FunctionNode> CreateFunctionNodes(int numberOfNodes)
        {
            List<FunctionNode> functionNodes = new List<FunctionNode>();
            for (int i = 0; i < numberOfNodes; i++)
                functionNodes.Add(new FunctionNode("f", 0, null, null, null, 0, 0));
            return functionNodes;
        }
        
        private List<ExportNode> CreateExportNodes(int numberOfNodes)
        {
            List<ExportNode> exportNodes = new List<ExportNode>();
            for (int i = 0; i < numberOfNodes; i++)
                exportNodes.Add(new ExportNode(null, 0, 0));
            return exportNodes;
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
            typeHelper.VisitBinaryNumOp(Arg.Do<IBinaryNumberOperator>(exp => visitBinaryNumOpWasCalled = true), Arg.Any<List<TypeNode>>());
            ITypeChecker typeChecker = new TypeChecker(typeHelper);
            
            typeChecker.Dispatch(multExpNode, new List<TypeNode>()); 
            
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
            typeHelper.VisitBinaryNumOp(Arg.Do<IBinaryNumberOperator>(expNode => actualNode = expNode), Arg.Any<List<TypeNode>>());
            ITypeChecker typeChecker = new TypeChecker(typeHelper);
            
            typeChecker.Dispatch(multExpNode, new List<TypeNode>()); 
            
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
            typeHelper.VisitBinaryNumOp(Arg.Any<IBinaryNumberOperator>(), Arg.Any<List<TypeNode>>()).Returns(expectedTypeNode);
            ITypeChecker typeChecker = new TypeChecker(typeHelper);
            
            TypeNode actualTypeNode = typeChecker.Dispatch(multExpNode, new List<TypeNode>()); 
            
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
            typeHelper.VisitFunctionCall(Arg.Do<FunctionCallExpression>(exp => visitFunctionCallWasCalled = true), Arg.Any<List<TypeNode>>());
            ITypeChecker typeChecker = new TypeChecker(typeHelper);
            
            typeChecker.Dispatch(funcCallExpr, new List<TypeNode>()); 
            
            Assert.IsTrue(visitFunctionCallWasCalled);
        }
        
        [TestMethod]
        public void Dispatch_ExpressionNodeIsFunctionCallExpression_CorrectNodePassedToVisit()
        {
            FunctionCallExpression funcCallExpr = new FunctionCallExpression("f", new List<ExpressionNode>(), 1, 1);
            ITypeHelper typeHelper = Substitute.For<ITypeHelper>();
            FunctionCallExpression actualNode = null;
            typeHelper.VisitFunctionCall(Arg.Do<FunctionCallExpression>(expNode => actualNode = expNode), Arg.Any<List<TypeNode>>());
            ITypeChecker typeChecker = new TypeChecker(typeHelper);
            
            typeChecker.Dispatch(funcCallExpr, new List<TypeNode>()); 
            
            funcCallExpr.Should().BeEquivalentTo(actualNode);
        }
        
        [TestMethod]
        public void Dispatch_ExpressionNodeIsFunctionCallExpression_ExpectToReturnSameTypeAsHelperMethod()
        {
            FunctionCallExpression funcCallExpr = new FunctionCallExpression("f", new List<ExpressionNode>(), 1, 1);
            ITypeHelper typeHelper = Substitute.For<ITypeHelper>();
            TypeNode expectedTypeNode = new TypeNode(TypeEnum.Function, 1, 1);
            typeHelper.VisitFunctionCall(Arg.Any<FunctionCallExpression>(), Arg.Any<List<TypeNode>>()).Returns(expectedTypeNode);
            ITypeChecker typeChecker = new TypeChecker(typeHelper);
            
            TypeNode actualTypeNode = typeChecker.Dispatch(funcCallExpr, new List<TypeNode>()); 
            
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
            typeHelper.VisitIdentifier(Arg.Do<IdentifierExpression>(exp => visitIdentifierWasCalled = true), Arg.Any<List<TypeNode>>());
            ITypeChecker typeChecker = new TypeChecker(typeHelper);
            
            typeChecker.Dispatch(idExpressionNode, new List<TypeNode>()); 
            
            Assert.IsTrue(visitIdentifierWasCalled);
        }
        
        [TestMethod]
        public void Dispatch_ExpressionNodeIsIdentifierExpression_CorrectNodePassedToVisit()
        {
            IdentifierExpression idExpressionNode = new IdentifierExpression("i", 1, 1);
            ITypeHelper typeHelper = Substitute.For<ITypeHelper>();
            IdentifierExpression actualNode = null;
            typeHelper.VisitIdentifier(Arg.Do<IdentifierExpression>(expNode => actualNode = expNode), Arg.Any<List<TypeNode>>());
            ITypeChecker typeChecker = new TypeChecker(typeHelper);
            
            typeChecker.Dispatch(idExpressionNode, new List<TypeNode>()); 
            
            Assert.AreEqual(idExpressionNode, actualNode);
        }
        
        [TestMethod]
        public void Dispatch_ExpressionNodeIsIdentifierExpression_ExpectToReturnSameTypeAsHelperMethod()
        {
            IdentifierExpression idExpressionNode = new IdentifierExpression("i", 1, 1);

            TypeNode expectedTypeNode = new TypeNode(TypeEnum.Real, 1, 3);
            ITypeHelper typeHelper = Substitute.For<ITypeHelper>();
            typeHelper.VisitIdentifier(Arg.Any<IdentifierExpression>(), Arg.Any<List<TypeNode>>()).Returns(expectedTypeNode);
            ITypeChecker typeChecker = new TypeChecker(typeHelper);
            
            TypeNode actualTypeNode = typeChecker.Dispatch(idExpressionNode, new List<TypeNode>()); 
            
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
            typeHelper.VisitIntegerLiteral(Arg.Do<IntegerLiteralExpression>(exp => visitIntegerLiteralWasCalled = true), Arg.Any<List<TypeNode>>());
            ITypeChecker typeChecker = new TypeChecker(typeHelper);
            
            typeChecker.Dispatch(intLiteralExpression, new List<TypeNode>()); 
            
            Assert.IsTrue(visitIntegerLiteralWasCalled);
        }
        
        [TestMethod]
        public void Dispatch_ExpressionNodeIsIntegerLiteralExpression_CorrectNodePassedToVisit()
        {
            IntegerLiteralExpression intLiteralExpression = new IntegerLiteralExpression("1", 1, 1);
            ITypeHelper typeHelper = Substitute.For<ITypeHelper>();
            IntegerLiteralExpression actualNode = null;
            typeHelper.VisitIntegerLiteral(Arg.Do<IntegerLiteralExpression>(intLitNode => actualNode = intLitNode), Arg.Any<List<TypeNode>>());
            ITypeChecker typeChecker = new TypeChecker(typeHelper);
            
            typeChecker.Dispatch(intLiteralExpression, new List<TypeNode>()); 
            
            Assert.AreEqual(intLiteralExpression, actualNode);
        }
        
        [TestMethod]
        public void Dispatch_ExpressionNodeIsIntegerLiteralExpression_ExpectToReturnSameTypeAsHelperMethod()
        {
            IntegerLiteralExpression intLiteralExpression = new IntegerLiteralExpression("1", 1, 1);
            TypeNode expectedTypeNode = new TypeNode(TypeEnum.Real, 1, 3);
            ITypeHelper typeHelper = Substitute.For<ITypeHelper>();
            typeHelper.VisitIntegerLiteral(Arg.Any<IntegerLiteralExpression>(), Arg.Any<List<TypeNode>>()).Returns(expectedTypeNode);
            ITypeChecker typeChecker = new TypeChecker(typeHelper);
            
            TypeNode actualTypeNode = typeChecker.Dispatch(intLiteralExpression, new List<TypeNode>()); 
            
            Assert.AreEqual(expectedTypeNode, actualTypeNode);
        }
            # endregion
            
            
            # region RealLiteralExpression
        [TestMethod]
        public void Dispatch_ExpressionNodeIsRealLiteralExpression_CorrectVisitMethodCalled()
        {
            RealLiteralExpression realLiteralExpression = new RealLiteralExpression("1.0", 1, 1);
            bool visitRealLiteralWasCalled = false;
            ITypeHelper typeHelper = Substitute.For<ITypeHelper>();
            typeHelper.VisitRealLiteral(Arg.Do<RealLiteralExpression>(exp => visitRealLiteralWasCalled = true), Arg.Any<List<TypeNode>>());
            ITypeChecker typeChecker = new TypeChecker(typeHelper);
            
            typeChecker.Dispatch(realLiteralExpression, new List<TypeNode>()); 
            
            Assert.IsTrue(visitRealLiteralWasCalled);
        }
        
        [TestMethod]
        public void Dispatch_ExpressionNodeIsRealLiteralExpression_CorrectNodePassedToVisit()
        {
            RealLiteralExpression realLiteralExpression = new RealLiteralExpression("1.0", 1, 1);
            ITypeHelper typeHelper = Substitute.For<ITypeHelper>();
            RealLiteralExpression actualNode = null;
            typeHelper.VisitRealLiteral(Arg.Do<RealLiteralExpression>(intLitNode => actualNode = intLitNode), Arg.Any<List<TypeNode>>());
            ITypeChecker typeChecker = new TypeChecker(typeHelper);
            
            typeChecker.Dispatch(realLiteralExpression, new List<TypeNode>()); 
            
            Assert.AreEqual(realLiteralExpression, actualNode);
        }
        
        [TestMethod]
        public void Dispatch_ExpressionNodeIsRealLiteralExpression_ExpectToReturnSameTypeAsHelperMethod()
        {
            RealLiteralExpression realLiteralExpression = new RealLiteralExpression("1.0", 1, 1);
            TypeNode expectedTypeNode = new TypeNode(TypeEnum.Real, 1, 3);
            ITypeHelper typeHelper = Substitute.For<ITypeHelper>();
            typeHelper.VisitRealLiteral(Arg.Any<RealLiteralExpression>(), Arg.Any<List<TypeNode>>()).Returns(expectedTypeNode);
            ITypeChecker typeChecker = new TypeChecker(typeHelper);
            
            TypeNode actualTypeNode = typeChecker.Dispatch(realLiteralExpression, new List<TypeNode>()); 
            
            Assert.AreEqual(expectedTypeNode, actualTypeNode);
        }
            # endregion
            
        # endregion
    }
}
