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
using System.Globalization;

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
            return _interpreter.DispatchInt(node.Child, parameters).ToString();
        }

        public string CastBooleanToString(CastFromBooleanExpression node, List<Object> parameters)
        {
            return _interpreter.DispatchBoolean(node.Child, parameters).ToString();
        }

        public string CastRealToString(CastFromRealExpression node, List<Object> parameters)
        {
            double val = _interpreter.DispatchReal(node.Child, parameters);
            return val.ToString("G", CultureInfo.InvariantCulture);
        }
    }
}
