using FluentAssertions;
using NSubstitute;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using ASTLib.Nodes.ExpressionNodes;
using ASTLib.Nodes.ExpressionNodes.OperationNodes;
using ASTLib.Interfaces;
using ASTLib.Nodes;
using ASTLib;

namespace ReferenceHandlerLib.Tests
{
    [TestClass]
    public class ReferenceHandlerTests
    {
        #region dispatchIdentifer
        [TestMethod]
        public void Dispatch_IdentifierExpAndStringList_CorrectListPassedToVisitIdentifier()
        {   
            List<string> expected = new List<string>() { "id" };
            IdentifierExpression input1 = new IdentifierExpression("id", 1, 1);
            List<string> input2 = new List<string>() { "id" };
            IReferenceHelper helper = Substitute.For<IReferenceHelper>();
            ReferenceHandler refHandler = new ReferenceHandler(helper);
            List<string> res = new List<string>();
            helper.VisitIdentifier(Arg.Any<IdentifierExpression>(), Arg.Do<List<string>>(x => res = x));
            
            refHandler.Dispatch(input1, input2);
                        
            res.Should().BeEquivalentTo(expected);
        }

        [TestMethod]
        public void Dispatch_IdentifierExpAndStringList_CorrectIdentifierExpPassedToVisitIdentifier()
        {
            IdentifierExpression expected = new IdentifierExpression("id", 1, 1);
            IdentifierExpression input1 = expected;
            List<string> input2 = new List<string>() { "id" };
            IReferenceHelper helper = Substitute.For<IReferenceHelper>();
            ReferenceHandler refHandler = new ReferenceHandler(helper);
            IdentifierExpression res = null;
            helper.VisitIdentifier(Arg.Do<IdentifierExpression>(x => res = x), Arg.Any<List<string>>());

            refHandler.Dispatch(input1, input2);
                        
            res.Should().BeEquivalentTo(expected);
        }
        #endregion

        #region dispatchNonIdentifier
        [TestMethod]
        public void Dispatch_INonIdentifierExpAndStringList_CorrectListPassed()
        {
            List<string> expected = new List<string>() { "id" };
            PowerExpression input1 = new PowerExpression(null, null, 0, 0);
            List<string> input2 = new List<string>() { "id" };
            IReferenceHelper helper = Substitute.For<IReferenceHelper>();
            ReferenceHandler refHandler = new ReferenceHandler(helper);
            List<string> res = null;
            helper.VisitNonIdentifier(Arg.Any<PowerExpression>(), Arg.Do<List<string>>(x => res = x));

            refHandler.Dispatch(input1, input2);

            res.Should().BeEquivalentTo(expected);
        }

        [TestMethod]
        public void Dispatch_INonIdentifierAndStringList_CorrectIdentifierExpPassed()
        {
            PowerExpression expected = new PowerExpression(null, null, 1, 1);
            PowerExpression input1 = expected;
            List<string> input2 = new List<string>() { "id" };
            IReferenceHelper helper = Substitute.For<IReferenceHelper>();
            ReferenceHandler refHandler = new ReferenceHandler(helper);
            INonIdentifierExpression res = null;
            helper.VisitNonIdentifier(Arg.Do<INonIdentifierExpression>(x => res = x), Arg.Any<List<string>>());

            refHandler.Dispatch(input1, input2);

            res.Should().BeEquivalentTo(expected);
        }

        #endregion

        #region function
        [TestMethod]
        public void Dispatch_FunctionCallExprAndStringList_CorrectListPassed()
        {
            List<string> expected = new List<string>() { "id" };
            FunctionCallExpression input1 = new FunctionCallExpression("func", null, 0, 0);
            List<string> input2 = new List<string>() { "id" };
            IReferenceHelper helper = Substitute.For<IReferenceHelper>();
            ReferenceHandler refHandler = new ReferenceHandler(helper);
            List<string> res = null;
            helper.VisitFunctionCall(Arg.Any<FunctionCallExpression>(), Arg.Do<List<string>>(x => res = x));

            refHandler.Dispatch(input1, input2);

            res.Should().BeEquivalentTo(expected);
        }

