using System;
using System.Collections.Generic;
using ASTLib;
using ASTLib.Nodes;
using ASTLib.Nodes.ExpressionNodes;
using ASTLib.Nodes.ExpressionNodes.OperationNodes;
using ASTLib.Nodes.TypeNodes;
using InterpreterLib;
using InterpreterLib.Helpers;
using InterpBooleanHelper = InterpreterLib.Helpers.BooleanHelper;
using InterpreterSetHelper = InterpreterLib.Helpers.SetHelper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ASTLib.Objects;

namespace SingleCompIntepreterLib.Tests
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

        public static void AddExportNode(AST ast, ExportNode exportNode)
        {
            ast.Exports.Add(exportNode);
        }

        public static SetExpression GetSet(int indexCount, List<Tuple<int, int>> bounds)
        {

            ElementNode elementNode = new ElementNode(null, GetListOfStrings(indexCount), 0, 0);
            List<BoundNode> boundNodes = new List<BoundNode>();

            foreach (Tuple<int, int> bound in bounds)
            {
                boundNodes.Add(new BoundNode(null, GetIntLit(bound.Item1), GetIntLit(bound.Item2), 0, 0));
            }
            return new SetExpression(elementNode, boundNodes, new BooleanLiteralExpression(true, 0, 0), 0, 0);
        }

        private static List<string> GetListOfStrings(int indexCount)
        {
            List<string> list = new List<string>(indexCount);
            for (int i = 0; i < indexCount; i++)
            {
                list.Add("");
            }
            return list;
        }

        private static IntegerLiteralExpression GetIntLit(int value)
        {
            return new IntegerLiteralExpression(value, 0, 0);
        }

        internal static ElementExpression GetElementExpression(int reference)
        {
            return GetElementExpression(new List<int>() { reference });
        }

        internal static ElementExpression GetElementExpression(List<int> references)
        {
            List<ExpressionNode> exprNodes = new List<ExpressionNode>();

            foreach (int reference in references)
            {
                exprNodes.Add(GetIdExpression(reference, true));
            }

            return new ElementExpression(exprNodes, 0, 0);
        }
        internal static ElementExpression GetElementExpression(int reference, int offset)
        {
            return GetElementExpression(new List<int>() { reference }, new List<int>() { offset });
        }

        internal static ElementExpression GetElementExpression(List<int> references, List<int> offset)
        {
            List<ExpressionNode> exprNodes = new List<ExpressionNode>();

            for (int i = 0; i < references.Count; i++)
            {
                exprNodes.Add(GetAdditionNode(GetIdExpression(references[i], true), GetIntLit(offset[i])));
            }

            return new ElementExpression(exprNodes, 0, 0);
        }

        private static ExpressionNode GetAdditionNode(ExpressionNode left, IntegerLiteralExpression right)
        {
            return new AdditionExpression(left, right, 0, 0);
        }

        internal static IdentifierExpression GetIdExpression(int reference, bool isLocal)
        {
            IdentifierExpression idExpr = new IdentifierExpression(null, 0, 0);
            idExpr.Reference = reference;
            idExpr.IsLocal = isLocal;
            return idExpr;
        }

        internal static ConditionNode GetConditionNode(ElementExpression elementExpr, int indexCount, int reference)
        {
            ElementNode elementNode = GetElementNode(GetListOfStrings(indexCount), reference);
            return new ConditionNode(new List<ElementNode>() { elementNode }, null, elementExpr, 0, 0);
        }

        private static ElementNode GetElementNode(List<string> indexIdentifiers, int reference)
        {
            ElementNode elementNode = new ElementNode(null, indexIdentifiers, 0, 0);
            elementNode.Reference = reference;
            return elementNode;
        }

        internal static FunctionNode GetElementFunctionNode(ConditionNode condNode)
        {
            return new FunctionNode(null, condNode, new List<string>(), GetFunctionType(TypeEnum.Element, TypeEnum.Element), 0, 0);
        }

        private static FunctionTypeNode GetFunctionType(TypeEnum parameterType, TypeEnum returnType)
        {
            return new FunctionTypeNode(GetType(returnType), new List<TypeNode>() { GetType(parameterType) }, 0, 0);
        }

        private static TypeNode GetType(TypeEnum type)
        {
            return new TypeNode(type, 0, 0);
        }

        internal static ExportNode GetExportNode(GraphExpression graphExpr)
        {
            return new ExportNode(graphExpr, GetStringLit("navn"), new List<ExpressionNode>(), new List<ExpressionNode>(), 0, 0);
        }

        internal static ExportNode GetExportNode(GraphExpression graphExpr, string filename)
        {
            return new ExportNode(graphExpr, GetStringLit(filename), new List<ExpressionNode>(), new List<ExpressionNode>(), 0, 0);
        }

        private static StringLiteralExpression GetStringLit(string value)
        {
            return new StringLiteralExpression(value, 0, 0);
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

        internal static void GetGraphExpression(int vertexCount, int edgeCount, out GraphExpression graphExpr, out FunctionNode src, out FunctionNode dst)
        {
            SetExpression vertexSetExpr = GetSet(1, new List<Tuple<int, int>>() { new Tuple<int, int>(1, vertexCount) });
            SetExpression edgeSetExpr = GetSet(1, new List<Tuple<int, int>>() { new Tuple<int, int>(1, edgeCount) });
            IdentifierExpression srcId = GetIdExpression(0, false);
            IdentifierExpression dstId = GetIdExpression(1, false);
            graphExpr = new GraphExpression(vertexSetExpr, edgeSetExpr, srcId, dstId, 0, 0);
            ElementExpression srcElementExpr = GetElementExpression(1);
            ConditionNode srcCondNode = GetConditionNode(srcElementExpr, 1, 0);
            src = GetElementFunctionNode(srcCondNode);
            ElementExpression dstElementExpr = GetElementExpression(1, 1);
            ConditionNode dstCondNode = GetConditionNode(dstElementExpr, 1, 0);
            dst = GetElementFunctionNode(dstCondNode);
        }

        internal static void GetGraphExpression(int vertexCount, int edgeCount, int indexCount, out GraphExpression graphExpr, out FunctionNode src, out FunctionNode dst)
        {
            SetExpression vertexSetExpr = GetSet(indexCount, GetBoundList(indexCount, 1, vertexCount));
            SetExpression edgeSetExpr = GetSet(indexCount, GetBoundList(indexCount, 1, edgeCount));
            IdentifierExpression srcId = GetIdExpression(0, false);
            IdentifierExpression dstId = GetIdExpression(1, false);
            graphExpr = new GraphExpression(vertexSetExpr, edgeSetExpr, srcId, dstId, 0, 0);
            ElementExpression srcElementExpr = GetElementExpression(GetIntList(1, indexCount));
            ConditionNode srcCondNode = GetConditionNode(srcElementExpr, indexCount, 0);
            src = GetElementFunctionNode(srcCondNode);
            ElementExpression dstElementExpr = GetElementExpression(GetIntList(1, indexCount), GetIntListWithValue(indexCount, 1));
            ConditionNode dstCondNode = GetConditionNode(dstElementExpr, indexCount, 0);
            dst = GetElementFunctionNode(dstCondNode);
        }

        private static List<Tuple<int, int>> GetBoundList(int indexCount, int startIndex, int endIndex)
        {
            List<Tuple<int, int>> list = new List<Tuple<int, int>>();
            for (int i = 0; i < indexCount; i++)
            {
                list.Add(new Tuple<int, int>(startIndex, endIndex));
            }
            return list;
        }

        private static List<int> GetIntList(int startIndex, int indexCount)
        {
            List<int> list = new List<int>();
            for (int i = startIndex; i <= indexCount; i++)
            {
                list.Add(i);
            }
            return list;
        }

        private static List<int> GetIntListWithValue(int indexCount, int value)
        {
            List<int> list = new List<int>();
            for (int i = 0; i < indexCount; i++)
            {
                list.Add(value);
            }
            return list;
        }
    }

    [TestClass]
    public class InterpreterTests
    {
        [DataRow(1)]
        [DataRow(2)]
        [DataRow(10)]
        [TestMethod]
        public void Interpret_GraphWithXVertices_CorrectVertexCount(int vertexCount)
        {
            int expected = vertexCount;
            int edgeCount = 0;
            GraphExpression graphExpr;
            FunctionNode src, dst;

            Utilities.GetGraphExpression(vertexCount, edgeCount, out graphExpr, out src, out dst);

            AST ast = Utilities.GetAstSkeleton();
            Utilities.AddFunctionNode(ast, src);
            Utilities.AddFunctionNode(ast, dst);
            Utilities.AddExportNode(ast, Utilities.GetExportNode(graphExpr));

            Interpreter interpreter = Utilities.GetInterpreter();
            List<LabelGraph> res = interpreter.Interpret(ast);

            Assert.AreEqual(expected, res[0].VertexCount);
        }

        [DataRow(1)]
        [DataRow(2)]
        [DataRow(10)]
        [TestMethod]
        public void Interpret_GraphWithXEdges_CorrectEdgeCount(int edgeCount)
        {
            int expected = edgeCount;
            int vertexCount = edgeCount + 1;
            GraphExpression graphExpr;
            FunctionNode src, dst;

            Utilities.GetGraphExpression(vertexCount, edgeCount, out graphExpr, out src, out dst);

            AST ast = Utilities.GetAstSkeleton();
            Utilities.AddFunctionNode(ast, src);
            Utilities.AddFunctionNode(ast, dst);
            Utilities.AddExportNode(ast, Utilities.GetExportNode(graphExpr));

            Interpreter interpreter = Utilities.GetInterpreter();
            List<LabelGraph> res = interpreter.Interpret(ast);

            Assert.AreEqual(expected, res[0].SrcList.Count);
            Assert.AreEqual(expected, res[0].DstList.Count);
        }

        [DataRow("name1")]
        [DataRow("name2")]
        [TestMethod]
        public void Interpret_GraphWithXFilename_CorrectFilename(string filename)
        {
            string expected = filename;
            int vertexCount = 2;
            int edgeCount = 1;
            GraphExpression graphExpr;
            FunctionNode src, dst;

            Utilities.GetGraphExpression(vertexCount, edgeCount, out graphExpr, out src, out dst);

            AST ast = Utilities.GetAstSkeleton();
            Utilities.AddFunctionNode(ast, src);
            Utilities.AddFunctionNode(ast, dst);
            Utilities.AddExportNode(ast, Utilities.GetExportNode(graphExpr, filename));

            Interpreter interpreter = Utilities.GetInterpreter();
            List<LabelGraph> res = interpreter.Interpret(ast);

            Assert.AreEqual(expected, res[0].FileName);
        }

        [DataRow(1)]
        [DataRow(2)]
        [DataRow(10)]
        [TestMethod]
        public void Interpret_XExportedGraphs_CorrectLabelGraphCount(int graphCount)
        {
            int expected = graphCount;
            int vertexCount = 2;
            int edgeCount = 1;
            GraphExpression graphExpr;
            FunctionNode src = null;
            FunctionNode dst = null;

            Utilities.GetGraphExpression(vertexCount, edgeCount, out graphExpr, out src, out dst);
            AST ast = Utilities.GetAstSkeleton();
            Utilities.AddFunctionNode(ast, src);
            Utilities.AddFunctionNode(ast, dst);

            for (int i = 0; i < graphCount; i++)
            {
                Utilities.AddExportNode(ast, Utilities.GetExportNode(graphExpr));
            }

            Interpreter interpreter = Utilities.GetInterpreter();
            List<LabelGraph> res = interpreter.Interpret(ast);

            Assert.AreEqual(expected, res.Count);
        }

        [DataRow(1)]
        [DataRow(2)]
        [DataRow(10)]
        [TestMethod]
        public void Interpret_XVertexIndices_CorrectVertexCount(int indexCount)
        {
            int vertexCount = 2;
            int edgeCount = 0;
            int expected = (int) Math.Pow(vertexCount, indexCount);
            GraphExpression graphExpr;
            FunctionNode src = null;
            FunctionNode dst = null;

            Utilities.GetGraphExpression(vertexCount, edgeCount, indexCount, out graphExpr, out src, out dst);
            AST ast = Utilities.GetAstSkeleton();
            Utilities.AddFunctionNode(ast, src);
            Utilities.AddFunctionNode(ast, dst);
            Utilities.AddExportNode(ast, Utilities.GetExportNode(graphExpr));

            Interpreter interpreter = Utilities.GetInterpreter();
            List<LabelGraph> res = interpreter.Interpret(ast);

            Assert.AreEqual(expected, res[0].VertexCount);
        }

        [DataRow(1)]
        [DataRow(2)]
        [DataRow(3)]
        [TestMethod]
        public void Interpret_XEdgeIndices_CorrectEdgeCount(int indexCount)
        {
            int edgeCount = 2;
            int vertexCount = edgeCount + 1;
            int expected = (int)Math.Pow(edgeCount, indexCount);
            GraphExpression graphExpr;
            FunctionNode src = null;
            FunctionNode dst = null;

            Utilities.GetGraphExpression(vertexCount, edgeCount, indexCount, out graphExpr, out src, out dst);
            AST ast = Utilities.GetAstSkeleton();
            Utilities.AddFunctionNode(ast, src);
            Utilities.AddFunctionNode(ast, dst);
            Utilities.AddExportNode(ast, Utilities.GetExportNode(graphExpr));

            Interpreter interpreter = Utilities.GetInterpreter();
            List<LabelGraph> res = interpreter.Interpret(ast);

            Assert.AreEqual(expected, res[0].SrcList.Count);
            Assert.AreEqual(expected, res[0].DstList.Count);
        }
    }
}
