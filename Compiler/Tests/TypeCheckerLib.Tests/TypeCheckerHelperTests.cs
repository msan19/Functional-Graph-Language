using ASTLib;
using ASTLib.Interfaces;
using ASTLib.Nodes;
using ASTLib.Nodes.ExpressionNodes;
using ASTLib.Nodes.ExpressionNodes.OperationNodes;
using ASTLib.Nodes.TypeNodes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.Collections.Generic;

namespace TypeCheckerLib.Tests
{
    [TestClass]
    public class TypeCheckerHelperTests
    {
        #region Export
        #endregion

        #region Function

        #endregion

        #region Binary Num Operator
        // Int Real -> Cast Node
        // Int Real -> Append Int to Cast Node
        // Int Int  -> Still ints as children
        // Int Real -> Return Real
        // Int Int  -> Return Int

        [TestMethod]
        public void BinaryNumOp_MultiplicationExpressionWithIntAndReal_InsertedIntToRealCastNode()
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
        public void BinaryNumOp_MultiplicationExpressionWithIntAndReal_AppendedIntNodeToTypeCast()
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

        [TestMethod]
        public void BinaryNumOp_MultiplicationExpressionWithTwoInt_LeftNodeIsReal()
        {
            var expected = typeof(IntegerLiteralExpression);
            IntegerLiteralExpression intLit1 = new IntegerLiteralExpression("1", 1, 1);
            IntegerLiteralExpression intLit2 = new IntegerLiteralExpression("2", 2, 2);
            IBinaryNumberOperator input1 = new MultiplicationExpression(intLit1, intLit2, 1, 1);
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
        public void BinaryNumOp_MultiplicationExpressionWithTwoInt_RightNodeIsReal()
        {
            var expected = typeof(IntegerLiteralExpression);
            IntegerLiteralExpression intLit1 = new IntegerLiteralExpression("1", 1, 1);
            IntegerLiteralExpression intLit2 = new IntegerLiteralExpression("2", 2, 2);
            IBinaryNumberOperator input1 = new MultiplicationExpression(intLit1, intLit2, 1, 1);
            ITypeChecker parent = Substitute.For<ITypeChecker>();
            parent.Dispatch(Arg.Any<RealLiteralExpression>()).Returns(new TypeNode(TypeEnum.Real, 1, 1));
            parent.Dispatch(Arg.Any<IntegerLiteralExpression>()).Returns(new TypeNode(TypeEnum.Integer, 1, 1));
            TypeHelper typeHelper = new TypeHelper()
            {
                TypeChecker = parent
            };

            typeHelper.VisitBinaryNumOp(input1);
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
            parent.Dispatch(Arg.Any<RealLiteralExpression>()).Returns(new TypeNode(TypeEnum.Real, 1, 1));
            parent.Dispatch(Arg.Any<IntegerLiteralExpression>()).Returns(new TypeNode(TypeEnum.Integer, 1, 1));
            TypeHelper typeHelper = new TypeHelper()
            {
                TypeChecker = parent
            };

            var res = typeHelper.VisitBinaryNumOp(input1).Type;

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
            parent.Dispatch(Arg.Any<RealLiteralExpression>()).Returns(new TypeNode(TypeEnum.Real, 1, 1));
            parent.Dispatch(Arg.Any<IntegerLiteralExpression>()).Returns(new TypeNode(TypeEnum.Integer, 1, 1));
            TypeHelper typeHelper = new TypeHelper()
            {
                TypeChecker = parent
            };

            var res = typeHelper.VisitBinaryNumOp(input1).Type;

            Assert.AreEqual(expected, res);
        }
        #endregion

        #region Function Call
        // Function with no input           -> Return Int
        // One perfect match                -> Return Real
        // One perfect one normal matches   -> Return Real
        // One perfect one normal matches multiple inputs   -> Return Real
        // Two perfect matches              -> Throw Error
        // One match wrong type             -> Throw Error
        // No match                         -> Throw Error 
        // One perfect match                        -> Return Function
        // One perfect match with function as input -> Return Int
        // One match with function as input         -> Throw Error

        [TestMethod]
        public void FunctionCall_NoChildren_IntegerType()
        {
            TypeEnum expected = TypeEnum.Integer;
            FunctionCallExpression input1 = new FunctionCallExpression("", new List<ExpressionNode>(), 1, 1);
            input1.References = new List<int>() { 0 };
            var ast = GetAst();
            ast.Functions.Add(GetFunctionNode(TypeEnum.Integer, new List<TypeEnum>()));
            TypeHelper typeHelper = new TypeHelper();
            typeHelper.SetAstRoot(ast);

            var res = typeHelper.VisitFunctionCall(input1).Type;

            Assert.AreEqual(expected, res);
        }

        [TestMethod]
        public void FunctionCall_OnePerfectMatchOneWithIntChild_RealType()
        {
            TypeEnum expected = TypeEnum.Real;
            var children = new List<ExpressionNode>
            {
                new IntegerLiteralExpression("0", 0, 0)
            };
            FunctionCallExpression input1 = new FunctionCallExpression("", children, 1, 1);
            input1.References = new List<int>() { 0 };
            var ast = GetAst();
            ast.Functions.Add(GetFunctionNode(TypeEnum.Real, new List<TypeEnum>() { TypeEnum.Integer }));
            ITypeChecker parent = Substitute.For<ITypeChecker>();
            parent.Dispatch(Arg.Any<IntegerLiteralExpression>()).Returns(new TypeNode(TypeEnum.Integer, 1, 1));
            TypeHelper typeHelper = new TypeHelper { TypeChecker = parent };
            typeHelper.SetAstRoot(ast);
            var res = typeHelper.VisitFunctionCall(input1).Type;

            Assert.AreEqual(expected, res);
        }

        [TestMethod]
        public void FunctionCall_TwoMatchesAndOnePerfectMatchWithOneIntChild_RealType()
        {
            TypeEnum expected = TypeEnum.Real;
            var children = new List<ExpressionNode>
            {
                new IntegerLiteralExpression("0", 0, 0)
            };
            FunctionCallExpression input1 = new FunctionCallExpression("", children, 1, 1);
            input1.References = new List<int>() { 0, 1 };
            var ast = GetAst();
            ast.Functions.Add(GetFunctionNode(TypeEnum.Integer, new List<TypeEnum>() { TypeEnum.Real }));
            ast.Functions.Add(GetFunctionNode(TypeEnum.Real, new List<TypeEnum>() { TypeEnum.Integer }));
            ITypeChecker parent = Substitute.For<ITypeChecker>();
            parent.Dispatch(Arg.Any<IntegerLiteralExpression>()).Returns(new TypeNode(TypeEnum.Integer, 1, 1));
            parent.Dispatch(Arg.Any<RealLiteralExpression>()).Returns(new TypeNode(TypeEnum.Real, 1, 1));
            TypeHelper typeHelper = new TypeHelper { TypeChecker = parent };
            typeHelper.SetAstRoot(ast);

            var res = typeHelper.VisitFunctionCall(input1).Type;

            Assert.AreEqual(expected, res);
        }

        [TestMethod]
        public void FunctionCall_TwoMatchesAndOnePerfectMatchWithMultipleChildren_RealType()
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
            input1.References = new List<int>() { 0, 1 };
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
            parent.Dispatch(Arg.Any<IntegerLiteralExpression>()).Returns(new TypeNode(TypeEnum.Integer, 1, 1));
            parent.Dispatch(Arg.Any<RealLiteralExpression>()).Returns(new TypeNode(TypeEnum.Real, 1, 1));
            TypeHelper typeHelper = new TypeHelper { TypeChecker = parent };
            typeHelper.SetAstRoot(ast);

            var res = typeHelper.VisitFunctionCall(input1).Type;

            Assert.AreEqual(expected, res);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void FunctionCall_TwoPerfectMatchWithOneIntChild_ThrowError()
        {
            var children = new List<ExpressionNode>
            {
                new IntegerLiteralExpression("0", 0, 0)
            };
            FunctionCallExpression input1 = new FunctionCallExpression("", children, 1, 1);
            input1.References = new List<int>() { 0, 1 };
            var ast = GetAst();
            ast.Functions.Add(GetFunctionNode(TypeEnum.Integer, new List<TypeEnum>() { TypeEnum.Integer }));
            ast.Functions.Add(GetFunctionNode(TypeEnum.Real, new List<TypeEnum>() { TypeEnum.Integer }));
            ITypeChecker parent = Substitute.For<ITypeChecker>();
            parent.Dispatch(Arg.Any<IntegerLiteralExpression>()).Returns(new TypeNode(TypeEnum.Integer, 1, 1));
            parent.Dispatch(Arg.Any<RealLiteralExpression>()).Returns(new TypeNode(TypeEnum.Real, 1, 1));
            TypeHelper typeHelper = new TypeHelper { TypeChecker = parent };
            typeHelper.SetAstRoot(ast);

            var res = typeHelper.VisitFunctionCall(input1).Type;
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void FunctionCall_OneMatchWithOneIntChild_ThrowError()
        {
            var children = new List<ExpressionNode>
            {
                new IntegerLiteralExpression("0", 0, 0)
            };
            FunctionCallExpression input1 = new FunctionCallExpression("", children, 1, 1);
            input1.References = new List<int>() { 0 };
            var ast = GetAst();
            ast.Functions.Add(GetFunctionNode(TypeEnum.Integer, new List<TypeEnum>() { TypeEnum.Real }));
            ITypeChecker parent = Substitute.For<ITypeChecker>();
            parent.Dispatch(Arg.Any<IntegerLiteralExpression>()).Returns(new TypeNode(TypeEnum.Integer, 1, 1));
            parent.Dispatch(Arg.Any<RealLiteralExpression>()).Returns(new TypeNode(TypeEnum.Real, 1, 1));
            TypeHelper typeHelper = new TypeHelper { TypeChecker = parent };
            typeHelper.SetAstRoot(ast);

            var res = typeHelper.VisitFunctionCall(input1).Type;
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void FunctionCall_NoMatch_ThrowError()
        {
            FunctionCallExpression input1 = new FunctionCallExpression("", new List<ExpressionNode>(), 1, 1);
            input1.References = new List<int>() { };
            var ast = GetAst();
            ast.Functions.Add(GetFunctionNode(TypeEnum.Integer, new List<TypeEnum>() { TypeEnum.Real }));
            TypeHelper typeHelper = new TypeHelper();
            typeHelper.SetAstRoot(ast);

            var res = typeHelper.VisitFunctionCall(input1).Type;
        }

        [TestMethod]
        public void FunctionCall_NoChildren_FunctionType()
        {
            TypeEnum expectedFuncOutput = TypeEnum.Integer;
            TypeEnum expectedFuncInput = TypeEnum.Real;
            FunctionCallExpression input1 = new FunctionCallExpression("", new List<ExpressionNode>(), 1, 1);
            input1.References = new List<int>() { 0 };
            var ast = GetAst();
            ast.Functions.Add(GetFunctionNodeWithFunctionOutput(TypeEnum.Integer, new List<TypeEnum>() { TypeEnum.Real }, new List<TypeEnum>()));
            TypeHelper typeHelper = new TypeHelper();
            typeHelper.SetAstRoot(ast);

            var res = (FunctionTypeNode) typeHelper.VisitFunctionCall(input1);

            Assert.AreEqual(expectedFuncOutput, res.ReturnType.Type);
            Assert.AreEqual(expectedFuncInput, res.ParameterTypes[0].Type);
        }

        [TestMethod]
        public void FunctionCall_PerfectMatchFunctionInput_IntType()
        {
            TypeEnum expected= TypeEnum.Integer;
            var children = new List<ExpressionNode>
            {
                new IdentifierExpression("0", 0, 0) { Reference = 0 }
            };
            FunctionCallExpression input1 = new FunctionCallExpression("", children, 1, 1);
            input1.References = new List<int>() { 1 };

            var ast = GetAst();
            ast.Functions.Add(new FunctionNode("id", 0, null, null, GetFunctionType(TypeEnum.Real, new List<TypeEnum>() { TypeEnum.Real }), 0, 0));
            ast.Functions.Add(new FunctionNode("id", 1, null, null, GetFunctionType(TypeEnum.Integer, GetFunctionType(TypeEnum.Real, new List<TypeEnum>() { TypeEnum.Real })), 0, 0));

            ITypeChecker parent = Substitute.For<ITypeChecker>();
            parent.Dispatch(Arg.Any<IdentifierExpression>()).Returns(GetFunctionType(TypeEnum.Real, new List<TypeEnum>() { TypeEnum.Real }));
            TypeHelper typeHelper = new TypeHelper { TypeChecker = parent };
            typeHelper.SetAstRoot(ast);

            var res = typeHelper.VisitFunctionCall(input1).Type;

            Assert.AreEqual(expected, res);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void FunctionCall_NoMatchFunctionInput_IntType()
        {
            var children = new List<ExpressionNode>
            {
                new IdentifierExpression("0", 0, 0) { Reference = 0 }
            };
            FunctionCallExpression input1 = new FunctionCallExpression("", children, 1, 1);
            input1.References = new List<int>() { 1 };

            var ast = GetAst();
            ast.Functions.Add(new FunctionNode("id", 0, null, null, GetFunctionType(TypeEnum.Real, new List<TypeEnum>() { TypeEnum.Real }), 0, 0));
            ast.Functions.Add(new FunctionNode("id", 1, null, null, GetFunctionType(TypeEnum.Integer, GetFunctionType(TypeEnum.Real, new List<TypeEnum>() { TypeEnum.Integer })), 0, 0));

            ITypeChecker parent = Substitute.For<ITypeChecker>();
            parent.Dispatch(Arg.Any<IdentifierExpression>()).Returns(GetFunctionType(TypeEnum.Real, new List<TypeEnum>() { TypeEnum.Real }));
            TypeHelper typeHelper = new TypeHelper { TypeChecker = parent };
            typeHelper.SetAstRoot(ast);

            var res = typeHelper.VisitFunctionCall(input1).Type;
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

        private FunctionNode GetFunctionNodeWithFunctionOutput(TypeEnum funcInputType, List<TypeEnum> funcOutputs, List<TypeEnum> inputTypes)
        {
            var inputs = new List<TypeNode>();
            foreach (var input in inputTypes)
                inputs.Add(new TypeNode(input, 0, 0));

            var functOutput = new TypeNode(funcInputType, 0, 0);
            var funcInputs = new List<TypeNode>();
            foreach (var input in funcOutputs)
                funcInputs.Add(new TypeNode(input, 0, 0));
            var output = new FunctionTypeNode(functOutput, funcInputs, 0, 0);

            return new FunctionNode("id", 0, null, null,
                new FunctionTypeNode(output,
                inputs, 0, 0), 0, 0);
        }

        private FunctionNode GetFunctionNode(TypeEnum output, List<TypeEnum> inputTypes)
        {
            var inputs = new List<TypeNode>();
            foreach (var input in inputTypes)
                inputs.Add(new TypeNode(input, 0, 0));

            return new FunctionNode("id", 0, null, null,
                new FunctionTypeNode(new TypeNode(output, 0, 0),
                inputs, 0, 0), 0, 0);
        }
        #endregion

        #region Identifier
        #endregion

        #region Integer 
        // Returns Integer
        [TestMethod]
        public void Int_IntLiteral_IntTypeNode()
        {
            var expected = TypeEnum.Integer;
            IntegerLiteralExpression input1 = new IntegerLiteralExpression("2", 2, 2);
            TypeHelper typeHelper = new TypeHelper();

            var res = typeHelper.VisitIntegerLiteral(input1).Type;

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

            var res = typeHelper.VisitRealLiteral(input1).Type;

            Assert.AreEqual(expected, res);
        }
        #endregion

    }
}
