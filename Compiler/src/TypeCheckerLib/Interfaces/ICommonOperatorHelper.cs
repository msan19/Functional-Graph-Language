using System;
using System.Collections.Generic;
using ASTLib;
using ASTLib.Interfaces;
using ASTLib.Nodes.ExpressionNodes;
using ASTLib.Nodes.ExpressionNodes.CommonOperationNodes;
using ASTLib.Nodes.ExpressionNodes.CommonOperationNodes.ElementAndSetOperations;
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
        TypeNode VisitRelationalOperator(IRelationOperator node, List<TypeNode> parameterTypes);
        TypeNode VisitEquivalenceOperator(IEquivalenceOperator node, List<TypeNode> parameterTypes);
        TypeNode VisitElement(ElementExpression n, List<TypeNode> parameterTypes);
        TypeNode VisitIn(InExpression node, List<TypeNode> parameterTypes);
        TypeNode VisitISetGraphField(ISetGraphField node, List<TypeNode> parameterTypes);
        TypeNode VisitIFunctionGraphField(IFunctionGraphField node, List<TypeNode> parameterTypes);
        TypeNode VisitGraph(GraphExpression node, List<TypeNode> parameterTypes);
    }
}