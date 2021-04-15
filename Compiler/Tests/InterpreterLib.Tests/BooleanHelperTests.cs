using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using ASTLib;
using ASTLib.Nodes;
using ASTLib.Nodes.ExpressionNodes;
using ASTLib.Nodes.ExpressionNodes.BooleanOperationNodes;
using ASTLib.Nodes.ExpressionNodes.CommonOperationNodes;
using ASTLib.Nodes.ExpressionNodes.CommonOperationNodes.RelationalOperationNodes;
using ASTLib.Nodes.TypeNodes;
using ASTLib.Objects;
using InterpreterLib.Helpers;
using InterpreterLib.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;

namespace InterpreterLib.Tests
{
    [TestClass]
    public class BooleanHelperTests
    {
        #region IdentifierBoolean
        [DataRow("true", true)]
        [DataRow("false", false)]
        [TestMethod]
        public void Identifier_Index0_CorrectValuesReturned(string input, bool expected)
        {
            var expression = GetIdentifier(0);
            var parameters = new List<object>() { input };
            IInterpreterBoolean parent = Substitute.For<IInterpreterBoolean>();

            BooleanHelper booleanHelper = Utilities.GetBooleanHelper(parent);

            bool res = booleanHelper.IdentifierBoolean(expression, parameters);

            Assert.AreEqual(expected, res);
        }

        [DataRow("true", true)]
        [DataRow("false", false)]
        [TestMethod]
        public void Identifier_Index1_CorrectValuesReturned(string input, bool expected)
        {
            var expression = GetIdentifier(1);
            var parameters = new List<object>() { "testing", input };
            IInterpreterBoolean parent = Substitute.For<IInterpreterBoolean>();

            BooleanHelper booleanHelper = Utilities.GetBooleanHelper(parent);

            bool res = booleanHelper.IdentifierBoolean(expression, parameters);

            Assert.AreEqual(expected, res);
        }

        private IdentifierExpression GetIdentifier(int reference)
        {
            return new IdentifierExpression("", 0, 0) { Reference = reference };
        }
        #endregion

        #region NotBoolean
        [DataRow(false, true)]
        [DataRow(true, false)]
        [TestMethod]
        public void NotBoolean_CorrectValuesReturned(bool input, bool expected)
        {
            BooleanLiteralExpression child = Utilities.GetBoolLitExpression(input);

            var expr = new NotExpression(child, 0, 0);
            IInterpreterBoolean parent = Substitute.For<IInterpreterBoolean>();
            parent.DispatchBoolean(child, Arg.Any<List<object>>()).Returns(input);

            BooleanHelper booleanHelper = Utilities.GetBooleanHelper(parent);

            bool res = booleanHelper.NotBoolean(expr, new List<object>());

            Assert.AreEqual(expected, res);
        }

        [TestMethod]
        public void NotBoolean__CheckParametersPassedDown()
        {
            BooleanLiteralExpression child = Utilities.GetBoolLitExpression();

            List<object> parameters = Utilities.GetParameterList(4);
            List<object> expectedParams = parameters;
            var expression = new NotExpression(child, 0, 0);
            IInterpreterBoolean parent = Substitute.For<IInterpreterBoolean>();
            List<object> lhsParams = new List<object>();
            List<object> rhsParams = new List<object>();
            parent.DispatchBoolean(child, Arg.Do<List<object>>(x => lhsParams = x));

            BooleanHelper booleanHelper = Utilities.GetBooleanHelper(parent);

            booleanHelper.NotBoolean(expression, parameters);

            Assert.AreEqual(expectedParams, lhsParams);
        }
        #endregion

        #region AndBoolean
        [DataRow(true, true, true)]
        [DataRow(true, false, false)]
        [DataRow(false, true, false)]
        [DataRow(false, false, false)]
        [TestMethod]
        public void AndBoolean__CorrectValuesReturned(bool lhsValue, bool rhsValue, bool expected)
        {
            BooleanLiteralExpression lhs = new BooleanLiteralExpression(lhsValue, 0, 0);
            BooleanLiteralExpression rhs = new BooleanLiteralExpression(rhsValue, 0, 0);

            AndExpression andExpr = new AndExpression(lhs, rhs, 0, 0);
            IInterpreterBoolean parent = Substitute.For<IInterpreterBoolean>();
            parent.DispatchBoolean(lhs, Arg.Any<List<object>>()).Returns(lhsValue);
            parent.DispatchBoolean(rhs, Arg.Any<List<object>>()).Returns(rhsValue);

            BooleanHelper booleanHelper = Utilities.GetBooleanHelper(parent);

            bool res = booleanHelper.AndBoolean(andExpr, new List<object>());

            Assert.AreEqual(expected, res);
        }

