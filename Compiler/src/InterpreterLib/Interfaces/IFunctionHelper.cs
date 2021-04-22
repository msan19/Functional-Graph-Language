using System;
using System.Collections.Generic;
using ASTLib;
using ASTLib.Nodes;
using ASTLib.Nodes.ExpressionNodes;
using ASTLib.Nodes.ExpressionNodes.CommonOperationNodes.GraphFields;
using ASTLib.Nodes.ExpressionNodes.OperationNodes;
using ASTLib.Nodes.TypeNodes;
using ASTLib.Objects;

namespace InterpreterLib.Interfaces
{
    public interface IFunctionHelper : IInterpreterHelper
    {
        void FunctionInterpreter(IInterpreterFunction interpreter);
        Function IdentifierFunction(IdentifierExpression node, List<Object> parameters);
        Function SrcField(SrcGraphField node, List<Object> parameters);
        Function DstField(DstGraphField node, List<Object> parameters);
    }
}