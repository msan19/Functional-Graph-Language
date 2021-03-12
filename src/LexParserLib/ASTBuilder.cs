using System.Collections.Generic;
using ASTLib;
using ASTLib.Nodes;
using Hime.Redist;

namespace LexParserLib
{
    public class ASTBuilder
    {
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

        public AST VisitDeclarations(ASTNode node)
        {
            if (node.Children.Count == 1)
            {
                List<FunctionNode> functionNodes = new List<FunctionNode>();
                List<ExportNode> exportNodes = new List<ExportNode>();
                AST ast = new AST(functionNodes, exportNodes);

                if (node.Children[0].Symbol.Name == "export")
                {
                    // Create export node
                }
                else
                {
                    // Create function node
                }
            }
            return null;
        } 
        
        public Node VisitDeclaration(ASTNode node)
        {
            return null;
        } 
        
        
        
    }
    
}