        [TestMethod]
        public void AndBoolean__CheckParametersPassedDown()
        {
            BooleanLiteralExpression lhs = Utilities.GetBoolLitExpression();
            BooleanLiteralExpression rhs = Utilities.GetBoolLitExpression();

            List<object> parameters = Utilities.GetParameterList(4);
            List<object> expectedParams = parameters;
            var expression = new AndExpression(lhs, rhs, 0, 0);
            IInterpreterBoolean parent = Substitute.For<IInterpreterBoolean>();
            List<object> lhsParams = new List<object>();
            List<object> rhsParams = new List<object>();
            parent.DispatchBoolean(lhs, Arg.Do<List<object>>(x => lhsParams = x));
            parent.DispatchBoolean(rhs, Arg.Do<List<object>>(x => rhsParams = x));

            BooleanHelper booleanHelper = Utilities.GetBooleanHelper(parent);

            booleanHelper.AndBoolean(expression, parameters);

            Assert.AreEqual(expectedParams, lhsParams);
            Assert.AreEqual(expectedParams, rhsParams);
        }
        #endregion

        #region OrBoolean
        [DataRow(true, true, true)]
        [DataRow(true, false, true)]
        [DataRow(false, true, true)]
        [DataRow(false, false, false)]
        [TestMethod]
        public void OrBoolean__CorrectValuesReturned(bool lhsValue, bool rhsValue, bool expected)
        {
            BooleanLiteralExpression lhs = Utilities.GetBoolLitExpression(lhsValue);
            BooleanLiteralExpression rhs = Utilities.GetBoolLitExpression(rhsValue);

            OrExpression orExpr = new OrExpression(lhs, rhs, 0, 0);
            IInterpreterBoolean parent = Substitute.For<IInterpreterBoolean>();
            parent.DispatchBoolean(lhs, Arg.Any<List<object>>()).Returns(lhsValue);
            parent.DispatchBoolean(rhs, Arg.Any<List<object>>()).Returns(rhsValue);

            BooleanHelper booleanHelper = Utilities.GetBooleanHelper(parent);

            bool res = booleanHelper.OrBoolean(orExpr, new List<object>());

            Assert.AreEqual(expected, res);
        }

        [TestMethod]
        public void OrBoolean__CheckParametersPassedDown()
        {
            BooleanLiteralExpression lhs = Utilities.GetBoolLitExpression();
            BooleanLiteralExpression rhs = Utilities.GetBoolLitExpression();

            List<object> parameters = Utilities.GetParameterList(4);
            List<object> expectedParams = parameters;
            var expression = new OrExpression(lhs, rhs, 0, 0);
            IInterpreterBoolean parent = Substitute.For<IInterpreterBoolean>();
            List<object> lhsParams = new List<object>();
            List<object> rhsParams = new List<object>();
            parent.DispatchBoolean(lhs, Arg.Do<List<object>>(x => lhsParams = x));
            parent.DispatchBoolean(rhs, Arg.Do<List<object>>(x => rhsParams = x));

            BooleanHelper booleanHelper = Utilities.GetBooleanHelper(parent);

            booleanHelper.OrBoolean(expression, parameters);

            Assert.AreEqual(expectedParams, lhsParams);
            Assert.AreEqual(expectedParams, rhsParams);
        }
        #endregion

        #region LiteralBoolean
        [DataRow(false, false)]
        [DataRow(true, true)]
        [TestMethod]
        public void LiteralBoolean_BooleanLiteralExpression_ReturnedCorrectValue(bool value, bool expected)
        {
            BooleanLiteralExpression e = new BooleanLiteralExpression(value, 0, 0);
            IInterpreterBoolean parent = Substitute.For<IInterpreterBoolean>();
            BooleanHelper booleanHelper = Utilities.GetBooleanHelper(parent);

            bool result = booleanHelper.LiteralBoolean(e);

            Assert.AreEqual(expected, result);
        }
        #endregion

        #region EqualBoolean
        [DataRow(0, 1, false)]
        [DataRow(1, 0, false)]
        [DataRow(0, 0, true)]
        [TestMethod]
        public void EqualBoolean_Int_CorrectValuesReturned(int lhsValue, int rhsValue, bool expected)
        {
            var lhs = Utilities.GetIntLitExpression();
            var rhs = Utilities.GetIntLitExpression();

            EqualExpression expression = new EqualExpression(lhs, rhs, 0, 0);
            expression.Type = TypeEnum.Integer;
            IInterpreterBoolean parent = Substitute.For<IInterpreterBoolean>();
            parent.DispatchInt(lhs, Arg.Any<List<object>>()).Returns(lhsValue);
            parent.DispatchInt(rhs, Arg.Any<List<object>>()).Returns(rhsValue);

            BooleanHelper booleanHelper = Utilities.GetBooleanHelper(parent);

            bool res = booleanHelper.EqualBoolean(expression, new List<object>());

            Assert.AreEqual(expected, res);
        }

