using System;
using System.Collections.Generic;
using ASTLib;
using ASTLib.Interfaces;
using ASTLib.Nodes.ExpressionNodes;
using ASTLib.Nodes.ExpressionNodes.CommonOperationNodes;
using ASTLib.Nodes.ExpressionNodes.OperationNodes;
using ASTLib.Nodes.TypeNodes;

namespace TypeCheckerLib.Interfaces
{
    public interface INumberHelper : ITypeHelper
    {
        TypeNode VisitPower(PowerExpression node, List<TypeNode> parameterTypes);
        TypeNode VisitBinaryNumOp(IBinaryNumberOperator binaryNode, List<TypeNode> parameterTypes);
        TypeNode VisitNegative(NegativeExpression negativeExpression, List<TypeNode> parameterTypes);
    }
}