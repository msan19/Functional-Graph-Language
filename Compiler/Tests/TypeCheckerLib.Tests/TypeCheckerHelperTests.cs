using ASTLib;
using ASTLib.Interfaces;
using ASTLib.Nodes;
using ASTLib.Nodes.ExpressionNodes;
using ASTLib.Nodes.ExpressionNodes.OperationNodes;
using ASTLib.Nodes.TypeNodes;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using TypeCheckerLib.Helpers;

namespace TypeCheckerLib.Tests
{
    [TestClass]
    public class TypeCheckerHelperTests
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
            var funcType = GetFunctionType(TypeEnum.Integer, new List<TypeEnum>());
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
            var funcType = GetFunctionType(functionReturnType, new List<TypeEnum>());
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
            var funcType = GetFunctionType(functionReturnType, new List<TypeEnum>());
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
            var funcType = GetFunctionType(TypeEnum.Real, new List<TypeEnum>());
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

        #region Binary Num Operator
        [TestMethod]
        public void BinaryNumOp__CorrectParameterPassDown()
        {
            var expected = new List<TypeNode>()
            {
                GetFunctionType(TypeEnum.Integer, new List<TypeEnum>() {TypeEnum.Integer})
            };
            IntegerLiteralExpression intLit = new IntegerLiteralExpression("1", 1, 1);
            RealLiteralExpression realLit = new RealLiteralExpression("2.2", 2, 2);
            IBinaryNumberOperator input1 = new MultiplicationExpression(intLit, realLit, 1, 1);
            
            ITypeChecker parent = Substitute.For<ITypeChecker>();
            List<TypeNode> res = null;
            parent.Dispatch(Arg.Any<RealLiteralExpression>(), Arg.Do<List<TypeNode>>(x => res = x)).Returns(new TypeNode(TypeEnum.Real, 1, 1));
            parent.Dispatch(Arg.Any<IntegerLiteralExpression>(), Arg.Any<List<TypeNode>>()).Returns(new TypeNode(TypeEnum.Integer, 1, 1));
            TypeHelper typeHelper = new TypeHelper()
            {
                TypeChecker = parent
            };

            typeHelper.VisitBinaryNumOp(input1, expected.ToList());
         
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
            ITypeChecker parent = Substitute.For<ITypeChecker>();
            parent.Dispatch(Arg.Any<RealLiteralExpression>(), Arg.Any<List<TypeNode>>()).Returns(new TypeNode(TypeEnum.Real, 1, 1));
            parent.Dispatch(Arg.Any<IntegerLiteralExpression>(), Arg.Any<List<TypeNode>>()).Returns(new TypeNode(TypeEnum.Integer, 1, 1));
            TypeHelper typeHelper = new TypeHelper()
            {
                TypeChecker = parent
            };

            typeHelper.VisitBinaryNumOp(input1, null);
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
            ITypeChecker parent = Substitute.For<ITypeChecker>();
            parent.Dispatch(Arg.Any<RealLiteralExpression>(), Arg.Any<List<TypeNode>>()).Returns(new TypeNode(TypeEnum.Real, 1, 1));
            parent.Dispatch(Arg.Any<IntegerLiteralExpression>(), Arg.Any<List<TypeNode>>()).Returns(new TypeNode(TypeEnum.Integer, 1, 1));
            TypeHelper typeHelper = new TypeHelper()
            {
                TypeChecker = parent
            };

            typeHelper.VisitBinaryNumOp(input1, null);
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
            ITypeChecker parent = Substitute.For<ITypeChecker>();
            parent.Dispatch(Arg.Any<RealLiteralExpression>(), Arg.Any<List<TypeNode>>()).Returns(new TypeNode(TypeEnum.Real, 1, 1));
            parent.Dispatch(Arg.Any<IntegerLiteralExpression>(), Arg.Any<List<TypeNode>>()).Returns(new TypeNode(TypeEnum.Integer, 1, 1));
            TypeHelper typeHelper = new TypeHelper()
            {
                TypeChecker = parent
            };

            typeHelper.VisitBinaryNumOp(input1, null);
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
            ITypeChecker parent = Substitute.For<ITypeChecker>();
            parent.Dispatch(Arg.Any<RealLiteralExpression>(), Arg.Any<List<TypeNode>>()).Returns(new TypeNode(TypeEnum.Real, 1, 1));
            parent.Dispatch(Arg.Any<IntegerLiteralExpression>(), Arg.Any<List<TypeNode>>()).Returns(new TypeNode(TypeEnum.Integer, 1, 1));
            TypeHelper typeHelper = new TypeHelper()
            {
                TypeChecker = parent
            };

            typeHelper.VisitBinaryNumOp(input1, null);
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
            ITypeChecker parent = Substitute.For<ITypeChecker>();
            parent.Dispatch(Arg.Any<RealLiteralExpression>(), Arg.Any<List<TypeNode>>()).Returns(new TypeNode(TypeEnum.Real, 1, 1));
            parent.Dispatch(Arg.Any<IntegerLiteralExpression>(), Arg.Any<List<TypeNode>>()).Returns(new TypeNode(TypeEnum.Integer, 1, 1));
            TypeHelper typeHelper = new TypeHelper()
            {
                TypeChecker = parent
            };

            var res = typeHelper.VisitBinaryNumOp(input1, null).Type;

            Assert.AreEqual(expected, res);
        }