        [DataRow(0, 1, false)]
        [DataRow(1, 0, false)]
        [DataRow(0, 0, true)]
        [TestMethod]
        public void EqualBoolean_Function_CorrectValuesReturned(int lhsValue, int rhsValue, bool expected)
        {
            var lhs = Utilities.GetFuncCallExpresssion();
            var rhs = Utilities.GetFuncCallExpresssion();

            EqualExpression expression = new EqualExpression(lhs, rhs, 0, 0);
            expression.Type = TypeEnum.Function;
            IInterpreterBoolean parent = Substitute.For<IInterpreterBoolean>();
            parent.DispatchFunction(lhs, Arg.Any<List<object>>()).Returns(lhsValue);
            parent.DispatchFunction(rhs, Arg.Any<List<object>>()).Returns(rhsValue);

            BooleanHelper booleanHelper = Utilities.GetBooleanHelper(parent);

            bool res = booleanHelper.EqualBoolean(expression, new List<object>());

            Assert.AreEqual(expected, res);
        }

        [DataRow(0.0, 1.0, false)]
        [DataRow(1.0, 0.0, false)]
        [DataRow(0.0, 0.0, true)]
        [TestMethod]
        public void EqualBoolean_Real_CorrectValuesReturned(double lhsValue, double rhsValue, bool expected)
        {
            var lhs = Utilities.GetRealLitExpression();
            var rhs = Utilities.GetRealLitExpression();

            EqualExpression expression = new EqualExpression(lhs, rhs, 0, 0);
            expression.Type = TypeEnum.Real;
            IInterpreterBoolean parent = Substitute.For<IInterpreterBoolean>();
            parent.DispatchReal(lhs, Arg.Any<List<object>>()).Returns(lhsValue);
            parent.DispatchReal(rhs, Arg.Any<List<object>>()).Returns(rhsValue);

            BooleanHelper booleanHelper = Utilities.GetBooleanHelper(parent);

            bool res = booleanHelper.EqualBoolean(expression, new List<object>());

            Assert.AreEqual(expected, res);
        }

        [DataRow(true, true, true)]
        [DataRow(true, false, false)]
        [DataRow(false, true, false)]
        [DataRow(false, false, true)]
        [TestMethod]
        public void EqualBoolean_Bool_CorrectValuesReturned(bool lhsValue, bool rhsValue, bool expected)
        {
            var lhs = Utilities.GetRealLitExpression();
            var rhs = Utilities.GetRealLitExpression();

            EqualExpression expression = new EqualExpression(lhs, rhs, 0, 0);
            expression.Type = TypeEnum.Boolean;
            IInterpreterBoolean parent = Substitute.For<IInterpreterBoolean>();
            parent.DispatchBoolean(lhs, Arg.Any<List<object>>()).Returns(lhsValue);
            parent.DispatchBoolean(rhs, Arg.Any<List<object>>()).Returns(rhsValue);

            BooleanHelper booleanHelper = Utilities.GetBooleanHelper(parent);

            bool res = booleanHelper.EqualBoolean(expression, new List<object>());

            Assert.AreEqual(expected, res);
        }

        [DataRow(new int[] { 0 }, new int[] { 0 }, true)]
        [DataRow(new int[] { 0 }, new int[] { 1 }, false)]
        [DataRow(new int[] { 1, 0 }, new int[] { 1 }, false)]
        [DataRow(new int[] { 1, 17 }, new int[] { 1, 17 }, true)]
        [TestMethod]
        public void EqualBoolean_Element_CorrectValuesReturned(int[] a, int[] b, bool expected)
        {
            Element lhsValue = new Element(a.ToList());
            Element rhsValue = new Element(b.ToList());

            var lhs = Utilities.GetRealLitExpression();
            var rhs = Utilities.GetRealLitExpression();

            EqualExpression expression = new EqualExpression(lhs, rhs, 0, 0);
            expression.Type = TypeEnum.Element;
            IInterpreterBoolean parent = Substitute.For<IInterpreterBoolean>();
            parent.DispatchElement(lhs, Arg.Any<List<object>>()).Returns(lhsValue);
            parent.DispatchElement(rhs, Arg.Any<List<object>>()).Returns(rhsValue);

            BooleanHelper booleanHelper = Utilities.GetBooleanHelper(parent);

            bool res = booleanHelper.EqualBoolean(expression, new List<object>());

            Assert.AreEqual(expected, res);
        }

        [DataRow(0, 0, true)]
        [DataRow(1, 0, false)]
        [DataRow(2, 3, false)]
        [DataRow(3, 3, true)]
        [TestMethod]
        public void EqualBoolean_Set_CorrectValuesReturned(int leftDataRow, int rightDataRow,  bool expected)
        {
            Set lhsValue = GetSet(leftDataRow);
            Set rhsValue = GetSet(rightDataRow);

            var lhs = Utilities.GetRealLitExpression();
            var rhs = Utilities.GetRealLitExpression();

            EqualExpression expression = new EqualExpression(lhs, rhs, 0, 0);
            expression.Type = TypeEnum.Set;
            IInterpreterBoolean parent = Substitute.For<IInterpreterBoolean>();
            parent.DispatchSet(lhs, Arg.Any<List<object>>()).Returns(lhsValue);
            parent.DispatchSet(rhs, Arg.Any<List<object>>()).Returns(rhsValue);

            BooleanHelper booleanHelper = Utilities.GetBooleanHelper(parent);

            bool res = booleanHelper.EqualBoolean(expression, new List<object>());

            Assert.AreEqual(expected, res);
        }

