using ASTLib;
using ASTLib.Nodes;
using ASTLib.Nodes.ExpressionNodes;
using ASTLib.Nodes.ExpressionNodes.NumberOperationNodes;
using ASTLib.Nodes.ExpressionNodes.OperationNodes;
using ASTLib.Nodes.TypeNodes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ReferenceHandlerLib;
using System;
using System.Collections.Generic;
using TypeCheckerLib;
using TypeCheckerLib.Helpers;
using TypeBooleanHelper = TypeCheckerLib.Helpers.BooleanHelper;
using TypeSetHelper = TypeCheckerLib.Helpers.SetHelper;

namespace ReferenceHandlerAndTypeChecker.Tests
{
    public static class Utilities
    {
        public static AST GetMultiGraphExample(int n)
        {
            string nId = "n";
            string elementId = "e";
            string id = "i";

            var ast = GetAstSkeleton();
            
            var vertexSetFunc = GetFunctionNode("vertexSet", GetSetExpr(n), "n", TypeEnum.Integer, TypeEnum.Set);
            var edgeSet = GetSetExpr(n);
            AddFunctionNode(ast, vertexSetFunc);

            var add = GetAddNode(GetIdentifierExpr(id), GetIntLit(1));
            var mod = GetModExpr(add, GetIdentifierExpr(nId));
            var dst = GetFunctionNode("edgeFunc", GetElementNode(elementId, id), GetElementExpr(mod), new List<string>() { "e", "n" }, new List<TypeEnum>() { TypeEnum.Element, TypeEnum.Integer }, TypeEnum.Element );
            var src = GetFunctionNode("edgeFunc", GetIdentifierExpr(elementId), "e", TypeEnum.Element, TypeEnum.Element);
            AddFunctionNode(ast, dst);
            AddFunctionNode(ast, src);


            var vLabel = GetFunctionNode("vLabel", GetElementNode(elementId, id), GetAddNode(GetStringLit("Vertex: "), GetIdentifierExpr(id)), "e", TypeEnum.Element, TypeEnum.String);
            var eLabel = GetFunctionNode("eLabel", GetElementNode(elementId, id), GetAddNode(GetStringLit("Edge: "), GetIdentifierExpr(id)), "e", TypeEnum.Element, TypeEnum.String);
            AddFunctionNode(ast, vLabel);
            AddFunctionNode(ast, eLabel);

            var anonymSrc = GetAnonymFunc(GetFunctionCall("edgeFunc", new List<string>() { "e" }), "e", TypeEnum.Element);
            var anonymDst = GetAnonymFunc(GetFunctionCall("edgeFunc", new List<string>() { "e", "n" }), "e", TypeEnum.Element);
            var graph = GetGraphExpr(GetFunctionCall("vertexSet", "n"), edgeSet, anonymSrc, anonymDst);
            var graphFunc = GetFunctionNode("graphFunc", graph, "n", TypeEnum.Integer, TypeEnum.Graph);
            AddFunctionNode(ast, graphFunc);
            
            var export = GetExportNode(GetFunctionCall("graphFunc", GetIntLit(n)), GetStringLit("MultiIntegration"), GetIdentifierExpr("vLabel"), GetIdentifierExpr("eLabel"));
            AddExportNode(ast, export);
            return ast;
        }

        private static ExportNode GetExportNode(ExpressionNode exporValue, ExpressionNode fileName, ExpressionNode vLabel, ExpressionNode eLabel)
        {
            return new ExportNode(exporValue, fileName, new List<ExpressionNode>() { vLabel }, new List<ExpressionNode>() { eLabel }, 0, 0);
        }

        private static GraphExpression GetGraphExpr(ExpressionNode vertecies, ExpressionNode edges, ExpressionNode src, ExpressionNode dst)
        {
            return new GraphExpression(vertecies, edges, src, dst, 0, 0);
        }

        private static AnonymousFunctionExpression GetAnonymFunc(ExpressionNode expr, string id, TypeEnum type)
        {
            return new AnonymousFunctionExpression(new List<string>() { id }, new List<TypeNode>() { GetTypeNode(type) }, expr, 0, 0);
        }

        private static FunctionCallExpression GetFunctionCall(string funcId, List<string> parameters)
        {
            return new FunctionCallExpression(funcId, parameters.ConvertAll<ExpressionNode>(x => GetIdentifierExpr(x)), 0, 0);
        }

        private static ExpressionNode GetFunctionCall(string funcId, ExpressionNode input)
        {
            return new FunctionCallExpression(funcId, new List<ExpressionNode>() { input }, 0, 0);
        }

        private static FunctionCallExpression GetFunctionCall(string funcId, string parameter)
        {
            return new FunctionCallExpression(funcId, new List<ExpressionNode>() { GetIdentifierExpr(parameter) }, 0, 0);
        }

        private static StringLiteralExpression GetStringLit(string v)
        {
            return new StringLiteralExpression(v, 0, 0);
        }

