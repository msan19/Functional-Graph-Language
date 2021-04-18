using System;
using System.Collections.Generic;
using ASTLib;
using ASTLib.Nodes;
using ASTLib.Nodes.ExpressionNodes;
using ASTLib.Nodes.TypeNodes;
using ASTLib.Objects;

namespace InterpreterLib.Interfaces
{
    public interface IFunctionHelper : IInterpreterHelper
    {
        Function IdentifierFunction(IdentifierExpression node, List<Object> parameters);
    }
}