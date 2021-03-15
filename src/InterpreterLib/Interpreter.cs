using System;
using ASTLib;

namespace InterpreterLib
{
    public class Interpreter
    {
        private readonly InterpretorHelper _helper;

        public Interpreter(InterpretorHelper helper)
        {
            _helper = helper;
        }

        public string Interpret(AST root)
        {
            throw new NotImplementedException();
        }
    }
}