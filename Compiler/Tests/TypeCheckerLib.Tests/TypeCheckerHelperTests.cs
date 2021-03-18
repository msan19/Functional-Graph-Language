﻿using ASTLib.Interfaces;
using ASTLib.Nodes.ExpressionNodes;
using ASTLib.Nodes.ExpressionNodes.OperationNodes;
using ASTLib.Nodes.TypeNodes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System.Collections.Generic;

namespace TypeCheckerLib.Tests
{
    [TestClass]
    public class TypeCheckerHelperTests
    {
        #region Binary Num Operator
        [TestMethod]
        public void VisitBinaryNumOp_MultiplicationExpressionWithIntAndReal_InsertedIntToRealCastNode()
        {
            var expected = typeof(CastFromIntegerExpression);
            IntegerLiteralExpression intLit = new IntegerLiteralExpression("1", 1, 1);
            RealLiteralExpression realLit = new RealLiteralExpression("2.2", 2, 2);
            IBinaryNumberOperator input1 = new MultiplicationExpression(intLit, realLit, 1, 1);
            ITypeChecker parent = Substitute.For<ITypeChecker>();
            parent.Dispatch(Arg.Any<RealLiteralExpression>()).Returns(new TypeNode(TypeEnum.Real, 1, 1));
            parent.Dispatch(Arg.Any<IntegerLiteralExpression>()).Returns(new TypeNode(TypeEnum.Integer, 1, 1));
            TypeHelper typeHelper = new TypeHelper()
            {
                TypeChecker = parent
            };

            typeHelper.VisitBinaryNumOp(input1);
            var res = input1.Children[0].GetType();

            Assert.AreEqual(expected, res);
        }

        [TestMethod]
        public void VisitBinaryNumOp_MultiplicationExpressionWithIntAndReal_AppendedIntNodeToTypeCast()
        {
            var expected = typeof(IntegerLiteralExpression);
            IntegerLiteralExpression intLit = new IntegerLiteralExpression("1", 1, 1);
            RealLiteralExpression realLit = new RealLiteralExpression("2.2", 2, 2);
            IBinaryNumberOperator input1 = new MultiplicationExpression(intLit, realLit, 1, 1);
            ITypeChecker parent = Substitute.For<ITypeChecker>();
            parent.Dispatch(Arg.Any<RealLiteralExpression>()).Returns(new TypeNode(TypeEnum.Real, 1, 1));
            parent.Dispatch(Arg.Any<IntegerLiteralExpression>()).Returns(new TypeNode(TypeEnum.Integer, 1, 1));
            TypeHelper typeHelper = new TypeHelper()
            {
                TypeChecker = parent
            };

            typeHelper.VisitBinaryNumOp(input1);
            var res = input1.Children[0].Children[0].GetType();

            Assert.AreEqual(expected, res);
        }
        #endregion
    }
}