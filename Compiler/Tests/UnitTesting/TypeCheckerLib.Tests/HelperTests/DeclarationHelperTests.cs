using System;
using System.Collections.Generic;
using System.Linq;
using ASTLib.Exceptions;
using ASTLib.Exceptions.Invalid;
using ASTLib.Exceptions.NotMatching;
using ASTLib.Nodes;
using ASTLib.Nodes.ExpressionNodes;
using ASTLib.Nodes.ExpressionNodes.CastExpressionNodes;
using ASTLib.Nodes.ExpressionNodes.NumberOperationNodes;
using ASTLib.Nodes.ExpressionNodes.OperationNodes;
using ASTLib.Nodes.TypeNodes;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using TypeCheckerLib.Helpers;
using TypeCheckerLib.Interfaces;
using InvalidCastException = ASTLib.Exceptions.Invalid.InvalidCastException;

namespace TypeCheckerLib.Tests.HelperTests
{
    [TestClass]
    public class DeclarationHelperTests
    {
        #region Export
        // GetType is called with an empty list of TypeNode (Check that length == 0)
        // Param 1 not Graph    -> Throw Exception
        // Param 2 not String   -> Throw Exception

        // One of Param 3 not empty or (elem->string) func  -> Throw Exception
        // One of Param 4 not empty or (elem->string) func  -> Throw Exception

        // Valid input Graph, String                    -> No exception
        // Valid input Graph, String, Emply func lists  -> No exception
        // Valid input Graph, String, x and y func lists-> No exception
        
