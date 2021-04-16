using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ASTLib.Exceptions;
using ASTLib.Interfaces;
using ASTLib.Nodes;
using ASTLib.Nodes.ExpressionNodes;
using ASTLib.Nodes.ExpressionNodes.CommonOperationNodes.ElementAndSetOperations;
using ASTLib.Nodes.ExpressionNodes.CommonOperationNodes.RelationalOperationNodes;
using ASTLib.Nodes.ExpressionNodes.SetOperationNodes;
using ASTLib.Nodes.TypeNodes;
using ASTLib.Objects;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using TypeCheckerLib.Helpers;
using TypeCheckerLib.Interfaces;

namespace TypeCheckerLib.Tests.HelperTests
{
    [TestClass]
    public class SetHelperTests
    {
        private UnionExpression GetUnionExpression(TypeEnum left, TypeEnum right)
        {
            ExpressionNode leftNode = GetSet();
            ExpressionNode rightNode = GetSet();
            var node = new UnionExpression(leftNode, rightNode, 0, 0);
            return node;
        }

        private SubsetExpression GetSubsetExpression(TypeEnum left, TypeEnum right)
        {
            ExpressionNode leftNode = GetSet();
            ExpressionNode rightNode = GetSet();
            var node = new SubsetExpression(leftNode, rightNode, 0, 0);
            return node;
        }

        private SetExpression GetSet()
        {
            ElementNode element = GetElement();
            List<BoundNode> bounds = GetBounds();
            ExpressionNode predicate = GetPredicate();
            var node = new SetExpression(element, bounds, predicate, 0, 0);
            return node;
        }

        private ElementNode GetElement()
        {
            var node = new ElementNode("e", new List<string>() { }, 0, 0);
            return node;
        }

        private List<BoundNode> GetBounds()
        {
            ExpressionNode smallest = new IntegerLiteralExpression("0", 0, 0);
            ExpressionNode largest = new IntegerLiteralExpression("5", 0, 0);
            BoundNode node1 = new BoundNode("i", smallest, largest, 0, 0);
            List<BoundNode> bounds = new List<BoundNode>() { node1 } ;
            return bounds;
        }

        private ExpressionNode GetPredicate()
        {
            LessExpression predicate = GetLessExpression(TypeEnum.Integer, TypeEnum.Integer);
            return predicate;
        }

        private LessExpression GetLessExpression(TypeEnum left, TypeEnum right)
        {
            ExpressionNode leftNode = GetLiteral(left);
            ExpressionNode rightNode = GetLiteral(right);
            var node = new LessExpression(leftNode, rightNode, 0, 0);
            return node;
        }

        private ExpressionNode GetLiteral(TypeEnum type)
        {
            return type switch
            {
                TypeEnum.Real => new RealLiteralExpression("2.2", 0, 0),
                TypeEnum.Boolean => new BooleanLiteralExpression(true, 0, 0),
                TypeEnum.Integer => new IntegerLiteralExpression("1", 1, 1),
                TypeEnum.Function => throw new Exception("Functions is not supported in GetBinaryOperator"),
                _ => throw new NotImplementedException()
            };
        }

        #region VisitBinarySetOp
        [TestMethod]
        public void VisitBinarySetOp_UnionExpressionWithSetAndSet_ReturnsSetTypeNode()
        {
            var expected = TypeEnum.Set;
            IBinarySetOperator input1 = GetUnionExpression(TypeEnum.Set, TypeEnum.Set);

            SetHelper helper = Utilities.GetHelper<SetHelper>();
            var res = helper.VisitBinarySetOp(input1, null).Type;

            Assert.AreEqual(expected, res);
        }

        [TestMethod]
        [ExpectedException(typeof(UnmatchableTypesException))]
        public void VisitBinarySetOp_UnionExpressionWithIntAndBoolChilds_ThrowsException()
        {
            UnionExpression union = new UnionExpression(GetLiteral(TypeEnum.Integer), GetLiteral(TypeEnum.Boolean), 0, 0);
            
            SetHelper helper = Utilities.GetHelper<SetHelper>();
            helper.VisitBinarySetOp(union, null);
        }

