using ASTLib;
using ASTLib.Nodes;
using ASTLib.Nodes.ExpressionNodes;
using System;
using System.Collections.Generic;

namespace InterpreterLib.Helpers
{
    public class IntegerHelper
    {

        public IInterpreter Interpreter { get; set; }

        public int FunctionInteger(FunctionNode node, List<Object> parameters)
        {
            throw new NotImplementedException();
        }

        private int ConditionInteger(ConditionNode node, List<Object> parameters)
        {
            throw new NotImplementedException();
        }

        public int AdditionInteger(AdditionExpression node, List<Object> parameters)
        {
            throw new NotImplementedException();
        }

        public int SubtractionInteger(SubtractionExpression node, List<Object> parameters)
        {
            throw new NotImplementedException();
        }

        public int MultiplicationInteger(MultiplicationExpression node, List<Object> parameters)
        {
            throw new NotImplementedException();
        }

        public int DivisionInteger(DivisionExpression node, List<Object> parameters)
        {
            throw new NotImplementedException();
        }

        public int ModuloInteger(ModuloExpression node, List<Object> parameters)
        {
            throw new NotImplementedException();
        }

        public int AbsoluteInteger(AbsoluteValueExpression node, List<Object> parameters)
        {
            throw new NotImplementedException();
        }

        public int PowerInteger(PowerExpression node, List<Object> parameters)
        {
            throw new NotImplementedException();
        }

        public int IdentifierInteger(IdentifierExpression node, List<Object> parameters)
        {
            throw new NotImplementedException();
        }

        public int LiteralInteger(IntegerLiteralExpression node, List<Object> parameters)
        {
            throw new NotImplementedException();
        }

        public int FunctionCallInteger(FunctionCallExpression node, List<Object> parameters)
        {
            throw new NotImplementedException();
        }

    }
}