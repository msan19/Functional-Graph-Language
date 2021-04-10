using System;
using System.Collections.Generic;
using ASTLib;
using ASTLib.Nodes;
using ASTLib.Nodes.ExpressionNodes;
using ASTLib.Nodes.TypeNodes;

namespace InterpreterLib.Interfaces
{
    public interface IFunctionHelper : IInterpreterHelper
    {

        int IdentifierFunction(IdentifierExpression node, List<Object> parameters);

    }
}