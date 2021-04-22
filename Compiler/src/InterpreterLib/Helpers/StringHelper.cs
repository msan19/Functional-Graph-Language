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
using ASTLib.Nodes.ExpressionNodes.CastExpressionNodes;

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

        public string LiteralString(StringLiteralExpression node, List<Object> parameters)
        {
            return node.Value;
        }

        public string CastIntegerToString(CastFromIntegerExpression node, List<Object> parameters)
        {
            throw new NotImplementedException();
        }

        public string CastBooleanToString(CastFromBooleanExpression node, List<Object> parameters)
        {
            throw new NotImplementedException();
        }

        public string CastRealToString(CastFromRealExpression node, List<Object> parameters)
        {
            throw new NotImplementedException();
        }
    }
}