        [TestMethod]
        public void BinaryNumOp_MultiplicationExpressionWithTwoInt_ReturnsIntTypeNode()
        {
            var expected = TypeEnum.Integer;
            IntegerLiteralExpression intLit1 = new IntegerLiteralExpression("1", 1, 1);
            IntegerLiteralExpression intLit2 = new IntegerLiteralExpression("2", 2, 2);
            IBinaryNumberOperator input1 = new MultiplicationExpression(intLit1, intLit2, 1, 1);
            ITypeChecker parent = Substitute.For<ITypeChecker>();
            parent.Dispatch(Arg.Any<RealLiteralExpression>(), Arg.Any<List<TypeNode>>()).Returns(new TypeNode(TypeEnum.Real, 1, 1));
            parent.Dispatch(Arg.Any<IntegerLiteralExpression>(), Arg.Any<List<TypeNode>>()).Returns(new TypeNode(TypeEnum.Integer, 1, 1));
            TypeHelper typeHelper = new TypeHelper()
            {
                TypeChecker = parent
            };

            var res = typeHelper.VisitBinaryNumOp(input1, null).Type;

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
            TypeHelper typeHelper = new TypeHelper()
            {
                TypeChecker = parent
            };

            var res = typeHelper.VisitBinaryNumOp(input1, null).Type;
        }
        
        #endregion

        #region Function Call

        // Check for correct Parameter Passdown
        [TestMethod]
        public void FunctionCall_Local_CorrectParameterPassDown()
        {
            var expected = new List<TypeNode>()
            {
                GetFunctionType(TypeEnum.Integer, new List<TypeEnum>() {TypeEnum.Integer})
            };
            var children = new List<ExpressionNode>
            {
                new IntegerLiteralExpression("0", 0, 0)
            };
            FunctionCallExpression input1 = new FunctionCallExpression("", children, 1, 1);
            input1.LocalReference = 0;
            input1.GlobalReferences = new List<int>() { 0 };

            var ast = GetAst();
            ast.Functions.Add(GetFunctionNode(TypeEnum.Real, new List<TypeEnum>() { TypeEnum.Integer }));

            List<TypeNode> parameterTypes = expected;

            ITypeChecker parent = Substitute.For<ITypeChecker>();
            List<TypeNode> res = null;
            parent.Dispatch(Arg.Any<IntegerLiteralExpression>(), Arg.Do<List<TypeNode>>(x => res = x)).Returns(new TypeNode(TypeEnum.Integer, 1, 1));
            TypeHelper typeHelper = new TypeHelper { TypeChecker = parent };
            typeHelper.SetAstRoot(ast);

            typeHelper.VisitFunctionCall(input1, parameterTypes.ToList());

            res.Should().BeEquivalentTo(expected);
        }

