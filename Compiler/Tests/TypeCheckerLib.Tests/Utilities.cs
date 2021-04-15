using System;
using System.Collections.Generic;
using System.Linq;
using ASTLib;
using ASTLib.Nodes;
using ASTLib.Nodes.ExpressionNodes;
using ASTLib.Nodes.ExpressionNodes.BooleanOperationNodes;
using ASTLib.Nodes.ExpressionNodes.CommonOperationNodes;
using ASTLib.Nodes.ExpressionNodes.CommonOperationNodes.RelationalOperationNodes;
using ASTLib.Nodes.ExpressionNodes.NumberOperationNodes;
using ASTLib.Nodes.TypeNodes;
using NSubstitute;
using TypeCheckerLib.Interfaces;

namespace TypeCheckerLib.Tests
{
    public static class Utilities
    {
        private static void SetTypeCheckerDefaultValues(ITypeChecker typeChecker)
        {
            typeChecker.Dispatch(Arg.Any<RealLiteralExpression>(), Arg.Any<List<TypeNode>>()).Returns(new TypeNode(TypeEnum.Real, 1, 1));
            typeChecker.Dispatch(Arg.Any<IntegerLiteralExpression>(), Arg.Any<List<TypeNode>>()).Returns(new TypeNode(TypeEnum.Integer, 1, 1));
            typeChecker.Dispatch(Arg.Any<BooleanLiteralExpression>(), Arg.Any<List<TypeNode>>()).Returns(new TypeNode(TypeEnum.Boolean, 1, 1));
            typeChecker.Dispatch(Arg.Any<SetExpression>(), Arg.Any<List<TypeNode>>()).Returns(new TypeNode(TypeEnum.Set, 1, 1));
        }

        public static T GetHelper<T>() where T : ITypeHelper, new()
        {
            ITypeChecker parent = Substitute.For<ITypeChecker>();
            SetTypeCheckerDefaultValues(parent);

            var helper = new T();
            helper.Initialize(GetAst(), parent.Dispatch);
            return helper;
        }
        public static T GetHelper<T>(ITypeChecker parent) where T : ITypeHelper, new()
        {
            var helper = new T();
            helper.Initialize(GetAst(), parent.Dispatch);
            return helper;
        }

        internal static object GetIntepretorOnlyWith(IBooleanHelper boolHelper)
        {
            throw new NotImplementedException();
        }

        public static T GetHelper<T>(AST root) where T : ITypeHelper, new()
        {
            ITypeChecker parent = Substitute.For<ITypeChecker>();
            SetTypeCheckerDefaultValues(parent);

            var helper = new T();
            helper.Initialize(root, parent.Dispatch);
            return helper;
        }
        public static T GetHelper<T>(AST root, ITypeChecker parent) where T : ITypeHelper, new()
        {
            var helper = new T();
            helper.Initialize(root, parent.Dispatch);
            return helper;
        }

        public static ITypeChecker GetTypeCheckerOnlyWith(IDeclarationHelper declarationHelper)
        {
            ICommonOperatorHelper commonOperatorHelper = Substitute.For<ICommonOperatorHelper>();
            INumberHelper numberHelper = Substitute.For<INumberHelper>();
            IBooleanHelper booleanHelper = Substitute.For<IBooleanHelper>();
            ISetHelper setHelper = Substitute.For<ISetHelper>();
            return new TypeChecker(declarationHelper, numberHelper, commonOperatorHelper, booleanHelper, setHelper);
        }
        
        public static ITypeChecker GetTypeCheckerOnlyWith(ICommonOperatorHelper commonOperatorHelper)
        {
            IDeclarationHelper declarationHelper = Substitute.For<IDeclarationHelper>();
            INumberHelper numberHelper = Substitute.For<INumberHelper>();
            IBooleanHelper booleanHelper = Substitute.For<IBooleanHelper>();
            ISetHelper setHelper = Substitute.For<ISetHelper>();
            return new TypeChecker(declarationHelper, numberHelper, commonOperatorHelper, booleanHelper, setHelper);
        }
        
        public static ITypeChecker GetTypeCheckerOnlyWith(INumberHelper numberHelper)
        {
            IDeclarationHelper declarationHelper = Substitute.For<IDeclarationHelper>();
            ICommonOperatorHelper commonOperatorHelper = Substitute.For<ICommonOperatorHelper>();
            IBooleanHelper booleanHelper = Substitute.For<IBooleanHelper>();
            ISetHelper setHelper = Substitute.For<ISetHelper>();
            return new TypeChecker(declarationHelper, numberHelper, commonOperatorHelper, booleanHelper, setHelper);
        }
        
        public static ITypeChecker GetTypeCheckerOnlyWith(IBooleanHelper booleanHelper)
        {
            IDeclarationHelper declarationHelper = Substitute.For<IDeclarationHelper>();
            ICommonOperatorHelper commonOperatorHelper = Substitute.For<ICommonOperatorHelper>();
            INumberHelper numberHelper = Substitute.For<INumberHelper>();
            ISetHelper setHelper = Substitute.For<ISetHelper>();
            return new TypeChecker(declarationHelper, numberHelper, commonOperatorHelper, booleanHelper, setHelper);
        }
        
        public static ITypeChecker GetTypeCheckerOnlyWith(ISetHelper setHelper)
        {
            IDeclarationHelper declarationHelper = Substitute.For<IDeclarationHelper>();
            INumberHelper numberHelper = Substitute.For<INumberHelper>();
            ICommonOperatorHelper commonOperatorHelper = Substitute.For<ICommonOperatorHelper>();
            IBooleanHelper booleanHelper = Substitute.For<IBooleanHelper>();
            return new TypeChecker(declarationHelper, numberHelper, commonOperatorHelper, booleanHelper, setHelper);
        }

