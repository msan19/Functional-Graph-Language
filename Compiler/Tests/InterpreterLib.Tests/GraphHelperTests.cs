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