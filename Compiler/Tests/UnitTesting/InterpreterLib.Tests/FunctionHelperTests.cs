using ASTLib;
using ASTLib.Nodes;
using ASTLib.Nodes.ExpressionNodes;
using ASTLib.Nodes.ExpressionNodes.CommonOperationNodes.GraphFields;
using ASTLib.Nodes.ExpressionNodes.OperationNodes;
using ASTLib.Nodes.TypeNodes;
using ASTLib.Objects;
using FluentAssertions;
using InterpreterLib.Helpers;
using InterpreterLib.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;

namespace InterpreterLib.Tests
{
    [TestClass]
    public class FunctionHelperTests
    {
        private FunctionHelper SetupHelper(IInterpreterFunction parent)
        {
            FunctionHelper setHelper = new FunctionHelper();
            setHelper.FunctionInterpreter(parent);
            return setHelper;
        }

        #region IdentifierFunction
        [DataRow(false, 1, 0)]
        [DataRow(false, 10, 0)]
        [DataRow(true, 0, 0)]
        [DataRow(true, 4, 1)]
        [TestMethod]
        public void IdentifierFunction_IdentifierExpressionAndObjectList_ReturnsCorrectResult(bool isLocal, int reference, int parameters)
        {
            IdentifierExpression identifier = new IdentifierExpression("", 0, 0)
            {
                IsLocal = isLocal,
                Reference = reference
            };
            FunctionHelper fhelper = new FunctionHelper();
            List<Object> array = GetData(parameters);
            int expected = isLocal ? ((Function) array[reference]).Reference : reference;

            Function res = fhelper.IdentifierFunction(identifier, array);

            Assert.AreEqual(res.Reference, expected);
        }

        public List<Object> GetData(int i)
        {
            List<List<Object>> data = new List<List<Object>> { new List<Object>{ new Function(0), 18, "" }, 
                                                               new List<Object>{ 0, 18, "", 104, new Function(17) } };
            return data[i];
        }
        #endregion

        #region SrcField
        [TestMethod]
        public void SrcField_SrcGraphFieldAndListOfObjects_ReturnsCorrectResult()
        {
            IdentifierExpression identifier = new IdentifierExpression("test", 0, 0);
            SrcGraphField input1 = new SrcGraphField(identifier, 0, 0);
            List<Object> list = new List<Object>();
            Function expected = new Function(1);
            Graph graph = new Graph(null, null, expected, null);

            IInterpreterFunction parent = Substitute.For<IInterpreterFunction>();
            parent.DispatchGraph(identifier, Arg.Any<List<object>>()).Returns(graph);
            FunctionHelper functionHelper = SetupHelper(parent);

            Function res = functionHelper.SrcField(input1, list);

            res.Should().BeEquivalentTo(expected);
        }

        [TestMethod]
        public void SrcField_SrcGraphFieldAndListOfObjects_ReturnsCorrectReference()
        {
            IdentifierExpression identifier = new IdentifierExpression("test", 0, 0);
            SrcGraphField input1 = new SrcGraphField(identifier, 0, 0);
            List<Object> list = new List<Object>();
            Function expected = new Function(1);
            Graph graph = new Graph(null, null, expected, null);

            IInterpreterFunction parent = Substitute.For<IInterpreterFunction>();
            parent.DispatchGraph(identifier, Arg.Any<List<object>>()).Returns(graph);
            FunctionHelper functionHelper = SetupHelper(parent);

            Function res = functionHelper.SrcField(input1, list);

            Assert.AreEqual(expected.Reference, res.Reference);
        }
        #endregion

        #region DstField
        [TestMethod]
        public void DstField_DstGraphFieldAndListOfObjects_ReturnsCorrectResult()
        {
            IdentifierExpression identifier = new IdentifierExpression("test", 0, 0);
            DstGraphField input1 = new DstGraphField(identifier, 0, 0);
            List<Object> list = new List<Object>();
            Function expected = new Function(1);
            Graph graph = new Graph(null, null, null, expected);

            IInterpreterFunction parent = Substitute.For<IInterpreterFunction>();
            parent.DispatchGraph(identifier, Arg.Any<List<object>>()).Returns(graph);
            FunctionHelper functionHelper = SetupHelper(parent);

            Function res = functionHelper.DstField(input1, list);

            res.Should().BeEquivalentTo(expected);
        }

        [TestMethod]
        public void DstField_DstGraphFieldAndListOfObjects_ReturnsCorrectReference()
        {
            IdentifierExpression identifier = new IdentifierExpression("test", 0, 0);
            DstGraphField input1 = new DstGraphField(identifier, 0, 0);
            List<Object> list = new List<Object>();
            Function expected = new Function(1);
            Graph graph = new Graph(null, null, null, expected);

            IInterpreterFunction parent = Substitute.For<IInterpreterFunction>();
            parent.DispatchGraph(identifier, Arg.Any<List<object>>()).Returns(graph);
            FunctionHelper functionHelper = SetupHelper(parent);

            Function res = functionHelper.DstField(input1, list);

            Assert.AreEqual(expected.Reference, res.Reference);
        }
        #endregion
    }
}