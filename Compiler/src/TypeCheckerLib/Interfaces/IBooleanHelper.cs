using System;
using System.Collections.Generic;
using ASTLib;
using ASTLib.Nodes.ExpressionNodes;
using ASTLib.Nodes.TypeNodes;

namespace TypeCheckerLib.Interfaces
{
    public interface IBooleanHelper
    {
        void Initialize(AST root, Func<ExpressionNode, List<TypeNode>, TypeNode> dispatcher);
    }
}