        [TestMethod]
        public void FunctionCall_Global_CorrectParameterPassDown()
        {
            var expected = new List<TypeNode>()
            {
                GetFunctionType(TypeEnum.Integer, new List<TypeEnum>() {TypeEnum.Integer})
            };

            var children = new List<ExpressionNode>
            {
                new IntegerLiteralExpression("0", 0, 0)
            };
            FunctionCallExpression input1 = new FunctionCallExpression("", children, 1, 1);
            input1.GlobalReferences = new List<int>() { 0 };
            var ast = GetAst();
            ast.Functions.Add(GetFunctionNode(TypeEnum.Real, new List<TypeEnum>() { TypeEnum.Integer }));
            
            ITypeChecker parent = Substitute.For<ITypeChecker>();
            List<TypeNode> res = null;
            parent.Dispatch(Arg.Any<IntegerLiteralExpression>(), Arg.Do<List<TypeNode>>(x => res = x)).Returns(new TypeNode(TypeEnum.Integer, 1, 1));
            TypeHelper typeHelper = new TypeHelper { TypeChecker = parent };
            typeHelper.SetAstRoot(ast);
            typeHelper.VisitFunctionCall(input1, expected.ToList());

            res.Should().BeEquivalentTo(expected);
        }

        // Only global references
        // 1 glob ref, 1 match with no input            -> Return Int
        // 1 glob ref, 1 match with input               -> Return Real
        // 2 glob ref, 1 match                          -> Return Real
        // 2 glob ref, 1 match with multiple input      -> Return Real
        // 2 glob ref, 2 matches                        -> Throw Error
        // 1 glob ref, 0 match                          -> Throw Error
        // 0 matches                                    -> Throw Error
        // 1 global ref, 1 match                        -> Return Function
        // 1 global ref, 1 match with function input    -> Return Int
        // 2 global ref, 0 match with function as input -> Throw Error

        // Only local references
        // 1 local, 0 match                             -> Throw Error

        // Both global and local references
        // 1 glob, 1 local                              -> local match
        // 1 glob, 1 local with local match             -> glob ref removed
        // 1 glob, 1 local                              -> glob match
        // 1 glob, 1 local with glob match              -> local ref removed

        [TestMethod]
        public void FunctionCall_OneGlobalRefAndOneMatchWithNoInput_IntegerType()
        {
            TypeEnum expected = TypeEnum.Integer;
            FunctionCallExpression input1 = new FunctionCallExpression("", new List<ExpressionNode>(), 1, 1);
            input1.GlobalReferences = new List<int>() { 0 };
            var ast = GetAst();
            ast.Functions.Add(GetFunctionNode(TypeEnum.Integer, new List<TypeEnum>()));
            TypeHelper typeHelper = new TypeHelper();
            typeHelper.SetAstRoot(ast);

            var res = typeHelper.VisitFunctionCall(input1, null).Type;

            Assert.AreEqual(expected, res);
        }

        [TestMethod]
        public void FunctionCall_OneGlobalReferenceAndOneMatchWithInput_RealType()
        {
            TypeEnum expected = TypeEnum.Real;
            var children = new List<ExpressionNode>
            {
                new IntegerLiteralExpression("0", 0, 0)
            };
            FunctionCallExpression input1 = new FunctionCallExpression("", children, 1, 1);
            input1.GlobalReferences = new List<int>() { 0 };
            var ast = GetAst();
            ast.Functions.Add(GetFunctionNode(TypeEnum.Real, new List<TypeEnum>() { TypeEnum.Integer }));
            ITypeChecker parent = Substitute.For<ITypeChecker>();
            parent.Dispatch(Arg.Any<IntegerLiteralExpression>(), Arg.Any<List<TypeNode>>()).Returns(new TypeNode(TypeEnum.Integer, 1, 1));
            TypeHelper typeHelper = new TypeHelper { TypeChecker = parent };
            typeHelper.SetAstRoot(ast);
            var res = typeHelper.VisitFunctionCall(input1, null).Type;

            Assert.AreEqual(expected, res);
        }

