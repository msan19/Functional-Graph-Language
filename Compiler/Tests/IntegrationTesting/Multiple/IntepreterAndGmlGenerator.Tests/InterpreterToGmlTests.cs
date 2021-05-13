using ASTLib;
using ASTLib.Nodes;
using ASTLib.Nodes.ExpressionNodes;
using ASTLib.Nodes.TypeNodes;
using FileGeneratorLib;
using InterpreterLib;
using InterpreterLib.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using InterpBooleanHelper = InterpreterLib.Helpers.BooleanHelper;
using InterpreterSetHelper = InterpreterLib.Helpers.SetHelper;

namespace IntepreterAndGmlGenerator.Tests
{
    public static class Utilities
    {
        public static AST GetMultiGraphExample(int n)
        {
            var ast = GetAstSkeleton();

            var vertexSetFunc = GetFunctionNode("vertexSet", GetSetExpr(n), "n", TypeEnum.Integer, TypeEnum.Set);
            var edgeSet = GetSetExpr(n);
            AddFunctionNode(ast, vertexSetFunc);

            //var add = GetAddNode(GetIdentifierExpr(id), GetIntLit(1));
            //var mod = GetModExpr(add, GetIdentifierExpr(nId));
            //var dst = GetFunctionNode("edgeFunc", GetElementNode(elementId, id), GetElementExpr(mod), new List<string>() { "e", "n" }, new List<TypeEnum>() { TypeEnum.Element, TypeEnum.Integer }, TypeEnum.Element);
            //var src = GetFunctionNode("edgeFunc", GetIdentifierExpr(elementId), "e", TypeEnum.Element, TypeEnum.Element);
            //AddFunctionNode(ast, dst);
            //AddFunctionNode(ast, src);


            //var vLabel = GetFunctionNode("vLabel", GetElementNode(elementId, id), GetAddNode(GetStringLit("Vertex: "), GetIdentifierExpr(id)), "e", TypeEnum.Element, TypeEnum.String);
            //var eLabel = GetFunctionNode("eLabel", GetElementNode(elementId, id), GetAddNode(GetStringLit("Edge: "), GetIdentifierExpr(id)), "e", TypeEnum.Element, TypeEnum.String);
            //AddFunctionNode(ast, vLabel);
            //AddFunctionNode(ast, eLabel);

            //var anonymSrc = GetAnonymFunc(GetFunctionCall("edgeFunc", new List<string>() { "e" }), "e", TypeEnum.Element);
            //var anonymDst = GetAnonymFunc(GetFunctionCall("edgeFunc", new List<string>() { "e", "n" }), "e", TypeEnum.Element);
            //var graph = GetGraphExpr(GetFunctionCall("vertexSet", "n"), edgeSet, anonymSrc, anonymDst);
            //var graphFunc = GetFunctionNode("graphFunc", graph, "n", TypeEnum.Integer, TypeEnum.Graph);
            //AddFunctionNode(ast, graphFunc);

            //var export = GetExportNode(GetFunctionCall("graphFunc", GetIntLit(n)), GetStringLit("MultiIntegration"), GetIdentifierExpr("vLabel"), GetIdentifierExpr("eLabel"));
            //AddExportNode(ast, export);

            return ast;
        }

        private static FunctionNode GetFunctionNode(string name, ExpressionNode expr, string parameter, TypeEnum inputType, TypeEnum returnType)
        {
            return GetFunctionNode(name, expr, new List<string>() { parameter }, new List<TypeEnum>() { inputType }, returnType);
        }

        private static FunctionNode GetFunctionNode(string name, ExpressionNode expr, List<string> parameters, List<TypeEnum> inputTypes, TypeEnum returnType)
        {
            return new FunctionNode(name, GetCondition(expr), parameters, GetFunctionTypeNode(inputTypes, returnType), 0, 0);
        }

        private static ConditionNode GetCondition(ExpressionNode expr)
        {
            return new ConditionNode(expr, 0, 0);
        }

        private static FunctionTypeNode GetFunctionTypeNode(List<TypeEnum> inputTypes, TypeEnum returnType)
        {
            return new FunctionTypeNode(GetTypeNode(returnType), inputTypes.ConvertAll(x => GetTypeNode(x)), 0, 0);
        }

        private static TypeNode GetTypeNode(TypeEnum type)
        {
            return new TypeNode(type, 0, 0);
        }

        private static SetExpression GetSetExpr(int n)
        {
            return new SetExpression(GetElementNode("x", "i"), new List<BoundNode>() { GetBound("i", 0, n) }, GetBoolLit(true), 0, 0);
        }

        private static ElementNode GetElementNode(string elementId, string index)
        {
            return new ElementNode(elementId, new List<string>() { index }, 0, 0);
        }

        private static BoundNode GetBound(string id, int start, int end)
        {
            return new BoundNode(id, GetIntLit(start), GetIntLit(end - 1), 0, 0);
        }

        private static BooleanLiteralExpression GetBoolLit(bool v)
        {
            return new BooleanLiteralExpression(v, 0, 0);
        }

        private static IntegerLiteralExpression GetIntLit(int v)
        {
            return new IntegerLiteralExpression(v, 0, 0);
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

        public static AST GetAstSkeleton()
        {
            List<FunctionNode> functionNodes = new List<FunctionNode>();
            List<ExportNode> exportNodes = new List<ExportNode>();
            return new AST(functionNodes, exportNodes, 0, 0);
        }

        internal static Interpreter GetInterpreter()
        {
            return new Interpreter(new GenericHelper(),
                                           new FunctionHelper(),
                                           new IntegerHelper(),
                                           new RealHelper(),
                                           new InterpBooleanHelper(),
                                           new InterpreterSetHelper(),
                                           new ElementHelper(),
                                           new StringHelper(),
                                           new GraphHelper(),
                                           false);
        }

        internal static GmlGenerator GetGmlGenerator()
        {
            return new GmlGenerator();
        }
    }


    [TestClass]
    public class InterpreterToGmlTests
    {
        // Everything

        [TestMethod]
        public void TestMethod1()
        {
            var ast = Utilities.GetMultiGraphExample(2);

            var interpreter = Utilities.GetInterpreter();
            var gmlGenerator = Utilities.GetGmlGenerator();
            var interpretRes = interpreter.Interpret(ast);
            var gml = gmlGenerator.Generate(interpretRes)[0];

            Assert.AreEqual("MultiIntegration", gml.FileName);
        }
    }
}
