using System;
using System.Collections.Generic;
using ASTLib;
using ASTLib.Nodes.ExpressionNodes;
using ASTLib.Nodes.ExpressionNodes.CommonOperationNodes.RelationalOperationNodes;
using ASTLib.Nodes.ExpressionNodes.OperationNodes;
using ASTLib.Nodes.TypeNodes;

namespace TypeCheckerLib.Interfaces
{
    public interface ICommonOperatorHelper : ITypeHelper
    {
        TypeNode VisitAbsoluteValue(AbsoluteValueExpression n, List<TypeNode> parameterTypes);
        TypeNode VisitAddition(AdditionExpression n, List<TypeNode> parameterTypes);
        TypeNode VisitSubtraction(SubtractionExpression n, List<TypeNode> parameterTypes);
        TypeNode VisitGreaterEqual(GreaterEqualExpression node, List<TypeNode> parameterTypes);
        TypeNode VisitGreater(GreaterExpression node, List<TypeNode> parameterTypes);
        TypeNode VisitLessEqual(LessEqualExpression node, List<TypeNode> parameterTypes);
        TypeNode VisitLess(LessExpression node, List<TypeNode> parameterTypes);
    }
}