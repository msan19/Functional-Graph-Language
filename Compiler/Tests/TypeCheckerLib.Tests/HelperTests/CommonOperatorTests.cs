using ASTLib.Exceptions;
using ASTLib.Interfaces;
using ASTLib.Nodes.ExpressionNodes;
using ASTLib.Nodes.ExpressionNodes.CommonOperationNodes;
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

        private GreaterExpression GetGreaterExpression(TypeEnum left, TypeEnum right)
        {
            ExpressionNode leftNode = GetLiteral(left);
            ExpressionNode rightNode = GetLiteral(right);
            var node = new GreaterExpression(leftNode, rightNode, 0, 0);
            return node;
        }

        private LessEqualExpression GetLessEqualExpression(TypeEnum left, TypeEnum right)
        {
            ExpressionNode leftNode = GetLiteral(left);
            ExpressionNode rightNode = GetLiteral(right);
            var node = new LessEqualExpression(leftNode, rightNode, 0, 0);
            return node;
        }

        private LessExpression GetLessExpression(TypeEnum left, TypeEnum right)
        {
            ExpressionNode leftNode = GetLiteral(left);
            ExpressionNode rightNode = GetLiteral(right);
            var node = new LessExpression(leftNode, rightNode, 0, 0);
            return node;
        }

        private EqualExpression GetEqualExpression(TypeEnum left, TypeEnum right)
        {
            ExpressionNode leftNode = GetLiteral(left);
            ExpressionNode rightNode = GetLiteral(right);
            var node = new EqualExpression(leftNode, rightNode, 0, 0);
            return node;
        }

        private NotEqualExpression GetNotEqualExpression(TypeEnum left, TypeEnum right)
        {
            ExpressionNode leftNode = GetLiteral(left);
            ExpressionNode rightNode = GetLiteral(right);
            var node = new NotEqualExpression(leftNode, rightNode, 0, 0);
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

        [TestMethod]
        public void VisitAddition_AdditionExpressionWithIntAndBoolean_ThrowsException()
        {

            IntegerLiteralExpression intLit = new IntegerLiteralExpression("1", 1, 1);
            BooleanLiteralExpression booleanExpr = new BooleanLiteralExpression(true, 0, 0);
            AdditionExpression input = new AdditionExpression(intLit, booleanExpr, 1, 1);
            ITypeChecker parent = Substitute.For<ITypeChecker>();
            parent.Dispatch(Arg.Any<IntegerLiteralExpression>(), Arg.Any<List<TypeNode>>()).Returns(new TypeNode(TypeEnum.Integer, 1, 1));
            parent.Dispatch(Arg.Any<BooleanLiteralExpression>(), Arg.Any<List<TypeNode>>()).Returns(new TypeNode(TypeEnum.Boolean, 1, 1));
            CommonOperatorHelper helper = Utilities.GetHelper<CommonOperatorHelper>(parent);

            Assert.ThrowsException<UnmatchableTypesException>(() => helper.VisitAddition(input, null));
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

        [TestMethod]
        public void VisitSubtraction_SubtractionExpressionWithIntAndBoolean_ThrowsException()
        {

            IntegerLiteralExpression intLit = new IntegerLiteralExpression("1", 1, 1);
            BooleanLiteralExpression booleanExpr = new BooleanLiteralExpression(true, 0, 0);
            SubtractionExpression input = new SubtractionExpression(intLit, booleanExpr, 1, 1);
            ITypeChecker parent = Substitute.For<ITypeChecker>();
            parent.Dispatch(Arg.Any<IntegerLiteralExpression>(), Arg.Any<List<TypeNode>>()).Returns(new TypeNode(TypeEnum.Integer, 1, 1));
            parent.Dispatch(Arg.Any<BooleanLiteralExpression>(), Arg.Any<List<TypeNode>>()).Returns(new TypeNode(TypeEnum.Boolean, 1, 1));
            CommonOperatorHelper helper = Utilities.GetHelper<CommonOperatorHelper>(parent);

            Assert.ThrowsException<UnmatchableTypesException>(() => helper.VisitSubtraction(input, null));
        }

        #endregion

        #region VisitAbsoluteValue
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

        [TestMethod]
        public void VisitAbsoluteValue_AbsoluteValueExpressionWithBoolean_ThrowsException()
        {
            BooleanLiteralExpression booleanExpr = new BooleanLiteralExpression(false, 0, 0);
            AbsoluteValueExpression input = new AbsoluteValueExpression(booleanExpr, 1, 1);
            ITypeChecker parent = Substitute.For<ITypeChecker>();
            parent.Dispatch(Arg.Any<BooleanLiteralExpression>(), Arg.Any<List<TypeNode>>()).Returns(new TypeNode(TypeEnum.Boolean, 1, 1));
            CommonOperatorHelper helper = Utilities.GetHelper<CommonOperatorHelper>(parent);

            Assert.ThrowsException<AbsoluteValueTypeException>(() => helper.VisitAbsoluteValue(input, null));
        }

        #endregion

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

            helper.VisitRelationalOperator(input1, expected.ToList());

            res.Should().BeEquivalentTo(expected);
        }

        // Int Real -> Cast Node
        // Int Real -> Return Boolean

        [TestMethod]
        public void VisitRelationalOperators_GreaterEqualExpressionWithIntAndReal_InsertedIntToRealCastNode()
        {
            var expected = typeof(CastFromIntegerExpression);
            GreaterEqualExpression input1 = GetGreaterEqualExpression(TypeEnum.Integer, TypeEnum.Real);

            CommonOperatorHelper helper = Utilities.GetHelper<CommonOperatorHelper>();
            helper.VisitRelationalOperator(input1, null);
            var res = input1.Children[0].GetType();

            Assert.AreEqual(expected, res);
        }

        [TestMethod]
        public void VisitRelationalOperators_GreaterEqualExpressionWithIntAndReal_ReturnsBooleanTypeNode()
        {
            var expected = TypeEnum.Boolean;
            GreaterEqualExpression input1 = GetGreaterEqualExpression(TypeEnum.Integer, TypeEnum.Real);

            CommonOperatorHelper helper = Utilities.GetHelper<CommonOperatorHelper>();
            var res = helper.VisitRelationalOperator(input1, null).Type;

            Assert.AreEqual(expected, res);
        }

        [TestMethod]
        public void VisitRelationalOperators_ForGreaterExpression_CorrectParameterPassDown()
        {
            var expected = new List<TypeNode>()
            {
                Utilities.GetFunctionType(TypeEnum.Integer, new List<TypeEnum>() {TypeEnum.Integer})
            };
            GreaterExpression input1 = GetGreaterExpression(TypeEnum.Integer, TypeEnum.Real);

            List<TypeNode> res = null;
            ITypeChecker parent = Substitute.For<ITypeChecker>();
            parent.Dispatch(Arg.Any<RealLiteralExpression>(), Arg.Do<List<TypeNode>>(x => res = x)).Returns(new TypeNode(TypeEnum.Real, 1, 1));
            parent.Dispatch(Arg.Any<IntegerLiteralExpression>(), Arg.Any<List<TypeNode>>()).Returns(new TypeNode(TypeEnum.Integer, 1, 1));
            CommonOperatorHelper helper = Utilities.GetHelper<CommonOperatorHelper>(parent);

            helper.VisitRelationalOperator(input1, expected.ToList());

            res.Should().BeEquivalentTo(expected);
        }

        [TestMethod]
        public void VisitRelationalOperators_GreaterExpressionWithIntAndReal_InsertedIntToRealCastNode()
        {
            var expected = typeof(CastFromIntegerExpression);
            GreaterExpression input1 = GetGreaterExpression(TypeEnum.Integer, TypeEnum.Real);

            CommonOperatorHelper helper = Utilities.GetHelper<CommonOperatorHelper>();
            helper.VisitRelationalOperator(input1, null);
            var res = input1.Children[0].GetType();

            Assert.AreEqual(expected, res);
        }

        [TestMethod]
        public void VisitRelationalOperators_GreaterExpressionWithIntAndReal_ReturnsBooleanTypeNode()
        {
            var expected = TypeEnum.Boolean;
            GreaterExpression input1 = GetGreaterExpression(TypeEnum.Integer, TypeEnum.Real);

            CommonOperatorHelper helper = Utilities.GetHelper<CommonOperatorHelper>();
            var res = helper.VisitRelationalOperator(input1, null).Type;

            Assert.AreEqual(expected, res);
        }

        [TestMethod]
        public void VisitRelationalOperators_ForLessEqualExpression_CorrectParameterPassDown()
        {
            var expected = new List<TypeNode>()
            {
                Utilities.GetFunctionType(TypeEnum.Integer, new List<TypeEnum>() {TypeEnum.Integer})
            };
            LessEqualExpression input1 = GetLessEqualExpression(TypeEnum.Integer, TypeEnum.Real);

            List<TypeNode> res = null;
            ITypeChecker parent = Substitute.For<ITypeChecker>();
            parent.Dispatch(Arg.Any<RealLiteralExpression>(), Arg.Do<List<TypeNode>>(x => res = x)).Returns(new TypeNode(TypeEnum.Real, 1, 1));
            parent.Dispatch(Arg.Any<IntegerLiteralExpression>(), Arg.Any<List<TypeNode>>()).Returns(new TypeNode(TypeEnum.Integer, 1, 1));
            CommonOperatorHelper helper = Utilities.GetHelper<CommonOperatorHelper>(parent);

            helper.VisitRelationalOperator(input1, expected.ToList());

            res.Should().BeEquivalentTo(expected);
        }

        [TestMethod]
        public void VisitRelationalOperators_LessEqualExpressionWithIntAndReal_InsertedIntToRealCastNode()
        {
            var expected = typeof(CastFromIntegerExpression);
            LessEqualExpression input1 = GetLessEqualExpression(TypeEnum.Integer, TypeEnum.Real);

            CommonOperatorHelper helper = Utilities.GetHelper<CommonOperatorHelper>();
            helper.VisitRelationalOperator(input1, null);
            var res = input1.Children[0].GetType();

            Assert.AreEqual(expected, res);
        }

        [TestMethod]
        public void VisitRelationalOperators_LessEqualExpressionWithIntAndReal_ReturnsBooleanTypeNode()
        {
            var expected = TypeEnum.Boolean;
            LessEqualExpression input1 = GetLessEqualExpression(TypeEnum.Integer, TypeEnum.Real);

            CommonOperatorHelper helper = Utilities.GetHelper<CommonOperatorHelper>();
            var res = helper.VisitRelationalOperator(input1, null).Type;

            Assert.AreEqual(expected, res);
        }

        [TestMethod]
        public void VisitRelationalOperators_ForLessExpression_CorrectParameterPassDown()
        {
            var expected = new List<TypeNode>()
            {
                Utilities.GetFunctionType(TypeEnum.Integer, new List<TypeEnum>() {TypeEnum.Integer})
            };
            LessExpression input1 = GetLessExpression(TypeEnum.Integer, TypeEnum.Real);

            List<TypeNode> res = null;
            ITypeChecker parent = Substitute.For<ITypeChecker>();
            parent.Dispatch(Arg.Any<RealLiteralExpression>(), Arg.Do<List<TypeNode>>(x => res = x)).Returns(new TypeNode(TypeEnum.Real, 1, 1));
            parent.Dispatch(Arg.Any<IntegerLiteralExpression>(), Arg.Any<List<TypeNode>>()).Returns(new TypeNode(TypeEnum.Integer, 1, 1));
            CommonOperatorHelper helper = Utilities.GetHelper<CommonOperatorHelper>(parent);

            helper.VisitRelationalOperator(input1, expected.ToList());

            res.Should().BeEquivalentTo(expected);
        }

        [TestMethod]
        public void VisitRelationalOperators_LessExpressionWithIntAndReal_InsertedIntToRealCastNode()
        {
            var expected = typeof(CastFromIntegerExpression);
            LessExpression input1 = GetLessExpression(TypeEnum.Integer, TypeEnum.Real);

            CommonOperatorHelper helper = Utilities.GetHelper<CommonOperatorHelper>();
            helper.VisitRelationalOperator(input1, null);
            var res = input1.Children[0].GetType();

            Assert.AreEqual(expected, res);
        }

        [TestMethod]
        public void VisitRelationalOperators_LessExpressionWithIntAndReal_ReturnsBooleanTypeNode()
        {
            var expected = TypeEnum.Boolean;
            LessExpression input1 = GetLessExpression(TypeEnum.Integer, TypeEnum.Real);

            CommonOperatorHelper helper = Utilities.GetHelper<CommonOperatorHelper>();
            var res = helper.VisitRelationalOperator(input1, null).Type;

            Assert.AreEqual(expected, res);
        }

        [TestMethod]
        public void VisitRelationalOperators_ExpressionWithABoolean_ThrowsError()
        {
            BooleanLiteralExpression booleanExpr = new BooleanLiteralExpression(true, 1, 1);
            IntegerLiteralExpression rightNode = new IntegerLiteralExpression("1", 1, 1);
            LessExpression input = new LessExpression(booleanExpr, rightNode, 0, 0);
            CommonOperatorHelper helper = Utilities.GetHelper<CommonOperatorHelper>();

            Assert.ThrowsException<UnmatchableTypesException>(() => helper.VisitRelationalOperator(input, null));
        }
        #endregion

        #region VisitEquivalenceOperator
        [TestMethod]
        public void VisitEquivalenceOperator_ForEqualExpression_CorrectParameterPassDown()
        {
            var expected = new List<TypeNode>()
            {
                Utilities.GetFunctionType(TypeEnum.Integer, new List<TypeEnum>() {TypeEnum.Integer})
            };

            EqualExpression input1 = GetEqualExpression(TypeEnum.Integer, TypeEnum.Real);

            List<TypeNode> res = null;
            ITypeChecker parent = Substitute.For<ITypeChecker>();
            parent.Dispatch(Arg.Any<RealLiteralExpression>(), Arg.Do<List<TypeNode>>(x => res = x)).Returns(new TypeNode(TypeEnum.Real, 1, 1));
            parent.Dispatch(Arg.Any<IntegerLiteralExpression>(), Arg.Any<List<TypeNode>>()).Returns(new TypeNode(TypeEnum.Integer, 1, 1));
            CommonOperatorHelper helper = Utilities.GetHelper<CommonOperatorHelper>(parent);

            helper.VisitEquivalenceOperator(input1, expected.ToList());

            res.Should().BeEquivalentTo(expected);
        }

        [TestMethod]
        public void VisitEquivalenceOperator_EqualExpressionWithIntegerAndReal_ReturnsBooleanTypeNode()
        {
            var expected = TypeEnum.Boolean;
            IEquivalenceOperator input1 = GetEqualExpression(TypeEnum.Integer, TypeEnum.Real);

            CommonOperatorHelper helper = Utilities.GetHelper<CommonOperatorHelper>();
            var res = helper.VisitEquivalenceOperator(input1, null).Type;

            Assert.AreEqual(expected, res);
        }

        [TestMethod]
        public void VisitEquivalenceOperator_EqualExpressionWithIntegerAndInteger_ReturnsBooleanTypeNode()
        {
            var expected = TypeEnum.Boolean;
            IEquivalenceOperator input1 = GetEqualExpression(TypeEnum.Integer, TypeEnum.Integer);

            CommonOperatorHelper helper = Utilities.GetHelper<CommonOperatorHelper>();
            var res = helper.VisitEquivalenceOperator(input1, null).Type;

            Assert.AreEqual(expected, res);
        }

        [TestMethod]
        public void VisitEquivalenceOperator_EqualExpressionWithRealAndReal_ReturnsBooleanTypeNode()
        {
            var expected = TypeEnum.Boolean;
            IEquivalenceOperator input1 = GetEqualExpression(TypeEnum.Real, TypeEnum.Real);

            CommonOperatorHelper helper = Utilities.GetHelper<CommonOperatorHelper>();
            var res = helper.VisitEquivalenceOperator(input1, null).Type;

            Assert.AreEqual(expected, res);
        }

        [TestMethod]
        [ExpectedException(typeof(UnmatchableTypesException))]
        public void VisitEquivalenceOperator_EqualExpressionWithIntegerAndBoolean_ThrowsUnmatchableTypesException()
        {
            IEquivalenceOperator input1 = GetEqualExpression(TypeEnum.Integer, TypeEnum.Boolean);

            CommonOperatorHelper helper = Utilities.GetHelper<CommonOperatorHelper>();
            var res = helper.VisitEquivalenceOperator(input1, null).Type;
        }

        [TestMethod]
        [ExpectedException(typeof(UnmatchableTypesException))]
        public void VisitEquivalenceOperator_EqualExpressionWithBooleanAndReal_ThrowsUnmatchableTypesException()
        {
            IEquivalenceOperator input1 = GetEqualExpression(TypeEnum.Boolean, TypeEnum.Real);

            CommonOperatorHelper helper = Utilities.GetHelper<CommonOperatorHelper>();
            var res = helper.VisitEquivalenceOperator(input1, null).Type;
        }

        [TestMethod]
        public void VisitEquivalenceOperator_NotEqualExpressionWithRealAndReal_ReturnsBooleanTypeNode()
        {
            var expected = TypeEnum.Boolean;
            IEquivalenceOperator input1 = GetNotEqualExpression(TypeEnum.Real, TypeEnum.Real);

            CommonOperatorHelper helper = Utilities.GetHelper<CommonOperatorHelper>();
            var res = helper.VisitEquivalenceOperator(input1, null).Type;

            Assert.AreEqual(expected, res);
        }

        #endregion
    }
}