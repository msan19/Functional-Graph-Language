using ASTLib;
using ASTLib.Nodes;
using ASTLib.Nodes.ExpressionNodes;
using System;
using System.Collections.Generic;

namespace InterpreterLib
{
    public class InterpretorHelper
    {

        public Interpreter Interpreter { get; set; }

        public double ExportDouble(ExportNode node, List<Object> parameters)
        {
            throw new NotImplementedException();
        }

        public double FunctionDouble(FunctionNode node, List<Object> parameters)
        {
            throw new NotImplementedException();
        }

        private double ConditionDouble(ConditionNode node, List<Object> parameters)
        {
            throw new NotImplementedException();
        }

        public int Function(FunctionNode node, List<Object> parameters)
        {
            throw new NotImplementedException();
        }

        private int Condition(ConditionNode node, List<Object> parameters)
        {
            throw new NotImplementedException();
        }

        public int AdditionInteger(AdditionExpression node, List<Object> parameters)
        {
            throw new NotImplementedException();
        }

        public double AdditionReal(AdditionExpression node, List<Object> parameters)
        {
            throw new NotImplementedException();
        }

        public int SubtractionInteger(SubtractionExpression node, List<Object> parameters)
        {
            throw new NotImplementedException();
        }

        public double SubtractionReal(SubtractionExpression node, List<Object> parameters)
        {
            throw new NotImplementedException();
        }

        public int MultiplicationInteger(MultiplicationExpression node, List<Object> parameters)
        {
            throw new NotImplementedException();
        }

        public double MultiplicationReal(MultiplicationExpression node, List<Object> parameters)
        {
            throw new NotImplementedException();
        }

        public int DivisionInteger(DivisionExpression node, List<Object> parameters)
        {
            throw new NotImplementedException();
        }

        public double DivisionReal(DivisionExpression node, List<Object> parameters)
        {
            throw new NotImplementedException();
        }

        public int ModuloInteger(ModuloExpression node, List<Object> parameters)
        {
            throw new NotImplementedException();
        }

        public double ModuloReal(ModuloExpression node, List<Object> parameters)
        {
            throw new NotImplementedException();
        }

        public int AbsoluteInteger(AbsoluteValueExpression node, List<Object> parameters)
        {
            throw new NotImplementedException();
        }

        public double AbsoluteReal(AbsoluteValueExpression node, List<Object> parameters)
        {
            throw new NotImplementedException();
        }

        public int PowerInteger(PowerExpression node, List<Object> parameters)
        {
            throw new NotImplementedException();
        }

        public double PowerReal(PowerExpression node, List<Object> parameters)
        {
            throw new NotImplementedException();
        }

        public int IdentifierFunction(IdentifierExpression node, List<Object> parameters)
        {
            throw new NotImplementedException();
        }

        public double IdentifierReal(IdentifierExpression node, List<Object> parameters)
        {
            throw new NotImplementedException();
        }

        public int IdentifierInteger(IdentifierExpression node, List<Object> parameters)
        {
            throw new NotImplementedException();
        }

        public double LiteralReal(RealLiteralExpression node, List<Object> parameters)
        {
            throw new NotImplementedException();
        }

        public int LiteralInteger(IntegerLiteralExpression node, List<Object> parameters)
        {
            throw new NotImplementedException();
        }

        public double CastIntegerToReal(IntegerCastExpression node, List<Object> parameters)
        {
            throw new NotImplementedException();
        }

        public int FunctionCallFunction(FunctionCallExpression node, List<Object> parameters)
        {
            throw new NotImplementedException();
        }

        public double FunctionCallReal(FunctionCallExpression node, List<Object> parameters)
        {
            throw new NotImplementedException();
        }

        public int FunctionCallInteger(FunctionCallExpression node, List<Object> parameters)
        {
            throw new NotImplementedException();
        }

    }
}