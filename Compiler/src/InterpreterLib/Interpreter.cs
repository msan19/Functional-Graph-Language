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
            _helper.interpreter = this;
        }

        public List<double> Interpret(AST node)
        {
            throw new NotImplementedException();
        }

        public int DispatchInt(ExpressionNode node)
        {
            throw new NotImplementedException();
        }

        public double DispatchReal(ExpressionNode node)
        {
            throw new NotImplementedException();
        }

        public int DispatchFunction(ExpressionNode node)
        { 
            throw new NotImplementedException();
        }
    }
}