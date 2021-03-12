using System.Collections.Generic;
using ASTLib;
using ASTLib.Nodes;
using ASTLib.Nodes.ExpressionNodes;
using Hime.Redist;

namespace LexParserLib
{
    public class ASTBuilder
    {
        private const int PARAMETER_IDs_POS = 5;
        
        public AST GetAST(ASTNode root)
        {
            // root.Children
            // foreach (var childNode in root.Children)
            // {
            //     ASTNode test = childNode;
            //     if (true)
            //     {
            //         
            //     }
            // }
            
            List<FunctionNode> functionNodes = new List<FunctionNode>();
            List<ExportNode> exportNodes = new List<ExportNode>();
            return new AST(functionNodes, exportNodes);
        }

        public AST VisitDeclarations(ASTNode himeNode)
        {
            if (himeNode.Children.Count == 1)
            {
                List<FunctionNode> functionNodes = new List<FunctionNode>();
                List<ExportNode> exportNodes = new List<ExportNode>();
                AST ast = new AST(functionNodes, exportNodes);
                InsertDeclaration(ast, himeNode.Children[0]);
                return ast;
            }
            else
            {
                AST ast = VisitDeclarations(himeNode.Children[0]);
                InsertDeclaration(ast, himeNode.Children[1]);
                return ast;
            }
        }

        private void InsertDeclaration(AST ast, ASTNode himeNode)
        {
            if (himeNode.Symbol.Name == "export")
            {
                ExportNode exportNode = CreateExportNode(himeNode.Children[0]);
                ast.Exports.Add(exportNode);
            }
            else
            {
                FunctionNode functionNode = CreateFunctionNode(himeNode.Children[0]);
                ast.Functions.Add(functionNode);
            }
        }
        
        private ExportNode CreateExportNode(ASTNode himeNode)
        {
            ExpressionNode expressionNode = CreateExpression(himeNode.Children[0]);
            return new ExportNode(expressionNode);
        }
        
        private FunctionNode CreateFunctionNode(ASTNode himeNode)
        {
            ASTNode himeFuncNode = himeNode.Children[0];
            ASTNode himeExpressionNode = himeNode.Children[2];
            List<string> parameterIdentifiers = VisitIdentifiers(himeFuncNode.Children[PARAMETER_IDs_POS]);
            // return new FunctionNode(new ConditionNode(CreateExpression(himeExpressionNode)), parameterIdentifiers, );
            return null; // TODO
        }

        public List<string> VisitIdentifiers(ASTNode himeNode)
        {
            if (himeNode.Children.Count == 1)
            {
                return null;
            }
            else
            {
                List<string> identifiers = VisitIdentifiers(himeNode.Children[0]);
                // identifiers. // TODO
                return null;
            }
        }
        
        private ExpressionNode CreateExpression(ASTNode himeNode)
        {
            // TODO
            return null;
        }
    }
    
}