        [TestMethod]
        [ExpectedException(typeof(UnmatchableTypesException))]
        public void Export_InvalidGraph_Exception()
        {
            var intLit = Utilities.GetIntLit();
            var input1 = Utilities.GetExportNode(intLit);

            var parent = Utilities.GetDefaultTypeChecker();
            var helper = Utilities.GetHelper<DeclarationHelper>(parent);

            helper.VisitExport(input1);
        }
        [TestMethod]
        [ExpectedException(typeof(UnmatchableTypesException))]
        public void Export_InvalidString_Exception()
        {
            var intLit = Utilities.GetIntLit();
            var graph = Utilities.GetGraph();
            var input1 = Utilities.GetExportNode(graph, intLit);

            var parent = Utilities.GetDefaultTypeChecker();
            var helper = Utilities.GetHelper<DeclarationHelper>(parent);

            helper.VisitExport(input1);
        }
        [DataRow(4, 2)]
        [DataRow(1, 0)]
        [TestMethod]
        [ExpectedException(typeof(UnmatchableTypesException))]
        public void Export_InvalidVertexFunc_Exception(int funcs, int wrongIndex)
        {
            var graph = Utilities.GetGraph();
            var name = Utilities.GetStringLit();
            var vertexFuncs = Utilities.GetAttributeFuncs(funcs);
            var edgeFuncs = Utilities.GetAttributeFuncs(0);
            var input1 = Utilities.GetExportNode(graph, name, vertexFuncs, edgeFuncs);

            var parent = Utilities.GetDefaultTypeCheckerWithAttributeFunction();
            parent.Dispatch(vertexFuncs[wrongIndex], Arg.Is<List<TypeNode>>(x => x.Count == 0)).Returns(Utilities.GetFuncTypeNode(TypeEnum.Element, TypeEnum.Integer));
            var helper = Utilities.GetHelper<DeclarationHelper>(parent);

            helper.VisitExport(input1);
        }
        [DataRow(4, 2)]
        [DataRow(1, 0)]
        [TestMethod]
        [ExpectedException(typeof(UnmatchableTypesException))]
        public void Export_InvalidEdgeFunc_Exception(int funcs, int wrongIndex)
        {
            var graph = Utilities.GetGraph();
            var name = Utilities.GetStringLit();
            var vertexFuncs = Utilities.GetAttributeFuncs(0);
            var edgeFuncs = Utilities.GetAttributeFuncs(funcs);
            var input1 = Utilities.GetExportNode(graph, name, vertexFuncs, edgeFuncs);

            var parent = Utilities.GetDefaultTypeCheckerWithAttributeFunction();
            parent.Dispatch(edgeFuncs[wrongIndex], Arg.Is<List<TypeNode>>(x => x.Count == 0)).Returns(Utilities.GetFuncTypeNode(TypeEnum.Element, TypeEnum.Integer));
            var helper = Utilities.GetHelper<DeclarationHelper>(parent);

            helper.VisitExport(input1);
        }
        [DataRow(0, 0)]
        [DataRow(3, 5)]
        [DataRow(2, 0)]
        [DataRow(0, 2)]
        [TestMethod]
        public void Export_Valid_NoException(int vertexNum, int edgeNum)
        {
            var graph = Utilities.GetGraph();
            var name = Utilities.GetStringLit();
            var vertexFuncs = Utilities.GetAttributeFuncs(vertexNum);
            var edgeFuncs = Utilities.GetAttributeFuncs(edgeNum);
            var input1 = Utilities.GetExportNode(graph, name, vertexFuncs, edgeFuncs);

            var parent = Utilities.GetDefaultTypeCheckerWithAttributeFunction();
            var helper = Utilities.GetHelper<DeclarationHelper>(parent);

            helper.VisitExport(input1);
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
            IDeclarationHelper declarationHelper = Utilities.GetHelper<DeclarationHelper>(parent);

            declarationHelper.VisitFunction(input1);

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
        [DataRow(TypeEnum.Boolean, TypeEnum.Boolean)]
        [TestMethod]
        public void Function_Type_CorrectPrimitiveType(TypeEnum functionReturnType, TypeEnum dispatcherReturnType)
        {
            var condition = new ConditionNode(new AdditionExpression(null, null, 0, 0), 0, 0);
            var funcType = Utilities.GetFunctionType(functionReturnType, new List<TypeEnum>());
            FunctionNode input1 = new FunctionNode("", condition, null, funcType, 0, 0);

            ITypeChecker parent = Substitute.For<ITypeChecker>();
            parent.Dispatch(Arg.Any<ExpressionNode>(), Arg.Any<List<TypeNode>>()).Returns(new TypeNode(dispatcherReturnType, 1, 1));
            IDeclarationHelper declarationHelper = Utilities.GetHelper<DeclarationHelper>(parent);

            declarationHelper.VisitFunction(input1);
        }

        [TestMethod]
        public void Function_Type_CorrectFunctionType()
        {
            var condition = new ConditionNode(new AdditionExpression(null, null, 0, 0), 0, 0);
            var returnType = new FunctionTypeNode(new TypeNode(TypeEnum.Integer, 1, 1), new List<TypeNode>(), 0, 0);
            var funcType = new FunctionTypeNode(returnType, new List<TypeNode>(), 0, 0);
            FunctionNode input1 = new FunctionNode("", condition, null, funcType, 0, 0);

            ITypeChecker parent = Substitute.For<ITypeChecker>();
            parent.Dispatch(Arg.Any<ExpressionNode>(), Arg.Any<List<TypeNode>>()).Returns(returnType);
            IDeclarationHelper declarationHelper = Utilities.GetHelper<DeclarationHelper>(parent);

            declarationHelper.VisitFunction(input1);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidCastException))]
        public void Function_Type_InvalidCastOnFunctionType()
        {
            var condition = new ConditionNode(new AdditionExpression(null, null, 0, 0), 0, 0);
            var returnType = new FunctionTypeNode(new TypeNode(TypeEnum.Integer, 1, 1), new List<TypeNode>(), 0, 0);
            var funcType = new FunctionTypeNode(returnType, new List<TypeNode>(), 0, 0);
            FunctionNode input1 = new FunctionNode("", condition, null, funcType, 0, 0);
            var actualReturnType = new FunctionTypeNode(new TypeNode(TypeEnum.Real, 1, 1), new List<TypeNode>(), 0, 0);

            ITypeChecker parent = Substitute.For<ITypeChecker>();
            parent.Dispatch(Arg.Any<ExpressionNode>(), Arg.Any<List<TypeNode>>()).Returns(actualReturnType);
            IDeclarationHelper declarationHelper = Utilities.GetHelper<DeclarationHelper>(parent);

            declarationHelper.VisitFunction(input1);
        }