        [TestMethod]
        public void FunctionCall_TwoGlobalRefsAndOneMatchWithSingleInput_RealType()
        {
            TypeEnum expected = TypeEnum.Real;
            var children = new List<ExpressionNode>
            {
                new IntegerLiteralExpression("0", 0, 0)
            };
            FunctionCallExpression input1 = new FunctionCallExpression("", children, 1, 1);
            input1.GlobalReferences = new List<int>() { 0, 1 };
            
            var ast = GetAst();
            ast.Functions.Add(GetFunctionNode(TypeEnum.Integer, new List<TypeEnum>() { TypeEnum.Real }));
            ast.Functions.Add(GetFunctionNode(TypeEnum.Real, new List<TypeEnum>() { TypeEnum.Integer }));
            
            ITypeChecker parent = Substitute.For<ITypeChecker>();
            parent.Dispatch(Arg.Any<IntegerLiteralExpression>(), Arg.Any<List<TypeNode>>()).Returns(new TypeNode(TypeEnum.Integer, 1, 1));
            parent.Dispatch(Arg.Any<RealLiteralExpression>(), Arg.Any<List<TypeNode>>()).Returns(new TypeNode(TypeEnum.Real, 1, 1));
            TypeHelper typeHelper = new TypeHelper { TypeChecker = parent };
            typeHelper.SetAstRoot(ast);

            var res = typeHelper.VisitFunctionCall(input1, null).Type;

            Assert.AreEqual(expected, res);
        }

