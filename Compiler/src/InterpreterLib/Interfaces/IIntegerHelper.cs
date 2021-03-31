using System;
using System.Collections.Generic;
using ASTLib;
using ASTLib.Nodes;
using ASTLib.Nodes.ExpressionNodes;
using ASTLib.Nodes.ExpressionNodes.OperationNodes;
using ASTLib.Nodes.TypeNodes;

namespace InterpreterLib.Interfaces
{
    public interface IIntegerHelper : IInterpretorHelper
    {
        void SetInterpreter(IInterpreterInteger interpreter);

        int ConditionInteger(ConditionNode node, List<Object> parameters);

        int AdditionInteger(AdditionExpression node, List<Object> parameters);

        int SubtractionInteger(SubtractionExpression node, List<Object> parameters);

        int MultiplicationInteger(MultiplicationExpression node, List<Object> parameters);

        int DivisionInteger(DivisionExpression node, List<Object> parameters);

        int ModuloInteger(ModuloExpression node, List<Object> parameters);

        int AbsoluteInteger(AbsoluteValueExpression node, List<Object> parameters);
        
        int IdentifierInteger(IdentifierExpression node, List<Object> parameters);

        int LiteralInteger(IntegerLiteralExpression node, List<Object> parameters);

        int FunctionCallInteger(FunctionCallExpression node, List<Object> parameters);
    }
}