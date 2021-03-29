using ASTLib;
using ASTLib.Nodes.ExpressionNodes;
using ASTLib.Nodes.TypeNodes;
using System;
using System.Collections.Generic;
using System.Text;

namespace TypeCheckerLib.Interfaces
{
    public interface ITypeHelper
    {
        void Initialize(AST root, Func<ExpressionNode, List<TypeNode>, TypeNode> dispatcher);
    }
}
