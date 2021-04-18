using ASTLib;
using ASTLib.Nodes;
using ASTLib.Nodes.ExpressionNodes;
using System;
using System.Collections.Generic;
using InterpreterLib.Interfaces;
using ASTLib.Nodes.TypeNodes;
using ASTLib.Objects;

namespace InterpreterLib.Helpers
{
    public class FunctionHelper : IFunctionHelper
    {
        public Function IdentifierFunction(IdentifierExpression node, List<Object> parameters)
        {
            return node.IsLocal ? (Function) parameters[node.Reference] : new Function(node.Reference);
        }

    }
}