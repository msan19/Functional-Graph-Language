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
    public class IntegerHelper : IIntegerHelper
    {

        private IInterpreterInteger _interpreter;

        public void SetInterpreter(IInterpreterInteger interpreter) 
        {
            _interpreter = interpreter;
        }

        public int AdditionInteger(AdditionExpression node, List<Object> parameters)
        {
            int leftOperand = _interpreter.DispatchInt(node.Children[0], parameters);
            int rightOperand = _interpreter.DispatchInt(node.Children[1], parameters);

            return leftOperand + rightOperand;
        }

        public int SubtractionInteger(SubtractionExpression node, List<Object> parameters)
        {
            int leftOperand = _interpreter.DispatchInt(node.Children[0], parameters);
            int rightOperand = _interpreter.DispatchInt(node.Children[1], parameters);

            return leftOperand - rightOperand;
        }

        public int MultiplicationInteger(MultiplicationExpression node, List<Object> parameters)
        {
            int leftOperand = _interpreter.DispatchInt(node.Children[0], parameters);
            int rightOperand = _interpreter.DispatchInt(node.Children[1], parameters);

            return leftOperand * rightOperand;
        }

        public int DivisionInteger(DivisionExpression node, List<Object> parameters)
        {
            int leftOperand =  _interpreter.DispatchInt(node.Children[0], parameters);
            int rightOperand = _interpreter.DispatchInt(node.Children[1], parameters);

            if (rightOperand == 0) { throw new DivisionByZeroException(node); }

            return leftOperand / rightOperand;
        }

        public int ModuloInteger(ModuloExpression node, List<Object> parameters)
        {
            int leftOperand =  _interpreter.DispatchInt(node.Children[0], parameters);
            int rightOperand = _interpreter.DispatchInt(node.Children[1], parameters);

            if (rightOperand == 0) { throw new DivisionByZeroException(node); }

            return ModuloCalculation(leftOperand, rightOperand);
        }

        private int ModuloCalculation(int leftOperand, int rightOperand)
        {
            return (int)(leftOperand - rightOperand * Math.Floor((double)(leftOperand / rightOperand)));
        }

        public int AbsoluteInteger(AbsoluteValueExpression node, List<Object> parameters)
        {
            if (node.Type == TypeEnum.Integer)
            {
                int operand = _interpreter.DispatchInt(node.Children[0], parameters);
                return Math.Abs(operand);
            }
            else if (node.Type == TypeEnum.Set)
            {
                Set operand = _interpreter.DispatchSet(node.Children[0], parameters);
                return operand.Elements.Count;
            }
            else
                throw new InvalidAbsoluteIntegerException(node);
        }
        
        public int IdentifierInteger(IdentifierExpression node, List<Object> parameters)
        {
            return (int)parameters[node.Reference];
        }

        public int LiteralInteger(IntegerLiteralExpression node, List<Object> parameters)
        {
            return node.Value;
        }

        public int NegativeInteger(NegativeExpression node, List<object> parameters)
        {
            return - _interpreter.DispatchInt(node.Children[0], parameters);
        }
    }
}