using ASTLib.Nodes.ExpressionNodes;
using ASTLib.Objects;
using System;
using System.Collections.Generic;
using System.Text;
using ASTLib.Nodes.ExpressionNodes.SetOperationNodes;
using ASTLib.Nodes;
using ASTLib.Nodes.ExpressionNodes.OperationNodes;

namespace InterpreterLib.Interfaces
{
    public interface ISetHelper : IInterpreterHelper
    {
        Set ExportSet(ExportNode node);
        void SetInterpreter(IInterpreterSet interpreter);
        Set SetExpression(SetExpression node, List<Object> parameters);
        Set UnionSet(UnionExpression node, List<Object> parameters);
        Set IntersectionSet(IntersectionExpression node, List<Object> parameters);
        Set SubtractionSet(SubtractionExpression node, List<object> parameters);
    }
}
