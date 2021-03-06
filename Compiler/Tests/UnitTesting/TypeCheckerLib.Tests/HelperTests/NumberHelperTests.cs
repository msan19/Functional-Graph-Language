using System;
using System.Collections.Generic;
using System.Linq;
using ASTLib.Exceptions;
using ASTLib.Exceptions.NotMatching;
using ASTLib.Interfaces;
using ASTLib.Nodes.ExpressionNodes;
using ASTLib.Nodes.ExpressionNodes.CastExpressionNodes;
using ASTLib.Nodes.ExpressionNodes.CommonOperationNodes;
using ASTLib.Nodes.ExpressionNodes.NumberOperationNodes;
using ASTLib.Nodes.ExpressionNodes.OperationNodes;
using ASTLib.Nodes.TypeNodes;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using TypeCheckerLib.Helpers;
using TypeCheckerLib.Interfaces;

namespace TypeCheckerLib.Tests.HelperTests
{
    [TestClass]
    public class NumberHelperTests
    {
        private MultiplicationExpression GetMultiplicationExpression(TypeEnum left, TypeEnum right)
        {
            ExpressionNode leftNode = GetLiteral(left);
            ExpressionNode rightNode = GetLiteral(right);
            var node = new MultiplicationExpression(leftNode, rightNode, 0, 0);
            return node;
        }

