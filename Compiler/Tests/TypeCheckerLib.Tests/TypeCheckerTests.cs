using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using ASTLib;
using ASTLib.Nodes;
using ASTLib.Nodes.ExpressionNodes;
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
        public void CheckTypes_NoExportNodesAndFourFunctionNodes_CalledCorrectNumberOfTimes()
        {
            
        }
        
        # endregion
        
        # region Dispatch
        # endregion

    }
}
