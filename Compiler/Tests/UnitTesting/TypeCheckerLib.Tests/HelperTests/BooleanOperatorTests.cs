using ASTLib.Nodes.ExpressionNodes;
using ASTLib.Nodes.ExpressionNodes.BooleanOperationNodes;
using ASTLib.Nodes.TypeNodes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ASTLib.Interfaces;
using System;
using TypeCheckerLib.Helpers;
using ASTLib.Exceptions;
using ASTLib.Exceptions.NotMatching;
using UnmatchableTypesException = ASTLib.Exceptions.NotMatching.UnmatchableTypesException;

namespace TypeCheckerLib.Tests.HelperTests
{
    [TestClass]
    public class BooleanOperatorTests
    {
        private NotExpression GetNotExpression(TypeEnum child)
        {
            ExpressionNode childNode = GetLiteral(child);
            var node = new NotExpression(childNode, 0, 0);
            return node;
        }

        private AndExpression GetAndExpression(TypeEnum left, TypeEnum right)
        {
            ExpressionNode leftNode = GetLiteral(left);
            ExpressionNode rightNode = GetLiteral(right);
            var node = new AndExpression(leftNode, rightNode, 0, 0);
            return node;
        }

        private ExpressionNode GetLiteral(TypeEnum type)
        {
            return type switch
            {
                TypeEnum.Real => new RealLiteralExpression("2.2", 0, 0),
                TypeEnum.Integer => new IntegerLiteralExpression("1", 1, 1),
                TypeEnum.Boolean => new BooleanLiteralExpression(true, 2, 2),
                TypeEnum.Function => throw new Exception("Functions is not supported in GetBinaryOperator"),
                _ => throw new NotImplementedException()
            };
        }

        #region VisitNot
        [TestMethod]
        [ExpectedException(typeof(UnmatchableTypesException))]
        public void VisitNot_IntChild_ThrowsException()
        {
            NotExpression input1 = GetNotExpression(TypeEnum.Integer);

            BooleanHelper helper = Utilities.GetHelper<BooleanHelper>();
            helper.VisitNot(input1, null);
        }

        [TestMethod]
        [ExpectedException(typeof(UnmatchableTypesException))]
        public void VisitNot_RealChild_ThrowsException()
        {
            NotExpression input1 = GetNotExpression(TypeEnum.Real);

            BooleanHelper helper = Utilities.GetHelper<BooleanHelper>();
            helper.VisitNot(input1, null);
        }

        [TestMethod]
        public void VisitNot_BoolChild_ReturnsBooleanTypeNode()
        {
            var expected = TypeEnum.Boolean;
            NotExpression input1 = GetNotExpression(TypeEnum.Boolean);

            BooleanHelper helper = Utilities.GetHelper<BooleanHelper>();
            var res = helper.VisitNot(input1, null).Type;

            Assert.AreEqual(expected, res);
        }
        #endregion

        #region VisitBinaryBooleanOperator
        [TestMethod]
        [ExpectedException(typeof(UnmatchableTypesException))]
        public void VisitBinaryBoolOp_AndExpressionWithRealAndRealChilds_ThrowsException()
        {
            IBinaryBooleanOperator input1 = GetAndExpression(TypeEnum.Real, TypeEnum.Real);

            BooleanHelper helper = Utilities.GetHelper<BooleanHelper>();
            helper.VisitBinaryBoolOp(input1, null);
        }

        [TestMethod]
        [ExpectedException(typeof(UnmatchableTypesException))]
        public void VisitBinaryBoolOp_AndExpressionWithIntAndIntChilds_ThrowsException()
        {
            IBinaryBooleanOperator input1 = GetAndExpression(TypeEnum.Real, TypeEnum.Real);

            BooleanHelper helper = Utilities.GetHelper<BooleanHelper>();
            helper.VisitBinaryBoolOp(input1, null);
        }

        [TestMethod]
        [ExpectedException(typeof(UnmatchableTypesException))]
        public void VisitBinaryBoolOp_AndExpressionWithIntAndRealChilds_ThrowsException()
        {
            IBinaryBooleanOperator input1 = GetAndExpression(TypeEnum.Integer, TypeEnum.Real);

            BooleanHelper helper = Utilities.GetHelper<BooleanHelper>();
            helper.VisitBinaryBoolOp(input1, null);
        }

        [TestMethod]
        [ExpectedException(typeof(UnmatchableTypesException))]
        public void VisitBinaryBoolOp_AndExpressionWithIntAndBoolChilds_ThrowsException()
        {
            IBinaryBooleanOperator input1 = GetAndExpression(TypeEnum.Integer, TypeEnum.Boolean);

            BooleanHelper helper = Utilities.GetHelper<BooleanHelper>();
            helper.VisitBinaryBoolOp(input1, null);
        }

        [TestMethod]
        public void VisitBinaryBoolOp_AndExpressionWithBoolAndBoolChilds_ReturnsBooleanTypeNode()
        {
            var expected = TypeEnum.Boolean;
            IBinaryBooleanOperator input1 = GetAndExpression(TypeEnum.Boolean, TypeEnum.Boolean);

            BooleanHelper helper = Utilities.GetHelper<BooleanHelper>();
            var res = helper.VisitBinaryBoolOp(input1, null).Type;

            Assert.AreEqual(expected, res);
        }

        #endregion

    }
}