        private PowerExpression GetPowerExpression(TypeEnum left, TypeEnum right)
        {
            ExpressionNode leftNode = GetLiteral(left);
            ExpressionNode rightNode = GetLiteral(right);
            var node = new PowerExpression(leftNode, rightNode, 0, 0);
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

        #region Binary Num Operator
        [TestMethod]
        public void BinaryNumOp__CorrectParameterPassDown()
        {
            var expected = new List<TypeNode>()
            {
                Utilities.GetFunctionType(TypeEnum.Integer, new List<TypeEnum>() {TypeEnum.Integer})
            };
            IBinaryNumberOperator input1 = GetMultiplicationExpression(TypeEnum.Integer, TypeEnum.Real);
            
            List<TypeNode> res = null;
            ITypeChecker parent = Substitute.For<ITypeChecker>();
            parent.Dispatch(Arg.Any<RealLiteralExpression>(), Arg.Do<List<TypeNode>>(x => res = x)).Returns(new TypeNode(TypeEnum.Real, 1, 1));
            parent.Dispatch(Arg.Any<IntegerLiteralExpression>(), Arg.Any<List<TypeNode>>()).Returns(new TypeNode(TypeEnum.Integer, 1, 1));
            NumberHelper helper = Utilities.GetHelper<NumberHelper>(parent);

            helper.VisitBinaryNumOp(input1, expected.ToList());
         
            res.Should().BeEquivalentTo(expected);
        }

        // Int Real -> Cast Node
        // Int Real -> Append Int to Cast Node
        // Int Int  -> Still ints as children
        // Int Real -> Return Real
        // Int Int  -> Return Int
        // Int Func -> Throw Error 

        [TestMethod]
        public void BinaryNumOp_MultiplicationExpressionWithIntAndReal_InsertedIntToRealCastNode()
        {
            var expected = typeof(CastFromIntegerExpression);
            IBinaryNumberOperator input1 = GetMultiplicationExpression(TypeEnum.Integer, TypeEnum.Real);

            NumberHelper helper = Utilities.GetHelper<NumberHelper>();
            helper.VisitBinaryNumOp(input1, null);
            var res = input1.Children[0].GetType();

            Assert.AreEqual(expected, res);
        }

        [TestMethod]
        public void BinaryNumOp_MultiplicationExpressionWithIntAndReal_AppendedIntNodeToTypeCast()
        {
            var expected = typeof(IntegerLiteralExpression);
            IBinaryNumberOperator input1 = GetMultiplicationExpression(TypeEnum.Integer, TypeEnum.Real);

            NumberHelper helper = Utilities.GetHelper<NumberHelper>();
            helper.VisitBinaryNumOp(input1, null);
            var res = input1.Children[0].Children[0].GetType();

            Assert.AreEqual(expected, res);
        }

        [TestMethod]
        public void BinaryNumOp_MultiplicationExpressionWithTwoInt_LeftNodeIsReal()
        {
            var expected = typeof(IntegerLiteralExpression);
            IBinaryNumberOperator input1 = GetMultiplicationExpression(TypeEnum.Integer, TypeEnum.Integer);

            NumberHelper helper = Utilities.GetHelper<NumberHelper>();
            helper.VisitBinaryNumOp(input1, null);
            var res = input1.Children[0].GetType();

            Assert.AreEqual(expected, res);
        }

        [TestMethod]
        public void BinaryNumOp_MultiplicationExpressionWithTwoInt_RightNodeIsReal()
        {
            var expected = typeof(IntegerLiteralExpression);
            IBinaryNumberOperator input1 = GetMultiplicationExpression(TypeEnum.Integer, TypeEnum.Integer);

            NumberHelper helper = Utilities.GetHelper<NumberHelper>();
            helper.VisitBinaryNumOp(input1, null);
            var res = input1.Children[1].GetType();

            Assert.AreEqual(expected, res);
        }

        [TestMethod]
        public void BinaryNumOp_MultiplicationExpressionWithIntAndReal_ReturnsRealTypeNode()
        {
            var expected = TypeEnum.Real;
            IBinaryNumberOperator input1 = GetMultiplicationExpression(TypeEnum.Integer, TypeEnum.Real);

            NumberHelper helper = Utilities.GetHelper<NumberHelper>();
            var res = helper.VisitBinaryNumOp(input1, null).Type;

            Assert.AreEqual(expected, res);
        }

        [TestMethod]
        public void BinaryNumOp_MultiplicationExpressionWithTwoInt_ReturnsIntTypeNode()
        {
            var expected = TypeEnum.Integer;
            IBinaryNumberOperator input1 = GetMultiplicationExpression(TypeEnum.Integer, TypeEnum.Integer);

            NumberHelper helper = Utilities.GetHelper<NumberHelper>();
            var res = helper.VisitBinaryNumOp(input1, null).Type;

            Assert.AreEqual(expected, res);
        }
        
        // Int Func -> Throw Error 
        [TestMethod]
        [ExpectedException(typeof(UnmatchableTypesException))]
        public void BinaryNumOp_MultiplicationExpressionWithIntAndFunc_ThrowsException()
        {
            
            IntegerLiteralExpression intLit1 = new IntegerLiteralExpression("1", 1, 1);
            IdentifierExpression func = new IdentifierExpression("f", 0, 0);
            IBinaryNumberOperator input1 = new MultiplicationExpression(intLit1, func, 1, 1);
            ITypeChecker parent = Substitute.For<ITypeChecker>();
            parent.Dispatch(Arg.Any<IntegerLiteralExpression>(), Arg.Any<List<TypeNode>>()).Returns(new TypeNode(TypeEnum.Integer, 1, 1));
            parent.Dispatch(Arg.Any<IdentifierExpression>(), Arg.Any<List<TypeNode>>()).Returns(new TypeNode(TypeEnum.Function, 1, 1));
            NumberHelper helper = Utilities.GetHelper<NumberHelper>(parent);


            helper.VisitBinaryNumOp(input1, null);
        }


        [TestMethod]
        public void BinaryNumOp_MultiplicationExpressionWithBooleanAndReal_ThrowsException()
        {
            BooleanLiteralExpression boolLit = new BooleanLiteralExpression(true, 0, 0);
            RealLiteralExpression realLit = new RealLiteralExpression("1.1", 0, 0);
            IBinaryNumberOperator input = new MultiplicationExpression(boolLit, realLit, 1, 1);
            ITypeChecker parent = Substitute.For<ITypeChecker>();
            parent.Dispatch(Arg.Any<BooleanLiteralExpression>(), Arg.Any<List<TypeNode>>()).Returns(new TypeNode(TypeEnum.Boolean, 1, 1));
            parent.Dispatch(Arg.Any<RealLiteralExpression>(), Arg.Any<List<TypeNode>>()).Returns(new TypeNode(TypeEnum.Real, 1, 1));
            NumberHelper helper = Utilities.GetHelper<NumberHelper>(parent);

            Assert.ThrowsException<UnmatchableTypesException>(() => helper.VisitBinaryNumOp(input, null));
        }
        #endregion

        #region VisitPower
        /*
        [TestMethod]
        public void VisitPower_CorrectParameterPassDown()
        {
            var expected = new List<TypeNode>()
            {
                Utilities.GetFunctionType(TypeEnum.Integer, new List<TypeEnum>() {TypeEnum.Integer})
            };
            PowerExpression input1 = GetPowerExpression(TypeEnum.Integer, TypeEnum.Real);

            List<TypeNode> res = null;
            ITypeChecker parent = Substitute.For<ITypeChecker>();
            parent.Dispatch(Arg.Any<RealLiteralExpression>(), Arg.Do<List<TypeNode>>(x => res = x)).Returns(new TypeNode(TypeEnum.Real, 1, 1));
            parent.Dispatch(Arg.Any<IntegerLiteralExpression>(), Arg.Any<List<TypeNode>>()).Returns(new TypeNode(TypeEnum.Integer, 1, 1));
            NumberHelper helper = Utilities.GetHelper<NumberHelper>(parent);

            helper.VisitPower(input1, expected.ToList());

            res.Should().BeEquivalentTo(expected);
        }

        // Int Real -> Cast Node
        // Int Real -> Append Int to Cast Node
        // Int Int  -> Still ints as children
        // Int Real -> Return Real
        // Int Int  -> Return Int
        // Int Func -> Throw Error 

        [TestMethod]
        public void VisitPower_PowerExpressionWithIntAndReal_InsertedIntToRealCastNode()
        {
            var expected = typeof(CastFromIntegerExpression);
            PowerExpression input1 = GetPowerExpression(TypeEnum.Integer, TypeEnum.Real);

            NumberHelper helper = Utilities.GetHelper<NumberHelper>();
            helper.VisitPower(input1, null);
            var res = input1.Children[0].GetType();

            Assert.AreEqual(expected, res);
        }

        [TestMethod]
        public void VisitPower_PowerExpressionWithIntAndReal_AppendedIntNodeToTypeCast()
        {
            var expected = typeof(IntegerLiteralExpression);
            PowerExpression input1 = GetPowerExpression(TypeEnum.Integer, TypeEnum.Real);

            NumberHelper helper = Utilities.GetHelper<NumberHelper>();
            helper.VisitPower(input1, null);
            var res = input1.Children[0].Children[0].GetType();

            Assert.AreEqual(expected, res);
        }

        [TestMethod]
        public void VisitPower_PowerExpressionWithTwoInt_LeftInsertedIntToRealCastNode()
        {
            var expected = typeof(CastFromIntegerExpression);
            PowerExpression input1 = GetPowerExpression(TypeEnum.Integer, TypeEnum.Integer);

            NumberHelper helper = Utilities.GetHelper<NumberHelper>();
            helper.VisitPower(input1, null);
            var res = input1.Children[0].GetType();

            Assert.AreEqual(expected, res);
        }

        [TestMethod]
        public void VisitPower_PowerExpressionWithTwoInt_RightInsertedIntToRealCastNode()
        {
            var expected = typeof(CastFromIntegerExpression);
            PowerExpression input1 = GetPowerExpression(TypeEnum.Integer, TypeEnum.Integer);

            NumberHelper helper = Utilities.GetHelper<NumberHelper>();
            helper.VisitPower(input1, null);
            var res = input1.Children[1].GetType();

            Assert.AreEqual(expected, res);
        }

        [TestMethod]
        public void VisitPower_PowerExpressionWithIntAndReal_ReturnsRealTypeNode()
        {
            var expected = TypeEnum.Real;
            PowerExpression input1 = GetPowerExpression(TypeEnum.Integer, TypeEnum.Real);

            NumberHelper helper = Utilities.GetHelper<NumberHelper>();
            var res = helper.VisitPower(input1, null).Type;

            Assert.AreEqual(expected, res);
        }

        [TestMethod]
        public void VisitPower_PowerExpressionWithTwoInt_ReturnsRealTypeNode()
        {
            var expected = TypeEnum.Real;
            PowerExpression input1 = GetPowerExpression(TypeEnum.Integer, TypeEnum.Integer);

            NumberHelper helper = Utilities.GetHelper<NumberHelper>();
            var res = helper.VisitPower(input1, null).Type;

            Assert.AreEqual(expected, res);
        }

        // Int Func -> Throw Error 
        [TestMethod]
        [ExpectedException(typeof(ASTLib.Exceptions.UnmatchableTypesException))]
        public void VisitPower_PowerExpressionWithIntAndFunc_ThrowsException()
        {

            IntegerLiteralExpression intLit1 = new IntegerLiteralExpression("1", 1, 1);
            IdentifierExpression func = new IdentifierExpression("f", 0, 0);
            PowerExpression input1 = new PowerExpression(intLit1, func, 1, 1);
            ITypeChecker parent = Substitute.For<ITypeChecker>();
            parent.Dispatch(Arg.Any<IntegerLiteralExpression>(), Arg.Any<List<TypeNode>>()).Returns(new TypeNode(TypeEnum.Integer, 1, 1));
            parent.Dispatch(Arg.Any<IdentifierExpression>(), Arg.Any<List<TypeNode>>()).Returns(new TypeNode(TypeEnum.Function, 1, 1));
            NumberHelper helper = Utilities.GetHelper<NumberHelper>(parent);


            helper.VisitPower(input1, null);
        }

        [TestMethod]
        public void VisitPower_PowerExpressionWithBooleans_ThrowsException()
        {
            BooleanLiteralExpression boolExpr =  new BooleanLiteralExpression(true, 1, 1);
            PowerExpression input = new PowerExpression(boolExpr, boolExpr, 1, 1);
            ITypeChecker parent = Substitute.For<ITypeChecker>();
            parent.Dispatch(Arg.Any<BooleanLiteralExpression>(), Arg.Any<List<TypeNode>>()).Returns(new TypeNode(TypeEnum.Boolean, 1, 1));

            NumberHelper helper = Utilities.GetHelper<NumberHelper>(parent);

            Assert.ThrowsException<UnmatchableTypesException>(() => helper.VisitPower(input, null));
        }
        */
        #endregion


        #region VisitNegative
        // Format "- Term"
        // if Term is type Boolean    --> throw exception (not boolean operator, use ! instead)
        // if Term is type Real       --> OK
        // if Term is type Integer    --> OK

        [TestMethod]
        public void VisitNegative_Integer_ReturnsTypeInteger()
        {
            var expected = TypeEnum.Integer;
            NegativeExpression negExpr = Utilities.GetNegativeExpressionWithInt();

            NumberHelper helper = Utilities.GetHelper<NumberHelper>();
            var res = helper.VisitNegative(negExpr, null).Type;

            Assert.AreEqual(expected, res);
        }

        [TestMethod]
        public void VisitNegative_Real_ReturnsTypeReal()
        {
            var expected = TypeEnum.Real;
            NegativeExpression negExpr = Utilities.GetNegativeExpressionWithReal();

            NumberHelper helper = Utilities.GetHelper<NumberHelper>();
            var res = helper.VisitNegative(negExpr, null).Type;

            Assert.AreEqual(expected, res);
        }
        
        [ExpectedException(typeof(UnableToNegateTermException))]
        [TestMethod]
        public void VisitNegative_Boolean_CausesUnableToNegateTermException()
        {
            NegativeExpression negExpr = Utilities.GetNegativeExpressionWithBool();
            NumberHelper helper = Utilities.GetHelper<NumberHelper>();
            
            var res = helper.VisitNegative(negExpr, null).Type;
        }
        
        #endregion
    }
}