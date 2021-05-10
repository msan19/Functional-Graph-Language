using ASTLib;
using ASTLib.Nodes;
using ASTLib.Nodes.ExpressionNodes;
using ASTLib.Nodes.ExpressionNodes.CastExpressionNodes;
using ASTLib.Nodes.ExpressionNodes.NumberOperationNodes;
using ASTLib.Nodes.ExpressionNodes.OperationNodes;
using ASTLib.Nodes.TypeNodes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
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

        internal static FunctionNode GetFunction(string id, TypeEnum output, List<TypeEnum> inputTypes, List<string> ids, ConditionNode condition)
        {
            return new FunctionNode(id, condition, ids, GetFunctionTypeNode(inputTypes, output), 0, 0);
        }

        private static FunctionTypeNode GetFunctionTypeNode(List<TypeEnum> inputTypes, TypeEnum output)
        {
            return new FunctionTypeNode(GetTypeNode(output), GetTypeNode(inputTypes), 0, 0);
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
    }

    [TestClass]
    public class TypeCheckerTests
    {
        // Insert Cast Node
        // Throw error on invalid Types
        // Anonymous Function
        //  Chakc if it is correct in given context
        //  Move Anonym to global scope 
        // Removed irrelevant references on functions

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
            var func = Utilities.GetFunction("func", output, types, ids, cond);
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
