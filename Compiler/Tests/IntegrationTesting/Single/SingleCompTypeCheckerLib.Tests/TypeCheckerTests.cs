using ASTLib;
using ASTLib.Exceptions.NotMatching;
using ASTLib.Nodes;
using ASTLib.Nodes.ExpressionNodes;
using ASTLib.Nodes.ExpressionNodes.CastExpressionNodes;
using ASTLib.Nodes.ExpressionNodes.NumberOperationNodes;
using ASTLib.Nodes.ExpressionNodes.OperationNodes;
using ASTLib.Nodes.TypeNodes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using TypeCheckerLib;
using TypeCheckerLib.Helpers;
using TypeBooleanHelper = TypeCheckerLib.Helpers.BooleanHelper;
using TypeSetHelper = TypeCheckerLib.Helpers.SetHelper;

namespace SingleCompTypeCheckerLib.Tests
{
    public static class Utilities
    {
        public static AST GetAstSkeleton()
        {
            List<FunctionNode> functionNodes = new List<FunctionNode>();
            List<ExportNode> exportNodes = new List<ExportNode>();
            return new AST(functionNodes, exportNodes, 0, 0);
        }

        public static void AddFunctionNode(AST ast, FunctionNode functionNode)
        {
            ast.Functions.Add(functionNode);
        }

        internal static AdditionExpression GetAdditionExpr(string leftId, int leftRef, string rightId, int rightRef)
        {
            return new AdditionExpression(GetIdentifierExpr(leftId, leftRef), GetIdentifierExpr(rightId, rightRef),0,0);
        }

        private static IdentifierExpression GetIdentifierExpr(string leftId, int reference)
        {
            var res = new IdentifierExpression(leftId, 0, 0);
            res.Reference = reference;
            res.IsLocal = true;
            return res;
        }

        internal static FunctionNode GetFunction(TypeEnum output, List<TypeEnum> inputTypes, ConditionNode condition)
        {
            return new FunctionNode("Func", condition, new List<string>(), GetFunctionTypeNode(inputTypes, output), 0, 0);
        }
        internal static FunctionNode GetFunction(TypeNode output, List<TypeEnum> inputTypes, ConditionNode condition)
        {
            return new FunctionNode("Func", condition, new List<string>(), GetFunctionTypeNode(inputTypes, output), 0, 0);
        }
        internal static FunctionNode GetFunction(TypeEnum output, List<TypeNode> inputTypes, ConditionNode condition)
        {
            return new FunctionNode("Func", condition, new List<string>(), GetFunctionTypeNode(inputTypes, output), 0, 0);
        }

        private static FunctionTypeNode GetFunctionTypeNode(List<TypeNode> inputTypes, TypeEnum output)
        {
            return new FunctionTypeNode(GetTypeNode(output), inputTypes, 0, 0);
        }

        private static FunctionTypeNode GetFunctionTypeNode(List<TypeEnum> inputTypes, TypeEnum output)
        {
            return new FunctionTypeNode(GetTypeNode(output), GetTypeNode(inputTypes), 0, 0);
        }
        private static FunctionTypeNode GetFunctionTypeNode(List<TypeEnum> inputTypes, TypeNode output)
        {
            return new FunctionTypeNode(output, GetTypeNode(inputTypes), 0, 0);
        }

        private static TypeNode GetTypeNode(TypeEnum type)
        {
            return new TypeNode(type, 0, 0);
        }

        private static List<TypeNode> GetTypeNode(List<TypeEnum> types)
        {
            var res = new List<TypeNode>();
            foreach (var type in types)
                res.Add(GetTypeNode(type));
            return res;
        }

        internal static ConditionNode GetCondition(ExpressionNode expr)
        {
            return new ConditionNode(expr, 0, 0);
        }

        internal static TypeChecker GetTypeChecker()
        {
            return new TypeChecker(new DeclarationHelper(),
                                           new NumberHelper(),
                                           new CommonOperatorHelper(),
                                           new TypeBooleanHelper(),
                                           new TypeSetHelper());
        }