        private Set GetSet(int i)
        {
            Element[] elements = new Element[] { new Element(0),
                                                 new Element(new List<int> { 13, 4}),
                                                 new Element(new List<int> { 4, 10})};
            Set[] sets = new Set[] { new Set(new List<Element> { elements[0], 
                                                                 elements[2] }),
                                     new Set(new List<Element> { elements[0],
                                                                 elements[1] }),
                                     new Set(new List<Element> { elements[1] }),
                                     new Set(new List<Element> { elements[0],
                                                                 elements[2],
                                                                 elements[1]})};
            return sets[i];
        }

        [TestMethod]
        public void EqualBoolean_Int_CheckParametersPassedDown()
        {
            var lhs = Utilities.GetIntLitExpression();
            var rhs = Utilities.GetIntLitExpression();

            List<object> parameters = Utilities.GetParameterList(4);
            List<object> expectedParams = parameters;
            var expression = new EqualExpression(lhs, rhs, 0, 0);
            expression.Type = TypeEnum.Integer;
            IInterpreterBoolean parent = Substitute.For<IInterpreterBoolean>();
            List<object> lhsParams = new List<object>();
            List<object> rhsParams = new List<object>();
            parent.DispatchInt(lhs, Arg.Do<List<object>>(x => lhsParams = x));
            parent.DispatchInt(rhs, Arg.Do<List<object>>(x => rhsParams = x));

            BooleanHelper booleanHelper = Utilities.GetBooleanHelper(parent);

            booleanHelper.EqualBoolean(expression, parameters);
            
            Assert.AreEqual(expectedParams, lhsParams);
            Assert.AreEqual(expectedParams, rhsParams);
        }
        
        [TestMethod]
        public void EqualBoolean_Function_CheckParametersPassedDown()
        {
            var lhs = Utilities.GetFuncCallExpresssion();
            var rhs = Utilities.GetFuncCallExpresssion();

            List<object> parameters = Utilities.GetParameterList(4);
            List<object> expectedParams = parameters;
            var expression = new EqualExpression(lhs, rhs, 0, 0);
            expression.Type = TypeEnum.Function;
            IInterpreterBoolean parent = Substitute.For<IInterpreterBoolean>();
            List<object> lhsParams = new List<object>();
            List<object> rhsParams = new List<object>();
            parent.DispatchFunction(lhs, Arg.Do<List<object>>(x => lhsParams = x));
            parent.DispatchFunction(rhs, Arg.Do<List<object>>(x => rhsParams = x));

            BooleanHelper booleanHelper = Utilities.GetBooleanHelper(parent);

            booleanHelper.EqualBoolean(expression, parameters);
            
            Assert.AreEqual(expectedParams, lhsParams);
            Assert.AreEqual(expectedParams, rhsParams);
        }
        
        [TestMethod]
        public void EqualBoolean_Real_CheckParametersPassedDown()
        {
            var lhs = Utilities.GetRealLitExpression();
            var rhs = Utilities.GetRealLitExpression();

            List<object> parameters = Utilities.GetParameterList(4);
            List<object> expectedParams = parameters;
            var expression = new EqualExpression(lhs, rhs, 0, 0);
            expression.Type = TypeEnum.Real;
            IInterpreterBoolean parent = Substitute.For<IInterpreterBoolean>();
            List<object> lhsParams = new List<object>();
            List<object> rhsParams = new List<object>();
            parent.DispatchReal(lhs, Arg.Do<List<object>>(x => lhsParams = x));
            parent.DispatchReal(rhs, Arg.Do<List<object>>(x => rhsParams = x));

            BooleanHelper booleanHelper = Utilities.GetBooleanHelper(parent);

            booleanHelper.EqualBoolean(expression, parameters);
            
            Assert.AreEqual(expectedParams, lhsParams);
            Assert.AreEqual(expectedParams, rhsParams);
        }

        [TestMethod]
        public void EqualBoolean_Bool_CheckParametersPassedDown()
        {
            var lhs = Utilities.GetBoolLitExpression();
            var rhs = Utilities.GetBoolLitExpression();

            List<object> parameters = Utilities.GetParameterList(4);
            List<object> expectedParams = parameters;
            var expression = new EqualExpression(lhs, rhs, 0, 0);
            expression.Type = TypeEnum.Boolean;
            IInterpreterBoolean parent = Substitute.For<IInterpreterBoolean>();
            List<object> lhsParams = new List<object>();
            List<object> rhsParams = new List<object>();
            parent.DispatchBoolean(lhs, Arg.Do<List<object>>(x => lhsParams = x));
            parent.DispatchBoolean(rhs, Arg.Do<List<object>>(x => rhsParams = x));

            BooleanHelper booleanHelper = Utilities.GetBooleanHelper(parent);

            booleanHelper.EqualBoolean(expression, parameters);
            
            Assert.AreEqual(expectedParams, lhsParams);
            Assert.AreEqual(expectedParams, rhsParams);
        }

