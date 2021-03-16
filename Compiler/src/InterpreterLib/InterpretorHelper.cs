using ASTLib;
using ASTLib.Nodes;
using ASTLib.Nodes.ExpressionNodes;
using System;

namespace InterpreterLib
{
    public class InterpretorHelper
    {

        public Interpreter Interpreter { get; set; }

        public double ExportDouble(ExportNode node)
        {
            throw new NotImplementedException();
        }

        public double FunctionDouble(FunctionNode node)
        {
            throw new NotImplementedException();
        }

        private double ConditionDouble(ConditionNode node)
        {
            throw new NotImplementedException();
        }

        public int Function(FunctionNode node)
        {
            throw new NotImplementedException();
        }

        private int Condition(ConditionNode node)
        {
            throw new NotImplementedException();
        }

        public int AdditionInteger(AdditionExpression node)
        {
            throw new NotImplementedException();
        }

        public double AdditionReal(AdditionExpression node)
        {
            throw new NotImplementedException();
        }

        public int SubtractionInteger(SubtractionExpression node)
        {
            throw new NotImplementedException();
        }

        public double SubtractionReal(SubtractionExpression node)
        {
            throw new NotImplementedException();
        }

        public int MultiplicationInteger(MultiplicationExpression node)
        {
            throw new NotImplementedException();
        }

        public double MultiplicationReal(MultiplicationExpression node)
        {
            throw new NotImplementedException();
        }

        public int DivisionInteger(DivisionExpression node)
        {
            throw new NotImplementedException();
        }

        public double DivisionReal(DivisionExpression node)
        {
            throw new NotImplementedException();
        }

        public int ModuloInteger(ModuloExpression node)
        {
            throw new NotImplementedException();
        }

        public double ModuloReal(ModuloExpression node)
        {
            throw new NotImplementedException();
        }

        public int AbsoluteInteger(AbsoluteValueExpression node)
        {
            throw new NotImplementedException();
        }

        public double AbsoluteReal(AbsoluteValueExpression node)
        {
            throw new NotImplementedException();
        }

        public int PowerInteger(PowerExpression node)
        {
            throw new NotImplementedException();
        }

        public double PowerReal(PowerExpression node)
        {
            throw new NotImplementedException();
        }

        public int IdentifierFunction(IdentifierExpression node)
        {
            throw new NotImplementedException();
        }

        public double IdentifierReal(IdentifierExpression node)
        {
            throw new NotImplementedException();
        }

        public int IdentifierInteger(IdentifierExpression node)
        {
            throw new NotImplementedException();
        }

        public double LiteralReal(RealLiteralExpression node)
        {
            throw new NotImplementedException();
        }

        public int LiteralInteger(IntegerLiteralExpression node)
        {
            throw new NotImplementedException();
        }

        public double CastIntegerToReal(IntegerCastExpression node)
        {
            throw new NotImplementedException();
        }

        public int FunctionCallFunction(FunctionCallExpression node)
        {
            throw new NotImplementedException();
        }

        public double FunctionCallReal(FunctionCallExpression node)
        {
            throw new NotImplementedException();
        }

        public int FunctionCallInteger(FunctionCallExpression node)
        {
            throw new NotImplementedException();
        }

    }
}