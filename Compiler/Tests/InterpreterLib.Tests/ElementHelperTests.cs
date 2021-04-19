using ASTLib.Nodes.ExpressionNodes;
using FluentAssertions;
using InterpreterLib.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InterpreterLib.Tests
{
    [TestClass]
    public class ElementHelperTests
    {
        #region Element
        [DataRow(new int[] { 1 })]
        [DataRow(new int[] { 1, 2 })]
        [TestMethod]
        public void Element_XChildren_CorrectIds(int[] ids)
        {
            var childs = Utilities.GetIntLitExpressions(ids.Length);
            var node = Utilities.GetElementExpression(childs);
            var parameters = Utilities.GetParameterList();

            var parent = Utilities.GetIntepreterElement(childs, ids);
            var helper = Utilities.GetElementHelper(parent);

            var res = helper.Element(node, parameters);

            Assert.AreEqual(ids.Length, res.Indices.Count);
            for (int i = 0; i < ids.Length; i++)
                Assert.AreEqual(ids[i], res.Indices[i]);
        }

        [DataRow(new int[] { 1 })]
        [DataRow(new int[] { 1, 2 })]
        [TestMethod]
        public void Element_XParameters_CorrectParametersPassedAlong(int[] ps)
        {
            int elemId = 1;

            var child = Utilities.GetIntLitExpression();
            var node = Utilities.GetElementExpression(child);
            var parameters = ps.ToList().ConvertAll<object>(x => x);
            var inputParamas = parameters.ToList();

            var res = new List<object>();
            var parent = Utilities.GetIntepreterElementWithParamsOut(child, elemId, x => res = x);
            var helper = Utilities.GetElementHelper(parent);

            helper.Element(node, inputParamas);

            res.Should().BeEquivalentTo(parameters);
        }

        [DataRow(new int[] { 1 })]
        [DataRow(new int[] { 1, 2 })]
        [TestMethod]
        public void Element_XParameters_DoNotChangeParameters(int[] ps)
        {
            var ids = new int[] { 1 };
            var childs = Utilities.GetIntLitExpressions(ids.Length);
            var node = Utilities.GetElementExpression(childs);
            var parameters = ps.ToList().ConvertAll<object>(x => x);
            var inputParamas = parameters.ToList();

            var parent = Utilities.GetIntepreterElement(childs, ids);
            var helper = Utilities.GetElementHelper(parent);

            var res = helper.Element(node, inputParamas);

            inputParamas.Should().BeEquivalentTo(parameters);
        }
        #endregion
    }
}
