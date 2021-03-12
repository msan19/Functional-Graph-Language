using ASTLib.Nodes;
using ASTLib.Nodes.ExpressionNodes;
using System;
using System.Collections.Generic;

namespace ASTLib
{
    public class AST: Node
    {
        public List<ExpressionNode> Functions { get; private set; }
        public List<ExportNode> Exports { get; private set; }

        public AST(List<ExpressionNode> functions, List<ExportNode> exports)
        {
            Functions = functions;
            Exports = exports;
        }

    }
}