        internal static List<int> GetIntList(int num)
        {
            var res = new List<int>();
            for (int i = 0; i < num; i++)
                res.Add(i);
            return res;
        }
        internal static List<int> GetIntList(int start, int end)
        {
            var res = new List<int>();
            for (int i = start; i < end; i++)
                res.Add(i);
            return res;
        }

        internal static FunctionCallExpression GetFunctionCallExpr(List<int> globals, int local, List<ExpressionNode> inputs)
        {
            var res = new FunctionCallExpression("func", inputs, 0, 0);
            res.GlobalReferences = globals;
            res.LocalReference = local;
            return res;
        }

        internal static List<ExpressionNode> GetIntLitList(int num)
        {
            var res = new List<ExpressionNode>();
            for (int i = 0; i < num; i++)
                res.Add(new IntegerLiteralExpression(i, 0, 0));
            return res;
        }

        internal static void AddFunctions(AST ast, int num, TypeEnum output, TypeEnum input, TypeEnum wrongType)
        {
            var inputs = new List<TypeEnum>();
            for (int i = 0; i < num; i++)
                inputs.Add(wrongType);
            for (int i = 0; i < num; i++)
            {
                inputs[i] = (input);
                AddFunctionNode(ast, GetFunction(output, inputs, GetCondition(GetIntLit())));
            }
        }

        internal static ExpressionNode GetIntLit()
        {
            return new IntegerLiteralExpression(0, 0, 0);
        }

        internal static ExpressionNode GetBoolLit()
        {
            return new BooleanLiteralExpression(false, 0, 0);
        }

        internal static ExpressionNode GetLit(TypeEnum x)
        {
            return x switch
            {
                TypeEnum.InvalidType => throw new NotImplementedException(),
                TypeEnum.Real => GetRealLit(),
                TypeEnum.Integer => GetIntLit(),
                TypeEnum.Boolean => GetBoolLit(),
                TypeEnum.String => GetStringLit(),
                _ => throw new NotImplementedException(),
            };
        }

        internal static StringLiteralExpression GetStringLit()
        {
            return new StringLiteralExpression("string", 0, 0);
        }

        internal static RealLiteralExpression GetRealLit()
        {
            return new RealLiteralExpression(0, 0, 0);
        }

        internal static TypeNode GetFunctionType(TypeEnum returnType, List<TypeEnum> inputs)
        {
            return new FunctionTypeNode(GetTypeNode(returnType), GetTypeNode(inputs), 0, 0);
        }

        internal static AnonymousFunctionExpression GetAnonymFunction()
        {
            return new AnonymousFunctionExpression(new List<string>(), new List<TypeNode>(), GetIntLit(), 0, 0);
        }
    }

    [TestClass]
    public class TypeCheckerTests
    {
        private static int NO_LOCAL_REF => FunctionCallExpression.NO_LOCAL_REF;
        // Insert Cast Node
        // Removed irrelevant references on functions
        //  Global
        //  Local
        //  Both
        // Anonymous Function
        //  Move Anonym to global scope (functions list)
        // Throw error on invalid Types