        [TestMethod]
        public void EqualBoolean_Set_CheckParametersPassedDown()
        {
            var lhs = Utilities.GetBoolLitExpression();
            var rhs = Utilities.GetBoolLitExpression();

            List<object> parameters = Utilities.GetParameterList(4);
            List<object> expectedParams = parameters;
            var expression = new EqualExpression(lhs, rhs, 0, 0);
            expression.Type = TypeEnum.Set;
            IInterpreterBoolean parent = Substitute.For<IInterpreterBoolean>();
            List<object> lhsParams = new List<object>();
            List<object> rhsParams = new List<object>();
            Set testSet = new Set(new Element(17));
            parent.DispatchSet(lhs, Arg.Do<List<object>>(x => lhsParams = x)).Returns(testSet);
            parent.DispatchSet(rhs, Arg.Do<List<object>>(x => rhsParams = x)).Returns(testSet);

            BooleanHelper booleanHelper = Utilities.GetBooleanHelper(parent);

            booleanHelper.EqualBoolean(expression, parameters);

            Assert.AreEqual(expectedParams, lhsParams);
            Assert.AreEqual(expectedParams, rhsParams);
        }

        [TestMethod]
        public void EqualBoolean_Element_CheckParametersPassedDown()
        {
            var lhs = Utilities.GetBoolLitExpression();
            var rhs = Utilities.GetBoolLitExpression();

            List<object> parameters = Utilities.GetParameterList(4);
            List<object> expectedParams = parameters;
            var expression = new EqualExpression(lhs, rhs, 0, 0);
            expression.Type = TypeEnum.Element;
            IInterpreterBoolean parent = Substitute.For<IInterpreterBoolean>();
            List<object> lhsParams = new List<object>();
            List<object> rhsParams = new List<object>();
            Element testElement = new Element(17);
            parent.DispatchElement(lhs, Arg.Do<List<object>>(x => lhsParams = x)).Returns(testElement);
            parent.DispatchElement(rhs, Arg.Do<List<object>>(x => rhsParams = x)).Returns(testElement);

            BooleanHelper booleanHelper = Utilities.GetBooleanHelper(parent);

            booleanHelper.EqualBoolean(expression, parameters);

            Assert.AreEqual(expectedParams, lhsParams);
            Assert.AreEqual(expectedParams, rhsParams);
        }
        #endregion

        #region NotEqualBoolean
        [DataRow(0, 1, true)]
        [DataRow(1, 0, true)]
        [DataRow(0, 0, false)]
        [TestMethod]
        public void NotEqualBoolean_Int_CorrectValuesReturned(int lhsValue, int rhsValue, bool expected)
        {
            var lhs = Utilities.GetIntLitExpression();
            var rhs = Utilities.GetIntLitExpression();

            var expression = new NotEqualExpression(lhs, rhs, 0, 0);
            expression.Type = TypeEnum.Integer;
            IInterpreterBoolean parent = Substitute.For<IInterpreterBoolean>();
            parent.DispatchInt(lhs, Arg.Any<List<object>>()).Returns(lhsValue);
            parent.DispatchInt(rhs, Arg.Any<List<object>>()).Returns(rhsValue);

            BooleanHelper booleanHelper = Utilities.GetBooleanHelper(parent);

            bool res = booleanHelper.NotEqualBoolean(expression, new List<object>());

            Assert.AreEqual(expected, res);
        }

        [DataRow(0, 1, true)]
        [DataRow(1, 0, true)]
        [DataRow(0, 0, false)]
        [TestMethod]
        public void NotEqualBoolean_Function_CorrectValuesReturned(int lhsValue, int rhsValue, bool expected)
        {
            var lhs = Utilities.GetIntLitExpression();
            var rhs = Utilities.GetIntLitExpression();

            var expression = new NotEqualExpression(lhs, rhs, 0, 0);
            expression.Type = TypeEnum.Function;
            IInterpreterBoolean parent = Substitute.For<IInterpreterBoolean>();
            parent.DispatchFunction(lhs, Arg.Any<List<object>>()).Returns(lhsValue);
            parent.DispatchFunction(rhs, Arg.Any<List<object>>()).Returns(rhsValue);

            BooleanHelper booleanHelper = Utilities.GetBooleanHelper(parent);

            bool res = booleanHelper.NotEqualBoolean(expression, new List<object>());

            Assert.AreEqual(expected, res);
        }

