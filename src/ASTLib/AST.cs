using ASTLib.Nodes;
using System.Collections.Generic;

namespace ASTLib
{
    public class AST: Node
    {
        public List<FunctionNode> Functions { get; private set; }
        public List<FunctionNode> Exports { get; private set; }

        public AST(List<FunctionNode> functions, List<FunctionNode> exports)
        {
            Functions = functions;
            Exports = exports;
        }

    }
}