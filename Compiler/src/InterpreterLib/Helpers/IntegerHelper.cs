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

        private AST _root;

        public void SetASTRoot(AST root)
        {
            _root = root;
        }

        public int ConditionInteger(ConditionNode node, List<Object> parameters)
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

            if (rightOperand == 0) { throw new DivideByZeroException(); }

            return leftOperand / rightOperand;
        }

        public int ModuloInteger(ModuloExpression node, List<Object> parameters)
        {
            int leftOperand = Interpreter.DispatchInt(node.Children[0], parameters);
            int rightOperand = Interpreter.DispatchInt(node.Children[1], parameters);

            return ModuloCalculation(leftOperand, rightOperand);
        }

        private int ModuloCalculation(int leftOperand, int rightOperand)
        {
            if(rightOperand == 0) { throw new DivideByZeroException(); }
            return (int)(leftOperand - rightOperand * Math.Floor((double)(leftOperand / rightOperand)));
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
            FunctionNode funcNode;

            _ = node.GlobalReferences.Count >= 1 ? funcNode = _root.Functions[node.GlobalReferences[0]]
                                             : funcNode = _root.Functions[(int)parameters[node.LocalReference]];

            return FunctionCallIntegerIterator(node, funcNode, parameters);
        }

        private int FunctionCallIntegerIterator(FunctionCallExpression node, FunctionNode funcNode, List<Object> parameters)
        {
            List<object> listOfFuncParam = new List<object>();

            for (int i = 0; i < node.Children.Count; i++)
            {
                TypeEnum parameterType = funcNode.FunctionType.ParameterTypes[i].Type;
                listOfFuncParam.Add(Interpreter.Dispatch(node.Children[i], parameters, parameterType));
            }

            return Interpreter.FunctionInteger(funcNode, listOfFuncParam);
        }
    }
}