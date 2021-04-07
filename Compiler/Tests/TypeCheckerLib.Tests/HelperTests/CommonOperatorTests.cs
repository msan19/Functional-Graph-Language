using ASTLib.Interfaces;
using ASTLib.Nodes.ExpressionNodes;
using ASTLib.Nodes.ExpressionNodes.CommonOperationNodes.RelationalOperationNodes;
using ASTLib.Nodes.ExpressionNodes.OperationNodes;
using ASTLib.Nodes.TypeNodes;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using TypeCheckerLib.Helpers;
using TypeCheckerLib.Interfaces;

namespace TypeCheckerLib.Tests.HelperTests
{
    [TestClass]
    public class CommonOperatorTests
    {
        private AdditionExpression GetAdditionExpression(TypeEnum left, TypeEnum right)
        {
            ExpressionNode leftNode = GetLiteral(left);
            ExpressionNode rightNode = GetLiteral(right);
            var node = new AdditionExpression(leftNode, rightNode, 0, 0);
            return node;
        }

        private SubtractionExpression GetSubtractionExpression(TypeEnum left, TypeEnum right)
        {
            ExpressionNode leftNode = GetLiteral(left);
            ExpressionNode rightNode = GetLiteral(right);
            var node = new SubtractionExpression(leftNode, rightNode, 0, 0);
            return node;
        }

        private AbsoluteValueExpression GetAbsoluteValueExpression(TypeEnum type)
        {
            ExpressionNode literal = GetLiteral(type);
            var node = new AbsoluteValueExpression(literal, 0, 0);
            return node;
        }

        private GreaterEqualExpression GetGreaterEqualExpression(TypeEnum left, TypeEnum right)
        {
            ExpressionNode leftNode = GetLiteral(left);
            ExpressionNode rightNode = GetLiteral(right);
            var node = new GreaterEqualExpression(leftNode, rightNode, 0, 0);
            return node;
        }

        private ExpressionNode GetLiteral(TypeEnum type)
        {
            return type switch
            {
                TypeEnum.Real => new RealLiteralExpression("2.2", 0, 0),
                TypeEnum.Integer => new IntegerLiteralExpression("1", 1, 1),
                TypeEnum.Function => throw new Exception("Functions is not supported in GetBinaryOperator"),
                _ => throw new NotImplementedException()
            };
        }

        #region VisitAddition
        [TestMethod]
        public void VisitAddition__CorrectParameterPassDown()
        {
            var expected = new List<TypeNode>()
            {
                Utilities.GetFunctionType(TypeEnum.Integer, new List<TypeEnum>() {TypeEnum.Integer})
            };
            AdditionExpression input1 = GetAdditionExpression(TypeEnum.Integer, TypeEnum.Real);

            List<TypeNode> res = null;
            ITypeChecker parent = Substitute.For<ITypeChecker>();
            parent.Dispatch(Arg.Any<RealLiteralExpression>(), Arg.Do<List<TypeNode>>(x => res = x)).Returns(new TypeNode(TypeEnum.Real, 1, 1));
            parent.Dispatch(Arg.Any<IntegerLiteralExpression>(), Arg.Any<List<TypeNode>>()).Returns(new TypeNode(TypeEnum.Integer, 1, 1));
            CommonOperatorHelper helper = Utilities.GetHelper<CommonOperatorHelper>(parent);

            helper.VisitAddition(input1, expected.ToList());

            res.Should().BeEquivalentTo(expected);
        }

        // Int Real -> Cast Node
        // Int Real -> Append Int to Cast Node
        // Int Int  -> Still ints as children
        // Int Real -> Return Real
        // Int Int  -> Return Int
        // Int Func -> Throw Error 

        [TestMethod]
        public void VisitAddition_AdditionExpressionWithIntAndReal_InsertedIntToRealCastNode()
        {
            var expected = typeof(CastFromIntegerExpression);
            AdditionExpression input1 = GetAdditionExpression(TypeEnum.Integer, TypeEnum.Real);

            CommonOperatorHelper helper = Utilities.GetHelper<CommonOperatorHelper>();
            helper.VisitAddition(input1, null);
            var res = input1.Children[0].GetType();

            Assert.AreEqual(expected, res);
        }