        #region Throw Errors 
        [DataRow(TypeEnum.Real, TypeEnum.String, TypeEnum.Real)]
        [DataRow(TypeEnum.String, TypeEnum.String, TypeEnum.Real)]
        [DataRow(TypeEnum.Integer, TypeEnum.Integer, TypeEnum.Boolean)]
        [DataRow(TypeEnum.Real, TypeEnum.String, TypeEnum.Boolean)]
        [TestMethod]
        [ExpectedException(typeof(ASTLib.Exceptions.Invalid.InvalidCastException))]
        public void BadInput_CastProblem_ThrowError(TypeEnum left, TypeEnum right, TypeEnum output)
        {
            string leftId = "a";
            int leftRef = 0;
            string rightId = "b";
            int rightRef = 1;
            var types = new List<TypeEnum>() { left, right };

            var ast = Utilities.GetAstSkeleton();
            var expr = Utilities.GetAdditionExpr(leftId, leftRef, rightId, rightRef);
            var cond = Utilities.GetCondition(expr);
            var func = Utilities.GetFunction(output, types, cond);
            Utilities.AddFunctionNode(ast, func);

            var typeChecker = Utilities.GetTypeChecker();
            typeChecker.CheckTypes(ast);
        }
        [DataRow(TypeEnum.Boolean, TypeEnum.Real, TypeEnum.Boolean)]
        [DataRow(TypeEnum.Boolean, TypeEnum.Integer, TypeEnum.Boolean)]
        [TestMethod]
        [ExpectedException(typeof(UnmatchableTypesException))]
        public void BadInput_WrongInput_ThrowError(TypeEnum left, TypeEnum right, TypeEnum output)
        {
            string leftId = "a";
            int leftRef = 0;
            string rightId = "b";
            int rightRef = 1;
            var types = new List<TypeEnum>() { left, right };

            var ast = Utilities.GetAstSkeleton();
            var expr = Utilities.GetAdditionExpr(leftId, leftRef, rightId, rightRef);
            var cond = Utilities.GetCondition(expr);
            var func = Utilities.GetFunction(output, types, cond);
            Utilities.AddFunctionNode(ast, func);

            var typeChecker = Utilities.GetTypeChecker();
            typeChecker.CheckTypes(ast);
        }
        #endregion

        #region Anonymous Functions
        [TestMethod]
        public void AnonymFunctions__AddToFunctions()
        {
            var ast = Utilities.GetAstSkeleton();

            var expr = Utilities.GetAnonymFunction();
            var cond = Utilities.GetCondition(expr);
            var func = Utilities.GetFunction(Utilities.GetFunctionType(TypeEnum.Integer, new List<TypeEnum>()), new List<TypeEnum>(), cond);
            Utilities.AddFunctionNode(ast, func);

            var typeChecker = Utilities.GetTypeChecker();
            typeChecker.CheckTypes(ast);

            Assert.AreEqual(2, ast.Functions.Count);
        }
        #endregion

