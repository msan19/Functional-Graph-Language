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
        private Func<ExpressionNode, List<object>, int> _dispatchInt;
        private Func<ExpressionNode, List<object>, TypeEnum, object> _dispatch;
        private Func<FunctionNode, List<object>, int> _functionInteger;
        private AST _root;

        public void SetASTRoot(AST root)
        {
            _root = root;
        }

        public void SetUpInts(Func<ExpressionNode, List<Object>, int> dispatchInt,
                       Func<ExpressionNode, List<object>, TypeEnum, Object> dispatch,
                       Func<FunctionNode, List<Object>, int> functionInteger)
        {
            _dispatchInt = dispatchInt;
            _dispatch = dispatch;
            _functionInteger = functionInteger;
        }

        public int ConditionInteger(ConditionNode node, List<Object> parameters)
        {
            return _dispatchInt(node.ReturnExpression, parameters);
        }

        public int AdditionInteger(AdditionExpression node, List<Object> parameters)
        {
            int leftOperand =  _dispatchInt(node.Children[0], parameters);
            int rightOperand = _dispatchInt(node.Children[1], parameters);

            return leftOperand + rightOperand;
        }

        public int SubtractionInteger(SubtractionExpression node, List<Object> parameters)
        {
            int leftOperand =  _dispatchInt(node.Children[0], parameters);
            int rightOperand = _dispatchInt(node.Children[1], parameters);

            return leftOperand - rightOperand;
        }

        public int MultiplicationInteger(MultiplicationExpression node, List<Object> parameters)
        {
            int leftOperand =  _dispatchInt(node.Children[0], parameters);
            int rightOperand = _dispatchInt(node.Children[1], parameters);

            return leftOperand * rightOperand;
        }

        public int DivisionInteger(DivisionExpression node, List<Object> parameters)
        {
            int leftOperand =  _dispatchInt(node.Children[0], parameters);
            int rightOperand = _dispatchInt(node.Children[1], parameters);

            if (rightOperand == 0) { throw new DivideByZeroException(); }

            return leftOperand / rightOperand;
        }

        public int ModuloInteger(ModuloExpression node, List<Object> parameters)
        {
            int leftOperand =  _dispatchInt(node.Children[0], parameters);
            int rightOperand = _dispatchInt(node.Children[1], parameters);

            return ModuloCalculation(leftOperand, rightOperand);
        }

        private int ModuloCalculation(int leftOperand, int rightOperand)
        {
            if(rightOperand == 0) { throw new DivideByZeroException(); }
            return (int)(leftOperand - rightOperand * Math.Floor((double)(leftOperand / rightOperand)));
        }

        public int AbsoluteInteger(AbsoluteValueExpression node, List<Object> parameters)
        {
            int operand = _dispatchInt(node.Children[0], parameters);

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
                listOfFuncParam.Add(_dispatch(node.Children[i], parameters, parameterType));
            }

            return _functionInteger(funcNode, listOfFuncParam);
        }
    }
}