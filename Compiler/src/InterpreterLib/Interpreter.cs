using System;
using System.Collections.Generic;
using ASTLib;
using ASTLib.Nodes.ExpressionNodes;

namespace InterpreterLib
{
    public class Interpreter
    {
        private readonly InterpretorHelper _helper;

        public Interpreter(InterpretorHelper helper)
        {
            _helper = helper;
            _helper.Interpreter = this;
        }

        public List<double> Interpret(AST node)
        {
            throw new NotImplementedException();
        }

        public int DispatchInt(ExpressionNode node, List<Object> parameters)
        {
            throw new NotImplementedException();
        }

        public double DispatchReal(ExpressionNode node, List<Object> parameters)
        {
            throw new NotImplementedException();
        }

        public int DispatchFunction(ExpressionNode node, List<Object> parameters)
        { 
            throw new NotImplementedException();
        }
    }
}