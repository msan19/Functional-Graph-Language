using ASTLib;
using ASTLib.Nodes;
using ASTLib.Nodes.ExpressionNodes;
using System;
using System.Collections.Generic;
using InterpreterLib.Interfaces;
using ASTLib.Nodes.TypeNodes;
using ASTLib.Objects;
using ASTLib.Nodes.ExpressionNodes.OperationNodes;
using ASTLib.Nodes.ExpressionNodes.CommonOperationNodes.GraphFields;
using System.Linq;

namespace InterpreterLib.Helpers
{
    public class FunctionHelper : IFunctionHelper
    {
        private IInterpreterFunction _interpreter;

        public void FunctionInterpreter(IInterpreterFunction interpreter)
        {
            _interpreter = interpreter;
        }

        public Function IdentifierFunction(IdentifierExpression node, List<Object> parameters)
        {
            return node.IsLocal ? (Function) parameters[node.Reference] : new Function(node.Reference);
        }

        public Function SrcField(SrcGraphField node, List<Object> parameters)
        {
            Graph graph = _interpreter.DispatchGraph(node.Children[0], parameters);

            return graph.Src;
        }

        public Function DstField(DstGraphField node, List<Object> parameters)
        {
            Graph graph = _interpreter.DispatchGraph(node.Children[0], parameters);

            return graph.Dst;
        }

        public Function AnonymousFunction(AnonymousFunctionExpression e, List<object> parameters)
        {
            return new Function(e.Reference, parameters.ToList());
        }
    }
}