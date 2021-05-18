using System.Collections.Generic;
using ASTLib;
using ASTLib.Nodes;
using ASTLib.Nodes.ExpressionNodes;
using ASTLib.Nodes.ExpressionNodes.NumberOperationNodes;
using ASTLib.Nodes.ExpressionNodes.OperationNodes;
using ASTLib.Nodes.TypeNodes;
using InterpreterLib;
using InterpreterLib.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TypeCheckerLib;
using TypeCheckerLib.Helpers;
using TypeBooleanHelper = TypeCheckerLib.Helpers.BooleanHelper;
using TypeSetHelper = TypeCheckerLib.Helpers.SetHelper;
using InterpBooleanHelper = InterpreterLib.Helpers.BooleanHelper;
using InterpreterSetHelper = InterpreterLib.Helpers.SetHelper;
using ASTLib.Objects;

namespace TypeCheckerAndIntepreter.Tests
{
    public static class Utilities
    {
        public static AST GetMultiGraphExample()
        {
            AST ast = GetAstSkeleton();

            FunctionCallExpression vertices = GetFunctionCallExpression("vertexSet", GetIdentifierExpression("n", 0, true), 1);
            SetExpression edges = GetSetExpression(GetElementNode("x", "i", -1), GetBoundNode("i", 0, GetAdditionExpression(GetIdentifierExpression("n", 0, true), -1)));
            AnonymousFunctionExpression src = GetAnonymousFunctionExpression("e", TypeEnum.Element, GetFunctionCallExpression("edgeFunc", GetIdentifierExpression("e", 1, true), 2));
            AnonymousFunctionExpression dst = GetAnonymousFunctionExpression("e", TypeEnum.Element, GetFunctionCallExpression("edgeFunc", new List<ExpressionNode>() { GetIdentifierExpression("e", 1, true), GetIdentifierExpression("n", 0, true) }, 3));
            GraphExpression graphExpression = GetGraphExpression(vertices, edges, src, dst);
            FunctionNode graphFunc = GetFunctionNode(graphExpression, "graphFunc", "n", TypeEnum.Integer, TypeEnum.Graph);
            AddFunctionNode(ast, graphFunc);

            FunctionNode vertexSet = GetFunctionNode(GetSetExpression(GetElementNode("x", "i", -1), GetBoundNode("i", 0, GetAdditionExpression(GetIdentifierExpression("n", 0, true), -1))), "vertexSet", "n", TypeEnum.Integer, TypeEnum.Set);
            AddFunctionNode(ast, vertexSet);

            FunctionNode edgeFunc = GetFunctionNode(GetIdentifierExpression("e", 0, true), "edgeFunc", "e", TypeEnum.Element, TypeEnum.Element);
            AddFunctionNode(ast, edgeFunc);

            FunctionNode edgeFunc2 = GetFunctionNode(new List<ConditionNode>() { GetConditionNode(new List<ElementNode>() { GetElementNode("e", "i", 0) }, null, GetElementExpression(GetModuloExpression(GetAdditionExpression(GetIdentifierExpression("i", 2, true), 1), GetIdentifierExpression("n", 1, true)))) }, "edgeFunc", new List<string>() { "e", "n" }, new List<TypeEnum>() { TypeEnum.Element, TypeEnum.Integer }, TypeEnum.Element);
            AddFunctionNode(ast, edgeFunc2);

            FunctionNode vLabel = GetFunctionNode(new List<ConditionNode>() { GetConditionNode(new List<ElementNode>() { GetElementNode("e", "i", 0) }, null, GetAdditionExpression(GetStringLiteralExpression("Vertex: "), GetIdentifierExpression("i", 1, true))) }, "vLabel", new List<string>() { "e" }, new List<TypeEnum>() { TypeEnum.Element }, TypeEnum.String);
            AddFunctionNode(ast, vLabel);

            FunctionNode eLabel = GetFunctionNode(new List<ConditionNode>() { GetConditionNode(new List<ElementNode>() { GetElementNode("e", "i", 0) }, null, GetAdditionExpression(GetStringLiteralExpression("Edge: "), GetIdentifierExpression("i", 1, true))) }, "eLabel", new List<string>() { "e" }, new List<TypeEnum>() { TypeEnum.Element }, TypeEnum.String);
            AddFunctionNode(ast, eLabel);

            ExportNode export = GetExportNode(GetFunctionCallExpression("graphFunc", GetIntegerLiteralExpression(2), 0), "MultiIntegration", GetIdentifierExpression("vLabel", 4, false), GetIdentifierExpression("eLabel", 5, false));
            AddExportNode(ast, export);

            return ast;
        }

