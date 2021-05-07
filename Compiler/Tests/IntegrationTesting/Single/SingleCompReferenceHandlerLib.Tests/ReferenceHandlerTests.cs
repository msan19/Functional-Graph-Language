using System.Collections.Generic;
using System.Linq;
using ASTLib;
using ASTLib.Exceptions;
using ASTLib.Nodes;
using ASTLib.Nodes.ExpressionNodes;
using ASTLib.Nodes.TypeNodes;
using ASTLib.Objects;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ReferenceHandlerLib;

namespace SingleCompReferenceHandlerLib.Tests
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

        public static ExportNode GetExportNode(ExpressionNode exprNode)
        {
            StringLiteralExpression stringLitExpr = new StringLiteralExpression("FileName", 0, 0);
            return new ExportNode(exprNode, stringLitExpr, new List<ExpressionNode>(), 
                new List<ExpressionNode>(), 0, 0);
        }

        public static FunctionNode GetFunctionNode(string id)
        {
           
            IntegerLiteralExpression intLitExpr = new IntegerLiteralExpression(0, 0, 0);
            ConditionNode conditionNode = GetConditionNode(intLitExpr);
            FunctionTypeNode functionTypeNode = GetFunctionTypeNode();
            List<string> paramIDs = new List<string>();
            return new FunctionNode(id, conditionNode, paramIDs, functionTypeNode, 0, 0);
        }
        public static FunctionNode GetFunctionNode(string id, List<string> parameters, ConditionNode condition)
        {

            FunctionTypeNode functionTypeNode = GetFunctionTypeNode(parameters.Count, TypeEnum.Function);
            return new FunctionNode(id, condition, parameters, functionTypeNode, 0, 0);
        }

        internal static FunctionNode GetFunctionNode(string id, ConditionNode condition)
        {
            FunctionTypeNode functionTypeNode = GetFunctionTypeNode();
            return new FunctionNode(id, condition, new List<string>(), functionTypeNode, 0, 0);
        }

        internal static FunctionTypeNode GetFunctionTypeNode(int count, TypeEnum type)
        {
            var types = new List<TypeNode>();
            for (int i = 0; i < count; i++)
                types.Add(GetTypeNode(type));
            return new FunctionTypeNode(GetTypeNode(), types, 0, 0);
        }

        internal static TypeNode GetTypeNode(TypeEnum type)
        {
            return new TypeNode(type, 0, 0);
        }

        internal static FunctionTypeNode GetFunctionTypeNode()
        {
            return new FunctionTypeNode(GetTypeNode(), new List<TypeNode>(), 0, 0);
        }

        private static TypeNode GetTypeNode()
        {
            return new TypeNode(TypeEnum.Integer, 0, 0);
        }

        internal static ConditionNode GetConditionNode(ExpressionNode exprNode)
        {
            return new ConditionNode(exprNode, 0, 0);
        }

        internal static ConditionNode GetConditionNode(SetExpression setExpr, IdentifierExpression returnExpr)
        {
            return new ConditionNode(setExpr, returnExpr, 0, 0);
        }

        internal static FunctionCallExpression GetFunctionCallExpression(string id)
        {
            return new FunctionCallExpression(id, new List<ExpressionNode>(), 0, 0);
        }
        
        public static ReferenceHandler GetReferenceHandler()
        {
            ReferenceHelper referenceHelper = new ReferenceHelper();
            return new ReferenceHandler(referenceHelper, false);
        }

        internal static ConditionNode GetConditionNode(string[] elements)
        {
            return new ConditionNode(GetElements(elements), GetBoolLit(true), GetIntLit(), 0, 0);
        }

        internal static ConditionNode GetConditionNode(string[] elements, string id)
        {
            return new ConditionNode(GetElements(elements), GetIdentifierExpr(id), GetIntLit(), 0, 0);
        }

        internal static List<ElementNode> GetElements(string[] elements)
        {
            var res = new List<ElementNode>();
            foreach (var e in elements)
                res.Add(GetElementNode(e));
            return res;
        }

        internal static ElementNode GetElementNode(string name)
        {
            return new ElementNode(name, new List<string>(), 0, 0);
        }

        internal static ExpressionNode GetBoolLit(bool value)
        {
            return new BooleanLiteralExpression(value, 0, 0);
        }

        internal static ExpressionNode GetIntLit()
        {
            return new IntegerLiteralExpression(1, 0, 0);
        }

        internal static ExpressionNode GetIntLit(int v)
        {
            return new IntegerLiteralExpression(v, 0, 0);
        }


        internal static IdentifierExpression GetIdentifierExpr(string id)
        {
            return new IdentifierExpression(id, 0, 0);
        }

        internal static SetExpression GetSetExpr(string elementId, List<string> indecies, List<string> bounds)
        {
            return new SetExpression(GetElementNode(elementId, indecies), GetBounds(bounds), GetBoolLit(true), 0, 0);
        }

        internal static SetExpression GetSetExpr(List<string> indecies, List<string> bounds)
        {
            return new SetExpression(GetElementNode(indecies), GetBounds(bounds), GetBoolLit(true), 0, 0);
        }

        internal static SetExpression GetSetExpr(string elementId, List<string> indecies, List<string> bounds, ExpressionNode predicate)
        {
            return new SetExpression(GetElementNode(elementId, indecies), GetBounds(bounds), predicate, 0, 0);
        }

        internal static List<BoundNode> GetBounds(List<string> bounds)
        {
            var res = new List<BoundNode>();
            foreach (var bound in bounds)
            {
                res.Add(GetBound(bound));
            }
            return res;
        }

        internal static BoundNode GetBound(string bound)
        {
            return new BoundNode(bound, GetIntLit(0), GetIntLit(10), 0, 0);
        }
        internal static ElementNode GetElementNode(List<string> indecies)
        {
            return new ElementNode("elementNode", indecies, 0, 0);
        }

        internal static ElementNode GetElementNode(string elementId, List<string> indecies)
        {
            return new ElementNode(elementId, indecies, 0, 0);
        }
    }

    [TestClass]
    public class ReferenceHandlerTests
    {
        /*
          Cases:
            Identifier
                Global
                    Variable number
                Local
                    Variable number
                Both
                    Variable number
                    Hides a global
                        (If function call refers to function in parameter list (local), then this function from parameter list (the inner scope) should be used.)  

            Function Call
                Global
                    Variable number
                    Same Name
                        Overloading (Multiple functions, Same num params but different types) -> Multiple 
                        (Multiple functions, Not same num params) -> Only one selected
                         - Given the existence of a glob func which has the given number of parameters.
                Local
                    Variable number
                Both (a local function with same name as global function(s))
                    Variable number


            Condition
                Only Element(s)
                    Check that index-IDs introduced in element is accessible in returnExpr (can just be identifier expression)
                Only Predicate
                    Check that predicate has access to parameters.
                Both Element(s) and predicate
                    Check that index-IDs introduced in element is accessible in predicate 
            
            Set 
                Ensure bounds are sorted
                Check that identifiers introduced element is accessible in predicate
                - In this predicate, the element can be accessed through its identifier (ensure this is the case)
                Check that new identifiers are accessible inside the limits in the bounds.
                All parts (bounds, predicate, etc.) of the set have access to the identifiers in the scope before going into the scope of the set builder

            Anonymous Functions
                Ensure we have access to the identifiers in the new scope
                Ensure we have access to the identifiers in the old scope

              new scope: the parameterIdentifiers declared in the anonymous functions (appended to old scope)
              old scope: outer scope (the parameterIdentifiers in the function in which the given anonymous functions is declared/defined)

         */

        #region Set
        [DataRow(new string[] { "i", "j" }, new string[] { "j", "i" }, true)]
        [DataRow(new string[] { "i", "j", "k" }, new string[] { "j", "k", "i" }, true)]
        [TestMethod]
        public void Set_xBounds_SortBounds(string[] indecies, string[] bounds, bool doNotUse)
        {
            AST ast = Utilities.GetAstSkeleton();
            var setExpr = Utilities.GetSetExpr(indecies.ToList(), bounds.ToList());
            var condition = Utilities.GetConditionNode(setExpr);
            Utilities.AddFunctionNode(ast, Utilities.GetFunctionNode("func", condition));

            ReferenceHandler referenceHandler = Utilities.GetReferenceHandler();
            referenceHandler.InsertReferences(ast);
            var res = setExpr.Bounds;

            for (int i = 0; i < indecies.Length; i++)
                Assert.AreEqual(indecies[i], res[i].Identifier);
        }
        [DataRow("e", new string[] { "i", "j" }, "i", 1)]
        [DataRow("e", new string[] { "i", "j" }, "e", 0)]
        [TestMethod]
        public void Set_xBounds_AccesibleInPredicate(string elementId, string[] indecies, string id, int expected)
        {
            AST ast = Utilities.GetAstSkeleton();
            var predicate = Utilities.GetIdentifierExpr(id);
            var setExpr = Utilities.GetSetExpr(elementId, indecies.ToList(), indecies.ToList(), predicate);
            var condition = Utilities.GetConditionNode(setExpr);
            Utilities.AddFunctionNode(ast, Utilities.GetFunctionNode("func", condition));

            ReferenceHandler referenceHandler = Utilities.GetReferenceHandler();
            referenceHandler.InsertReferences(ast);
            var res = (IdentifierExpression)setExpr.Predicate;

            Assert.AreEqual(expected, res.Reference);
        }
        //[DataRow("e", new string[] { "i", "j" }, "i", 1)]
        //[DataRow("e", new string[] { "i", "j" }, "e", 0)]
        //[TestMethod]
        //public void Set_xBounds_AccesibleInReturnExpr(string elementId, string[] indecies, string id, int expected)
        //{
        //    AST ast = Utilities.GetAstSkeleton();
        //    var setExpr = Utilities.GetSetExpr(elementId, indecies.ToList(), indecies.ToList());
        //    var returnExpr = Utilities.GetIdentifierExpr(id);
        //    var condition = Utilities.GetConditionNode(setExpr, returnExpr);
        //    Utilities.AddFunctionNode(ast, Utilities.GetFunctionNode("func", condition));

        //    ReferenceHandler referenceHandler = Utilities.GetReferenceHandler();
        //    referenceHandler.InsertReferences(ast);
        //    var res = (IdentifierExpression)condition.ReturnExpression;

        //    Assert.AreEqual(expected, res.Reference);
        //}
        #endregion

        #region Condition
        [DataRow(new string[] { "f" })]
        [DataRow(new string[] { "f", "g" })]
        [DataRow(new string[] { "f", "g", "a", "b" })]
        [TestMethod]
        public void Function_xElements_CorrectIndexing(string[] elements)
        {
            AST ast = Utilities.GetAstSkeleton();
            var condition = Utilities.GetConditionNode(elements);
            Utilities.AddFunctionNode(ast, Utilities.GetFunctionNode("func", elements.ToList(), condition));

            ReferenceHandler referenceHandler = Utilities.GetReferenceHandler();
            referenceHandler.InsertReferences(ast);

            for (int i = 0; i < elements.Length; i++)
                Assert.AreEqual(i, condition.Elements[i].Reference);
        }
        [DataRow(new string[] { "f" }, "f", 0)]
        [DataRow(new string[] { "f", "g" }, "g", 1)]
        [DataRow(new string[] { "f", "g", "a", "b" }, "a", 2)]
        [TestMethod]
        public void Function_xElementsRefernceInPredicate_CorrectIndexing(string[] elements, string id, int expected)
        {
            AST ast = Utilities.GetAstSkeleton();
            var condition = Utilities.GetConditionNode(elements, id);
            Utilities.AddFunctionNode(ast, Utilities.GetFunctionNode("func", elements.ToList(), condition));

            ReferenceHandler referenceHandler = Utilities.GetReferenceHandler();
            referenceHandler.InsertReferences(ast);

            var res = (IdentifierExpression)condition.Condition;
            Assert.AreEqual(expected, res.Reference);
        }
        #endregion

        #region Function Call & Identifiers
        [DataRow(new string[]{"f"}, "f", 0)]
        [DataRow(new string[]{"f", "g"}, "f", 0)]
        [DataRow(new string[]{"f", "g", "a", "b"}, "a", 2)]
        [TestMethod]
        public void FunctionCall_VariableNumberOfGlobalReferences_CorrectIndexing(string[] functionNames, string functionId, int index)
        {
            AST ast = Utilities.GetAstSkeleton();
            FunctionCallExpression funcCallExpr = Utilities.GetFunctionCallExpression(functionId);
            Utilities.AddExportNode(ast, Utilities.GetExportNode(funcCallExpr));
            foreach (var functionName in functionNames)
                Utilities.AddFunctionNode(ast, Utilities.GetFunctionNode(functionName));

            ReferenceHandler referenceHandler = Utilities.GetReferenceHandler();
            referenceHandler.InsertReferences(ast);
            
            Assert.AreEqual(1, funcCallExpr.GlobalReferences.Count);
            Assert.AreEqual(index, funcCallExpr.GlobalReferences[0]);
        }
        [DataRow(new string[] { "f" }, "f", 1)]
        [DataRow(new string[] { "f", "f", "f" }, "f", 3)]
        [DataRow(new string[] { "f", "g" }, "f", 1)]
        [DataRow(new string[] { "f", "g", "a", "b" }, "a", 1)]
        [DataRow(new string[] { "f", "a", "a", "b" }, "a", 2)]
        [TestMethod]
        public void FunctionCall_VariableNumberOfGlobalReferencesWithSameName_CorrectIndexing(string[] functionNames, string functionId, int expected)
        {
            AST ast = Utilities.GetAstSkeleton();
            FunctionCallExpression funcCallExpr = Utilities.GetFunctionCallExpression(functionId);
            Utilities.AddExportNode(ast, Utilities.GetExportNode(funcCallExpr));
            foreach (var functionName in functionNames)
                Utilities.AddFunctionNode(ast, Utilities.GetFunctionNode(functionName));

            ReferenceHandler referenceHandler = Utilities.GetReferenceHandler();
            referenceHandler.InsertReferences(ast);

            Assert.AreEqual(expected, funcCallExpr.GlobalReferences.Count);
        }

        [DataRow(new string[] { "f" }, "f", 0)]
        [DataRow(new string[] { "f", "g" }, "f", 0)]
        [DataRow(new string[] { "f", "g", "a", "b" }, "a", 2)]
        [TestMethod]
        public void FunctionCall_VariableNumberOfLocalReferences_CorrectIndexing(string[] functionNames, string functionId, int index)
        {
            AST ast = Utilities.GetAstSkeleton();
            var condition = Utilities.GetConditionNode(Utilities.GetFunctionCallExpression(functionId));
            var func = Utilities.GetFunctionNode("Global", functionNames.ToList(), condition);
            Utilities.AddFunctionNode(ast, func);

            ReferenceHandler referenceHandler = Utilities.GetReferenceHandler();
            referenceHandler.InsertReferences(ast);
            var res = (FunctionCallExpression)condition.ReturnExpression;

            Assert.AreEqual(index, res.LocalReference);
            Assert.AreEqual(0, res.GlobalReferences.Count);
        }

        [DataRow(new string[] { "f" }, "f", 0)]
        [DataRow(new string[] { "f", "g" }, "f", 0)]
        [DataRow(new string[] { "f", "g", "a", "b" }, "a", 2)]
        [TestMethod]
        public void FunctionCall_VariableNumberOfBothReferences_CorrectLocalIndexing(string[] functionNames, string functionId, int index)
        {
            AST ast = Utilities.GetAstSkeleton();
            var condition = Utilities.GetConditionNode(Utilities.GetFunctionCallExpression(functionId));
            var func = Utilities.GetFunctionNode("Global", functionNames.ToList(), condition);
            Utilities.AddFunctionNode(ast, func);
            foreach (var functionName in functionNames)
                Utilities.AddFunctionNode(ast, Utilities.GetFunctionNode(functionName));

            ReferenceHandler referenceHandler = Utilities.GetReferenceHandler();
            referenceHandler.InsertReferences(ast);
            var res = (FunctionCallExpression)condition.ReturnExpression;

            Assert.AreEqual(index, res.LocalReference);
        }
        [DataRow(new string[] { "f" }, "f", 0)]
        [DataRow(new string[] { "f", "g" }, "f", 0)]
        [DataRow(new string[] { "f", "g", "a", "b" }, "a", 2)]
        [TestMethod]
        public void FunctionCall_VariableNumberOfBothReferences_CorrectGlobalIndexing(string[] functionNames, string functionId, int index)
        {
            AST ast = Utilities.GetAstSkeleton();
            foreach (var functionName in functionNames)
                Utilities.AddFunctionNode(ast, Utilities.GetFunctionNode(functionName));
            var condition = Utilities.GetConditionNode(Utilities.GetFunctionCallExpression(functionId));
            var func = Utilities.GetFunctionNode("Global", functionNames.ToList(), condition);
            Utilities.AddFunctionNode(ast, func);

            ReferenceHandler referenceHandler = Utilities.GetReferenceHandler();
            referenceHandler.InsertReferences(ast);
            var res = (FunctionCallExpression)condition.ReturnExpression;

            Assert.AreEqual(index, res.GlobalReferences[0]);
        }
        [DataRow(new string[] { "f", "f", "f" }, new string[] { "f" }, "f", 3, true)]
        [DataRow(new string[] { "f", "g", "g" }, new string[] { "f", "a" }, "g", 2, false)]
        [DataRow(new string[] { "f", "a", "g", "a", "b" }, new string[] { "f", "g", "a", "b" }, "a", 2, true)]
        [TestMethod]
        public void FunctionCall_VariableNumberOfBothReferences_CorrectCountIndexing(string[] global, string[] local, string functionId, int globalCount, bool hasLocal)
        {
            AST ast = Utilities.GetAstSkeleton();
            foreach (var functionName in global)
                Utilities.AddFunctionNode(ast, Utilities.GetFunctionNode(functionName));
            var condition = Utilities.GetConditionNode(Utilities.GetFunctionCallExpression(functionId));
            var func = Utilities.GetFunctionNode("Global", local.ToList(), condition);
            Utilities.AddFunctionNode(ast, func);

            ReferenceHandler referenceHandler = Utilities.GetReferenceHandler();
            referenceHandler.InsertReferences(ast);
            var res = (FunctionCallExpression)condition.ReturnExpression;

            Assert.AreEqual(globalCount, res.GlobalReferences.Count);
            Assert.AreEqual(hasLocal, res.LocalReference != FunctionCallExpression.NO_LOCAL_REF);
        }
        #endregion

    }
}
