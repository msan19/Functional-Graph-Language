using System;
using System.Collections.Generic;
using ASTLib;
using ASTLib.Nodes.ExpressionNodes;
using ASTLib.Nodes.ExpressionNodes.BooleanOperationNodes;
using ASTLib.Nodes.TypeNodes;
using ASTLib.Interfaces;

namespace TypeCheckerLib.Interfaces
{
    public interface IBooleanHelper : ITypeHelper
    {
        TypeNode VisitNot(NotExpression node, List<TypeNode> parameterTypes);

        TypeNode VisitBinaryBoolOp(IBinaryBooleanOperator binaryNode, List<TypeNode> parameterTypes);

    }
}