        [TestMethod]
        public void Dispatch_FunctionCallExprAndStringList_CorrectFunctionCallExprPassed()
        {
            FunctionCallExpression expected = new FunctionCallExpression("func", null, 0, 0);
            FunctionCallExpression input1 = expected;
            List<string> input2 = new List<string>() { "id" };
            IReferenceHelper helper = Substitute.For<IReferenceHelper>();
            ReferenceHandler refHandler = new ReferenceHandler(helper);
            FunctionCallExpression res = null;
            helper.VisitFunctionCall(Arg.Do<FunctionCallExpression>(x => res = x), Arg.Any<List<string>>());

            refHandler.Dispatch(input1, input2);

            res.Should().BeEquivalentTo(expected);
        }
        #endregion

        #region InsertReferences 
        [TestMethod]
        public void InsertReferences_AST_CorrectNumberOfCallsToVisitFunction()
        {
            List<ExportNode> exports = new List<ExportNode> { new ExportNode(null,0,0), 
                                                              new ExportNode(null,0,0),
                                                              new ExportNode(null,0,0)};
            List<FunctionNode> functions = new List<FunctionNode> { new FunctionNode("", 0,null,null,null,0,0),
                                                                    new FunctionNode("", 0,null,null,null,0,0),
                                                                    new FunctionNode("", 0,null,null,null,0,0),
                                                                    new FunctionNode("", 0,null,null,null,0,0)};
            AST ast = new AST(functions, exports, 0, 0);
            IReferenceHelper helper = Substitute.For<IReferenceHelper>();
            ReferenceHandler refHandler = new ReferenceHandler(helper);

            refHandler.InsertReferences(ast);

            helper.Received(4).VisitFunction(Arg.Any<FunctionNode>());
        }

        [TestMethod]
        public void InsertReferences_AST_CorrectNumberOfCallsToVisitExport()
        {
            List<ExportNode> exports = new List<ExportNode> { new ExportNode(null,0,0),
                                                              new ExportNode(null,0,0),
                                                              new ExportNode(null,0,0)};
            List<FunctionNode> functions = new List<FunctionNode> { new FunctionNode("", 0,null,null,null,0,0),
                                                                    new FunctionNode("", 0,null,null,null,0,0),
                                                                    new FunctionNode("", 0,null,null,null,0,0),
                                                                    new FunctionNode("", 0,null,null,null,0,0)};
            AST ast = new AST(functions, exports, 0, 0);
            IReferenceHelper helper = Substitute.For<IReferenceHelper>();
            ReferenceHandler refHandler = new ReferenceHandler(helper);

            refHandler.InsertReferences(ast);

            helper.Received(3).VisitExport(Arg.Any<ExportNode>());
        }

        [TestMethod]
        public void InsertReferences_AST_CorrectNumberOfCallsToBuildTable()
        {
            List<ExportNode> exports = new List<ExportNode> { new ExportNode(null,0,0),
                                                              new ExportNode(null,0,0),
                                                              new ExportNode(null,0,0)};
            List<FunctionNode> functions = new List<FunctionNode> { new FunctionNode("", 0,null,null,null,0,0),
                                                                    new FunctionNode("", 0,null,null,null,0,0),
                                                                    new FunctionNode("", 0,null,null,null,0,0),
                                                                    new FunctionNode("", 0,null,null,null,0,0)};
            AST ast = new AST(functions, exports, 0, 0);
            IReferenceHelper helper = Substitute.For<IReferenceHelper>();
            ReferenceHandler refHandler = new ReferenceHandler(helper);

            refHandler.InsertReferences(ast);

            helper.Received(1).BuildTable(Arg.Any<List<FunctionNode>>());
        }
        #endregion

        
    }
}
