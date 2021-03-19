using ASTLib;
using ASTLib.Nodes;
using ASTLib.Nodes.ExpressionNodes;
using System;
using System.Collections.Generic;
using ASTLib.Nodes.ExpressionNodes.OperationNodes;
using InterpreterLib.Interfaces;
using ASTLib.Nodes.TypeNodes;

namespace InterpreterLib.Helpers
{
    public class IntegerHelper : IIntegerHelper
    {

        public IInterpreter Interpreter { get; set; }

        private readonly AST _root;

        public IntegerHelper(AST root)
        {
            _root = root;
        }

        public int FunctionInteger(FunctionNode node, List<Object> parameters)
        {
            return ConditionInteger(node.Conditions[0], parameters);  
        }

        private int ConditionInteger(ConditionNode node, List<Object> parameters)
        {
            return Interpreter.DispatchInt(node.ReturnExpression, parameters);
        }

        public int AdditionInteger(AdditionExpression node, List<Object> parameters)
        {
            int leftOperand = Interpreter.DispatchInt(node.Children[0], parameters);
            int rightOperand = Interpreter.DispatchInt(node.Children[1], parameters);

            return leftOperand + rightOperand;
        }

        public int SubtractionInteger(SubtractionExpression node, List<Object> parameters)
        {
            int leftOperand = Interpreter.DispatchInt(node.Children[0], parameters);
            int rightOperand = Interpreter.DispatchInt(node.Children[1], parameters);

            return leftOperand - rightOperand;
        }

        public int MultiplicationInteger(MultiplicationExpression node, List<Object> parameters)
        {
            int leftOperand = Interpreter.DispatchInt(node.Children[0], parameters);
            int rightOperand = Interpreter.DispatchInt(node.Children[1], parameters);

            return leftOperand * rightOperand;
        }

        public int DivisionInteger(DivisionExpression node, List<Object> parameters)
        {
            int leftOperand = Interpreter.DispatchInt(node.Children[0], parameters);
            int rightOperand = Interpreter.DispatchInt(node.Children[1], parameters);

            return leftOperand / rightOperand;
        }

        public int ModuloInteger(ModuloExpression node, List<Object> parameters)
        {
            int leftOperand = Interpreter.DispatchInt(node.Children[0], parameters);
            int rightOperand = Interpreter.DispatchInt(node.Children[1], parameters);

            return leftOperand % rightOperand;
        }

        public int AbsoluteInteger(AbsoluteValueExpression node, List<Object> parameters)
        {
            int operand = Interpreter.DispatchInt(node.Children[0], parameters);

            if (node.Type == TypeEnum.Integer)
            {
                return Math.Abs(operand);
            }
            else
            {
                throw new Exception("Operand is not of type: Integer");
            }
        }
        
        public int IdentifierInteger(IdentifierExpression node, List<Object> parameters)
        {
            return (int)parameters[node.Reference];
        }

        public int LiteralInteger(IntegerLiteralExpression node, List<Object> parameters)
        {
            return node.Value;
        }

        public int FunctionCallInteger(FunctionCallExpression node, List<Object> parameters)
        {
            throw new NotImplementedException();
        }

    }
}