using ASTLib.Nodes;
using System.Collections.Generic;

namespace ASTLib
{
    public class AST : Node
    {
        public List<FunctionNode> Functions { get; private set; }
        public List<ExportNode> Exports { get; private set; }

        public AST(List<FunctionNode> functions, List<ExportNode> exports, int line, int letter) : base(line, letter)
        {
            Functions = functions;
            Exports = exports;
        }

    }
}