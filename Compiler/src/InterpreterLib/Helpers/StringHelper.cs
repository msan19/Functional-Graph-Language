using ASTLib;
using ASTLib.Nodes;
using ASTLib.Nodes.ExpressionNodes;
using System;
using System.Collections.Generic;
using ASTLib.Nodes.ExpressionNodes.OperationNodes;
using InterpreterLib.Interfaces;
using ASTLib.Nodes.TypeNodes;
using ASTLib.Exceptions;
using ASTLib.Nodes.ExpressionNodes.CommonOperationNodes;
using ASTLib.Objects;
using ASTLib.Nodes.ExpressionNodes.NumberOperationNodes;
namespace InterpreterLib.Helpers
{
    public class StringHelper : IStringHelper
    {
        private IInterpreterString _interpreter;

        public void SetInterpreter(IInterpreterString interpreter)
        {
            _interpreter = interpreter;
        }

        public string AdditionString(AdditionExpression node, List<Object> parameters)
        {
            string leftOperand = _interpreter.DispatchString(node.Children[0], parameters);
            string rightOperand = _interpreter.DispatchString(node.Children[1], parameters);

            return string.Concat(leftOperand, rightOperand);
        }
    }
}
