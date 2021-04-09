using ASTLib;
using ASTLib.Nodes;
using ASTLib.Nodes.ExpressionNodes;
using System;
using System.Collections.Generic;
using InterpreterLib.Interfaces;
using ASTLib.Nodes.TypeNodes;

namespace InterpreterLib.Helpers
{
    public class FunctionHelper : IFunctionHelper
    {
        IInterpreterFunction _interpreter;
        private List<FunctionNode> _functions;

        public void SetASTRoot(AST root)
        {
            _functions = root.Functions;
        }

        public void SetInterpreter(IInterpreterFunction interpreter)
        {
            _interpreter = interpreter;
        }

        public int IdentifierFunction(IdentifierExpression node, List<Object> parameters)
        {
            return node.IsLocal ? (int) parameters[node.Reference] : node.Reference;
        }

    }
}