        [DataRow(0.0, 1.0, true)]
        [DataRow(1.0, 0.0, true)]
        [DataRow(0.0, 0.0, false)]
        [TestMethod]
        public void NotEqualBoolean_Real_CorrectValuesReturned(double lhsValue, double rhsValue, bool expected)
        {
            var lhs = Utilities.GetRealLitExpression();
            var rhs = Utilities.GetRealLitExpression();

            var expression = new NotEqualExpression(lhs, rhs, 0, 0);
            expression.Type = TypeEnum.Real;
            IInterpreterBoolean parent = Substitute.For<IInterpreterBoolean>();
            parent.DispatchReal(lhs, Arg.Any<List<object>>()).Returns(lhsValue);
            parent.DispatchReal(rhs, Arg.Any<List<object>>()).Returns(rhsValue);

            BooleanHelper booleanHelper = Utilities.GetBooleanHelper(parent);

            bool res = booleanHelper.NotEqualBoolean(expression, new List<object>());

            Assert.AreEqual(expected, res);
        }

        [DataRow(true, true, false)]
        [DataRow(true, false, true)]
        [DataRow(false, true, true)]
        [DataRow(false, false, false)]
        [TestMethod]
        public void NotEqualBoolean_Bool_CorrectValuesReturned(bool lhsValue, bool rhsValue, bool expected)
        {
            var lhs = Utilities.GetRealLitExpression();
            var rhs = Utilities.GetRealLitExpression();

            var expression = new NotEqualExpression(lhs, rhs, 0, 0);
            expression.Type = TypeEnum.Boolean;
            IInterpreterBoolean parent = Substitute.For<IInterpreterBoolean>();
            parent.DispatchBoolean(lhs, Arg.Any<List<object>>()).Returns(lhsValue);
            parent.DispatchBoolean(rhs, Arg.Any<List<object>>()).Returns(rhsValue);

            BooleanHelper booleanHelper = Utilities.GetBooleanHelper(parent);

            bool res = booleanHelper.NotEqualBoolean(expression, new List<object>());

            Assert.AreEqual(expected, res);
        }
        
        [TestMethod]
        public void NotEqualBoolean_Int_CheckParametersPassedDown()
        {
            var lhs = Utilities.GetIntLitExpression();
            var rhs = Utilities.GetIntLitExpression();

            List<object> parameters = Utilities.GetParameterList(4);
            List<object> expectedParams = parameters;
            var expression = new NotEqualExpression(lhs, rhs, 0, 0);
            expression.Type = TypeEnum.Integer;
            IInterpreterBoolean parent = Substitute.For<IInterpreterBoolean>();
            List<object> lhsParams = new List<object>();
            List<object> rhsParams = new List<object>();
            parent.DispatchInt(lhs, Arg.Do<List<object>>(x => lhsParams = x));
            parent.DispatchInt(rhs, Arg.Do<List<object>>(x => rhsParams = x));

            BooleanHelper booleanHelper = Utilities.GetBooleanHelper(parent);

            booleanHelper.NotEqualBoolean(expression, parameters);
            
            Assert.AreEqual(expectedParams, lhsParams);
            Assert.AreEqual(expectedParams, rhsParams);
        }
        
        [TestMethod]
        public void NotEqualBoolean_Function_CheckParametersPassedDown()
        {
            var lhs = Utilities.GetFuncCallExpresssion();
            var rhs = Utilities.GetFuncCallExpresssion();

            List<object> parameters = Utilities.GetParameterList(4);
            List<object> expectedParams = parameters;
            var expression = new NotEqualExpression(lhs, rhs, 0, 0);
            expression.Type = TypeEnum.Function;
            IInterpreterBoolean parent = Substitute.For<IInterpreterBoolean>();
            List<object> lhsParams = new List<object>();
            List<object> rhsParams = new List<object>();
            parent.DispatchFunction(lhs, Arg.Do<List<object>>(x => lhsParams = x));
            parent.DispatchFunction(rhs, Arg.Do<List<object>>(x => rhsParams = x));

            BooleanHelper booleanHelper = Utilities.GetBooleanHelper(parent);

            booleanHelper.NotEqualBoolean(expression, parameters);
            
            Assert.AreEqual(expectedParams, lhsParams);
            Assert.AreEqual(expectedParams, rhsParams);
        }
        
        [TestMethod]
        public void NotEqualBoolean_Real_CheckParametersPassedDown()
        {
            var lhs = Utilities.GetRealLitExpression();
            var rhs = Utilities.GetRealLitExpression();

            List<object> parameters = Utilities.GetParameterList(4);
            List<object> expectedParams = parameters;
            var expression = new NotEqualExpression(lhs, rhs, 0, 0);
            expression.Type = TypeEnum.Real;
            IInterpreterBoolean parent = Substitute.For<IInterpreterBoolean>();
            List<object> lhsParams = new List<object>();
            List<object> rhsParams = new List<object>();
            parent.DispatchReal(lhs, Arg.Do<List<object>>(x => lhsParams = x));
            parent.DispatchReal(rhs, Arg.Do<List<object>>(x => rhsParams = x));

            BooleanHelper booleanHelper = Utilities.GetBooleanHelper(parent);

            booleanHelper.NotEqualBoolean(expression, parameters);
            
            Assert.AreEqual(expectedParams, lhsParams);
            Assert.AreEqual(expectedParams, rhsParams);
        }

