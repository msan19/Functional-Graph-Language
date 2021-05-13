using ASTLib;
using ASTLib.Nodes;
using ASTLib.Nodes.ExpressionNodes;
using ASTLib.Nodes.ExpressionNodes.NumberOperationNodes;
using ASTLib.Nodes.ExpressionNodes.OperationNodes;
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

            var add = GetAddNode(GetIdentifierExpr(id, 1, true), GetIntLit(1));
            var mod = GetModExpr(add, GetIdentifierExpr(nId, 1, true));
            var dst = GetFunctionNode("edgeFunc", GetElementNode(elementId, 0, id), GetElementExpr(mod), new List<string>() { "e", "n" }, new List<TypeEnum>() { TypeEnum.Element, TypeEnum.Integer }, TypeEnum.Element);
            var src = GetFunctionNode("edgeFunc", GetIdentifierExpr(elementId, 0, true), "e", TypeEnum.Element, TypeEnum.Element);
            AddFunctionNode(ast, src);
            AddFunctionNode(ast, dst);


            var vLabel = GetFunctionNode("vLabel", GetElementNode(elementId, 0, id), GetAddNode(GetStringLit("Vertex: "), GetCastFromIntToStringNode(GetIdentifierExpr(id, 1, true))), "e", TypeEnum.Element, TypeEnum.String);
            var eLabel = GetFunctionNode("eLabel", GetElementNode(elementId, 0, id), GetAddNode(GetStringLit("Edge: "), GetCastFromIntToStringNode(GetIdentifierExpr(id, 1, true))), "e", TypeEnum.Element, TypeEnum.String);
            AddFunctionNode(ast, vLabel);
            AddFunctionNode(ast, eLabel);

            var srcCall = GetFunctionCall("edgeFunc", 1, new List<string>() { "e" }, new List<int>() { 1 });
            var dstCall = GetFunctionCall("edgeFunc", 2, new List<string>() { "e", "n" }, new List<int>() { 1, 0 });
            var anonymSrc = GetAnonymFunc(srcCall, 6, "e", TypeEnum.Element);
            var anonymDst = GetAnonymFunc(dstCall, 7, "e", TypeEnum.Element);
            var graph = GetGraphExpr(GetFunctionCall("vertexSet", 0, "n", 0), edgeSet, anonymSrc, anonymDst);
            var graphFunc = GetFunctionNode("graphFunc", graph, "n", TypeEnum.Integer, TypeEnum.Graph);
            AddFunctionNode(ast, graphFunc);

            var export = GetExportNode(GetFunctionCall("graphFunc", 5, GetIntLit(n)), GetStringLit("MultiIntegration"), GetIdentifierExpr("vLabel", 3, false), GetIdentifierExpr("eLabel", 4, false));
            AddExportNode(ast, export);

            // Anonym functions created by the reference handler
            var srcFunc = GetFunctionNode("anonymSrc", srcCall, new List<string>() { "e" }, new List<TypeEnum>() { TypeEnum.Integer, TypeEnum.Element }, TypeEnum.Element);
            AddFunctionNode(ast, srcFunc);
            var dstFunc = GetFunctionNode("anonymDst", dstCall, new List<string>() { "e" }, new List<TypeEnum>() { TypeEnum.Integer, TypeEnum.Element }, TypeEnum.Element);
            AddFunctionNode(ast, dstFunc);

            return ast;
        }

        private static CastFromIntegerExpression GetCastFromIntToStringNode(ExpressionNode expr)
        {
            return new CastFromIntegerExpression(expr, 0, 0);
        }

        private static ExportNode GetExportNode(ExpressionNode exporValue, ExpressionNode fileName, ExpressionNode vLabel, ExpressionNode eLabel)
        {
            return new ExportNode(exporValue, fileName, new List<ExpressionNode>() { vLabel }, new List<ExpressionNode>() { eLabel }, 0, 0);
        }

        private static GraphExpression GetGraphExpr(ExpressionNode vertecies, ExpressionNode edges, ExpressionNode src, ExpressionNode dst)
        {
            return new GraphExpression(vertecies, edges, src, dst, 0, 0);
        }

        private static AnonymousFunctionExpression GetAnonymFunc(ExpressionNode expr, int funcRef, string id, TypeEnum type)
        {
            var res = new AnonymousFunctionExpression(new List<string>() { id }, new List<TypeNode>() { GetTypeNode(type) }, expr, 0, 0);
            res.Reference = funcRef;
            return res;
        }

        private static FunctionCallExpression GetFunctionCall(string funcId, int funcRef, List<string> parameters, List<int> parameterRefs)
        {
            var ids = new List<ExpressionNode>();
            for (int i = 0; i < parameters.Count; i++)
            {
                ids.Add(GetIdentifierExpr(parameters[i], parameterRefs[i], true));
            }

            var res = new FunctionCallExpression(funcId, ids, 0, 0);
            res.GlobalReferences = new List<int>() { funcRef };
            return res;
        }

        private static ExpressionNode GetFunctionCall(string funcId, int funcRef, ExpressionNode input)
        {
            var res = new FunctionCallExpression(funcId, new List<ExpressionNode>() { input }, 0, 0);
            res.GlobalReferences = new List<int>() { funcRef };
            return res;
        }

        private static FunctionCallExpression GetFunctionCall(string funcId, int funcRef, string parameter, int paramRef)
        {
            var res = new FunctionCallExpression(funcId, new List<ExpressionNode>() { GetIdentifierExpr(parameter, paramRef, true) }, 0, 0);
            res.GlobalReferences = new List<int>() { funcRef };
            return res;
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

        private static IdentifierExpression GetIdentifierExpr(string id, int reference, bool isLocal)
        {
            var res = new IdentifierExpression(id, 0, 0);
            res.Reference = reference;
            res.IsLocal = isLocal;
            return res;
        }

        private static IntegerLiteralExpression GetIntLit(int v)
        {
            return new IntegerLiteralExpression(v, 0, 0);
        }

        private static ExpressionNode GetElementExpr(ExpressionNode expr)
        {
            return new ElementExpression(new List<ExpressionNode>() { expr }, 0, 0);
        }

        private static ElementNode GetElementNode(string elementId, int reference, string index)
        {
            var res = new ElementNode(elementId, new List<string>() { index }, 0, 0);
            res.Reference = reference;
            return res;
        }

        private static SetExpression GetSetExpr(int n)
        {
            return new SetExpression(GetElementNode("x", -1, "i"), new List<BoundNode>() { GetBound("i", 0, n) }, GetBoolLit(true), 0, 0);
        }

        private static BoundNode GetBound(string id, int start, int end)
        {
            return new BoundNode(id, GetIntLit(start), GetIntLit(end - 1), 0, 0);
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
}