        [TestMethod]
        public void VisitAddition_AdditionExpressionWithIntAndReal_AppendedIntNodeToTypeCast()
        {
            var expected = typeof(IntegerLiteralExpression);
            AdditionExpression input1 = GetAdditionExpression(TypeEnum.Integer, TypeEnum.Real);

            CommonOperatorHelper helper = Utilities.GetHelper<CommonOperatorHelper>();
            helper.VisitAddition(input1, null);
            var res = input1.Children[0].Children[0].GetType();

            Assert.AreEqual(expected, res);
        }

        [TestMethod]
        public void VisitAddition_AdditionExpressionWithTwoInt_LeftNodeIsReal()
        {
            var expected = typeof(IntegerLiteralExpression);
            AdditionExpression input1 = GetAdditionExpression(TypeEnum.Integer, TypeEnum.Integer);

            CommonOperatorHelper helper = Utilities.GetHelper<CommonOperatorHelper>();
            helper.VisitAddition(input1, null);
            var res = input1.Children[0].GetType();

            Assert.AreEqual(expected, res);
        }

        [TestMethod]
        public void VisitAddition_AdditionExpressionWithTwoInt_RightNodeIsReal()
        {
            var expected = typeof(IntegerLiteralExpression);
            AdditionExpression input1 = GetAdditionExpression(TypeEnum.Integer, TypeEnum.Integer);

            CommonOperatorHelper helper = Utilities.GetHelper<CommonOperatorHelper>();
            helper.VisitAddition(input1, null);
            var res = input1.Children[1].GetType();

            Assert.AreEqual(expected, res);
        }

        [TestMethod]
        public void VisitAddition_AdditionExpressionWithIntAndReal_ReturnsRealTypeNode()
        {
            var expected = TypeEnum.Real;
            AdditionExpression input1 = GetAdditionExpression(TypeEnum.Integer, TypeEnum.Real);

            CommonOperatorHelper helper = Utilities.GetHelper<CommonOperatorHelper>();
            var res = helper.VisitAddition(input1, null).Type;

            Assert.AreEqual(expected, res);
        }

        [TestMethod]
        public void VisitAddition_AdditionExpressionWithTwoInt_ReturnsIntTypeNode()
        {
            var expected = TypeEnum.Integer;
            AdditionExpression input1 = GetAdditionExpression(TypeEnum.Integer, TypeEnum.Integer);

            CommonOperatorHelper helper = Utilities.GetHelper<CommonOperatorHelper>();
            var res = helper.VisitAddition(input1, null).Type;

            Assert.AreEqual(expected, res);
        }

        // Int Func -> Throw Error 
        [TestMethod]
        [ExpectedException(typeof(ASTLib.Exceptions.UnmatchableTypesException))]
        public void VisitAddition_AdditionExpressionWithIntAndFunc_ThrowsException()
        {

            IntegerLiteralExpression intLit1 = new IntegerLiteralExpression("1", 1, 1);
            IdentifierExpression func = new IdentifierExpression("f", 0, 0);
            AdditionExpression input1 = new AdditionExpression(intLit1, func, 1, 1);
            ITypeChecker parent = Substitute.For<ITypeChecker>();
            parent.Dispatch(Arg.Any<IntegerLiteralExpression>(), Arg.Any<List<TypeNode>>()).Returns(new TypeNode(TypeEnum.Integer, 1, 1));
            parent.Dispatch(Arg.Any<IdentifierExpression>(), Arg.Any<List<TypeNode>>()).Returns(new TypeNode(TypeEnum.Function, 1, 1));
            CommonOperatorHelper helper = Utilities.GetHelper<CommonOperatorHelper>(parent);


            helper.VisitAddition(input1, null);
        }

        #endregion

        #region VisitSubtraction
        [TestMethod]
        public void VisitSubtraction__CorrectParameterPassDown()
        {
            var expected = new List<TypeNode>()
            {
                Utilities.GetFunctionType(TypeEnum.Integer, new List<TypeEnum>() {TypeEnum.Integer})
            };
            SubtractionExpression input1 = GetSubtractionExpression(TypeEnum.Integer, TypeEnum.Real);

            List<TypeNode> res = null;
            ITypeChecker parent = Substitute.For<ITypeChecker>();
            parent.Dispatch(Arg.Any<RealLiteralExpression>(), Arg.Do<List<TypeNode>>(x => res = x)).Returns(new TypeNode(TypeEnum.Real, 1, 1));
            parent.Dispatch(Arg.Any<IntegerLiteralExpression>(), Arg.Any<List<TypeNode>>()).Returns(new TypeNode(TypeEnum.Integer, 1, 1));
            CommonOperatorHelper helper = Utilities.GetHelper<CommonOperatorHelper>(parent);

            helper.VisitSubtraction(input1, expected.ToList());

            res.Should().BeEquivalentTo(expected);
        }

