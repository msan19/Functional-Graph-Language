using System;
using System.Collections.Generic;
using ASTLib.Nodes;
using ASTLib.Nodes.ExpressionNodes;
using ASTLib.Nodes.ExpressionNodes.OperationNodes;
using ASTLib.Nodes.TypeNodes;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;

namespace TypeCheckerLib.Tests.HelperTests
{
    [TestClass]
    public class DeclarationHelperTests
    {
        #region Export
        [TestMethod]
        public void Export__CorrectParameterPassDown()
        {
            var expected = new List<TypeNode>();
            ExportNode input1 = new ExportNode(new AdditionExpression(null, null, 0, 0), 0, 0);
            ITypeChecker parent = Substitute.For<ITypeChecker>();
            List<TypeNode> res = null;
            parent.Dispatch(Arg.Any<ExpressionNode>(), Arg.Do<List<TypeNode>>(x => res = x)).Returns(new TypeNode(TypeEnum.Real, 1, 1));
            TypeHelper typeHelper = new TypeHelper()
            {
                TypeChecker = parent
            };

            typeHelper.VisitExport(input1);

            res.Should().BeEquivalentTo(expected);
        }

        // Real     -> 
        // Integer  ->  
        // Integer  -> Insert Cast Node
        // Func     -> Throw Exception
        [TestMethod]
        public void Export_Real_Nothing()
        {
            ExportNode input1 = new ExportNode(new AdditionExpression(null, null, 0, 0), 0, 0);
            ITypeChecker parent = Substitute.For<ITypeChecker>();
            parent.Dispatch(Arg.Any<ExpressionNode>(), Arg.Any<List<TypeNode>>()).Returns(new TypeNode(TypeEnum.Real, 1, 1));
            TypeHelper typeHelper = new TypeHelper()
            {
                TypeChecker = parent
            };

            typeHelper.VisitExport(input1);
        }
        [TestMethod]
        public void Export_Integer_Nothing()
        {
            ExportNode input1 = new ExportNode(new AdditionExpression(null, null, 0, 0), 0, 0);
            ITypeChecker parent = Substitute.For<ITypeChecker>();
            parent.Dispatch(Arg.Any<ExpressionNode>(), Arg.Any<List<TypeNode>>()).Returns(new TypeNode(TypeEnum.Integer, 1, 1));
            TypeHelper typeHelper = new TypeHelper()
            {
                TypeChecker = parent
            };

            typeHelper.VisitExport(input1);
        }
        [TestMethod]
        public void Export_Integer_InsertCastNode()
        {
            ExportNode input1 = new ExportNode(new AdditionExpression(null, null, 0, 0), 0, 0);
            ITypeChecker parent = Substitute.For<ITypeChecker>();
            parent.Dispatch(Arg.Any<ExpressionNode>(), Arg.Any<List<TypeNode>>()).Returns(new TypeNode(TypeEnum.Integer, 1, 1));
            TypeHelper typeHelper = new TypeHelper()
            {
                TypeChecker = parent
            };

            typeHelper.VisitExport(input1);
            var res = input1.ExportValue.GetType();

            Assert.AreEqual(typeof(CastFromIntegerExpression), res);
        }
        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void Export_Func_ThrowException()
        {
            ExportNode input1 = new ExportNode(new AdditionExpression(null, null, 0, 0), 0, 0);
            ITypeChecker parent = Substitute.For<ITypeChecker>();
            parent.Dispatch(Arg.Any<ExpressionNode>(), Arg.Any<List<TypeNode>>()).Returns(new TypeNode(TypeEnum.Function, 1, 1));
            TypeHelper typeHelper = new TypeHelper()
            {
                TypeChecker = parent
            };

            typeHelper.VisitExport(input1);
        }
        #endregion

        #region Function
        [TestMethod]
        public void Function__CorrectParameterPassDown()
        {
            var funcType = Utilities.GetFunctionType(TypeEnum.Integer, new List<TypeEnum>());
            var expected = funcType.ParameterTypes.ToList();

            var condition = new ConditionNode(new AdditionExpression(null, null, 0, 0), 0, 0);
            FunctionNode input1 = new FunctionNode("", condition, null, funcType, 0, 0);

            ITypeChecker parent = Substitute.For<ITypeChecker>();
            List<TypeNode> res = null;
            parent.Dispatch(Arg.Any<ExpressionNode>(), Arg.Do<List<TypeNode>>(x => res = x)).Returns(new TypeNode(TypeEnum.Integer, 1, 1));
            TypeHelper typeHelper = new TypeHelper()
            {
                TypeChecker = parent
            };

            typeHelper.VisitFunction(input1);

            res.Should().BeEquivalentTo(expected);
        }

        // Condition ReturnType RHS
        // IntegerInteger   -> Nothing
        // RealInteger      -> Insert Cast Node
        // IntegerReal      -> Throw Exception
        // RealFunction     -> Throw Exception

        /* Not in version 1
            // Conditions LHS booleans
            // Boolean      -> Nothing
            // Real         -> Throw Exception
            // Function     -> Throw Exception
        */
        [DataRow(TypeEnum.Integer, TypeEnum.Integer)]
        [DataRow(TypeEnum.Real, TypeEnum.Real)]
        [DataRow(TypeEnum.Function, TypeEnum.Function)]
        [TestMethod]
        public void Function_Type_CorrectType(TypeEnum functionReturnType, TypeEnum dispatcherReturnType)
        {
            var condition = new ConditionNode(new AdditionExpression(null, null, 0, 0), 0, 0);
            var funcType = Utilities.GetFunctionType(functionReturnType, new List<TypeEnum>());
            FunctionNode input1 = new FunctionNode("", condition, null, funcType, 0, 0);

            ITypeChecker parent = Substitute.For<ITypeChecker>();
            parent.Dispatch(Arg.Any<ExpressionNode>(), Arg.Any<List<TypeNode>>()).Returns(new TypeNode(dispatcherReturnType, 1, 1));
            TypeHelper typeHelper = new TypeHelper()
            {
                TypeChecker = parent
            };

            typeHelper.VisitFunction(input1);
        }

