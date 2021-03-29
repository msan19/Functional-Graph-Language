using System;
using System.Collections.Generic;
using ASTLib;
using ASTLib.Nodes.ExpressionNodes;
using ASTLib.Nodes.ExpressionNodes.OperationNodes;
using ASTLib.Nodes.TypeNodes;

namespace TypeCheckerLib.Interfaces
{
    public interface ICommonOperatorHelper
    {
        void Initialize(AST root, Func<ExpressionNode, List<TypeNode>, TypeNode> dispatcher);
        TypeNode VisitAddition(AdditionExpression n, List<TypeNode> parameterTypes);
        TypeNode VisitSubtraction(SubtractionExpression n, List<TypeNode> parameterTypes);
    }
}