        private static ExportNode GetExportNode(ExpressionNode exportValue, string fileName, ExpressionNode vertexLabel, ExpressionNode edgeLabel)
        {
            return new ExportNode(exportValue, GetStringLiteralExpression(fileName), new List<ExpressionNode>() { vertexLabel }, new List<ExpressionNode>() { edgeLabel }, 0, 0);
        }

        private static StringLiteralExpression GetStringLiteralExpression(string value)
        {
            return new StringLiteralExpression(value, 0, 0);
        }

        private static ModuloExpression GetModuloExpression(ExpressionNode dividend, ExpressionNode divisor)
        {
            return new ModuloExpression(dividend, divisor, 0, 0);
        }

        private static ElementExpression GetElementExpression(ExpressionNode child)
        {
            return new ElementExpression(new List<ExpressionNode>() { child }, 0, 0);
        }

        private static AnonymousFunctionExpression GetAnonymousFunctionExpression(string identifier, TypeEnum type, ExpressionNode returnValue)
        {
            return new AnonymousFunctionExpression(new List<string>() { identifier }, new List<TypeNode>() { GetTypeNode(type) }, returnValue, 0, 0);
        }

        private static AdditionExpression GetAdditionExpression(ExpressionNode leftExpression, int value)
        {
            return new AdditionExpression(leftExpression, GetIntegerLiteralExpression(value), 0, 0);
        }

        private static AdditionExpression GetAdditionExpression(ExpressionNode leftExpression, ExpressionNode rightExpression)
        {
            return new AdditionExpression(leftExpression, rightExpression, 0, 0);
        }

        private static IntegerLiteralExpression GetIntegerLiteralExpression(int value)
        {
            return new IntegerLiteralExpression(value, 0, 0);
        }

        private static BoundNode GetBoundNode(string identifier, int smallestValue, ExpressionNode largestValue)
        {
            return new BoundNode(identifier, GetIntegerLiteralExpression(smallestValue), largestValue, 0, 0);
        }

        private static ElementNode GetElementNode(string elementName, string identifier, int reference)
        {
            ElementNode elementNode = new ElementNode(elementName, new List<string>() { identifier }, 0, 0);
            elementNode.Reference = reference;
            return elementNode;
        }

        private static SetExpression GetSetExpression(ElementNode element, BoundNode bound)
        {
            return new SetExpression(element, new List<BoundNode>() { bound }, GetBooleanLiteralExpression(true), 0, 0);
        }

        private static BooleanLiteralExpression GetBooleanLiteralExpression(bool value)
        {
            return new BooleanLiteralExpression(value, 0, 0);
        }

        private static IdentifierExpression GetIdentifierExpression(string id, int reference, bool isLocal)
        {
            IdentifierExpression identifierExpression = new IdentifierExpression(id, 0, 0);
            identifierExpression.Reference = reference;
            identifierExpression.IsLocal = isLocal;
            return identifierExpression;
        }

        private static FunctionCallExpression GetFunctionCallExpression(string identifier, ExpressionNode child, int globalReference)
        {
            return GetFunctionCallExpression(identifier, new List<ExpressionNode>() { child }, new List<int>() { globalReference });
        }

        private static FunctionCallExpression GetFunctionCallExpression(string identifier, List<ExpressionNode> children, int globalReference)
        {

            return GetFunctionCallExpression(identifier, children, new List<int>() { globalReference });
        }

        private static FunctionCallExpression GetFunctionCallExpression(string identifier, List<ExpressionNode> children, List<int> globalReferences)
        {

            FunctionCallExpression functionCallExpression = new FunctionCallExpression(identifier, children, 0, 0);
            functionCallExpression.GlobalReferences = globalReferences;
            return functionCallExpression;
        }