        [DataRow(TypeEnum.Integer, TypeEnum.Real)]
        [DataRow(TypeEnum.Real, TypeEnum.Function)]
        [DataRow(TypeEnum.Function, TypeEnum.Integer)]
        [DataRow(TypeEnum.Function, TypeEnum.Real)]
        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void Function_Type_WrongTypeAndThrowException(TypeEnum functionReturnType, TypeEnum dispatcherReturnType)
        {
            var condition = new ConditionNode(new AdditionExpression(null, null, 0, 0), 0, 0);
            var funcType = Utilities.GetFunctionType(functionReturnType, new List<TypeEnum>());
            FunctionNode input1 = new FunctionNode("", condition, null, funcType, 0, 0);

            ITypeChecker parent = Substitute.For<ITypeChecker>();
            parent.Dispatch(Arg.Any<ExpressionNode>(), Arg.Any<List<TypeNode>>()).Returns(new TypeNode(dispatcherReturnType, 1, 1));
            TypeHelper typeHelper = new TypeHelper()
            {
                TypeChecker = parent
            };

            typeHelper.VisitFunction(input1);
        }

        [TestMethod]
        public void Function_ReturnRealGetInteger_InsertCastNode()
        {
            var expected = typeof(CastFromIntegerExpression);
            var condition = new ConditionNode(new AdditionExpression(null, null, 0, 0), 0, 0);
            var funcType = Utilities.GetFunctionType(TypeEnum.Real, new List<TypeEnum>());
            FunctionNode input1 = new FunctionNode("", condition, null, funcType, 0, 0);

            ITypeChecker parent = Substitute.For<ITypeChecker>();
            parent.Dispatch(Arg.Any<ExpressionNode>(), Arg.Any<List<TypeNode>>()).Returns(new TypeNode(TypeEnum.Integer, 1, 1));
            TypeHelper typeHelper = new TypeHelper()
            {
                TypeChecker = parent
            };

            typeHelper.VisitFunction(input1);
            var res = input1.Conditions[0].ReturnExpression.GetType();

            Assert.AreEqual(expected, res);
        }
        #endregion

        #region Identifier
        // Global
        // 1 glob, is match     -> int, real, function ... 
        // Local
        // 1 local, is match    -> int, real, function


        // 1 glob, is match     -> int, real, function ... 
        [DataRow(TypeEnum.Integer)]
        [DataRow(TypeEnum.Real)]
        [DataRow(TypeEnum.Function)]
        [TestMethod]
        public void Identifier_OneGlobalIsMatch_CorrectType(TypeEnum expectedType)
        {
            IdentifierExpression input1 = new IdentifierExpression("", 1, 1);
            input1.IsLocal = false;
            input1.Reference = 0;

            var funcParams = new List<TypeNode>()
            {
            };

            var ast = GetAst();
            ast.Functions.Add(GetFunctionNode(expectedType, new List<TypeEnum>()));
            
            TypeHelper typeHelper = new TypeHelper();
            typeHelper.SetAstRoot(ast);

            var res = typeHelper.VisitIdentifier(input1, funcParams).Type;

            Assert.AreEqual(expectedType, res);
        }

        // 1 local, is match    -> int, real, function
        [DataRow(TypeEnum.Integer)]
        [DataRow(TypeEnum.Real)]
        [DataRow(TypeEnum.Function)]
        [TestMethod]
        public void Identifier_OneLocalIsMatch_CorrectType(TypeEnum expectedType)
        {
            IdentifierExpression input1 = new IdentifierExpression("", 1, 1);
            input1.IsLocal = true;
            input1.Reference = 0;

            var funcParams = new List<TypeNode>()
            {
                GetTypeNode(expectedType),
            };

            TypeHelper typeHelper = new TypeHelper();

            var res = typeHelper.VisitIdentifier(input1, funcParams).Type;

            Assert.AreEqual(expectedType, res);
        }

        private TypeNode GetTypeNode(TypeEnum type)
        {
            return new TypeNode(type, 0, 0);
        }
        #endregion

        #region Integer 
        // Returns Integer
        [TestMethod]
        public void Int_IntLiteral_IntTypeNode()
        {
            var expected = TypeEnum.Integer;
            IntegerLiteralExpression input1 = new IntegerLiteralExpression("2", 2, 2);
            TypeHelper typeHelper = new TypeHelper();

            var res = typeHelper.VisitIntegerLiteral(input1, null).Type;

            Assert.AreEqual(expected, res);
        }
        #endregion

        #region Real 
        // Returns Real
        [TestMethod]
        public void Real_RealLiteral_RealTypeNode()
        {
            var expected = TypeEnum.Real;
            RealLiteralExpression input1 = new RealLiteralExpression("2.2", 2, 2);
            TypeHelper typeHelper = new TypeHelper();

            var res = typeHelper.VisitRealLiteral(input1, null).Type;

            Assert.AreEqual(expected, res);
        }
        #endregion
        
    }
}