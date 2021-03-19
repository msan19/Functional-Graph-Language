using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using ASTLib;
using ASTLib.Nodes;
using ASTLib.Nodes.ExpressionNodes;
using ASTLib.Nodes.TypeNodes;
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
        # endregion

    }
}
