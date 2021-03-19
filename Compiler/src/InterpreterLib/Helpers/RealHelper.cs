using ASTLib;
using ASTLib.Nodes;
using ASTLib.Nodes.ExpressionNodes;
using System;
using System.Collections.Generic;
using ASTLib.Nodes.ExpressionNodes.OperationNodes;
using InterpreterLib.Interfaces;
using System.Linq;

namespace InterpreterLib.Helpers
{
    public class RealHelper : IRealHelper
    {

        public IInterpreter Interpreter { get; set; }
        private AST _root;

        public void SetASTRoot(AST root)
        {
            _root = root;
        }

        public double ExportReal(ExportNode node, List<object> parameters)
        {
            return Interpreter.DispatchReal(node.ExportValue, parameters);
        }

        public double FunctionReal(FunctionNode node, List<object> parameters)
        {
            return ConditionReal(node.Conditions[0], parameters);
        }

        private double ConditionReal(ConditionNode node, List<object> parameters)
        {
            return Interpreter.DispatchReal(node.ReturnExpression, parameters);
        }

        public double AdditionReal(AdditionExpression node, List<object> parameters)
        {
            double leftOperand = Interpreter.DispatchReal(node.Children[0], parameters);
            double rightOperand = Interpreter.DispatchReal(node.Children[1], parameters);

            return leftOperand + rightOperand;
        }

        public double SubtractionReal(SubtractionExpression node, List<object> parameters)
        {
            double leftOperand = Interpreter.DispatchReal(node.Children[0], parameters);
            double rightOperand = Interpreter.DispatchReal(node.Children[1], parameters);

            return leftOperand - rightOperand;
        }

        public double MultiplicationReal(MultiplicationExpression node, List<object> parameters)
        {
            double leftOperand = Interpreter.DispatchReal(node.Children[0], parameters);
            double rightOperand = Interpreter.DispatchReal(node.Children[1], parameters);

            return leftOperand * rightOperand;
        }

        public double DivisionReal(DivisionExpression node, List<object> parameters)
        {
            double leftOperand = Interpreter.DispatchReal(node.Children[0], parameters);
            double rightOperand = Interpreter.DispatchReal(node.Children[1], parameters);

            if (rightOperand == 0.0) throw new Exception("Divisor cannot be zero");

            return leftOperand / rightOperand;
        }

        public double ModuloReal(ModuloExpression node, List<object> parameters)
        {
            double leftOperand = Interpreter.DispatchReal(node.Children[0], parameters);
            double rightOperand = Interpreter.DispatchReal(node.Children[1], parameters);

            if (rightOperand == 0.0) throw new Exception("Divisor cannot be zero");

            return leftOperand - rightOperand * Math.Floor(leftOperand / rightOperand);
        }

        public double AbsoluteReal(AbsoluteValueExpression node, List<object> parameters)
        {
            double operand = Interpreter.DispatchReal(node.Children[0], parameters);

            return Math.Abs(operand);
        }
        public double PowerReal(PowerExpression node, List<object> parameters)
        {
            double leftOperand = Interpreter.DispatchReal(node.Children[0], parameters);
            double rightOperand = Interpreter.DispatchReal(node.Children[1], parameters);

            return Math.Pow(leftOperand, rightOperand);
        }

        public double IdentifierReal(IdentifierExpression node, List<object> parameters)
        {
            // Gør noget med parameters[Reference]
            List<double> doubleParameters = new List<double>();
            foreach (object obj in parameters) doubleParameters.Add(Convert.ToDouble(obj));
            return doubleParameters[node.Reference];
        }

        public double LiteralReal(RealLiteralExpression node, List<object> parameters)
        {
            return node.Value;
        }

        public double CastIntegerToReal(CastFromIntegerExpression node, List<object> parameters)
        {
            return Convert.ToDouble(Interpreter.DispatchInt(node.Children[0], parameters));
        }

        public double FunctionCallReal(FunctionCallExpression node, List<object> parameters)
        {
            throw new NotImplementedException();
        }

    }
}