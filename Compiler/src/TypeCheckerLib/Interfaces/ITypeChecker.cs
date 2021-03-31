using System.Collections.Generic;
using ASTLib;
using ASTLib.Nodes.ExpressionNodes;
using ASTLib.Nodes.TypeNodes;

namespace TypeCheckerLib.Interfaces
{
    public interface ITypeChecker
    {
        void CheckTypes(AST root);
        TypeNode Dispatch(ExpressionNode node, List<TypeNode> parameterTypes);
    }
}