        [TestMethod]
        public void NotEqualBoolean_Bool_CheckParametersPassedDown()
        {
            var lhs = Utilities.GetBoolLitExpression();
            var rhs = Utilities.GetBoolLitExpression();

            List<object> parameters = Utilities.GetParameterList(4);
            List<object> expectedParams = parameters;
            var expression = new NotEqualExpression(lhs, rhs, 0, 0);
            expression.Type = TypeEnum.Boolean;
            IInterpreterBoolean parent = Substitute.For<IInterpreterBoolean>();
            List<object> lhsParams = new List<object>();
            List<object> rhsParams = new List<object>();
            parent.DispatchBoolean(lhs, Arg.Do<List<object>>(x => lhsParams = x));
            parent.DispatchBoolean(rhs, Arg.Do<List<object>>(x => rhsParams = x));

            BooleanHelper booleanHelper = Utilities.GetBooleanHelper(parent);

            booleanHelper.NotEqualBoolean(expression, parameters);
            
            Assert.AreEqual(expectedParams, lhsParams);
            Assert.AreEqual(expectedParams, rhsParams);
        }
        #endregion

        // Assumption: Relational operators only works for real
        //             - TyperChecker have casted children from type Integer to type Real

        #region GreaterEqualBoolean
        // a >= b

        [DataRow(0.0, 1.0, false)]
        [DataRow(1.0, 0.0, true)]
        [DataRow(0.0, 0.0, true)]
        [TestMethod]
        public void GreaterEqualBoolean_Real_CorrectValuesReturned(double lhsValue, double rhsValue, bool expected)
        {
            var lhs = Utilities.GetRealLitExpression();
            var rhs = Utilities.GetRealLitExpression();

            var expression = new GreaterEqualExpression(lhs, rhs, 0, 0);
            IInterpreterBoolean parent = Substitute.For<IInterpreterBoolean>();
            parent.DispatchReal(lhs, Arg.Any<List<object>>()).Returns(lhsValue);
            parent.DispatchReal(rhs, Arg.Any<List<object>>()).Returns(rhsValue);

            BooleanHelper booleanHelper = Utilities.GetBooleanHelper(parent);

            bool res = booleanHelper.GreaterEqualBoolean(expression, new List<object>());
            
            Assert.AreEqual(expected, res);
        }
        
        [TestMethod]
        public void GreaterEqualBoolean_Real_CheckParametersPassedDown()
        {
            var lhs = Utilities.GetRealLitExpression();
            var rhs = Utilities.GetRealLitExpression();

            List<object> parameters = Utilities.GetParameterList(4);
            List<object> expectedParams = parameters;
            var expression = new GreaterEqualExpression(lhs, rhs, 0, 0);
            IInterpreterBoolean parent = Substitute.For<IInterpreterBoolean>();
            List<object> lhsParams = new List<object>();
            List<object> rhsParams = new List<object>();
            parent.DispatchReal(lhs, Arg.Do<List<object>>(x => lhsParams = x));
            parent.DispatchReal(rhs, Arg.Do<List<object>>(x => rhsParams = x));

            BooleanHelper booleanHelper = Utilities.GetBooleanHelper(parent);

            booleanHelper.GreaterEqualBoolean(expression, parameters);
            
            Assert.AreEqual(expectedParams, lhsParams);
            Assert.AreEqual(expectedParams, rhsParams);
        }
        #endregion
        
        #region GreaterBoolean
        // a > b
        
        [DataRow(0.0, 1.0, false)]
        [DataRow(1.0, 0.0, true)]
        [DataRow(0.0, 0.0, false)]
        [TestMethod]
        public void GreaterBoolean_Real_CorrectValuesReturned(double lhsValue, double rhsValue, bool expected)
        {
            var lhs = Utilities.GetRealLitExpression();
            var rhs = Utilities.GetRealLitExpression();

            var expression = new GreaterExpression(lhs, rhs, 0, 0);
            IInterpreterBoolean parent = Substitute.For<IInterpreterBoolean>();
            parent.DispatchReal(lhs, Arg.Any<List<object>>()).Returns(lhsValue);
            parent.DispatchReal(rhs, Arg.Any<List<object>>()).Returns(rhsValue);

            BooleanHelper booleanHelper = Utilities.GetBooleanHelper(parent);

            bool res = booleanHelper.GreaterBoolean(expression, new List<object>());
            
            Assert.AreEqual(expected, res);
        }
        
