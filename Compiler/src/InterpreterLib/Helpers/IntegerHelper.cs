using ASTLib;
using ASTLib.Nodes;
using ASTLib.Nodes.ExpressionNodes;
using System;
using System.Collections.Generic;
using ASTLib.Nodes.ExpressionNodes.OperationNodes;
using InterpreterLib.Interfaces;
using ASTLib.Nodes.TypeNodes;
using ASTLib.Exceptions;

namespace InterpreterLib.Helpers
{
    public class IntegerHelper : IIntegerHelper
    {

        private IInterpreterInteger _interpreter;
        private AST _root;

        public void SetASTRoot(AST root)
        {
            _root = root;
        }

        public void SetInterpreter(IInterpreterInteger interpreter) 
        {
            _interpreter = interpreter;
        }

        public int? ConditionInteger(ConditionNode node, List<Object> parameters)
        {
            if (node.Condition == null || _interpreter.DispatchBoolean(node.Condition, parameters))
                return _interpreter.DispatchInt(node.ReturnExpression, parameters);
            return null;
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
            int operand = _interpreter.DispatchInt(node.Children[0], parameters);

            if (node.Type != TypeEnum.Integer) { throw new InvalidAbsoluteIntegerException(node); }

            return Math.Abs(operand);
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
            FunctionNode funcNode;
            if (node.LocalReference == FunctionCallExpression.NO_LOCAL_REF)
                funcNode = _root.Functions[node.GlobalReferences[0]];
            else
                funcNode = _root.Functions[(int)parameters[node.LocalReference]];

            return FunctionCallIntegerIterator(node, funcNode, parameters);
        }

        private int FunctionCallIntegerIterator(FunctionCallExpression node, FunctionNode funcNode, List<Object> parameters)
        {
            List<object> listOfFuncParam = new List<object>();

            for (int i = 0; i < node.Children.Count; i++)
            {
                TypeEnum parameterType = funcNode.FunctionType.ParameterTypes[i].Type;
                listOfFuncParam.Add(_interpreter.Dispatch(node.Children[i], parameters, parameterType));
            }

            return _interpreter.FunctionInteger(funcNode, listOfFuncParam);
        }
    }
}