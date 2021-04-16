using System;
using System.Collections.Generic;
using ASTLib;
using ASTLib.Interfaces;
using ASTLib.Nodes.ExpressionNodes;
using ASTLib.Nodes.ExpressionNodes.CommonOperationNodes;
using ASTLib.Nodes.ExpressionNodes.CommonOperationNodes.ElementAndSetOperations;
using ASTLib.Nodes.ExpressionNodes.OperationNodes;
using ASTLib.Nodes.TypeNodes;

namespace TypeCheckerLib.Interfaces
{
    public interface ISetHelper : ITypeHelper
    {
        TypeNode VisitBinarySetOp(IBinarySetOperator binaryNode, List<TypeNode> parameterTypes);
        TypeNode VisitSet(SetExpression node, List<TypeNode> parameterTypes);
        TypeNode VisitSubset(SubsetExpression node, List<TypeNode> parameterTypes);
    }
}
