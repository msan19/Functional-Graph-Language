using ASTLibrary.Nodes;
using System;
using System.Collections.Generic;

namespace ASTLibrary
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