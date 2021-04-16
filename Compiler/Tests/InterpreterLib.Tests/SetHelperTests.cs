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
using System.Runtime.CompilerServices;
using ASTLib.Nodes.ExpressionNodes.OperationNodes;

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
            List<Element> expectedElements = new List<Element>();
            for (int i = 0; i < indexPairs.Count; i++)
            {
                expectedElements.Add(new Element(indexPairs[i].ConvertAll(x => (int) x)));
            }


            parent.DispatchBoolean(lessExpr, Arg.Any<List<Object>>()).Returns(x => IsContained(indexPairs , (List<Object>)x[1]));

            Set result = setHelper.SetExpression(setExpr, new List<object>());
            Set expected = new Set(expectedElements);

            result.Should().BeEquivalentTo(expected);
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

        private Set getSetFrom2dArray(int[,] arr)
        {
            List<Element> elements = new List<Element>();

            for (int i = 0; i < arr.GetLength(0); i++)
            {
                List<int> indices = new List<int>();

                for (int j = 0; j < arr.GetLength(1); j++)
                {
                    indices.Add(arr[i, j]);
                }

                elements.Add(new Element(indices));
            }

            return new Set(elements);
        }

        [DataTestMethod]
        [DynamicData(nameof(IntersectionSet_TestDataMethod), DynamicDataSourceType.Method)]
        public void IntersectionSet_f_f(int[,] left, int[,] right, int[,] exp)
        {
            IInterpreterSet parent = Substitute.For<IInterpreterSet>();
            SetHelper setHelper = SetUpHelper(parent);
            SetExpression leftExpr = new SetExpression(null, null, null, 0, 0);
            SetExpression rightExpr = new SetExpression(null, null, null, 0, 0);
            IntersectionExpression intersectionExpr = new IntersectionExpression(leftExpr, rightExpr, 0, 0);
            parent.DispatchSet(leftExpr, Arg.Any<List<object>>()).Returns(getSetFrom2dArray(left));
            parent.DispatchSet(rightExpr, Arg.Any<List<object>>()).Returns(getSetFrom2dArray(right));

            Set result = setHelper.IntersectionSet(intersectionExpr, new List<object>());

            result.Should().BeEquivalentTo(getSetFrom2dArray(exp));
        }

        static IEnumerable<object[]> IntersectionSet_TestDataMethod()
        {
            return new[]
            {
                new[] 
                {
                    new int[4, 1] { { 0 }, { 1 }, { 2 }, { 3 } },
                    new int[4, 1] { { 2 }, { 3 }, { 4 }, { 5 } },
                    new int[2, 1] { { 2 }, { 3 } }
                },
                new[]
                {
                    new int[4, 2] { { 0, 1 }, { 2, 3 }, { 4, 5 }, { 6, 7 } },
                    new int[4, 2] { { 0, 1 }, { 3, 2 }, { 5, 4 }, { 6, 7 } },
                    new int[2, 2] { { 0, 1 }, { 6, 7 } }
                },
                new[]
                {
                    new int[3, 1] { { 0 }, { 1 }, { 2 } },
                    new int[5, 1] { { 3 }, { 4 }, { 5 }, { 6 }, { 7 } },
                    new int[0, 0]
                }
            };
        }

        #endregion
        
        
        #region UnionSet
        /*
            Cases:
            * One set is empty
            * Both sets have same 
                - Disjoint
                - Equal
                - Some common elements
            * One set is longer than the other
                - Disjoint
                    - rhs is longer
                    - lhs is longer
                - Some common elements
                    - rhs is longer
                    - lhs is longer                    
            * Test that it can handle Set with elements that differ in number of indices  
        */
        
        [DataTestMethod]
        [DynamicData(nameof(SetWithDifferentLength_Disjoint_TestDataMethod), DynamicDataSourceType.Method)]
        [DynamicData(nameof(SetWithDifferentLength_SomeCommonElements_TestDataMethod), DynamicDataSourceType.Method)]
        public void UnionSet_NotSameLength_(Set left, Set right, Set expected)
        {
            IInterpreterSet parent = Substitute.For<IInterpreterSet>();
            SetHelper setHelper = SetUpHelper(parent);
            SetExpression leftExpr = new SetExpression(null, null, null, 0, 0);
            SetExpression rightExpr = new SetExpression(null, null, null, 0, 0);
            UnionExpression intersectionExpr = new UnionExpression(leftExpr, rightExpr, 0, 0);
            parent.DispatchSet(leftExpr, Arg.Any<List<object>>()).Returns(left);
            parent.DispatchSet(rightExpr, Arg.Any<List<object>>()).Returns(right);

            Set result = setHelper.UnionSet(intersectionExpr, new List<object>());

            result.Should().BeEquivalentTo(expected);
        }

        static IEnumerable<object[]> SetWithDifferentLength_Disjoint_TestDataMethod()
        {
            return new[] {
                new[] 
                { 
                    CreateSet(new List<Element>{ new Element(1) } ),
                    CreateSet(new List<Element>{ new Element(3), new Element(4) } ),
                    CreateSet(new List<Element>{ new Element(1), new Element(3), new Element(4) } ),
                },
                new[] 
                {
                    CreateSet(new List<Element>{ new Element(3), new Element(4) } ),
                    CreateSet(new List<Element>{ new Element(1) } ),
                    CreateSet(new List<Element>{ new Element(1), new Element(3), new Element(4) } ),
                },
            };
        }
        
        static IEnumerable<object[]> SetWithDifferentLength_SomeCommonElements_TestDataMethod()
        {
            return new[] {
                new[] 
                { 
                    CreateSet(new List<Element>{ new Element(1) } ),
                    CreateSet(new List<Element>{ new Element(1), new Element(4) } ),
                    CreateSet(new List<Element>{ new Element(1), new Element(4) } ),
                },
                new[] 
                {
                    CreateSet(new List<Element>{ new Element(1), new Element(4) } ),
                    CreateSet(new List<Element>{ new Element(1) } ),
                    CreateSet(new List<Element>{ new Element(1), new Element(4) } ),
                },
            };
        }
        
        [DataTestMethod]
        [DynamicData(nameof(SetWithSameLength_Disjoint_TestDataMethod), DynamicDataSourceType.Method)]
        [DynamicData(nameof(SetWithSameLength_SomeCommonElements_TestDataMethod), DynamicDataSourceType.Method)]
        [DynamicData(nameof(SetWithSameLength_Equal_TestDataMethod), DynamicDataSourceType.Method)]
        public void UnionSet_SameLength_(Set left, Set right, Set expected)
        {
            IInterpreterSet parent = Substitute.For<IInterpreterSet>();
            SetHelper setHelper = SetUpHelper(parent);
            SetExpression leftExpr = new SetExpression(null, null, null, 0, 0);
            SetExpression rightExpr = new SetExpression(null, null, null, 0, 0);
            UnionExpression intersectionExpr = new UnionExpression(leftExpr, rightExpr, 0, 0);
            parent.DispatchSet(leftExpr, Arg.Any<List<object>>()).Returns(left);
            parent.DispatchSet(rightExpr, Arg.Any<List<object>>()).Returns(right);

            Set result = setHelper.UnionSet(intersectionExpr, new List<object>());

            result.Should().BeEquivalentTo(expected);
        }
        
        static IEnumerable<object[]> SetWithSameLength_Disjoint_TestDataMethod()
        {
            return new[] {
                new[] 
                { 
                    CreateSet(new List<Element>{ new Element(1), new Element(2) } ),
                    CreateSet(new List<Element>{ new Element(3), new Element(4) } ),
                    CreateSet(new List<Element>{ new Element(1), new Element(2), new Element(3), new Element(4) } ),
                },
                new[] 
                {
                    CreateSet(new List<Element>{ new Element(3), new Element(4) } ),
                    CreateSet(new List<Element>{ new Element(1), new Element(2) } ),
                    CreateSet(new List<Element>{ new Element(1),  new Element(2), new Element(3), new Element(4) } ),
                },
            };
        }
        
        static IEnumerable<object[]> SetWithSameLength_SomeCommonElements_TestDataMethod()
        {
            return new[] {
                new[] 
                { 
                    CreateSet(new List<Element>{ new Element(1), new Element(3) } ),
                    CreateSet(new List<Element>{ new Element(3), new Element(4) } ),
                    CreateSet(new List<Element>{ new Element(1), new Element(3), new Element(4) }),
                },
                new[] 
                {
                    CreateSet(new List<Element>{ new Element(3), new Element(4) } ),
                    CreateSet(new List<Element>{ new Element(1), new Element(3) } ),
                    CreateSet(new List<Element>{ new Element(1), new Element(3), new Element(4) } ),
                },
            };
        }
        
        static IEnumerable<object[]> SetWithSameLength_Equal_TestDataMethod()
        {
            return new[] {
                new[] 
                { 
                    CreateSet(new List<Element>{ new Element(1), new Element(2), new Element(3) } ),
                    CreateSet(new List<Element>{ new Element(1), new Element(2), new Element(3) } ),
                    CreateSet(new List<Element>{ new Element(1), new Element(2), new Element(3) } ),
                }
            };
        }
        
        [DataTestMethod]
        [DynamicData(nameof(SetWhereOneIsEmpty_TestDataMethod), DynamicDataSourceType.Method)]
        public void UnionSet_OneIsEmpty_(Set left, Set right, Set expected)
        {
            IInterpreterSet parent = Substitute.For<IInterpreterSet>();
            SetHelper setHelper = SetUpHelper(parent);
            SetExpression leftExpr = new SetExpression(null, null, null, 0, 0);
            SetExpression rightExpr = new SetExpression(null, null, null, 0, 0);
            UnionExpression intersectionExpr = new UnionExpression(leftExpr, rightExpr, 0, 0);
            parent.DispatchSet(leftExpr, Arg.Any<List<object>>()).Returns(left);
            parent.DispatchSet(rightExpr, Arg.Any<List<object>>()).Returns(right);

            Set result = setHelper.UnionSet(intersectionExpr, new List<object>());

            result.Should().BeEquivalentTo(expected);
        }
        
        static IEnumerable<object[]> SetWhereOneIsEmpty_TestDataMethod()
        {
            return new[] {
                new[] 
                { 
                    CreateSet(new List<Element>{  } ),
                    CreateSet(new List<Element>{ new Element(3), new Element(4) } ),
                    CreateSet(new List<Element>{ new Element(3), new Element(4) } ),
                },
                new[] 
                {
                    CreateSet(new List<Element>{ new Element(3), new Element(4) } ),
                    CreateSet(new List<Element>{  } ),
                    CreateSet(new List<Element>{ new Element(3), new Element(4) } ),
                },
            };
        }
        
        [DataTestMethod]
        [DynamicData(nameof(SetsWithVaryingNumberOfElementIndices_TestDataMethod), DynamicDataSourceType.Method)]
        public void UnionSet_SetsWithVaryingNumberOfElementIndices_(Set left, Set right, Set expected)
        {
            IInterpreterSet parent = Substitute.For<IInterpreterSet>();
            SetHelper setHelper = SetUpHelper(parent);
            SetExpression leftExpr = new SetExpression(null, null, null, 0, 0);
            SetExpression rightExpr = new SetExpression(null, null, null, 0, 0);
            UnionExpression intersectionExpr = new UnionExpression(leftExpr, rightExpr, 0, 0);
            parent.DispatchSet(leftExpr, Arg.Any<List<object>>()).Returns(left);
            parent.DispatchSet(rightExpr, Arg.Any<List<object>>()).Returns(right);

            Set result = setHelper.UnionSet(intersectionExpr, new List<object>());

            result.Should().BeEquivalentTo(expected);
        }
        
        static IEnumerable<object[]> SetsWithVaryingNumberOfElementIndices_TestDataMethod()
        {
            return new[] {
                new[] 
                { 
                    CreateSet(new List<Element>{ new Element(2), new Element(new List<int>(){1, 2}), 
                                                         new Element(new List<int>(){1, 4}) } ),
                    CreateSet(new List<Element>{ new Element(1), new Element(new List<int>(){1, 2}), 
                                                         new Element(new List<int>(){1, 2, 3}), new Element(new List<int>(){4, 2, 3})  } ),
                    CreateSet(new List<Element>
                    { 
                         new Element(1), new Element(2), 
                         new Element(new List<int>(){1, 2}), new Element(new List<int>(){1, 4}),
                         new Element(new List<int>(){1, 2, 3}), new Element(new List<int>(){4, 2, 3}) 
                    } ),
                    
                }
            };
        }
        
        private static Set CreateSet(List<Element> elements)
        {
            return new Set(elements);
        }

        #endregion

        #region SubtractionSet
        [DataTestMethod]
        [DynamicData(nameof(SubtractionSet_TestDataMethod), DynamicDataSourceType.Method)]
        public void SubtractionSet_f_f(int[,] left, int[,] right, int[,] expected)
        {
            IInterpreterSet parent = Substitute.For<IInterpreterSet>();
            SetHelper setHelper = SetUpHelper(parent);
            SetExpression lhsExpr = new SetExpression(null, null, null, 1, 1);
            SetExpression rhsExpr = new SetExpression(null, null, null, 1, 1);
            SubtractionExpression expr = new SubtractionExpression(lhsExpr, rhsExpr, 0, 0);
            parent.DispatchSet(lhsExpr, Arg.Any<List<object>>()).Returns(getSetFrom2dArray(left));
            parent.DispatchSet(rhsExpr, Arg.Any<List<object>>()).Returns(getSetFrom2dArray(right));

            Set res = setHelper.SubtractionSet(expr, new List<object>());

            res.Should().BeEquivalentTo(getSetFrom2dArray(expected));
        }

        static IEnumerable<object[]> SubtractionSet_TestDataMethod()
        {
            return new[]
            {
                new[]
                {
                    new int[4, 1] { { 1 }, { 2 }, { 3 }, { 4 } },
                    new int[4, 1] { { 1 }, { 2 }, { 4 }, { 5 } },
                    new int[1, 1] { { 3 } }
                },
                new[]
                {
                    new int[4, 2] { { 0, 1 }, { 2, 3 }, { 4, 5 }, { 6, 7 } },
                    new int[4, 2] { { 0, 1 }, { 3, 2 }, { 5, 4 }, { 6, 7 } },
                    new int[2, 2] { { 2, 3 }, { 4, 5 } }
                },
                new[]
                {
                    new int[3, 1] { { 3 }, { 4 }, { 5 } },
                    new int[4, 1] { { 3 }, { 4 }, { 5 }, { 6 } },
                    new int[0, 0] {}
                }
            };
        }
        #endregion
    }
}