        [TestMethod]
        public void Function_Type_CorrectConditionType()
        {
            var condition = new ConditionNode(new BooleanLiteralExpression(true, 0,0), 
                                              new AdditionExpression(null, null, 0, 0), 0, 0);
            var funcType = Utilities.GetFunctionType(TypeEnum.Integer, new List<TypeEnum>());
            FunctionNode input1 = new FunctionNode("", condition, null, funcType, 0, 0);

            ITypeChecker parent = Substitute.For<ITypeChecker>();
            parent.Dispatch(Arg.Any<AdditionExpression>(), Arg.Any<List<TypeNode>>()).Returns(
                            new TypeNode(TypeEnum.Integer, 1, 1));
            parent.Dispatch(Arg.Any<BooleanLiteralExpression>(), Arg.Any<List<TypeNode>>()).Returns(
                            new TypeNode(TypeEnum.Boolean, 1, 1));
            IDeclarationHelper declarationHelper = Utilities.GetHelper<DeclarationHelper>(parent);

            declarationHelper.VisitFunction(input1);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidCastException))]
        public void Function_Type_IncorrectConditionType()
        {
            var condition = new ConditionNode(new BooleanLiteralExpression(true, 0, 0),
                                              new AdditionExpression(null, null, 0, 0), 0, 0);
            var funcType = Utilities.GetFunctionType(TypeEnum.Integer, new List<TypeEnum>());
            FunctionNode input1 = new FunctionNode("", condition, null, funcType, 0, 0);

            ITypeChecker parent = Substitute.For<ITypeChecker>();
            parent.Dispatch(Arg.Any<AdditionExpression>(), Arg.Any<List<TypeNode>>()).Returns(
                            new TypeNode(TypeEnum.Integer, 1, 1));
            parent.Dispatch(Arg.Any<BooleanLiteralExpression>(), Arg.Any<List<TypeNode>>()).Returns(
                            new TypeNode(TypeEnum.Real, 1, 1));
            IDeclarationHelper declarationHelper = Utilities.GetHelper<DeclarationHelper>(parent);

            declarationHelper.VisitFunction(input1);
        }

        [DataRow(TypeEnum.Integer, TypeEnum.Real)]
        [DataRow(TypeEnum.Real, TypeEnum.Function)]
        [DataRow(TypeEnum.Function, TypeEnum.Integer)]
        [DataRow(TypeEnum.Function, TypeEnum.Real)]
        [DataRow(TypeEnum.Boolean, TypeEnum.Integer)]
        [DataRow(TypeEnum.Boolean, TypeEnum.Real)]
        [DataRow(TypeEnum.Boolean, TypeEnum.Function)]
        [TestMethod]
        [ExpectedException(typeof(InvalidCastException))]
        public void Function_Type_WrongTypeAndThrowException(TypeEnum functionReturnType, TypeEnum dispatcherReturnType)
        {
            var condition = new ConditionNode(new AdditionExpression(null, null, 0, 0), 0, 0);
            var funcType = Utilities.GetFunctionType(functionReturnType, new List<TypeEnum>());
            FunctionNode input1 = new FunctionNode("", condition, null, funcType, 0, 0);

            ITypeChecker parent = Substitute.For<ITypeChecker>();
            parent.Dispatch(Arg.Any<ExpressionNode>(), Arg.Any<List<TypeNode>>()).Returns(new TypeNode(dispatcherReturnType, 1, 1));
            IDeclarationHelper declarationHelper = Utilities.GetHelper<DeclarationHelper>(parent);

            declarationHelper.VisitFunction(input1);
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
            IDeclarationHelper declarationHelper = Utilities.GetHelper<DeclarationHelper>(parent);

            declarationHelper.VisitFunction(input1);
            var res = input1.Conditions[0].ReturnExpression.GetType();

            Assert.AreEqual(expected, res);
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

            var ast = Utilities.GetAst();
            ast.Functions.Add(Utilities.GetFunctionNode(TypeEnum.Real, new List<TypeEnum>() { TypeEnum.Integer }));

            List<TypeNode> parameterTypes = expected;

            ITypeChecker parent = Substitute.For<ITypeChecker>();
            List<TypeNode> res = null;
            parent.Dispatch(Arg.Any<IntegerLiteralExpression>(), Arg.Do<List<TypeNode>>(x => res = x)).Returns(new TypeNode(TypeEnum.Integer, 1, 1));
            IDeclarationHelper declarationHelper = Utilities.GetHelper<DeclarationHelper>(ast, parent);

            declarationHelper.VisitFunctionCall(input1, parameterTypes.ToList());

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
            var ast = Utilities.GetAst();
            ast.Functions.Add(Utilities.GetFunctionNode(TypeEnum.Real, new List<TypeEnum>() { TypeEnum.Integer }));
            
            ITypeChecker parent = Substitute.For<ITypeChecker>();
            List<TypeNode> res = null;
            parent.Dispatch(Arg.Any<IntegerLiteralExpression>(), Arg.Do<List<TypeNode>>(x => res = x)).Returns(new TypeNode(TypeEnum.Integer, 1, 1));
            IDeclarationHelper declarationHelper = Utilities.GetHelper<DeclarationHelper>(ast, parent);
            declarationHelper.VisitFunctionCall(input1, expected.ToList());

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
            var ast = Utilities.GetAst();
            ast.Functions.Add(Utilities.GetFunctionNode(TypeEnum.Integer, new List<TypeEnum>()));
            IDeclarationHelper declarationHelper = Utilities.GetHelper<DeclarationHelper>(ast);

            var res = declarationHelper.VisitFunctionCall(input1, null).Type;

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
            var ast = Utilities.GetAst();
            ast.Functions.Add(Utilities.GetFunctionNode(TypeEnum.Real, new List<TypeEnum>() { TypeEnum.Integer }));
            ITypeChecker parent = Substitute.For<ITypeChecker>();
            parent.Dispatch(Arg.Any<IntegerLiteralExpression>(), Arg.Any<List<TypeNode>>()).Returns(new TypeNode(TypeEnum.Integer, 1, 1));
            IDeclarationHelper declarationHelper = Utilities.GetHelper<DeclarationHelper>(ast);
            var res = declarationHelper.VisitFunctionCall(input1, null).Type;

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
            
            var ast = Utilities.GetAst();
            ast.Functions.Add(Utilities.GetFunctionNode(TypeEnum.Integer, new List<TypeEnum>() { TypeEnum.Real }));
            ast.Functions.Add(Utilities.GetFunctionNode(TypeEnum.Real, new List<TypeEnum>() { TypeEnum.Integer }));
            
            ITypeChecker parent = Substitute.For<ITypeChecker>();
            parent.Dispatch(Arg.Any<IntegerLiteralExpression>(), Arg.Any<List<TypeNode>>()).Returns(new TypeNode(TypeEnum.Integer, 1, 1));
            parent.Dispatch(Arg.Any<RealLiteralExpression>(), Arg.Any<List<TypeNode>>()).Returns(new TypeNode(TypeEnum.Real, 1, 1));
            IDeclarationHelper declarationHelper = Utilities.GetHelper<DeclarationHelper>(ast);

            var res = declarationHelper.VisitFunctionCall(input1, null).Type;

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
            var ast = Utilities.GetAst();
            ast.Functions.Add(Utilities.GetFunctionNode(TypeEnum.Integer, new List<TypeEnum>()
            {
                TypeEnum.Real, TypeEnum.Integer, TypeEnum.Integer, TypeEnum.Real
            }));
            ast.Functions.Add(Utilities.GetFunctionNode(TypeEnum.Real, new List<TypeEnum>() 
            { 
                TypeEnum.Integer, TypeEnum.Real, TypeEnum.Integer, TypeEnum.Real
            }));
            ITypeChecker parent = Substitute.For<ITypeChecker>();
            parent.Dispatch(Arg.Any<IntegerLiteralExpression>(), Arg.Any<List<TypeNode>>()).Returns(new TypeNode(TypeEnum.Integer, 1, 1));
            parent.Dispatch(Arg.Any<RealLiteralExpression>(), Arg.Any<List<TypeNode>>()).Returns(new TypeNode(TypeEnum.Real, 1, 1));
            IDeclarationHelper declarationHelper = Utilities.GetHelper<DeclarationHelper>(ast);

            var res = declarationHelper.VisitFunctionCall(input1, null).Type;

            Assert.AreEqual(expected, res);
        }

        [TestMethod]
        [ExpectedException(typeof(OverloadException))]
        public void FunctionCall_TwoGlobalReferencesAndTwoMatches_ThrowError()
        {
            var children = new List<ExpressionNode>
            {
                new IntegerLiteralExpression("0", 0, 0)
            };
            FunctionCallExpression input1 = new FunctionCallExpression("", children, 1, 1);
            input1.GlobalReferences = new List<int>() { 0, 1 };
            var ast = Utilities.GetAst();
            ast.Functions.Add(Utilities.GetFunctionNode(TypeEnum.Integer, new List<TypeEnum>() { TypeEnum.Integer }));
            ast.Functions.Add(Utilities.GetFunctionNode(TypeEnum.Real, new List<TypeEnum>() { TypeEnum.Integer }));
            ITypeChecker parent = Substitute.For<ITypeChecker>();
            parent.Dispatch(Arg.Any<IntegerLiteralExpression>(), Arg.Any<List<TypeNode>>()).Returns(new TypeNode(TypeEnum.Integer, 1, 1));
            parent.Dispatch(Arg.Any<RealLiteralExpression>(), Arg.Any<List<TypeNode>>()).Returns(new TypeNode(TypeEnum.Real, 1, 1));
            IDeclarationHelper declarationHelper = Utilities.GetHelper<DeclarationHelper>(ast);

            var res = declarationHelper.VisitFunctionCall(input1, null).Type;
        }

        [TestMethod]
        [ExpectedException(typeof(NoMatchingFunctionFoundException))]
        public void FunctionCall_OneGlobalAndZeroMatch_ThrowError()
        {
            var children = new List<ExpressionNode>
            {
                new IntegerLiteralExpression("0", 0, 0)
            };
            FunctionCallExpression input1 = new FunctionCallExpression("", children, 1, 1);
            input1.GlobalReferences = new List<int>() { 0 };
            var ast = Utilities.GetAst();
            ast.Functions.Add(Utilities.GetFunctionNode(TypeEnum.Integer, new List<TypeEnum>() { TypeEnum.Real }));
            ITypeChecker parent = Substitute.For<ITypeChecker>();
            parent.Dispatch(Arg.Any<IntegerLiteralExpression>(), Arg.Any<List<TypeNode>>()).Returns(new TypeNode(TypeEnum.Integer, 1, 1));
            parent.Dispatch(Arg.Any<RealLiteralExpression>(), Arg.Any<List<TypeNode>>()).Returns(new TypeNode(TypeEnum.Real, 1, 1));
            IDeclarationHelper declarationHelper = Utilities.GetHelper<DeclarationHelper>(ast);

            var res = declarationHelper.VisitFunctionCall(input1, null).Type;
        }

        [TestMethod]
        [ExpectedException(typeof(NoMatchingFunctionFoundException))]
        public void FunctionCall_ZeroGlobalRefAndZeroMatch_ThrowError()
        {
            FunctionCallExpression input1 = new FunctionCallExpression("", new List<ExpressionNode>(), 1, 1);
            input1.GlobalReferences = new List<int>() { };
            var ast = Utilities.GetAst();
            IDeclarationHelper declarationHelper = Utilities.GetHelper<DeclarationHelper>(ast);

            var res = declarationHelper.VisitFunctionCall(input1, null).Type;
        }

        [TestMethod]
        public void FunctionCall_OneGlobalAndOneMatchWithFunctionOutput_FunctionType()
        {
            TypeEnum expectedFuncOutput = TypeEnum.Integer;
            TypeEnum expectedFuncInput = TypeEnum.Real;
            FunctionCallExpression input1 = new FunctionCallExpression("", new List<ExpressionNode>(), 1, 1);
            input1.GlobalReferences = new List<int>() { 0 };
            var ast = Utilities.GetAst();
            ast.Functions.Add(GetFunctionNodeWithFunctionOutput(TypeEnum.Integer, new List<TypeEnum>() { TypeEnum.Real }, new List<TypeEnum>()));
            IDeclarationHelper declarationHelper = Utilities.GetHelper<DeclarationHelper>(ast);

            var res = (FunctionTypeNode) declarationHelper.VisitFunctionCall(input1, null);

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
            var ast = Utilities.GetAst();
            ast.Functions.Add(new FunctionNode("id", null, null, GetFunctionType(TypeEnum.Integer, GetFunctionType(TypeEnum.Real, new List<TypeEnum>() { TypeEnum.Real })), 0, 0));
            ITypeChecker parent = Substitute.For<ITypeChecker>();
            parent.Dispatch(Arg.Any<IdentifierExpression>(), Arg.Any<List<TypeNode>>()).Returns(GetFunctionType(TypeEnum.Real, new List<TypeEnum>() { TypeEnum.Real }));
            IDeclarationHelper declarationHelper = Utilities.GetHelper<DeclarationHelper>(ast, parent);

            var res = declarationHelper.VisitFunctionCall(input1, null).Type;

            Assert.AreEqual(expected, res);
        }

        [TestMethod]
        [ExpectedException(typeof(NoMatchingFunctionFoundException))]
        public void FunctionCall_TwoGlobalAndZeroMatch_ThrowException()
        {
            var children = new List<ExpressionNode>
            {
                new IdentifierExpression("0", 0, 0) { Reference = 0 }
            };
            FunctionCallExpression input1 = new FunctionCallExpression("", children, 1, 1);
            input1.GlobalReferences = new List<int>() { 0, 1 };
            var ast = Utilities.GetAst();
            ast.Functions.Add(new FunctionNode("id", null, null, GetFunctionType(TypeEnum.Real, new List<TypeEnum>() { TypeEnum.Real }), 0, 0));
            ast.Functions.Add(new FunctionNode("id", null, null, GetFunctionType(TypeEnum.Integer, GetFunctionType(TypeEnum.Real, new List<TypeEnum>() { TypeEnum.Integer })), 0, 0));
            ITypeChecker parent = Substitute.For<ITypeChecker>();
            parent.Dispatch(Arg.Any<IdentifierExpression>(), Arg.Any<List<TypeNode>>()).Returns(GetFunctionType(TypeEnum.Real, new List<TypeEnum>() { TypeEnum.Real }));
            IDeclarationHelper declarationHelper = Utilities.GetHelper<DeclarationHelper>(ast, parent);

            var res = declarationHelper.VisitFunctionCall(input1, null).Type;
        }

        // Only local references
        // 1 local, 0 match                             -> Throw Error
        [TestMethod]
        [ExpectedException(typeof(NoMatchingFunctionFoundException))]
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
            IDeclarationHelper declarationHelper = Utilities.GetHelper<DeclarationHelper>(parent);
            
            var res = declarationHelper.VisitFunctionCall(input1, parameterTypes).Type;
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
            
            var ast = Utilities.GetAst();
            ast.Functions.Add(Utilities.GetFunctionNode(TypeEnum.Real, new List<TypeEnum>() { TypeEnum.Integer }));

            List<TypeNode> parameterTypes = new List<TypeNode>()
            {
                GetFunctionType(TypeEnum.Integer, new List<TypeEnum>() {TypeEnum.Integer})
            };
            
            ITypeChecker parent = Substitute.For<ITypeChecker>();
            parent.Dispatch(Arg.Any<IntegerLiteralExpression>(), Arg.Any<List<TypeNode>>()).Returns(new TypeNode(TypeEnum.Integer, 1, 1));
            IDeclarationHelper declarationHelper = Utilities.GetHelper<DeclarationHelper>(ast);
            
            var res = declarationHelper.VisitFunctionCall(input1, parameterTypes).Type;

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

            var ast = Utilities.GetAst();
            ast.Functions.Add(Utilities.GetFunctionNode(TypeEnum.Real, new List<TypeEnum>() { TypeEnum.Integer }));

            List<TypeNode> parameterTypes = new List<TypeNode>()
            {
                GetFunctionType(TypeEnum.Integer, new List<TypeEnum>() {TypeEnum.Integer})
            };
            
            ITypeChecker parent = Substitute.For<ITypeChecker>();
            parent.Dispatch(Arg.Any<IntegerLiteralExpression>(), Arg.Any<List<TypeNode>>()).Returns(new TypeNode(TypeEnum.Integer, 1, 1));
            IDeclarationHelper declarationHelper = Utilities.GetHelper<DeclarationHelper>(ast);
            
            declarationHelper.VisitFunctionCall(input1, parameterTypes);
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
            
            var ast = Utilities.GetAst();
            ast.Functions.Add(Utilities.GetFunctionNode(TypeEnum.Real, new List<TypeEnum>() { TypeEnum.Integer }));

            List<TypeNode> parameterTypes = new List<TypeNode>()
            {
                GetFunctionType(TypeEnum.Integer, new List<TypeEnum>() {TypeEnum.Real})
            };
            
            ITypeChecker parent = Substitute.For<ITypeChecker>();
            parent.Dispatch(Arg.Any<IntegerLiteralExpression>(), Arg.Any<List<TypeNode>>()).Returns(new TypeNode(TypeEnum.Integer, 1, 1));
            IDeclarationHelper declarationHelper = Utilities.GetHelper<DeclarationHelper>(ast);
            
            var res = declarationHelper.VisitFunctionCall(input1, parameterTypes).Type;

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
            
            var ast = Utilities.GetAst();
            ast.Functions.Add(Utilities.GetFunctionNode(TypeEnum.Real, new List<TypeEnum>() { TypeEnum.Integer }));

            List<TypeNode> parameterTypes = new List<TypeNode>()
            {
                GetFunctionType(TypeEnum.Integer, new List<TypeEnum>() {TypeEnum.Real})
            };
            
            ITypeChecker parent = Substitute.For<ITypeChecker>();
            parent.Dispatch(Arg.Any<IntegerLiteralExpression>(), Arg.Any<List<TypeNode>>()).Returns(new TypeNode(TypeEnum.Integer, 1, 1));
            IDeclarationHelper declarationHelper = Utilities.GetHelper<DeclarationHelper>(ast);
            
            declarationHelper.VisitFunctionCall(input1, parameterTypes);
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
        [DataRow(TypeEnum.Boolean)]
        [TestMethod]
        public void Identifier_OneGlobalIsMatch_CorrectType(TypeEnum expectedType)
        {
            IdentifierExpression input1 = new IdentifierExpression("", 1, 1);
            input1.IsLocal = false;
            input1.Reference = 0;

            var funcParams = new List<TypeNode>();

            var ast = Utilities.GetAst();
            ast.Functions.Add(Utilities.GetFunctionNode(expectedType, new List<TypeEnum>()));
            
            IDeclarationHelper declarationHelper = Utilities.GetHelper<DeclarationHelper>(ast);

            var res = ((FunctionTypeNode) declarationHelper.VisitIdentifier(input1, funcParams)).ReturnType.Type;

            Assert.AreEqual(expectedType, res);
        }

        // 1 local, is match    -> int, real, function
        [DataRow(TypeEnum.Integer)]
        [DataRow(TypeEnum.Real)]
        [DataRow(TypeEnum.Function)]
        [DataRow(TypeEnum.Boolean)]
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

            IDeclarationHelper declarationHelper = Utilities.GetHelper<DeclarationHelper>();

            var res = declarationHelper.VisitIdentifier(input1, funcParams).Type;

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
            IDeclarationHelper declarationHelper = Utilities.GetHelper<DeclarationHelper>();

            var res = declarationHelper.VisitIntegerLiteral().Type;

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
            IDeclarationHelper declarationHelper = Utilities.GetHelper<DeclarationHelper>();

            var res = declarationHelper.VisitRealLiteral().Type;

            Assert.AreEqual(expected, res);
        }
        #endregion

        #region Bool 
        [TestMethod]
        public void VisitBooleanLiteral_GivenBooleanLiteral_ReturnsBooleanTypeNode()
        {
            TypeEnum expected = TypeEnum.Boolean;
            BooleanLiteralExpression input = new BooleanLiteralExpression(false, 2, 2);
            IDeclarationHelper declarationHelper = Utilities.GetHelper<DeclarationHelper>();

            TypeEnum res = declarationHelper.VisitBooleanLiteral().Type;

            Assert.AreEqual(expected, res);
        }
        #endregion

        #region CheckConditionNode
        [DataRow(2, 1)]
        [DataRow(10, 7)]
        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void CheckConditionNode_xElementsWithOneParameterNotElement_Exception(int elementNum, int indexOfNonElement)
        {
            var parameters = Utilities.GetTypeNodeListWithXElements(elementNum);
            parameters[indexOfNonElement] = Utilities.GetTypeNode(TypeEnum.Boolean);

            var elementNames = Utilities.GetListWithXStrings(elementNum);
            var expected = parameters.ToList();
            expected.AddRange(elementNames.ToList().ConvertAll(x => Utilities.GetTypeNode(TypeEnum.Integer)));
            var expectedType = Utilities.GetTypeNode(TypeEnum.Integer);

            var elements = Utilities.GetElements(elementNum, 1);
            var returnExpr = Utilities.GetIntLit();
            var conditionExpr = Utilities.GetGreaterExpression();
            var conditionNode = Utilities.GetConditionNode(elements, conditionExpr, returnExpr);

            var parent = Utilities.GetDefaultTypeChecker();
            var res = new List<TypeNode>();
            parent.Dispatch(conditionExpr, Arg.Do<List<TypeNode>>(x => res = x)).Returns(Utilities.GetTypeNode(TypeEnum.Boolean));
            var declarationHelper = Utilities.GetHelper<DeclarationHelper>(parent);

            declarationHelper.CheckConditionNode(expectedType, conditionNode, parameters);
        }

        [DataRow(1)]
        [DataRow(3)]
        [TestMethod]
        public void CheckConditionNode_xElementsWithOneIndex_AddToParameters(int elementNum)
        {
            var elementNames = Utilities.GetListWithXStrings(elementNum);

            var parameters = Utilities.GetTypeNodeListWithXElements(elementNum);
            var expected = parameters.ToList();
            expected.AddRange(elementNames.ToList().ConvertAll(x => Utilities.GetTypeNode(TypeEnum.Integer)));
            var expectedType = Utilities.GetTypeNode(TypeEnum.Integer);

            var elements = Utilities.GetElements(elementNum, 1);
            var returnExpr = Utilities.GetIntLit();
            var conditionExpr = Utilities.GetGreaterExpression();
            var conditionNode = Utilities.GetConditionNode(elements, conditionExpr, returnExpr);

            var parent = Utilities.GetDefaultTypeChecker();
            var res = new List<TypeNode>();
            parent.Dispatch(conditionExpr, Arg.Do<List<TypeNode>>(x => res = x)).Returns(Utilities.GetTypeNode(TypeEnum.Boolean));
            var declarationHelper = Utilities.GetHelper<DeclarationHelper>(parent);

            declarationHelper.CheckConditionNode(expectedType, conditionNode, parameters);

            res.Should().BeEquivalentTo(expected);
        }

        [DataRow(1)]
        [DataRow(3)]
        [TestMethod]
        public void CheckConditionNode_xElementsWithOneIndexWhereExpectedTypeStringReturnExprBool_AddToParameters(int elementNum)
        {
            var elementNames = Utilities.GetListWithXStrings(elementNum);

            var parameters = Utilities.GetTypeNodeListWithXElements(elementNum);
            var expected = parameters.ToList();
            expected.AddRange(elementNames.ToList().ConvertAll(x => Utilities.GetTypeNode(TypeEnum.Integer)));
            var expectedType = Utilities.GetTypeNode(TypeEnum.String);

            var elements = Utilities.GetElements(elementNum, 1);
            var returnExpr = Utilities.GetBoolLit(true);
            var conditionExpr = Utilities.GetGreaterExpression();
            var conditionNode = Utilities.GetConditionNode(elements, conditionExpr, returnExpr);

            var parent = Utilities.GetDefaultTypeChecker();
            var res = new List<TypeNode>();
            parent.Dispatch(conditionExpr, Arg.Do<List<TypeNode>>(x => res = x)).Returns(Utilities.GetTypeNode(TypeEnum.Boolean));
            var declarationHelper = Utilities.GetHelper<DeclarationHelper>(parent);

            declarationHelper.CheckConditionNode(expectedType, conditionNode, parameters);

            res.Should().BeEquivalentTo(expected);
        }

        [DataRow(1)]
        [DataRow(3)]
        [TestMethod]
        public void CheckConditionNode_xElementsWithOneIndexWhereExpectedTypeStringReturnExprReal_AddToParameters(int elementNum)
        {
            var elementNames = Utilities.GetListWithXStrings(elementNum);

            var parameters = Utilities.GetTypeNodeListWithXElements(elementNum);
            var expected = parameters.ToList();
            expected.AddRange(elementNames.ToList().ConvertAll(x => Utilities.GetTypeNode(TypeEnum.Integer)));
            var expectedType = Utilities.GetTypeNode(TypeEnum.String);

            var elements = Utilities.GetElements(elementNum, 1);
            var returnExpr = Utilities.GetRealLit();
            var conditionExpr = Utilities.GetGreaterExpression();
            var conditionNode = Utilities.GetConditionNode(elements, conditionExpr, returnExpr);

            var parent = Utilities.GetDefaultTypeChecker();
            var res = new List<TypeNode>();
            parent.Dispatch(conditionExpr, Arg.Do<List<TypeNode>>(x => res = x)).Returns(Utilities.GetTypeNode(TypeEnum.Boolean));
            var declarationHelper = Utilities.GetHelper<DeclarationHelper>(parent);

            declarationHelper.CheckConditionNode(expectedType, conditionNode, parameters);

            res.Should().BeEquivalentTo(expected);
        }

        [DataRow(1)]
        [DataRow(3)]
        [TestMethod]
        public void CheckConditionNode_ElementWithXIndicies_AddToParameters(int indexNum)
        {
            var indicies = Utilities.GetListWithXStrings(indexNum);

            var parameters = Utilities.GetTypeNodeListWithXElements(1);
            var expected = parameters.ToList();
            expected.AddRange(indicies.ToList().ConvertAll(x => Utilities.GetTypeNode(TypeEnum.Integer)));
            var expectedType = Utilities.GetTypeNode(TypeEnum.Integer);

            var elements = Utilities.GetElement(indicies, 0);
            var returnExpr = Utilities.GetIntLit();
            var conditionExpr = Utilities.GetGreaterExpression();
            var conditionNode = Utilities.GetConditionNode(elements, conditionExpr, returnExpr);

            var parent = Utilities.GetDefaultTypeChecker();
            var res = new List<TypeNode>();
            parent.Dispatch(conditionExpr, Arg.Do<List<TypeNode>>(x => res = x)).Returns(Utilities.GetTypeNode(TypeEnum.Boolean));
            var declarationHelper = Utilities.GetHelper<DeclarationHelper>(parent);

            declarationHelper.CheckConditionNode(expectedType, conditionNode, parameters);

            res.Should().BeEquivalentTo(expected);
        }

        [DataRow(1)]
        [DataRow(3)]
        [TestMethod]
        public void CheckConditionNode_ElementWithXIndicies_RemoveExtraParameters(int indexNum)
        {
            var parameters = Utilities.GetTypeNodeListWithXElements(1);
            var expected = parameters.ToList();
            var expectedType = Utilities.GetTypeNode(TypeEnum.Integer);

            var elements = Utilities.GetElement(Utilities.GetListWithXStrings(indexNum), 0);
            var returnExpr = Utilities.GetIntLit();
            var conditionExpr = Utilities.GetBoolLit(true);
            var conditionNode = Utilities.GetConditionNode(elements, conditionExpr, returnExpr);

            var parent = Utilities.GetDefaultTypeChecker();
            var declarationHelper = Utilities.GetHelper<DeclarationHelper>(parent);

            declarationHelper.CheckConditionNode(expectedType, conditionNode, parameters);

            parameters.Should().BeEquivalentTo(expected);
        }

        [TestMethod]
        public void CheckConditionNode_NoElements_NoException()
        {
            var parameters = Utilities.GetTypeNodeList();
            var expectedType = Utilities.GetTypeNode(TypeEnum.Integer);
            
            var returnExpr = Utilities.GetIntLit();
            var conditionExpr = Utilities.GetBoolLit(true);
            var conditionNode = Utilities.GetConditionNode(conditionExpr, returnExpr);

            var parent = Utilities.GetDefaultTypeChecker();
            var declarationHelper = Utilities.GetHelper<DeclarationHelper>(parent);

            declarationHelper.CheckConditionNode(expectedType, conditionNode, parameters);
        }

        [TestMethod]
        public void CheckConditionNode_NoCondition_NoException()
        {
            var parameters = Utilities.GetTypeNodeListWithXElements(1);
            var expectedType = Utilities.GetTypeNode(TypeEnum.Integer);

            var elements = Utilities.GetElement(Utilities.GetListWithXStrings(1), 0);
            var returnExpr = Utilities.GetIntLit();
            var conditionNode = Utilities.GetConditionNode(elements, null, returnExpr);

            var parent = Utilities.GetDefaultTypeChecker();
            var declarationHelper = Utilities.GetHelper<DeclarationHelper>(parent);

            declarationHelper.CheckConditionNode(expectedType, conditionNode, parameters);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidCastException))]
        public void CheckConditionNode_ConditionIsNotBoolean_Exception()
        {
            var parameters = Utilities.GetTypeNodeList();
            var expectedType = Utilities.GetTypeNode(TypeEnum.Integer);

            var returnExpr = Utilities.GetIntLit();
            var conditionExpr = Utilities.GetIntLit();
            var conditionNode = Utilities.GetConditionNode(conditionExpr, returnExpr);

            var parent = Utilities.GetDefaultTypeChecker();
            var declarationHelper = Utilities.GetHelper<DeclarationHelper>(parent);

            declarationHelper.CheckConditionNode(expectedType, conditionNode, parameters);
        }
        #endregion

    }
}