        // Int Real -> Cast Node
        // Int Real -> Append Int to Cast Node
        // Int Int  -> Still ints as children
        // Int Real -> Return Real
        // Int Int  -> Return Int
        // Int Func -> Throw Error 

        [TestMethod]
        public void VisitSubtraction_SubtractionExpressionWithIntAndReal_InsertedIntToRealCastNode()
        {
            var expected = typeof(CastFromIntegerExpression);
            SubtractionExpression input1 = GetSubtractionExpression(TypeEnum.Integer, TypeEnum.Real);

            CommonOperatorHelper helper = Utilities.GetHelper<CommonOperatorHelper>();
            helper.VisitSubtraction(input1, null);
            var res = input1.Children[0].GetType();

            Assert.AreEqual(expected, res);
        }

        [TestMethod]
        public void VisitSubtraction_SubtractionExpressionWithIntAndReal_AppendedIntNodeToTypeCast()
        {
            var expected = typeof(IntegerLiteralExpression);
            SubtractionExpression input1 = GetSubtractionExpression(TypeEnum.Integer, TypeEnum.Real);

            CommonOperatorHelper helper = Utilities.GetHelper<CommonOperatorHelper>();
            helper.VisitSubtraction(input1, null);
            var res = input1.Children[0].Children[0].GetType();

            Assert.AreEqual(expected, res);
        }

        [TestMethod]
        public void VisitSubtraction_SubtractionExpressionWithTwoInt_LeftNodeIsReal()
        {
            var expected = typeof(IntegerLiteralExpression);
            SubtractionExpression input1 = GetSubtractionExpression(TypeEnum.Integer, TypeEnum.Integer);

            CommonOperatorHelper helper = Utilities.GetHelper<CommonOperatorHelper>();
            helper.VisitSubtraction(input1, null);
            var res = input1.Children[0].GetType();

            Assert.AreEqual(expected, res);
        }

        [TestMethod]
        public void VisitSubtraction_SubtractionExpressionWithTwoInt_RightNodeIsReal()
        {
            var expected = typeof(IntegerLiteralExpression);
            SubtractionExpression input1 = GetSubtractionExpression(TypeEnum.Integer, TypeEnum.Integer);

            CommonOperatorHelper helper = Utilities.GetHelper<CommonOperatorHelper>();
            helper.VisitSubtraction(input1, null);
            var res = input1.Children[1].GetType();

            Assert.AreEqual(expected, res);
        }

        [TestMethod]
        public void VisitSubtraction_SubtractionExpressionWithIntAndReal_ReturnsRealTypeNode()
        {
            var expected = TypeEnum.Real;
            SubtractionExpression input1 = GetSubtractionExpression(TypeEnum.Integer, TypeEnum.Real);

            CommonOperatorHelper helper = Utilities.GetHelper<CommonOperatorHelper>();
            var res = helper.VisitSubtraction(input1, null).Type;

            Assert.AreEqual(expected, res);
        }

        [TestMethod]
        public void VisitSubtraction_SubtractionExpressionWithTwoInt_ReturnsIntTypeNode()
        {
            var expected = TypeEnum.Integer;
            SubtractionExpression input1 = GetSubtractionExpression(TypeEnum.Integer, TypeEnum.Integer);

            CommonOperatorHelper helper = Utilities.GetHelper<CommonOperatorHelper>();
            var res = helper.VisitSubtraction(input1, null).Type;

            Assert.AreEqual(expected, res);
        }

