using System.Collections.Generic;
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

        private static FunctionTypeNode GetFunctionTypeNode()
        {
            return new FunctionTypeNode(GetTypeNode(), new List<TypeNode>(), 0, 0);
        }

        private static TypeNode GetTypeNode()
        {
            return new TypeNode(TypeEnum.Integer, 0, 0);
        }

        private static ConditionNode GetConditionNode(ExpressionNode exprNode)
        {
            return new ConditionNode(exprNode, 0, 0);
        }

        public static FunctionCallExpression GetFunctionCallExpression(string id)
        {
            return new FunctionCallExpression(id, new List<ExpressionNode>(), 0, 0);
        }
        
        public static ReferenceHandler GetReferenceHandler()
        {
            ReferenceHelper referenceHelper = new ReferenceHelper();
            return new ReferenceHandler(referenceHelper, false);
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
    }
}
