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
using ASTLib.Nodes.ExpressionNodes.SetOperationNodes;
using System.Linq;
using FluentAssertions.Common;

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

        [DataTestMethod]
        [DynamicData(nameof(SetExpression_TestDataMethod), DynamicDataSourceType.Method)]
        public void SetExpression_f_f(List<int> min, List<int> max, List<List<object>> indexPairs)
        {
            IInterpreterSet parent = Substitute.For<IInterpreterSet>();
            SetHelper setHelper = SetUpHelper(parent);
            List<BoundNode> bounds = GetBounds(parent, min, max);
            LessExpression expr = new LessExpression(null, null, 0, 0);
            SetExpression setExpr = new SetExpression(null, bounds, expr, 0, 0);
            List<Element> expectedElements = new List<Element>();
            for (int i = 0; i < indexPairs.Count; i++)
            {
                expectedElements.Add(new Element(indexPairs[i].ConvertAll(x => (int) x)));
            }
            Set expected = new Set(expectedElements);
            parent.DispatchBoolean(expr, Arg.Any<List<Object>>()).Returns(x => IsContained(indexPairs, (List<Object>)x[1]));

            Set result = setHelper.SetExpression(setExpr, new List<object>());

            result.Should().BeEquivalentTo(expected);
        }

        static IEnumerable<object[]> SetExpression_TestDataMethod()
        {
            return new[]
            {
                new Object[]
                {
                    // {e[i, j] | 0 <= [i] < 5, 0 < [j] < 5, i < j}
                    new List<int> { 0, 1 },
                    new List<int> { 4, 4 },
                    new List<List<Object>>
                    {
                        new List<Object> { 0, 1 }, new List<Object> { 0, 2 }, new List<Object> { 0, 3 }, new List<Object> { 0, 4 },
                        new List<Object> { 1, 2 }, new List<Object> { 1, 3 }, new List<Object> { 1, 4 }, new List<Object> { 2, 3 },
                        new List<Object> { 2, 4 }, new List<Object> { 3, 4 }
                    }
                },
                new Object[]
                {
                    // {e[i] | 0 < [i] < 5, i == 2}
                    new List<int> { 1 },
                    new List<int> { 4 },
                    new List<List<Object>>
                    {
                        new List<Object> { 2 }
                    }
                }
            };
        }

        private bool IsContained(List<List<Object>> doubleList, List<Object> list)
        {
            foreach(List<Object> l in doubleList)
            {
                if(l.Count + 1 == list.Count)
                {
                    bool isAMatch = true;
                    for (int i = 0; i < l.Count; i++)
                        if (!l[i].Equals(list[i + 1]))
                            isAMatch = false;
                    if (isAMatch)
                        return true;
                }
            }
            return false;
        }

        #endregion

        #region IntersectionSet

        [DataTestMethod]
        [DynamicData(nameof(IntersectionSet_TestDataMethod), DynamicDataSourceType.Method)]
        public void IntersectionSet_ValidInput_ReturnsCorrectSet(Set left, Set right, Set exp)
        {
            IInterpreterSet parent = Substitute.For<IInterpreterSet>();
            SetHelper setHelper = SetUpHelper(parent);
            SetExpression leftExpr = new SetExpression(null, null, null, 0, 0);
            SetExpression rightExpr = new SetExpression(null, null, null, 0, 0);
            IntersectionExpression intersectionExpr = new IntersectionExpression(leftExpr, rightExpr, 0, 0);
            parent.DispatchSet(leftExpr, Arg.Any<List<object>>()).Returns(left);
            parent.DispatchSet(rightExpr, Arg.Any<List<object>>()).Returns(right);

            Set result = setHelper.IntersectionSet(intersectionExpr, new List<object>());

            result.Should().BeEquivalentTo(exp);
        }

        static IEnumerable<object[]> IntersectionSet_TestDataMethod()
        {
            return new[]
            {
                new[]
                {
                    new Set(new List<Element> { new Element(0), new Element(1), new Element(2), new Element(3) }),
                    new Set(new List<Element> { new Element(2), new Element(3), new Element(4), new Element(5) }),
                    new Set(new List<Element> { new Element(2), new Element(3) })
                },
                new[]
                {
                    new Set(new List<Element> { new Element(new List<int> { 0, 1 }), new Element(new List<int> { 2, 3 }),
                                                new Element(new List<int> { 4, 5 }), new Element(new List<int> { 6, 7 }) }),
                    new Set(new List<Element> { new Element(new List<int> { 0, 1 }), new Element(new List<int> { 3, 2 }),
                                                new Element(new List<int> { 5, 4 }), new Element(new List<int> { 6, 7 }) }),
                    new Set(new List<Element> { new Element(new List<int> { 0, 1 }), new Element(new List<int> { 6, 7 }) })
                },
                new[]
                {
                    new Set(new List<Element> { new Element(0), new Element(1), new Element(2) }),
                    new Set(new List<Element> { new Element(3), new Element(4), new Element(5), new Element(6), new Element(7) }),
                    new Set()
                },
                new[]
                {
                    new Set(new List<Element> { new Element(1), new Element(new List<int> { 1, 2 }), new Element(new List<int> { 1, 2, 3 }) }),
                    new Set(new List<Element> { new Element(0), new Element(new List<int> { 1, 2 }), new Element(new List<int> { 1, 2, 3 }) }),
                    new Set(new List<Element> { new Element(new List<int> { 1, 2 }), new Element(new List<int> { 1, 2, 3 }) }),
                }
            };
        }

        #endregion
    }
}
