using ASTLib;
using ASTLib.Nodes;
using ASTLib.Nodes.ExpressionNodes;
using ASTLib.Objects;
using System;
using System.Collections.Generic;
using System.Text;

namespace InterpreterLib.Interfaces
{
    public interface IGraphHelper : IInterpreterHelper
    {
        void SetInterpreter(IInterpreterGraph interpreter);
        void SetASTRoot(AST root);
        Graph GraphExpression(GraphExpression node, List<Object> parameters);
        LabelGraph ExportGraph(ExportNode n);
    }
}
