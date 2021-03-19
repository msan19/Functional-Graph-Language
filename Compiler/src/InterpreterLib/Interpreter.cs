using System;
using System.Collections.Generic;
using ASTLib;
using ASTLib.Nodes.ExpressionNodes;
using InterpreterLib.Helpers;
using InterpreterLib.Interfaces;

namespace InterpreterLib
{
    public class Interpreter : IInterpreter
    {
        private readonly IFunctionHelper _functionHelper;
        private readonly IIntegerHelper _integerHelper;
        private readonly IRealHelper _realHelper;

        public Interpreter(IFunctionHelper functionHelper, IIntegerHelper integerHelper, IRealHelper realHelper)
        {
            _functionHelper = functionHelper;
            _functionHelper.Interpreter = this;

            _integerHelper = integerHelper;
            _integerHelper.Interpreter = this;

            _realHelper = realHelper;
            _realHelper.Interpreter = this;
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
            return node switch
            {
                FunctionCallExpression e => _functionHelper.FunctionCallFunction(e, parameters),
                _ => throw new Exception($"{node.GetType()} has not been implemented in DispatchFunction")
            };
        }
    }
}