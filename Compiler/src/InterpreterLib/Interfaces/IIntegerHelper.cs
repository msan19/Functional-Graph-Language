using ASTLib;
using ASTLib.Nodes;
using ASTLib.Nodes.ExpressionNodes;
using System;
using System.Collections.Generic;

namespace InterpreterLib.Helpers
{
    public interface IIntegerHelper
    {

        Interpreter Interpreter { get; set; }

        int FunctionInteger(FunctionNode node, List<Object> parameters);

        int AdditionInteger(AdditionExpression node, List<Object> parameters);

        int SubtractionInteger(SubtractionExpression node, List<Object> parameters);

        int MultiplicationInteger(MultiplicationExpression node, List<Object> parameters);

        int DivisionInteger(DivisionExpression node, List<Object> parameters);

        int ModuloInteger(ModuloExpression node, List<Object> parameters);

        int AbsoluteInteger(AbsoluteValueExpression node, List<Object> parameters);

        int PowerInteger(PowerExpression node, List<Object> parameters);

        int IdentifierInteger(IdentifierExpression node, List<Object> parameters);

        int LiteralInteger(IntegerLiteralExpression node, List<Object> parameters);

        int FunctionCallInteger(FunctionCallExpression node, List<Object> parameters);

    }
}