        [TestMethod]
        public void FunctionCall_TwoGlobalRefsAndOneMatchWithMultipleInput_RealType()
        {
            TypeEnum expected = TypeEnum.Real;
            var children = new List<ExpressionNode>
            {
                new IntegerLiteralExpression("0", 0, 0),
                new RealLiteralExpression("1.1", 0, 0),
                new IntegerLiteralExpression("0", 0, 0),
                new RealLiteralExpression("1.1", 0, 0),
            };
            FunctionCallExpression input1 = new FunctionCallExpression("", children, 1, 1);
            input1.GlobalReferences = new List<int>() { 0, 1 };
            var ast = GetAst();
            ast.Functions.Add(GetFunctionNode(TypeEnum.Integer, new List<TypeEnum>()
            {
                TypeEnum.Real, TypeEnum.Integer, TypeEnum.Integer, TypeEnum.Real
            }));
            ast.Functions.Add(GetFunctionNode(TypeEnum.Real, new List<TypeEnum>() 
            { 
                TypeEnum.Integer, TypeEnum.Real, TypeEnum.Integer, TypeEnum.Real
            }));
            ITypeChecker parent = Substitute.For<ITypeChecker>();
            parent.Dispatch(Arg.Any<IntegerLiteralExpression>(), Arg.Any<List<TypeNode>>()).Returns(new TypeNode(TypeEnum.Integer, 1, 1));
            parent.Dispatch(Arg.Any<RealLiteralExpression>(), Arg.Any<List<TypeNode>>()).Returns(new TypeNode(TypeEnum.Real, 1, 1));
            TypeHelper typeHelper = new TypeHelper { TypeChecker = parent };
            typeHelper.SetAstRoot(ast);

            var res = typeHelper.VisitFunctionCall(input1, null).Type;

            Assert.AreEqual(expected, res);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void FunctionCall_TwoGlobalReferencesAndTwoMatches_ThrowError()
        {
            var children = new List<ExpressionNode>
            {
                new IntegerLiteralExpression("0", 0, 0)
            };
            FunctionCallExpression input1 = new FunctionCallExpression("", children, 1, 1);
            input1.GlobalReferences = new List<int>() { 0, 1 };
            var ast = GetAst();
            ast.Functions.Add(GetFunctionNode(TypeEnum.Integer, new List<TypeEnum>() { TypeEnum.Integer }));
            ast.Functions.Add(GetFunctionNode(TypeEnum.Real, new List<TypeEnum>() { TypeEnum.Integer }));
            ITypeChecker parent = Substitute.For<ITypeChecker>();
            parent.Dispatch(Arg.Any<IntegerLiteralExpression>(), Arg.Any<List<TypeNode>>()).Returns(new TypeNode(TypeEnum.Integer, 1, 1));
            parent.Dispatch(Arg.Any<RealLiteralExpression>(), Arg.Any<List<TypeNode>>()).Returns(new TypeNode(TypeEnum.Real, 1, 1));
            TypeHelper typeHelper = new TypeHelper { TypeChecker = parent };
            typeHelper.SetAstRoot(ast);

            var res = typeHelper.VisitFunctionCall(input1, null).Type;
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void FunctionCall_OneGlobalAndZeroMatch_ThrowError()
        {
            var children = new List<ExpressionNode>
            {
                new IntegerLiteralExpression("0", 0, 0)
            };
            FunctionCallExpression input1 = new FunctionCallExpression("", children, 1, 1);
            input1.GlobalReferences = new List<int>() { 0 };
            var ast = GetAst();
            ast.Functions.Add(GetFunctionNode(TypeEnum.Integer, new List<TypeEnum>() { TypeEnum.Real }));
            ITypeChecker parent = Substitute.For<ITypeChecker>();
            parent.Dispatch(Arg.Any<IntegerLiteralExpression>(), Arg.Any<List<TypeNode>>()).Returns(new TypeNode(TypeEnum.Integer, 1, 1));
            parent.Dispatch(Arg.Any<RealLiteralExpression>(), Arg.Any<List<TypeNode>>()).Returns(new TypeNode(TypeEnum.Real, 1, 1));
            TypeHelper typeHelper = new TypeHelper { TypeChecker = parent };
            typeHelper.SetAstRoot(ast);

            var res = typeHelper.VisitFunctionCall(input1, null).Type;
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void FunctionCall_ZeroGlobalRefAndZeroMatch_ThrowError()
        {
            FunctionCallExpression input1 = new FunctionCallExpression("", new List<ExpressionNode>(), 1, 1);
            input1.GlobalReferences = new List<int>() { };
            var ast = GetAst();
            TypeHelper typeHelper = new TypeHelper();
            typeHelper.SetAstRoot(ast);

            var res = typeHelper.VisitFunctionCall(input1, null).Type;
        }

        [TestMethod]
        public void FunctionCall_OneGlobalAndOneMatchWithFunctionOutput_FunctionType()
        {
            TypeEnum expectedFuncOutput = TypeEnum.Integer;
            TypeEnum expectedFuncInput = TypeEnum.Real;
            FunctionCallExpression input1 = new FunctionCallExpression("", new List<ExpressionNode>(), 1, 1);
            input1.GlobalReferences = new List<int>() { 0 };
            var ast = GetAst();
            ast.Functions.Add(GetFunctionNodeWithFunctionOutput(TypeEnum.Integer, new List<TypeEnum>() { TypeEnum.Real }, new List<TypeEnum>()));
            TypeHelper typeHelper = new TypeHelper();
            typeHelper.SetAstRoot(ast);

            var res = (FunctionTypeNode) typeHelper.VisitFunctionCall(input1, null);

            Assert.AreEqual(expectedFuncOutput, res.ReturnType.Type);
            Assert.AreEqual(expectedFuncInput, res.ParameterTypes[0].Type);
        }

        // func(((real) -> real) -> int
        [TestMethod]
        public void FunctionCall_OneGlobalAndOneMatchWithFunctionInput_IntType()
        {
            TypeEnum expected= TypeEnum.Integer;
            var children = new List<ExpressionNode>
            {
                new IdentifierExpression("0", 0, 0) { Reference = 0 }
            };
            FunctionCallExpression input1 = new FunctionCallExpression("", children, 1, 1);
            input1.GlobalReferences = new List<int>() { 0 };
            var ast = GetAst();
            ast.Functions.Add(new FunctionNode("id", null, null, GetFunctionType(TypeEnum.Integer, GetFunctionType(TypeEnum.Real, new List<TypeEnum>() { TypeEnum.Real })), 0, 0));
            ITypeChecker parent = Substitute.For<ITypeChecker>();
            parent.Dispatch(Arg.Any<IdentifierExpression>(), Arg.Any<List<TypeNode>>()).Returns(GetFunctionType(TypeEnum.Real, new List<TypeEnum>() { TypeEnum.Real }));
            TypeHelper typeHelper = new TypeHelper { TypeChecker = parent };
            typeHelper.SetAstRoot(ast);

            var res = typeHelper.VisitFunctionCall(input1, null).Type;

            Assert.AreEqual(expected, res);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void FunctionCall_TwoGlobalAndZeroMatch_ThrowException()
        {
            var children = new List<ExpressionNode>
            {
                new IdentifierExpression("0", 0, 0) { Reference = 0 }
            };
            FunctionCallExpression input1 = new FunctionCallExpression("", children, 1, 1);
            input1.GlobalReferences = new List<int>() { 0, 1 };
            var ast = GetAst();
            ast.Functions.Add(new FunctionNode("id", null, null, GetFunctionType(TypeEnum.Real, new List<TypeEnum>() { TypeEnum.Real }), 0, 0));
            ast.Functions.Add(new FunctionNode("id", null, null, GetFunctionType(TypeEnum.Integer, GetFunctionType(TypeEnum.Real, new List<TypeEnum>() { TypeEnum.Integer })), 0, 0));
            ITypeChecker parent = Substitute.For<ITypeChecker>();
            parent.Dispatch(Arg.Any<IdentifierExpression>(), Arg.Any<List<TypeNode>>()).Returns(GetFunctionType(TypeEnum.Real, new List<TypeEnum>() { TypeEnum.Real }));
            TypeHelper typeHelper = new TypeHelper { TypeChecker = parent };
            typeHelper.SetAstRoot(ast);

            var res = typeHelper.VisitFunctionCall(input1, null).Type;
        }

        // Only local references
        // 1 local, 0 match                             -> Throw Error
        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void FunctionCall_OneLocalRefAndZeroMatch_ThrowException()
        {
            var children = new List<ExpressionNode>
            {
                new IntegerLiteralExpression("0", 0, 0)
            };
            FunctionCallExpression input1 = new FunctionCallExpression("", children, 1, 1);
            input1.LocalReference = 0;
            
            List<TypeNode> parameterTypes = new List<TypeNode>()
            {
                GetFunctionType(TypeEnum.Integer, new List<TypeEnum>() {TypeEnum.Real})
            };
            
            ITypeChecker parent = Substitute.For<ITypeChecker>();
            parent.Dispatch(Arg.Any<IntegerLiteralExpression>(), Arg.Any<List<TypeNode>>()).Returns(new TypeNode(TypeEnum.Integer, 1, 1));
            //parent.Dispatch(Arg.Any<RealLiteralExpression>(), Arg.Any<List<TypeNode>>()).Returns(new TypeNode(TypeEnum.Real, 1, 1));
            TypeHelper typeHelper = new TypeHelper { TypeChecker = parent };
            
            var res = typeHelper.VisitFunctionCall(input1, parameterTypes).Type;
        }
        
        // Both global and local references
        // 1 glob, 1 local                              -> local match
        [TestMethod]
        public void FunctionCall_OneGlobAndOneLocal_LocalMatch()
        {
            TypeEnum expected = TypeEnum.Integer;
            var children = new List<ExpressionNode>
            {
                new IntegerLiteralExpression("0", 0, 0)
            };
            FunctionCallExpression input1 = new FunctionCallExpression("", children, 1, 1);
            input1.LocalReference = 0;
            input1.GlobalReferences = new List<int>(){0};
            
            var ast = GetAst();
            ast.Functions.Add(GetFunctionNode(TypeEnum.Real, new List<TypeEnum>() { TypeEnum.Integer }));

            List<TypeNode> parameterTypes = new List<TypeNode>()
            {
                GetFunctionType(TypeEnum.Integer, new List<TypeEnum>() {TypeEnum.Integer})
            };
            
            ITypeChecker parent = Substitute.For<ITypeChecker>();
            parent.Dispatch(Arg.Any<IntegerLiteralExpression>(), Arg.Any<List<TypeNode>>()).Returns(new TypeNode(TypeEnum.Integer, 1, 1));
            TypeHelper typeHelper = new TypeHelper { TypeChecker = parent };
            typeHelper.SetAstRoot(ast);
            
            var res = typeHelper.VisitFunctionCall(input1, parameterTypes).Type;

            Assert.AreEqual(expected, res);
        }
        
        // 1 glob, 1 local with local match             -> glob ref removed
        [TestMethod]
        public void FunctionCall_OneGlobAndOneLocal_GlobRefRemoved()
        {
            int expected = 0;
            var children = new List<ExpressionNode>
            {
                new IntegerLiteralExpression("0", 0, 0)
            };
            FunctionCallExpression input1 = new FunctionCallExpression("", children, 1, 1);
            input1.LocalReference = 0;
            input1.GlobalReferences = new List<int>(){0};

            var ast = GetAst();
            ast.Functions.Add(GetFunctionNode(TypeEnum.Real, new List<TypeEnum>() { TypeEnum.Integer }));

            List<TypeNode> parameterTypes = new List<TypeNode>()
            {
                GetFunctionType(TypeEnum.Integer, new List<TypeEnum>() {TypeEnum.Integer})
            };
            
            ITypeChecker parent = Substitute.For<ITypeChecker>();
            parent.Dispatch(Arg.Any<IntegerLiteralExpression>(), Arg.Any<List<TypeNode>>()).Returns(new TypeNode(TypeEnum.Integer, 1, 1));
            TypeHelper typeHelper = new TypeHelper { TypeChecker = parent };
            typeHelper.SetAstRoot(ast);
            
            typeHelper.VisitFunctionCall(input1, parameterTypes);
            var res = input1.GlobalReferences.Count;
            
            Assert.AreEqual(expected, res);
        }
        
        // 1 glob, 1 local                              -> glob match
        [TestMethod]
        public void FunctionCall_OneGlobAndOneLocal_GlobMatch()
        {
            TypeEnum expected = TypeEnum.Real;
            var children = new List<ExpressionNode>
            {
                new IntegerLiteralExpression("0", 0, 0)
            };
            FunctionCallExpression input1 = new FunctionCallExpression("", children, 1, 1);
            input1.LocalReference = 0;
            input1.GlobalReferences = new List<int>(){0};
            
            var ast = GetAst();
            ast.Functions.Add(GetFunctionNode(TypeEnum.Real, new List<TypeEnum>() { TypeEnum.Integer }));

            List<TypeNode> parameterTypes = new List<TypeNode>()
            {
                GetFunctionType(TypeEnum.Integer, new List<TypeEnum>() {TypeEnum.Real})
            };
            
            ITypeChecker parent = Substitute.For<ITypeChecker>();
            parent.Dispatch(Arg.Any<IntegerLiteralExpression>(), Arg.Any<List<TypeNode>>()).Returns(new TypeNode(TypeEnum.Integer, 1, 1));
            TypeHelper typeHelper = new TypeHelper { TypeChecker = parent };
            typeHelper.SetAstRoot(ast);
            
            var res = typeHelper.VisitFunctionCall(input1, parameterTypes).Type;

            Assert.AreEqual(expected, res);
        }
        
        // 1 glob, 1 local with glob match              -> local ref removed
        [TestMethod]
        public void FunctionCall_OneGlobAndOneLocal_LocalRefRemoved()
        {
            int expected = FunctionCallExpression.NO_LOCAL_REF;
            var children = new List<ExpressionNode>
            {
                new IntegerLiteralExpression("0", 0, 0)
            };
            FunctionCallExpression input1 = new FunctionCallExpression("", children, 1, 1);
            input1.LocalReference = 0;
            input1.GlobalReferences = new List<int>(){0};
            
            var ast = GetAst();
            ast.Functions.Add(GetFunctionNode(TypeEnum.Real, new List<TypeEnum>() { TypeEnum.Integer }));

            List<TypeNode> parameterTypes = new List<TypeNode>()
            {
                GetFunctionType(TypeEnum.Integer, new List<TypeEnum>() {TypeEnum.Real})
            };
            
            ITypeChecker parent = Substitute.For<ITypeChecker>();
            parent.Dispatch(Arg.Any<IntegerLiteralExpression>(), Arg.Any<List<TypeNode>>()).Returns(new TypeNode(TypeEnum.Integer, 1, 1));
            TypeHelper typeHelper = new TypeHelper { TypeChecker = parent };
            typeHelper.SetAstRoot(ast);
            
            typeHelper.VisitFunctionCall(input1, parameterTypes);
            var res = input1.LocalReference;
            
            Assert.AreEqual(expected, res);
        }
        
        private FunctionTypeNode GetFunctionType(TypeEnum returnType, FunctionTypeNode inputType)
        {
            return new FunctionTypeNode(new TypeNode(returnType, 0, 0), new List<TypeNode>() { inputType }, 0, 0);
        }

        private FunctionTypeNode GetFunctionType(TypeEnum returnType, List<TypeEnum> inputTypes)
        {
            var inputs = new List<TypeNode>();
            foreach (var input in inputTypes)
                inputs.Add(new TypeNode(input, 0, 0));
            return new FunctionTypeNode(new TypeNode(returnType, 0, 0), inputs, 0, 0);
        }

        private AST GetAst()
        {
            return new AST(new List<FunctionNode>(), new List<ExportNode>(), 0, 0);
        }

        private FunctionNode GetFunctionNodeWithFunctionOutput(TypeEnum funcOutput, List<TypeEnum> funcInputTypes, List<TypeEnum> inputToNewFunc)
        {
            var inputs = new List<TypeNode>();
            foreach (var input in inputToNewFunc)
                inputs.Add(new TypeNode(input, 0, 0));

            var functOutput = new TypeNode(funcOutput, 0, 0);
            var funcInputs = new List<TypeNode>();
            foreach (var input in funcInputTypes)
                funcInputs.Add(new TypeNode(input, 0, 0));
            var functionOutput = new FunctionTypeNode(functOutput, funcInputs, 0, 0);

            return new FunctionNode("id", null, null,
                new FunctionTypeNode(functionOutput,
                inputs, 0, 0), 0, 0);
        }

        private FunctionNode GetFunctionNode(TypeEnum output, List<TypeEnum> inputTypes)
        {
            var inputs = new List<TypeNode>();
            foreach (var input in inputTypes)
                inputs.Add(new TypeNode(input, 0, 0));

            return new FunctionNode("id", null, null,
                new FunctionTypeNode(new TypeNode(output, 0, 0),
                inputs, 0, 0), 0, 0);
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
