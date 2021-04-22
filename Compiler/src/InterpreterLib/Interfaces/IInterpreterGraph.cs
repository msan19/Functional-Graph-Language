using ASTLib.Nodes;
using ASTLib.Nodes.ExpressionNodes;
using ASTLib.Objects;
using System;
using System.Collections.Generic;
using System.Text;

namespace InterpreterLib.Interfaces
{
    public interface IInterpreterGraph
    {
        Set DispatchSet(ExpressionNode node, List<Object> parameters);
        Function DispatchFunction(ExpressionNode node, List<object> parameters);
<<<<<<< HEAD
        Graph DispatchGraph(ExpressionNode node, List<object> parameters);

=======
        string DispatchString(ExpressionNode node, List<object> parameters);
        Graph DispatchGraph(ExpressionNode node, List<object> parameters);
        T Function<T>(FunctionNode node, List<Object> parameters);
>>>>>>> 16822392b806d7975d7d0eb883d1e2cebff4dc5c
    }
}
