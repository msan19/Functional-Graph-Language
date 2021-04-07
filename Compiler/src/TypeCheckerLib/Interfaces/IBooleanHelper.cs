using System;
using System.Collections.Generic;
using ASTLib;
using ASTLib.Nodes.ExpressionNodes;
using ASTLib.Nodes.ExpressionNodes.BooleanOperationNodes;
using ASTLib.Nodes.TypeNodes;

namespace TypeCheckerLib.Interfaces
{
    public interface IBooleanHelper : ITypeHelper
    {
        TypeNode VisitAnd(AndExpression node, List<TypeNode> parameterTypes);
        TypeNode VisitNot(NotExpression node, List<TypeNode> parameterTypes);
        TypeNode VisitOr(OrExpression node, List<TypeNode> parameterTypes);
    }
}