using System;
using System.Collections.Generic;
using ASTLib;
using ASTLib.Nodes;
using ASTLib.Nodes.ExpressionNodes;
using ASTLib.Nodes.ExpressionNodes.CommonOperationNodes;
using ASTLib.Nodes.ExpressionNodes.NumberOperationNodes;
using ASTLib.Nodes.ExpressionNodes.OperationNodes;
using ASTLib.Nodes.TypeNodes;

namespace InterpreterLib.Interfaces
{
    public interface IIntegerHelper : IInterpreterHelper
    {
        void SetInterpreter(IInterpreterInteger interpreter);

        int AdditionInteger(AdditionExpression node, List<Object> parameters);

        int SubtractionInteger(SubtractionExpression node, List<Object> parameters);

        int MultiplicationInteger(MultiplicationExpression node, List<Object> parameters);

        int DivisionInteger(DivisionExpression node, List<Object> parameters);

        int ModuloInteger(ModuloExpression node, List<Object> parameters);

        int AbsoluteInteger(AbsoluteValueExpression node, List<Object> parameters);

        int PowerInteger(PowerExpression node, List<Object> parameters);

        int LiteralInteger(IntegerLiteralExpression node, List<Object> parameters);
        
        int NegativeInteger(NegativeExpression node, List<Object> parameters);
    }
}