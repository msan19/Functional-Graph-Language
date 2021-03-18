using ASTLib;
using ASTLib.Nodes;
using ASTLib.Nodes.ExpressionNodes;
using System;
using System.Collections.Generic;

namespace InterpreterLib.Helpers
{
    public class RealHelper : IRealHelper
    {

        public IInterpreter Interpreter { get; set; }

        public double ExportReal(ExportNode node, List<object> parameters)
        {
            throw new NotImplementedException();
        }

        public double FunctionReal(FunctionNode node, List<object> parameters)
        {
            throw new NotImplementedException();
        }

        private double ConditionReal(ConditionNode node, List<object> parameters)
        {
            throw new NotImplementedException();
        }

        public double AdditionReal(AdditionExpression node, List<object> parameters)
        {
            throw new NotImplementedException();
        }

        public double SubtractionReal(SubtractionExpression node, List<object> parameters)
        {
            throw new NotImplementedException();
        }

        public double MultiplicationReal(MultiplicationExpression node, List<object> parameters)
        {
            throw new NotImplementedException();
        }

        public double DivisionReal(DivisionExpression node, List<object> parameters)
        {
            throw new NotImplementedException();
        }

        public double ModuloReal(ModuloExpression node, List<object> parameters)
        {
            throw new NotImplementedException();
        }

        public double AbsoluteReal(AbsoluteValueExpression node, List<object> parameters)
        {
            throw new NotImplementedException();
        }
        public double PowerReal(PowerExpression node, List<object> parameters)
        {
            throw new NotImplementedException();
        }

        public double IdentifierReal(IdentifierExpression node, List<object> parameters)
        {
            throw new NotImplementedException();
        }

        public double LiteralReal(RealLiteralExpression node, List<object> parameters)
        {
            throw new NotImplementedException();
        }

        public double CastIntegerToReal(IntegerCastExpression node, List<object> parameters)
        {
            throw new NotImplementedException();
        }

        public double FunctionCallReal(FunctionCallExpression node, List<object> parameters)
        {
            throw new NotImplementedException();
        }

    }
}