        public static FunctionTypeNode GetFunctionType(TypeEnum returnType, List<TypeEnum> inputTypes)
        {
            var inputs = new List<TypeNode>();
            foreach (var input in inputTypes)
                inputs.Add(new TypeNode(input, 0, 0));
            return new FunctionTypeNode(new TypeNode(returnType, 0, 0), inputs, 0, 0);
        }
        
        public static FunctionTypeNode GetFunctionType(TypeEnum returnType, FunctionTypeNode inputType)
        {
            return new FunctionTypeNode(new TypeNode(returnType, 0, 0), new List<TypeNode>() { inputType }, 0, 0);
        }
        
        public static FunctionNode GetFunctionNodeWithFunctionOutput(TypeEnum funcOutput, List<TypeEnum> funcInputTypes, List<TypeEnum> inputToNewFunc)
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

        public static FunctionNode GetFunctionNode(TypeEnum output, List<TypeEnum> inputTypes)
        {
            var inputs = new List<TypeNode>();
            foreach (var input in inputTypes)
                inputs.Add(new TypeNode(input, 0, 0));

            return new FunctionNode("id", null, null,
                new FunctionTypeNode(new TypeNode(output, 0, 0),
                    inputs, 0, 0), 0, 0);
        }
        
        public static AST GetAst()
        {
            return new AST(new List<FunctionNode>(), new List<ExportNode>(), 0, 0);
        }

        public static NegativeExpression GetNegativeExpressionWithInt()
        {
            IntegerLiteralExpression intLitExpr = new IntegerLiteralExpression("", 0, 0);
            return new NegativeExpression(new List<ExpressionNode>(){intLitExpr}, 0, 0);
        }
        
        public static NegativeExpression GetNegativeExpressionWithReal()
        {
            RealLiteralExpression realLitExpr = new RealLiteralExpression("", 0, 0);
            return new NegativeExpression(new List<ExpressionNode>(){realLitExpr}, 0, 0);
        }
        
        public static NegativeExpression GetNegativeExpressionWithBool()
        {
            BooleanLiteralExpression boolLitExpr = new BooleanLiteralExpression(false, 0, 0);
            return new NegativeExpression(new List<ExpressionNode>(){boolLitExpr}, 0, 0);
        }

        public static TypeNode GetTypeNode(TypeEnum t)
        {
            return new TypeNode(t, 0, 0);
        }

        public static ConditionNode GetConditionNode(ExpressionNode conditionExpr, ExpressionNode returnExpr)
        {
            return new ConditionNode(conditionExpr, returnExpr, 0, 0);
        }

        public static ConditionNode GetConditionNode(List<ElementNode> elements, ExpressionNode conditionExpr, ExpressionNode returnExpr)
        {
            return new ConditionNode(elements, conditionExpr, returnExpr, 0, 0);
        }

        internal static IntegerLiteralExpression GetIntLit()
        {
            return new IntegerLiteralExpression(0, 0, 0);
        }

        internal static BooleanLiteralExpression GetBoolLit(bool v)
        {
            return new BooleanLiteralExpression(v, 0, 0);
        }

        internal static ITypeChecker GetDefaultTypeChecker()
        {
            var res = Substitute.For<ITypeChecker>();
            res.Dispatch(Arg.Any<IntegerLiteralExpression>(), Arg.Any<List<TypeNode>>()).Returns(GetTypeNode(TypeEnum.Integer));
            res.Dispatch(Arg.Any<RealLiteralExpression>(), Arg.Any<List<TypeNode>>()).Returns(GetTypeNode(TypeEnum.Real));
            res.Dispatch(Arg.Any<BooleanLiteralExpression>(), Arg.Any<List<TypeNode>>()).Returns(GetTypeNode(TypeEnum.Boolean));
            res.Dispatch(Arg.Any<FunctionCallExpression>(), Arg.Any<List<TypeNode>>()).Returns(GetTypeNode(TypeEnum.Function));
            res.Dispatch(Arg.Any<ElementExpression>(), Arg.Any<List<TypeNode>>()).Returns(GetTypeNode(TypeEnum.Element));
            res.Dispatch(Arg.Any<SetExpression>(), Arg.Any<List<TypeNode>>()).Returns(GetTypeNode(TypeEnum.Set));
            return res;
        }

        internal static List<ElementNode> GetElement(List<string> indexIds, int refId)
        {
            var res = new List<ElementNode>();
            var elem = new ElementNode("e", indexIds.ToList(), 0, 0);
            elem.Reference = refId;
            res.Add(elem);
            return res;
        }

        internal static List<string> GetListWithXStrings(int n)
        {
            var res = new List<string>();
            for (int i = 0; i < n; i++)
                res.Add("");
            return res;
        }

        internal static GreaterExpression GetGreaterExpression()
        {
            return new GreaterExpression(null, null, 0, 0);
        }

        internal static List<ElementNode> GetElements(int nElements, int nIndicies)
        {
            var res = new List<ElementNode>();
            for (int i = 0; i < nElements; i++)
                res.Add(GetElement(GetListWithXStrings(nIndicies), i).First());
            return res;
        }

        internal static List<TypeNode> GetTypeNodeList()
        {
            return new List<TypeNode>();
        }

        internal static List<TypeNode> GetTypeNodeListWithXElements(int elementNum)
        {
            var res = GetTypeNodeList();
            for (int i = 0; i < elementNum; i++)
                res.Add(GetTypeNode(TypeEnum.Element));
            return res;
        }
    }
}