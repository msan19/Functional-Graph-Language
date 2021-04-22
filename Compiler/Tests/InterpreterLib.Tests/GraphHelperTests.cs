using ASTLib;
using ASTLib.Nodes;
using ASTLib.Nodes.ExpressionNodes;
using ASTLib.Objects;
using FluentAssertions;
using InterpreterLib.Helpers;
using InterpreterLib.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using NSubstitute.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace InterpreterLib.Tests
{
    [TestClass]
    public class GraphHelperTests
    {
        private GraphHelper SetUpHelper(IInterpreterGraph parent)
        {
            GraphHelper graphHelper = new GraphHelper();
            graphHelper.SetInterpreter(parent);
            return graphHelper;
        }

        private Graph GetGraph()
        {
            Set V = new Set(new List<Element> { new Element(5), new Element(10), new Element(15) });
            Set E = new Set(new List<Element> { new Element(1), new Element(2), new Element(3) });
            Function src = new Function(0);
            Function dst = new Function(1);
            return new Graph(V, E, src, dst);
        }

        private AST GetAST()
        {
            return new AST(new List<FunctionNode> { new FunctionNode("", null, null, null, 0, 0),
                                                    new FunctionNode("", null, null, null, 1, 0),}, 
                           new List<ExportNode>(), 0,0);
        }

        private Element HandleFunctionNode(CallInfo list)
        {
            return new Element((((FunctionNode)list[0]).LineNumber == 0) ? GetElementIndex(list) * 5 : 20 - GetElementIndex(list) * 5);
        }

        private int GetElementIndex(Object list)
        {
            return ((Element)((List<Object>)((CallInfo)list)[1])[0]).Indices[0];
        }

        [TestMethod]
        public void ExportGraph_GraphAndFileName_ReturnsCorrectLabelGraph()
        {
            IInterpreterGraph parent = Substitute.For<IInterpreterGraph>();
            GraphHelper graphHelper = SetUpHelper(parent);
            graphHelper.SetASTRoot(GetAST());
            parent.Function<Element>(Arg.Any<FunctionNode>(), Arg.Any<List<Object>>()).Returns(x => HandleFunctionNode(x));
            parent.DispatchString(Arg.Any<ExpressionNode>(), Arg.Any<List<Object>>()).Returns("File");
            parent.DispatchGraph(Arg.Any<ExpressionNode>(), Arg.Any<List<Object>>()).Returns(GetGraph());
            List<int> src = new List<int> { 0, 1, 2 };
            List<int> dst = new List<int> { 2, 1, 0 };
            string[,] vertexLabels = new string[0,3];
            string[,] edgeLabels = new string[0,3];

            LabelGraph expected = new LabelGraph("File", src, dst, vertexLabels, edgeLabels);

            LabelGraph result = graphHelper.ExportGraph(new ExportNode(new IdentifierExpression("", 0, 0), 0, 0));

            result.Should().BeEquivalentTo(expected);
        }

        [TestMethod]
        public void ExportGraph_GraphAndFileName_ThrowsError()
        {
            IInterpreterGraph parent = Substitute.For<IInterpreterGraph>();
            GraphHelper graphHelper = SetUpHelper(parent);
            graphHelper.SetASTRoot(GetAST());
            parent.Function<Element>(Arg.Any<FunctionNode>(), Arg.Any<List<Object>>()).Returns(new Element(3));
            parent.DispatchString(Arg.Any<ExpressionNode>(), Arg.Any<List<Object>>()).Returns("File");
            parent.DispatchGraph(Arg.Any<ExpressionNode>(), Arg.Any<List<Object>>()).Returns(GetGraph());


            Assert.ThrowsException<Exception>(() => graphHelper.ExportGraph(new ExportNode(new IdentifierExpression("", 0, 0), 0, 0)));
        }

        [TestMethod]
        public void ExportGraph_GraphAndFileName_ReturnsCorrectLabelGraphWithLabels()
        {
            IInterpreterGraph parent = Substitute.For<IInterpreterGraph>();
            GraphHelper graphHelper = SetUpHelper(parent);
            AST ast = GetAST();
            graphHelper.SetASTRoot(ast);
            parent.Function<Element>(Arg.Any<FunctionNode>(), Arg.Any<List<Object>>()).Returns(new Element(5));
            parent.DispatchString(Arg.Any<ExpressionNode>(), Arg.Any<List<Object>>()).Returns("File");
            parent.DispatchGraph(Arg.Any<ExpressionNode>(), Arg.Any<List<Object>>()).Returns(GetGraph());
            parent.DispatchFunction(Arg.Any<IdentifierExpression>(), Arg.Any<List<Object>>()).Returns(new Function(0));
            parent.DispatchFunction(Arg.Any<FunctionCallExpression>(), Arg.Any<List<Object>>()).Returns(new Function(1));
            parent.Function<string>(ast.Functions[0], Arg.Any<List<Object>>()).Returns("a");
            parent.Function<string>(ast.Functions[1], Arg.Any<List<Object>>()).Returns("b");
            List<int> src = new List<int> { 0, 0, 0 };
            List<int> dst = new List<int> { 0, 0, 0 };
            string[,] vertexLabels = new string[,] { { "a", "a", "a" }, { "b", "b", "b" } };
            string[,] edgeLabels = new string[,] { { "a", "a", "a" } };
            LabelGraph expected = new LabelGraph("File", src, dst, vertexLabels, edgeLabels);
            IdentifierExpression identifier = new IdentifierExpression("", 0, 0);
            FunctionCallExpression functionCall = new FunctionCallExpression("", null, 0, 0);
            ExportNode node = new ExportNode(identifier, 
                                             identifier, 
                                             new List<ExpressionNode>() { identifier, functionCall},
                                             new List<ExpressionNode>() { identifier },
                                             0, 0);

            LabelGraph result = graphHelper.ExportGraph(node);

            result.Should().BeEquivalentTo(expected);
        }

        #region GraphExpression

        [TestMethod]
        public void GraphExpression_ValidExpressions_ReturnsCorrectGraph()
        {
            IInterpreterGraph parent = Substitute.For<IInterpreterGraph>();
            GraphHelper graphHelper = SetUpHelper(parent);
            SetExpression verticesExpr = new SetExpression(null, null, null, 1, 1);
            SetExpression edgesExpr = new SetExpression(null, null, null, 1, 1);
            IdentifierExpression srcExpr = new IdentifierExpression(null, 1, 1);
            IdentifierExpression dstExpr = new IdentifierExpression(null, 1, 1);
            GraphExpression graphExpr = new GraphExpression(verticesExpr, edgesExpr, srcExpr, dstExpr, 1, 1);
            Set expectedVerticesSet = new Set();
            Set expectedEdgesSet = new Set();
            Function expectedSrcFunc = new Function(42);
            Function expectedDstFunc = new Function(43);
            Graph expected = new Graph(expectedVerticesSet, expectedEdgesSet, expectedSrcFunc, expectedDstFunc);
            parent.DispatchSet(verticesExpr, Arg.Any<List<Object>>()).Returns(expectedVerticesSet);
            parent.DispatchSet(edgesExpr, Arg.Any<List<Object>>()).Returns(expectedEdgesSet);
            parent.DispatchFunction(srcExpr, Arg.Any<List<Object>>()).Returns(expectedSrcFunc);
            parent.DispatchFunction(dstExpr, Arg.Any<List<Object>>()).Returns(expectedDstFunc);

            Graph result = graphHelper.GraphExpression(graphExpr, new List<Object>());

            result.Should().BeEquivalentTo(expected);
        }

        #endregion
    }
}