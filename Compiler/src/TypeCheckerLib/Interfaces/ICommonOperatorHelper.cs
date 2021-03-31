using System;
using System.Collections.Generic;
using ASTLib;
using ASTLib.Nodes.ExpressionNodes;
using ASTLib.Nodes.ExpressionNodes.OperationNodes;
using ASTLib.Nodes.TypeNodes;

namespace TypeCheckerLib.Interfaces
{
    public interface ICommonOperatorHelper : ITypeHelper
    {
        TypeNode VisitAbsoluteValue(AbsoluteValueExpression n, List<TypeNode> parameterTypes);
        TypeNode VisitAddition(AdditionExpression n, List<TypeNode> parameterTypes);
        TypeNode VisitSubtraction(SubtractionExpression n, List<TypeNode> parameterTypes);
    }
}