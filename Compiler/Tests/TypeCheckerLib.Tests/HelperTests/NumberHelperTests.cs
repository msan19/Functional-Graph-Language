using System;
using System.Collections.Generic;
using System.Linq;
using ASTLib.Interfaces;
using ASTLib.Nodes.ExpressionNodes;
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
            IntegerLiteralExpression intLit = new IntegerLiteralExpression("1", 1, 1);
            RealLiteralExpression realLit = new RealLiteralExpression("2.2", 2, 2);
            IBinaryNumberOperator input1 = new MultiplicationExpression(intLit, realLit, 1, 1);
            
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
            IntegerLiteralExpression intLit = new IntegerLiteralExpression("1", 1, 1);
            RealLiteralExpression realLit = new RealLiteralExpression("2.2", 2, 2);
            IBinaryNumberOperator input1 = new MultiplicationExpression(intLit, realLit, 1, 1);

            //IBinaryNumberOperator input2 = GetBinaryOperator(TypeEnum.Integer, TypeEnum.Real, )

            NumberHelper helper = Utilities.GetHelper<NumberHelper>();
            helper.VisitBinaryNumOp(input1, null);
            var res = input1.Children[0].GetType();

            Assert.AreEqual(expected, res);
        }

        [TestMethod]
        public void BinaryNumOp_MultiplicationExpressionWithIntAndReal_AppendedIntNodeToTypeCast()
        {
            var expected = typeof(IntegerLiteralExpression);
            IntegerLiteralExpression intLit = new IntegerLiteralExpression("1", 1, 1);
            RealLiteralExpression realLit = new RealLiteralExpression("2.2", 2, 2);
            IBinaryNumberOperator input1 = new MultiplicationExpression(intLit, realLit, 1, 1);

            NumberHelper helper = Utilities.GetHelper<NumberHelper>();
            helper.VisitBinaryNumOp(input1, null);
            var res = input1.Children[0].Children[0].GetType();

            Assert.AreEqual(expected, res);
        }

        [TestMethod]
        public void BinaryNumOp_MultiplicationExpressionWithTwoInt_LeftNodeIsReal()
        {
            var expected = typeof(IntegerLiteralExpression);
            IntegerLiteralExpression intLit1 = new IntegerLiteralExpression("1", 1, 1);
            IntegerLiteralExpression intLit2 = new IntegerLiteralExpression("2", 2, 2);
            IBinaryNumberOperator input1 = new MultiplicationExpression(intLit1, intLit2, 1, 1);

            NumberHelper helper = Utilities.GetHelper<NumberHelper>();
            helper.VisitBinaryNumOp(input1, null);
            var res = input1.Children[0].GetType();

            Assert.AreEqual(expected, res);
        }

        [TestMethod]
        public void BinaryNumOp_MultiplicationExpressionWithTwoInt_RightNodeIsReal()
        {
            var expected = typeof(IntegerLiteralExpression);
            IntegerLiteralExpression intLit1 = new IntegerLiteralExpression("1", 1, 1);
            IntegerLiteralExpression intLit2 = new IntegerLiteralExpression("2", 2, 2);
            IBinaryNumberOperator input1 = new MultiplicationExpression(intLit1, intLit2, 1, 1);

            NumberHelper helper = Utilities.GetHelper<NumberHelper>();
            helper.VisitBinaryNumOp(input1, null);
            var res = input1.Children[1].GetType();

            Assert.AreEqual(expected, res);
        }

        [TestMethod]
        public void BinaryNumOp_MultiplicationExpressionWithIntAndReal_ReturnsRealTypeNode()
        {
            var expected = TypeEnum.Real;
            IntegerLiteralExpression intLit = new IntegerLiteralExpression("1", 1, 1);
            RealLiteralExpression realLit = new RealLiteralExpression("2.2", 2, 2);
            IBinaryNumberOperator input1 = new MultiplicationExpression(intLit, realLit, 1, 1);

            NumberHelper helper = Utilities.GetHelper<NumberHelper>();
            var res = helper.VisitBinaryNumOp(input1, null).Type;

            Assert.AreEqual(expected, res);
        }

        [TestMethod]
        public void BinaryNumOp_MultiplicationExpressionWithTwoInt_ReturnsIntTypeNode()
        {
            var expected = TypeEnum.Integer;
            IntegerLiteralExpression intLit1 = new IntegerLiteralExpression("1", 1, 1);
            IntegerLiteralExpression intLit2 = new IntegerLiteralExpression("2", 2, 2);
            IBinaryNumberOperator input1 = new MultiplicationExpression(intLit1, intLit2, 1, 1);

            NumberHelper helper = Utilities.GetHelper<NumberHelper>();
            var res = helper.VisitBinaryNumOp(input1, null).Type;

            Assert.AreEqual(expected, res);
        }
        
        // Int Func -> Throw Error 
        [TestMethod]
        [ExpectedException(typeof(Exception))]
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
        
        #endregion
    }
}