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

            List<FunctionNode> functionNodes = new List<FunctionNode>();
            List<ExportNode> exportNodes = new List<ExportNode>();
            return new AST(functionNodes, exportNodes);
        }
    }
}