        #region Irrelevant Function References 
        [DataRow(new TypeEnum[] { TypeEnum.Integer }, new TypeEnum[] { TypeEnum.Boolean })]
        [TestMethod]
        public void RemoveIrrelevantFuncReferences_OneGlobalOneReference_LocalScope(TypeEnum[] global, TypeEnum[] local)
        {
            var ast = Utilities.GetAstSkeleton();

            // Global
            var expr = Utilities.GetIntLit();
            var cond = Utilities.GetCondition(expr);
            var func = Utilities.GetFunction(TypeEnum.Integer, global.ToList(), cond);
            Utilities.AddFunctionNode(ast, func);
            // Local
            var funcType = Utilities.GetFunctionType(TypeEnum.Integer, local.ToList());
            // Function Call
            int globalRef = 0;
            int localRef = 0;
            var inputs = local.ToList().ConvertAll(x => Utilities.GetLit(x));
            expr = Utilities.GetFunctionCallExpr(new List<int>() { globalRef }, localRef, inputs);
            cond = Utilities.GetCondition(expr);
            func = Utilities.GetFunction(TypeEnum.Integer, new List<TypeNode>() { funcType }, cond);
            Utilities.AddFunctionNode(ast, func);

            var typeChecker = Utilities.GetTypeChecker();
            typeChecker.CheckTypes(ast);
            var res = (FunctionCallExpression)expr;

            Assert.AreEqual(localRef, res.LocalReference);
            Assert.AreEqual(0, res.GlobalReferences.Count);
        }
        [DataRow(new TypeEnum[] { TypeEnum.Integer }, new TypeEnum[] { TypeEnum.Boolean })]
        [TestMethod]
        public void RemoveIrrelevantFuncReferences_OneGlobalOneReference_GlobalScope(TypeEnum[] global, TypeEnum[] local)
        {
            var ast = Utilities.GetAstSkeleton();

            // Global
            var expr = Utilities.GetIntLit();
            var cond = Utilities.GetCondition(expr);
            var func = Utilities.GetFunction(TypeEnum.Integer, global.ToList(), cond);
            Utilities.AddFunctionNode(ast, func);
            // Local
            var funcType = Utilities.GetFunctionType(TypeEnum.Integer, local.ToList());
            // Function Call
            int globalRef = 0;
            int localRef = 0;
            var inputs = global.ToList().ConvertAll(x => Utilities.GetLit(x));
            expr = Utilities.GetFunctionCallExpr(new List<int>() { globalRef }, localRef, inputs);
            cond = Utilities.GetCondition(expr);
            func = Utilities.GetFunction(TypeEnum.Integer, new List<TypeNode>() { funcType }, cond);
            Utilities.AddFunctionNode(ast, func);

            var typeChecker = Utilities.GetTypeChecker();
            typeChecker.CheckTypes(ast);
            var res = (FunctionCallExpression)expr;

            Assert.AreEqual(NO_LOCAL_REF, res.LocalReference);
            Assert.AreEqual(1, res.GlobalReferences.Count);
            Assert.AreEqual(globalRef, res.GlobalReferences[0]);
        }
        [DataRow(3,1)]
        [DataRow(3,2)]
        [DataRow(3,3)]
        [TestMethod]
        public void RemoveIrrelevantFuncReferences_xGlobalReferences_OneReference(int numFuncs, int correctNum)
        {
            int expected = correctNum - 1;
            var output = TypeEnum.Integer;
            var input = TypeEnum.Integer;
            var wrongType = TypeEnum.Boolean;
            var typeParams = new List<TypeEnum>();

            var globals = Utilities.GetIntList(numFuncs);
            var local = NO_LOCAL_REF;
            var inputs = Utilities.GetIntLitList(correctNum);
            for (int i = correctNum; i < numFuncs; i++)
                inputs.Add(Utilities.GetBoolLit());

            var ast = Utilities.GetAstSkeleton();
            Utilities.AddFunctions(ast, numFuncs, output, input, wrongType);

            var expr = Utilities.GetFunctionCallExpr(globals, local, inputs);
            var cond = Utilities.GetCondition(expr);
            var func = Utilities.GetFunction(output, typeParams, cond);
            Utilities.AddFunctionNode(ast, func);

            var typeChecker = Utilities.GetTypeChecker();
            typeChecker.CheckTypes(ast);

            Assert.AreEqual(1, expr.GlobalReferences.Count);
            Assert.AreEqual(expected, expr.GlobalReferences[0]);
        }
        #endregion

        #region Cast Node
        [DataRow(TypeEnum.Real, TypeEnum.Integer, TypeEnum.Real)]
        [DataRow(TypeEnum.String, TypeEnum.Integer, TypeEnum.String)]
        [DataRow(TypeEnum.String, TypeEnum.Real, TypeEnum.String)]
        [DataRow(TypeEnum.String, TypeEnum.Boolean, TypeEnum.String)]
        [TestMethod]
        public void InserCastNode_Addition_CastIntToReal(TypeEnum left, TypeEnum right, TypeEnum output)
        {
            string leftId = "a";
            int leftRef = 0;
            string rightId = "b";
            int rightRef = 1;
            var ids = new List<string>() { leftId, rightId };
            var types = new List<TypeEnum>() { left, right };

            var ast = Utilities.GetAstSkeleton();
            var expr = Utilities.GetAdditionExpr(leftId, leftRef, rightId, rightRef);
            var cond = Utilities.GetCondition(expr);
            var func = Utilities.GetFunction(output, types, cond);
            Utilities.AddFunctionNode(ast, func);

            var typeChecker = Utilities.GetTypeChecker();
            typeChecker.CheckTypes(ast);
            var res = expr.Children[1].GetType();

            Assert.IsTrue(
                res == typeof(CastFromRealExpression) ||
                res == typeof(CastFromBooleanExpression) ||
                res == typeof(CastFromIntegerExpression));
        }
        #endregion
    }
}