        [TestMethod]
        [ExpectedException(typeof(UnmatchableTypesException))]
        public void VisitBinarySetOp_UnionExpressionWithIntAndSetChilds_ThrowsException()
        {
            UnionExpression union = new UnionExpression(GetLiteral(TypeEnum.Integer), GetSet(), 0, 0);

            SetHelper helper = Utilities.GetHelper<SetHelper>();
            helper.VisitBinarySetOp(union, null);
        }
        #endregion

        #region VisitSubset
        [TestMethod]
        public void VisitSubset_SubsetExpressionWithSetAndSet_ReturnsBooleanTypeNode()
        {
            var expected = TypeEnum.Boolean;
            SubsetExpression input1 = GetSubsetExpression(TypeEnum.Set, TypeEnum.Set);

            SetHelper helper = Utilities.GetHelper<SetHelper>();
            var res = helper.VisitSubset(input1, null).Type;

            Assert.AreEqual(expected, res);
        }

        [TestMethod]
        [ExpectedException(typeof(UnmatchableTypesException))]
        public void VisitSubset_SubsetExpressionWithIntAndSetChilds_ThrowsException()
        {
            SubsetExpression subset = new SubsetExpression(GetLiteral(TypeEnum.Integer), GetSet(), 0, 0);

            SetHelper helper = Utilities.GetHelper<SetHelper>();
            helper.VisitSubset(subset, null);
        }

        [TestMethod]
        [ExpectedException(typeof(UnmatchableTypesException))]
        public void VisitSubset_SubsetExpressionWithSetAndRealChilds_ThrowsException()
        {
            SubsetExpression subset = new SubsetExpression(GetSet(), GetLiteral(TypeEnum.Real), 0, 0);

            SetHelper helper = Utilities.GetHelper<SetHelper>();
            helper.VisitSubset(subset, null);
        }
        #endregion