        private static GraphExpression GetGraphExpression(ExpressionNode vertices, ExpressionNode edges, ExpressionNode src, ExpressionNode dst)
        {
            return new GraphExpression(vertices, edges, src, dst, 0, 0);
        }

        private static ConditionNode GetConditionNode(ExpressionNode returnExpression)
        {
            return new ConditionNode(returnExpression, 0, 0);
        }

        private static ElementNode GetElementNode(string elementName, List<string> identifiers)
        {
            return new ElementNode(elementName, identifiers, 0, 0);
        }

        private static ConditionNode GetConditionNode(List<ElementNode> elements, ExpressionNode conditionExpression, ExpressionNode returnExpression)
        {
            return new ConditionNode(elements, conditionExpression, returnExpression, 0, 0);
        }

        private static FunctionNode GetFunctionNode(ExpressionNode returnExpression, string identifier, string parameterIdentifier, TypeEnum parameterType, TypeEnum returnType)
        {
            return GetFunctionNode(new List<ConditionNode>() { GetConditionNode(returnExpression) }, identifier, new List<string>() { parameterIdentifier }, new List<TypeEnum>() { parameterType }, returnType);
        }

        private static FunctionNode GetFunctionNode(List<ConditionNode> conditions, string identifier, List<string> parameterIdentifiers, List<TypeEnum> parameterTypes, TypeEnum returnType)
        {
            return new FunctionNode(conditions, identifier, parameterIdentifiers, GetFunctionTypeNode(parameterTypes, returnType), 0, 0);
        }

        private static FunctionTypeNode GetFunctionTypeNode(List<TypeEnum> parameterTypes, TypeEnum returnType)
        {
            return new FunctionTypeNode(GetTypeNode(returnType), parameterTypes.ConvertAll(x => GetTypeNode(x)), 0, 0);
        }

        private static TypeNode GetTypeNode(TypeEnum type)
        {
            return new TypeNode(type, 0, 0);
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

        internal static TypeChecker GetTypeChecker()
        {
            return new TypeChecker(new DeclarationHelper(), new NumberHelper(), new CommonOperatorHelper(), new TypeBooleanHelper(), new TypeSetHelper());
        }

        internal static Interpreter GetInterpreter()
        {
            return new Interpreter(new GenericHelper(), new FunctionHelper(), new IntegerHelper(), new RealHelper(), new InterpBooleanHelper(), new InterpreterSetHelper(), new ElementHelper(), new StringHelper(), new GraphHelper(), false);
        }
    }

    [TestClass]
    public class TypeCheckerToInterpreterTests
    {
        [TestMethod]
        public void Overloading_TwoFunctionsWithSameIdButDifferentParameters_CorrectSrcList()
        {
            List<int> expected = new List<int>() { 0, 1 };
            AST ast = Utilities.GetMultiGraphExample();
            TypeChecker typeChecker = Utilities.GetTypeChecker();
            Interpreter interpreter = Utilities.GetInterpreter();

            typeChecker.CheckTypes(ast);
            List<LabelGraph> labelGraphs = interpreter.Interpret(ast);

            CollectionAssert.AreEqual(expected, labelGraphs[0].SrcList);
        }

        [TestMethod]
        public void Overloading_TwoFunctionsWithSameIdButDifferentParameters_CorrectDstList()
        {
            List<int> expected = new List<int>() { 1, 0 };
            AST ast = Utilities.GetMultiGraphExample();
            TypeChecker typeChecker = Utilities.GetTypeChecker();
            Interpreter interpreter = Utilities.GetInterpreter();

            typeChecker.CheckTypes(ast);
            List<LabelGraph> labelGraphs = interpreter.Interpret(ast);

            CollectionAssert.AreEqual(expected, labelGraphs[0].DstList);
        }

        [TestMethod]
        public void Casting_StringIntegerCast_CorrectVertexLabel()
        {
            string expected = "Vertex: 0";
            AST ast = Utilities.GetMultiGraphExample();
            TypeChecker typeChecker = Utilities.GetTypeChecker();
            Interpreter interpreter = Utilities.GetInterpreter();

            typeChecker.CheckTypes(ast);
            List<LabelGraph> labelGraphs = interpreter.Interpret(ast);

            Assert.AreEqual(expected, labelGraphs[0].VertexLabels[0, 0]);
        }
    }
}
