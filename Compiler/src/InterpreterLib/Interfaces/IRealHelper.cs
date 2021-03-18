using ASTLib;
using ASTLib.Nodes;
using ASTLib.Nodes.ExpressionNodes;
using System;
using System.Collections.Generic;

namespace InterpreterLib.Helpers
{
    public interface IRealHelper
    {
        IInterpreter Interpreter { get; set; }

        double ExportReal(ExportNode node, List<object> parameters);

        double FunctionReal(FunctionNode node, List<object> parameters);

        double AdditionReal(AdditionExpression node, List<object> parameters);

        double SubtractionReal(SubtractionExpression node, List<object> parameters);

        double MultiplicationReal(MultiplicationExpression node, List<object> parameters);

        double DivisionReal(DivisionExpression node, List<object> parameters);

        double ModuloReal(ModuloExpression node, List<object> parameters);

        double AbsoluteReal(AbsoluteValueExpression node, List<object> parameters);

        double PowerReal(PowerExpression node, List<object> parameters);

        double IdentifierReal(IdentifierExpression node, List<object> parameters);

        double LiteralReal(RealLiteralExpression node, List<object> parameters);

        double CastIntegerToReal(CastFromIntegerExpression node, List<object> parameters);

        double FunctionCallReal(FunctionCallExpression node, List<object> parameters);

    }
}