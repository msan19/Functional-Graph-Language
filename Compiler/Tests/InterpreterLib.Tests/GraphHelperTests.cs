using ASTLib;
using ASTLib.Nodes;
using ASTLib.Nodes.ExpressionNodes;
using ASTLib.Objects;
using FluentAssertions;
using InterpreterLib.Helpers;
using InterpreterLib.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
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
            Element e = new Element(7);
            Set V = new Set(e);
            Set E = new Set(e);
            Function src = new Function(0);
            Function dst = new Function(0);
            return new Graph(V, E, src, dst);
        }

        private AST GetAST()
        {
            return new AST(new List<FunctionNode> { new FunctionNode("", null, null, null, 0, 0) }, new List<ExportNode>(), 0,0);
        }

        [TestMethod]
        public void ExportGraph_GraphAndFileName_ReturnsCorrectLabelGraph()
        {
            IInterpreterGraph parent = Substitute.For<IInterpreterGraph>();
            GraphHelper graphHelper = SetUpHelper(parent);
            graphHelper.SetASTRoot(GetAST());
            parent.Function<Element>(Arg.Any<FunctionNode>(), Arg.Any<List<Object>>()).Returns(new Element(7));
            parent.DispatchString(Arg.Any<ExpressionNode>(), Arg.Any<List<Object>>()).Returns("File");
            parent.DispatchGraph(Arg.Any<ExpressionNode>(), Arg.Any<List<Object>>()).Returns(GetGraph());
            List<int> src = new List<int> { 0 };
            List<int> dst = new List<int> { 0 };
            string[,] vertexLabels = new string[1,0];
            string[,] edgeLabels = new string[1,0];
            LabelGraph expected = new LabelGraph("File", src, dst, vertexLabels, edgeLabels);

            LabelGraph result = graphHelper.ExportGraph(new ExportNode(new IdentifierExpression("", 0, 0), 0, 0));

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