        [TestMethod]
        [ExpectedException(typeof(InvalidSetTypeException))]
        public void VisitSet_SetExpression_ThrowsExceptionBoundsWrongType()
        {
            ExpressionNode smallestValue = new IntegerLiteralExpression(0, 0, 0);
            ExpressionNode biggestValue = new RealLiteralExpression(0, 0, 0);
            ExpressionNode predicate = new BooleanLiteralExpression(false, 0, 0);
            SetExpression set = new SetExpression(null, 
                                   new List<BoundNode> { 
                                   new BoundNode("", smallestValue, biggestValue, 0,0) },
                                   predicate,
                                   0, 0);

            SetHelper helper = Utilities.GetHelper<SetHelper>();

            helper.VisitSet(set, new List<TypeNode>());
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidSetTypeException))]
        public void VisitSet_SetExpression_ThrowsExceptionPredicateWrongType()
        {
            ExpressionNode smallestValue = new IntegerLiteralExpression(0, 0, 0);
            ExpressionNode biggestValue = new IntegerLiteralExpression(0, 0, 0);
            ExpressionNode predicate = new RealLiteralExpression(0.0, 0, 0);
            SetExpression set = new SetExpression(null,
                                   new List<BoundNode> {
                                   new BoundNode("", smallestValue, biggestValue, 0,0) },
                                   predicate,
                                   0, 0);

            SetHelper helper = Utilities.GetHelper<SetHelper>();

            helper.VisitSet(set, new List<TypeNode>());
        }

        [TestMethod]
        public void VisitSet_SetExpression_ReturnsCorrectType()
        {
            ExpressionNode smallestValue = new IntegerLiteralExpression(0, 0, 0);
            ExpressionNode biggestValue = new IntegerLiteralExpression(0, 0, 0);
            ExpressionNode predicate = new BooleanLiteralExpression(false, 0, 0);
            SetExpression set = new SetExpression(null,
                                   new List<BoundNode> {
                                   new BoundNode("", smallestValue, biggestValue, 0,0) },
                                   predicate,
                                   0, 0);

            SetHelper helper = Utilities.GetHelper<SetHelper>();

            TypeNode type = helper.VisitSet(set, new List<TypeNode>());

            Assert.AreEqual(TypeEnum.Set, type.Type);
        }

        [TestMethod]
        public void VisitSet_SetExpression_PassesCorrectListToBounds()
        {
            ExpressionNode smallestValue = new IntegerLiteralExpression(0, 0, 0);
            ExpressionNode biggestValue = new IntegerLiteralExpression(0, 0, 0);
            ExpressionNode predicate = new BooleanLiteralExpression(false, 0, 0);
            SetExpression set = new SetExpression(null,
                                   new List<BoundNode> {
                                   new BoundNode("", smallestValue, biggestValue, 0,0) },
                                   predicate,
                                   0, 0);

            ITypeChecker typeChecker = Substitute.For<ITypeChecker>();
            List<TypeNode> result = null;
            List<TypeNode> input = new List<TypeNode>() { new TypeNode(TypeEnum.Function,0,0) };
            List<TypeNode> expected = input;
            typeChecker.Dispatch(Arg.Any<IntegerLiteralExpression>(), 
                                 Arg.Do<List<TypeNode>>(x => result = x)).
                                    Returns(new TypeNode(TypeEnum.Integer, 0, 0));
            typeChecker.Dispatch(Arg.Any<BooleanLiteralExpression>(), 
                                 Arg.Any<List<TypeNode>>()).
                                    Returns(new TypeNode(TypeEnum.Boolean, 0, 0)); ;
            SetHelper helper = new SetHelper();
            helper.Initialize(Utilities.GetAst(), typeChecker.Dispatch);

            helper.VisitSet(set, input);

            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void VisitSet_SetExpression_PassesCorrectListToPredicate()
        {
            ExpressionNode smallestValue = new IntegerLiteralExpression(0, 0, 0);
            ExpressionNode biggestValue = new IntegerLiteralExpression(0, 0, 0);
            ExpressionNode predicate = new BooleanLiteralExpression(false, 0, 0);
            SetExpression set = new SetExpression(null,
                                   new List<BoundNode> {
                                   new BoundNode("", smallestValue, biggestValue, 0,0) },
                                   predicate,
                                   0, 0);

            ITypeChecker typeChecker = Substitute.For<ITypeChecker>();
            List<TypeNode> result = null;
            List<TypeNode> input = new List<TypeNode>() { new TypeNode(TypeEnum.Function, 0, 0) };
            List<TypeNode> expected = new List<TypeNode>() { new TypeNode(TypeEnum.Function, 0, 0),
                                                             new TypeNode(TypeEnum.Element, 0,0),
                                                             new TypeNode(TypeEnum.Integer, 0, 0)};
            typeChecker.Dispatch(Arg.Any<BooleanLiteralExpression>(), 
                                 Arg.Do<List<TypeNode>>(x => result = x.ToList())).
                                    Returns(new TypeNode(TypeEnum.Boolean, 0, 0));
            typeChecker.Dispatch(Arg.Any<IntegerLiteralExpression>(), 
                                 Arg.Any<List<TypeNode>>()).
                                    Returns(new TypeNode(TypeEnum.Integer, 0, 0));
            SetHelper helper = new SetHelper();
            helper.Initialize(Utilities.GetAst(), typeChecker.Dispatch);

            helper.VisitSet(set, input);

            result.Should().BeEquivalentTo(expected);
        }

        [TestMethod]
        public void VisitSet_SetExpression_PassedListUnaltered()
        {
            ExpressionNode smallestValue = new IntegerLiteralExpression(0, 0, 0);
            ExpressionNode biggestValue = new IntegerLiteralExpression(0, 0, 0);
            ExpressionNode predicate = new BooleanLiteralExpression(false, 0, 0);
            SetExpression set = new SetExpression(null,
                                   new List<BoundNode> {
                                   new BoundNode("", smallestValue, biggestValue, 0,0) },
                                   predicate,
                                   0, 0);
            SetHelper helper = Utilities.GetHelper<SetHelper>();
            List<TypeNode> expected = new List<TypeNode>();
            List<TypeNode> input = new List<TypeNode>();

            helper.VisitSet(set, input);

            input.Should().BeEquivalentTo(expected);
        }

    }
}
