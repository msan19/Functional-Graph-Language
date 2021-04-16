using ASTLib.Nodes.ExpressionNodes;
using ASTLib.Objects;
using System;
using System.Collections.Generic;
using System.Text;
using ASTLib.Nodes.ExpressionNodes.SetOperationNodes;
using ASTLib.Nodes.ExpressionNodes.OperationNodes;

namespace InterpreterLib.Interfaces
{
    public interface ISetHelper : IInterpreterHelper
    {
        public void SetInterpreter(IInterpreterSet interpreter);
        Set SetExpression(SetExpression node, List<object> parameters);
        Set UnionSet(UnionExpression node, List<object> parameters);
        Set IntersectionSet(IntersectionExpression node, List<object> parameters);
        Set SubtractionSet(SubtractionExpression node, List<object> parameters);
    }
}