        private static FunctionNode GetFunctionNode(string name, ExpressionNode expr, string parameter, TypeEnum inputType, TypeEnum returnType)
        {
            return GetFunctionNode(name, expr, new List<string>() { parameter }, new List<TypeEnum>() { inputType }, returnType);
        }

        private static FunctionNode GetFunctionNode(string name, ExpressionNode expr, List<string> parameters, List<TypeEnum> inputTypes, TypeEnum returnType)
        {
            return new FunctionNode(name, GetCondition(expr), parameters, GetFunctionTypeNode(inputTypes, returnType), 0, 0);
        }

        private static FunctionNode GetFunctionNode(string name, ElementNode elementNode, ExpressionNode expr, string parameter, TypeEnum inputType, TypeEnum returnType)
        {
            return GetFunctionNode(name, elementNode, expr, new List<string>() { parameter }, new List<TypeEnum>() { inputType }, returnType);
        }

        private static FunctionNode GetFunctionNode(string name, ElementNode elementNode, ExpressionNode expr, List<string> parameters, List<TypeEnum> inputTypes, TypeEnum returnType)
        {
            return new FunctionNode(name, GetCondition(elementNode, expr), parameters, GetFunctionTypeNode(inputTypes, returnType), 0, 0);
        }

        private static FunctionTypeNode GetFunctionTypeNode(List<TypeEnum> inputTypes, TypeEnum returnType)
        {
            return new FunctionTypeNode(GetTypeNode(returnType), inputTypes.ConvertAll(x => GetTypeNode(x)), 0, 0);
        }

        private static TypeNode GetTypeNode(TypeEnum type)
        {
            return new TypeNode(type, 0, 0);
        }

        private static ConditionNode GetCondition(ExpressionNode expr)
        {
            return new ConditionNode(expr, 0, 0);
        }

        private static ConditionNode GetCondition(ElementNode elementNode, ExpressionNode expr)
        {
            return new ConditionNode(new List<ElementNode> { elementNode }, null, expr, 0, 0);
        }

        private static BooleanLiteralExpression GetBoolLit(bool v)
        {
            return new BooleanLiteralExpression(v, 0, 0);
        }

        private static ExpressionNode GetModExpr(AdditionExpression dividend, IdentifierExpression divisor)
        {
            return new ModuloExpression(dividend, divisor, 0, 0);
        }

        private static AdditionExpression GetAddNode(ExpressionNode left, ExpressionNode right)
        {
            return new AdditionExpression(left, right, 0, 0);
        }

        private static IdentifierExpression GetIdentifierExpr(string id)
        {
            return new IdentifierExpression(id, 0, 0);
        }

        private static IntegerLiteralExpression GetIntLit(int v)
        {
            return new IntegerLiteralExpression(v, 0, 0);
        }

        private static ExpressionNode GetElementExpr(ExpressionNode expr)
        {
            return new ElementExpression(new List<ExpressionNode>() { expr }, 0, 0);
        }

        private static ElementNode GetElementNode(string elementId, string index)
        {
            return new ElementNode(elementId, new List<string>() { index }, 0, 0);
        }

        private static SetExpression GetSetExpr(int n)
        {
            return new SetExpression(GetElementNode("x", "i"), new List<BoundNode>() { GetBound("i", 0, n) }, GetBoolLit(true), 0, 0);
        }

        private static BoundNode GetBound(string id, int start, int end)
        {
            return new BoundNode(id, GetIntLit(start), GetIntLit(end - 1), 0, 0);
        }

        public static AST GetAstSkeleton()
        {
            List<FunctionNode> functionNodes = new List<FunctionNode>();
            List<ExportNode> exportNodes = new List<ExportNode>();
            return new AST(functionNodes, exportNodes, 0, 0);
        }

        public static int AddFunctionNode(AST ast, FunctionNode functionNode)
        {
            var res = ast.Functions.Count;
            ast.Functions.Add(functionNode);
            return res;
        }

        public static void AddExportNode(AST ast, ExportNode exportNode)
        {
            ast.Exports.Add(exportNode);
        }

        internal static ReferenceHandler GetReferenceHandler()
        {
            ReferenceHelper referenceHelper = new ReferenceHelper();
            return new ReferenceHandler(referenceHelper, false);
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
    public class ReferenceHandlerToTypeCheckerTests
    {
        // Function Ref
        // Parameter Ref
        // Anonym Function

        [TestMethod]
        public void ParameterRef_src_e()
        {
            var ast = Utilities.GetMultiGraphExample(2);

            var refHandler = Utilities.GetReferenceHandler();
            var typeChecker = Utilities.GetTypeChecker();
            refHandler.InsertReferences(ast);
            typeChecker.CheckTypes(ast);
         
            var res = (IdentifierExpression)ast.Functions[2].Conditions[0].ReturnExpression;
            Assert.IsTrue(res.IsLocal);
            Assert.AreEqual(0, res.Reference);
        }
    }
}