        [TestMethod]
        public void GreaterBoolean_Real_CheckParametersPassedDown()
        {
            var lhs = Utilities.GetRealLitExpression();
            var rhs = Utilities.GetRealLitExpression();

            List<object> parameters = Utilities.GetParameterList(4);
            List<object> expectedParams = parameters;
            var expression = new GreaterExpression(lhs, rhs, 0, 0);
            IInterpreterBoolean parent = Substitute.For<IInterpreterBoolean>();
            List<object> lhsParams = new List<object>();
            List<object> rhsParams = new List<object>();
            parent.DispatchReal(lhs, Arg.Do<List<object>>(x => lhsParams = x));
            parent.DispatchReal(rhs, Arg.Do<List<object>>(x => rhsParams = x));

            BooleanHelper booleanHelper = Utilities.GetBooleanHelper(parent);

            booleanHelper.GreaterBoolean(expression, parameters);
            
            Assert.AreEqual(expectedParams, lhsParams);
            Assert.AreEqual(expectedParams, rhsParams);
        }
        #endregion
        
        #region LessEqualBoolean
        // a <= b
        
        [DataRow(0.0, 1.0, true)]
        [DataRow(1.0, 0.0, false)]
        [DataRow(0.0, 0.0, true)]
        [TestMethod]
        public void LessEqualBoolean_Real_CorrectValuesReturned(double lhsValue, double rhsValue, bool expected)
        {
            var lhs = Utilities.GetRealLitExpression();
            var rhs = Utilities.GetRealLitExpression();

            var expression = new LessEqualExpression(lhs, rhs, 0, 0);
            IInterpreterBoolean parent = Substitute.For<IInterpreterBoolean>();
            parent.DispatchReal(lhs, Arg.Any<List<object>>()).Returns(lhsValue);
            parent.DispatchReal(rhs, Arg.Any<List<object>>()).Returns(rhsValue);

            BooleanHelper booleanHelper = Utilities.GetBooleanHelper(parent);

            bool res = booleanHelper.LessEqualBoolean(expression, new List<object>());
            
            Assert.AreEqual(expected, res);
        }
        
        [TestMethod]
        public void LessEqualBoolean_Real_CheckParametersPassedDown()
        {
            var lhs = Utilities.GetRealLitExpression();
            var rhs = Utilities.GetRealLitExpression();

            List<object> parameters = Utilities.GetParameterList(4);
            List<object> expectedParams = parameters;
            var expression = new LessEqualExpression(lhs, rhs, 0, 0);
            IInterpreterBoolean parent = Substitute.For<IInterpreterBoolean>();
            List<object> lhsParams = new List<object>();
            List<object> rhsParams = new List<object>();
            parent.DispatchReal(lhs, Arg.Do<List<object>>(x => lhsParams = x));
            parent.DispatchReal(rhs, Arg.Do<List<object>>(x => rhsParams = x));

            BooleanHelper booleanHelper = Utilities.GetBooleanHelper(parent);

            booleanHelper.LessEqualBoolean(expression, parameters);
            
            Assert.AreEqual(expectedParams, lhsParams);
            Assert.AreEqual(expectedParams, rhsParams);
        }
        #endregion
        
        #region LessBoolean
        // a < b
        
        [DataRow(0.0, 1.0, true)]
        [DataRow(1.0, 0.0, false)]
        [DataRow(0.0, 0.0, false)]
        [TestMethod]
        public void LessBoolean_Real_CorrectValuesReturned(double lhsValue, double rhsValue, bool expected)
        {
            var lhs = Utilities.GetRealLitExpression();
            var rhs = Utilities.GetRealLitExpression();

            var expression = new LessExpression(lhs, rhs, 0, 0);
            IInterpreterBoolean parent = Substitute.For<IInterpreterBoolean>();
            parent.DispatchReal(lhs, Arg.Any<List<object>>()).Returns(lhsValue);
            parent.DispatchReal(rhs, Arg.Any<List<object>>()).Returns(rhsValue);

            BooleanHelper booleanHelper = Utilities.GetBooleanHelper(parent);

            bool res = booleanHelper.LessBoolean(expression, new List<object>());
            
            Assert.AreEqual(expected, res);
        }
        
        [TestMethod]
        public void LessBoolean_Real_CheckParametersPassedDown()
        {
            var lhs = Utilities.GetRealLitExpression();
            var rhs = Utilities.GetRealLitExpression();

            List<object> parameters = Utilities.GetParameterList(4);
            List<object> expectedParams = parameters;
            var expression = new LessExpression(lhs, rhs, 0, 0);
            IInterpreterBoolean parent = Substitute.For<IInterpreterBoolean>();
            List<object> lhsParams = new List<object>();
            List<object> rhsParams = new List<object>();
            parent.DispatchReal(lhs, Arg.Do<List<object>>(x => lhsParams = x));
            parent.DispatchReal(rhs, Arg.Do<List<object>>(x => rhsParams = x));

            BooleanHelper booleanHelper = Utilities.GetBooleanHelper(parent);

            booleanHelper.LessBoolean(expression, parameters);
            
            Assert.AreEqual(expectedParams, lhsParams);
            Assert.AreEqual(expectedParams, rhsParams);
        }
        #endregion

    }
}