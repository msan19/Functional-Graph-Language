using ASTLib;
using ASTLib.Nodes.ExpressionNodes;
using System.Collections.Generic;

namespace ReferenceHandlerLib
{
    public interface IReferenceHandler
    {
        void Dispatch(ExpressionNode node, List<string> parameters);
        void InsertReferences(AST root);
    }
}