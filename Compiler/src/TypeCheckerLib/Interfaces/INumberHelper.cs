using System;
using System.Collections.Generic;
using ASTLib;
using ASTLib.Interfaces;
using ASTLib.Nodes.ExpressionNodes;
using ASTLib.Nodes.ExpressionNodes.OperationNodes;
using ASTLib.Nodes.TypeNodes;

namespace TypeCheckerLib.Interfaces
{
    public interface INumberHelper
    {
        void Initialize(AST root, Func<ExpressionNode, List<TypeNode>, TypeNode> dispatcher);
        TypeNode VisitBinaryNumOp(IBinaryNumberOperator binaryNode, List<TypeNode> parameterTypes);
    }
}