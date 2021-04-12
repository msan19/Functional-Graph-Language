using ASTLib;
using ASTLib.Nodes;
using ASTLib.Nodes.ExpressionNodes;
using System;
using System.Collections.Generic;
using ASTLib.Nodes.ExpressionNodes.OperationNodes;
using InterpreterLib.Interfaces;
using System.Linq;
using ASTLib.Nodes.TypeNodes;
using ASTLib.Exceptions;
using ASTLib.Nodes.ExpressionNodes.CommonOperationNodes;

namespace InterpreterLib.Helpers
{
    public class RealHelper : IRealHelper
    {
        private IInterpreterReal _interpreter;

        public void SetInterpreter(IInterpreterReal interpreter)
        {
            _interpreter = interpreter;
        }

        public double ExportReal(ExportNode node, List<object> parameters)
        {
            return _interpreter.DispatchReal(node.ExportValue, parameters);
        }

        public double AdditionReal(AdditionExpression node, List<object> parameters)
        {
            double leftOperand = _interpreter.DispatchReal(node.Children[0], parameters);
            double rightOperand = _interpreter.DispatchReal(node.Children[1], parameters);

            return leftOperand + rightOperand;
        }

        public double SubtractionReal(SubtractionExpression node, List<object> parameters)
        {
            double leftOperand = _interpreter.DispatchReal(node.Children[0], parameters);
            double rightOperand = _interpreter.DispatchReal(node.Children[1], parameters);

            return leftOperand - rightOperand;
        }

        public double MultiplicationReal(MultiplicationExpression node, List<object> parameters)
        {
            double leftOperand = _interpreter.DispatchReal(node.Children[0], parameters);
            double rightOperand = _interpreter.DispatchReal(node.Children[1], parameters);

            return leftOperand * rightOperand;
        }

        public double DivisionReal(DivisionExpression node, List<object> parameters)
        {
            double leftOperand = _interpreter.DispatchReal(node.Children[0], parameters);
            double rightOperand = _interpreter.DispatchReal(node.Children[1], parameters);

            if (rightOperand == 0.0) throw new DivisionByZeroException(node);

            return leftOperand / rightOperand;
        }

        public double ModuloReal(ModuloExpression node, List<object> parameters)
        {
            double leftOperand = _interpreter.DispatchReal(node.Children[0], parameters);
            double rightOperand = _interpreter.DispatchReal(node.Children[1], parameters);

            if (rightOperand == 0.0) throw new DivisionByZeroException(node);

            return leftOperand - rightOperand * Math.Floor(leftOperand / rightOperand);
        }

        public double AbsoluteReal(AbsoluteValueExpression node, List<object> parameters)
        {
            double operand = _interpreter.DispatchReal(node.Children[0], parameters);

            return Math.Abs(operand);
        }
        public double PowerReal(PowerExpression node, List<object> parameters)
        {
            double leftOperand = _interpreter.DispatchReal(node.Children[0], parameters);
            double rightOperand = _interpreter.DispatchReal(node.Children[1], parameters);

            return Math.Pow(leftOperand, rightOperand);
        }

        public double IdentifierReal(IdentifierExpression node, List<object> parameters)
        {
            return (double) parameters[node.Reference];
        }

        public double LiteralReal(RealLiteralExpression node, List<object> parameters)
        {
            return node.Value;
        }

        public double CastIntegerToReal(CastFromIntegerExpression node, List<object> parameters)
        {
            return Convert.ToDouble(_interpreter.DispatchInt(node.Children[0], parameters));
        }

        public double NegativeReal(NegativeExpression node, List<object> parameters)
        {
            return - _interpreter.DispatchReal(node.Children[0], parameters);
        }
    }
}