        // Int Func -> Throw Error 
        [TestMethod]
        [ExpectedException(typeof(ASTLib.Exceptions.UnmatchableTypesException))]
        public void VisitSubtraction_SubtractionExpressionWithIntAndFunc_ThrowsException()
        {

            IntegerLiteralExpression intLit1 = new IntegerLiteralExpression("1", 1, 1);
            IdentifierExpression func = new IdentifierExpression("f", 0, 0);
            SubtractionExpression input1 = new SubtractionExpression(intLit1, func, 1, 1);
            ITypeChecker parent = Substitute.For<ITypeChecker>();
            parent.Dispatch(Arg.Any<IntegerLiteralExpression>(), Arg.Any<List<TypeNode>>()).Returns(new TypeNode(TypeEnum.Integer, 1, 1));
            parent.Dispatch(Arg.Any<IdentifierExpression>(), Arg.Any<List<TypeNode>>()).Returns(new TypeNode(TypeEnum.Function, 1, 1));
            CommonOperatorHelper helper = Utilities.GetHelper<CommonOperatorHelper>(parent);


            helper.VisitSubtraction(input1, null);
        }

        #endregion

        [TestMethod]
        public void VisitAbsoluteValue_AbsoluteValueExpressionWithIntAndReal_ReturnsRealTypeNode()
        {
            var expected = TypeEnum.Real;
            AbsoluteValueExpression input1 = GetAbsoluteValueExpression(TypeEnum.Real);

            CommonOperatorHelper helper = Utilities.GetHelper<CommonOperatorHelper>();
            var res = helper.VisitAbsoluteValue(input1, null).Type;

            Assert.AreEqual(expected, res);
        }

        [TestMethod]
        public void VisitAbsoluteValue_AbsoluteValueExpressionWithTwoInt_ReturnsIntTypeNode()
        {
            var expected = TypeEnum.Integer;
            AbsoluteValueExpression input1 = GetAbsoluteValueExpression(TypeEnum.Integer);

            CommonOperatorHelper helper = Utilities.GetHelper<CommonOperatorHelper>();
            var res = helper.VisitAbsoluteValue(input1, null).Type;

            Assert.AreEqual(expected, res);
        }

        // Int Func -> Throw Error 
        [TestMethod]
        [ExpectedException(typeof(ASTLib.Exceptions.AbsoluteValueTypeException))]
        public void VisitAbsoluteValue_AbsoluteValueExpressionWithIntAndFunc_ThrowsException()
        {
            IdentifierExpression func = new IdentifierExpression("f", 0, 0);
            AbsoluteValueExpression input1 = new AbsoluteValueExpression(func, 1, 1);
            ITypeChecker parent = Substitute.For<ITypeChecker>();
            parent.Dispatch(Arg.Any<IntegerLiteralExpression>(), Arg.Any<List<TypeNode>>()).Returns(new TypeNode(TypeEnum.Integer, 1, 1));
            parent.Dispatch(Arg.Any<IdentifierExpression>(), Arg.Any<List<TypeNode>>()).Returns(new TypeNode(TypeEnum.Function, 1, 1));
            CommonOperatorHelper helper = Utilities.GetHelper<CommonOperatorHelper>(parent);


            helper.VisitAbsoluteValue(input1, null);
        }

        #region VisitRelationalOperators
        [TestMethod]
        public void VisitRelationalOperators_ForGreaterEqualExpression_CorrectParameterPassDown()
        {
            var expected = new List<TypeNode>()
            {
                Utilities.GetFunctionType(TypeEnum.Integer, new List<TypeEnum>() {TypeEnum.Integer})
            };
            GreaterEqualExpression input1 = GetGreaterEqualExpression(TypeEnum.Integer, TypeEnum.Real);

            List<TypeNode> res = null;
            ITypeChecker parent = Substitute.For<ITypeChecker>();
            parent.Dispatch(Arg.Any<RealLiteralExpression>(), Arg.Do<List<TypeNode>>(x => res = x)).Returns(new TypeNode(TypeEnum.Real, 1, 1));
            parent.Dispatch(Arg.Any<IntegerLiteralExpression>(), Arg.Any<List<TypeNode>>()).Returns(new TypeNode(TypeEnum.Integer, 1, 1));
            CommonOperatorHelper helper = Utilities.GetHelper<CommonOperatorHelper>(parent);

            helper.VisitRelationalOperators(input1, expected.ToList());

            res.Should().BeEquivalentTo(expected);
        }
        #endregion
    }
}