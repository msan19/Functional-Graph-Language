using System;
using System.Collections.Generic;
using System.Text;
using ASTLib.Nodes;
using ASTLib.Nodes.ExpressionNodes;
using ASTLib.Nodes.ExpressionNodes.CommonOperationNodes.RelationalOperationNodes;
using ASTLib.Objects;
using InterpreterLib.Helpers;
using InterpreterLib.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using FluentAssertions;

namespace InterpreterLib.Tests
{
    [TestClass]
    public class SetHelperTests
    {
        private SetHelper SetUpHelper(IInterpreterSet parent)
        {
            SetHelper setHelper = new SetHelper();
            setHelper.SetInterpreter(parent);
            return setHelper;
        }

        private List<BoundNode> GetBounds(IInterpreterSet parent, List<int> minValues, List<int> maxValues)
        {
            List<BoundNode> bounds = new List<BoundNode>();

            for (int i = 0; i < minValues.Count; i++)
            {
                IntegerLiteralExpression min = new IntegerLiteralExpression(minValues[i], 0, 0);
                IntegerLiteralExpression max = new IntegerLiteralExpression(maxValues[i], 0, 0);
                bounds.Add(new BoundNode(null, min, max, 0, 0));

                parent.DispatchInt(min, Arg.Any<List<object>>()).Returns(minValues[i]);
                parent.DispatchInt(max, Arg.Any<List<object>>()).Returns(maxValues[i]);
            }

            return bounds;
        }

        #region SetExpression

        [TestMethod]
        public void SetExpression_f_f()
        {
            // {e[i, j] | 0 <= [i] < 5, 0 < [j] < 5, i < j}

            IInterpreterSet parent = Substitute.For<IInterpreterSet>();
            SetHelper setHelper = SetUpHelper(parent);

            List<BoundNode> bounds = GetBounds(parent, new List<int> { 0, 1 }, new List<int> { 4, 4 });
            LessExpression lessExpr = new LessExpression(new IdentifierExpression("i", 0, 0), new IdentifierExpression("j", 0, 0), 0, 0);
            SetExpression setExpr = new SetExpression(null, bounds, lessExpr, 0, 0);

            List<List<object>> indexPairs = new List<List<object>> { new List<object> { 0, 1 }, new List<object> { 0, 2 }, new List<object> { 0, 3 },
                                                               new List<object> { 0, 4 }, new List<object> { 1, 2 }, new List<object> { 1, 3 },
                                                               new List<object> { 1, 4 }, new List<object> { 2, 3 }, new List<object> { 2, 4 },
                                                               new List<object> { 3, 4 } };

            //parent.DispatchBoolean(lessExpr, Arg.Any<List<object>>()).Returns(false);
            List<Element> els = new List<Element>();
            for (int i = 0; i < indexPairs.Count; i++)
            {
                els.Add(new Element(new List<int> { (int)indexPairs[i][0], (int)indexPairs[i][1] }));
                parent.DispatchBoolean(lessExpr, indexPairs[i]).Returns(true);
            }

            Set result = setHelper.SetExpression(setExpr, new List<object>());
            Set expected = new Set(els);

            result.Should().BeEquivalentTo(expected);
        }

        #endregion
    }
}
