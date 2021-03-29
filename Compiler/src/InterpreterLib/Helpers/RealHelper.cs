using ASTLib;
using ASTLib.Nodes;
using ASTLib.Nodes.ExpressionNodes;
using System;
using System.Collections.Generic;
using ASTLib.Nodes.ExpressionNodes.OperationNodes;
using InterpreterLib.Interfaces;
using System.Linq;
using ASTLib.Nodes.TypeNodes;

namespace InterpreterLib.Helpers
{
    public class RealHelper : IRealHelper
    {

        private Func<ExpressionNode, List<object>, double> _dispatchReal;
        private Func<ExpressionNode, List<object>, int> _dispatchInt;
        private Func<ExpressionNode, List<object>, TypeEnum, Object> _dispatch;
        private Func<FunctionNode, List<object>, double> _functionReal;

        private AST _root;

        public void SetASTRoot(AST root)
        {
            _root = root;
        }

        public void SetUpFuncs(Func<ExpressionNode, List<object>, double> dispatchReal,
                               Func<ExpressionNode, List<object>, int> dispatchInt,
                               Func<ExpressionNode, List<object>, TypeEnum, Object> dispatch,
                               Func<FunctionNode, List<object>, double> functionReal)
        {
            _dispatchReal = dispatchReal;
            _dispatchInt = dispatchInt;
            _dispatch = dispatch;
            _functionReal = functionReal;
        }

        public double ExportReal(ExportNode node, List<object> parameters)
        {
            return _dispatchReal(node.ExportValue, parameters);
        }

        public double ConditionReal(ConditionNode node, List<object> parameters)
        {
            return _dispatchReal(node.ReturnExpression, parameters);
        }

        public double AdditionReal(AdditionExpression node, List<object> parameters)
        {
            double leftOperand = _dispatchReal(node.Children[0], parameters);
            double rightOperand = _dispatchReal(node.Children[1], parameters);

            return leftOperand + rightOperand;
        }

        public double SubtractionReal(SubtractionExpression node, List<object> parameters)
        {
            double leftOperand = _dispatchReal(node.Children[0], parameters);
            double rightOperand = _dispatchReal(node.Children[1], parameters);

            return leftOperand - rightOperand;
        }

        public double MultiplicationReal(MultiplicationExpression node, List<object> parameters)
        {
            double leftOperand = _dispatchReal(node.Children[0], parameters);
            double rightOperand = _dispatchReal(node.Children[1], parameters);

            return leftOperand * rightOperand;
        }

        public double DivisionReal(DivisionExpression node, List<object> parameters)
        {
            double leftOperand = _dispatchReal(node.Children[0], parameters);
            double rightOperand = _dispatchReal(node.Children[1], parameters);

            if (rightOperand == 0.0) throw new Exception("Divisor cannot be zero");

            return leftOperand / rightOperand;
        }

        public double ModuloReal(ModuloExpression node, List<object> parameters)
        {
            double leftOperand = _dispatchReal(node.Children[0], parameters);
            double rightOperand = _dispatchReal(node.Children[1], parameters);

            if (rightOperand == 0.0) throw new Exception("Divisor cannot be zero");

            return leftOperand - rightOperand * Math.Floor(leftOperand / rightOperand);
        }

        public double AbsoluteReal(AbsoluteValueExpression node, List<object> parameters)
        {
            double operand = _dispatchReal(node.Children[0], parameters);

            return Math.Abs(operand);
        }
        public double PowerReal(PowerExpression node, List<object> parameters)
        {
            double leftOperand = _dispatchReal(node.Children[0], parameters);
            double rightOperand = _dispatchReal(node.Children[1], parameters);

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
            return Convert.ToDouble(_dispatchInt(node.Children[0], parameters));
        }

        public double FunctionCallReal(FunctionCallExpression node, List<object> parameters)
        {
            List<object> funcParameterValues = new List<object>();

            FunctionNode funcNode;
            if (node.GlobalReferences.Count >= 1)
                funcNode = _root.Functions[node.GlobalReferences[0]];
            else
                funcNode = _root.Functions[(int)parameters[node.LocalReference]];

            for (int i = 0; i < node.Children.Count; i++)
            {
                TypeEnum parameterType = funcNode.FunctionType.ParameterTypes[i].Type;
                funcParameterValues.Add(_dispatch(node.Children[i], parameters, parameterType));
            }

            return _functionReal(funcNode, funcParameterValues);
        }

    }
}