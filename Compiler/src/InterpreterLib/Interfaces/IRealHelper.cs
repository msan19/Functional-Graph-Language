using ASTLib;
using ASTLib.Nodes;
using ASTLib.Nodes.ExpressionNodes;
using System;
using System.Collections.Generic;

namespace InterpreterLib.Helpers
{
    public interface IRealHelper
    {

        Interpreter Interpreter { get; set; }

        double ExportReal(ExportNode node, List<object> parameters);

        double FunctionReal(FunctionNode node, List<object> parameters);

        public double AdditionReal(AdditionExpression node, List<object> parameters);

        public double SubtractionReal(SubtractionExpression node, List<object> parameters);

        public double MultiplicationReal(MultiplicationExpression node, List<object> parameters);

        public double DivisionReal(DivisionExpression node, List<object> parameters);

        public double ModuloReal(ModuloExpression node, List<object> parameters);

        public double AbsoluteReal(AbsoluteValueExpression node, List<object> parameters);
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