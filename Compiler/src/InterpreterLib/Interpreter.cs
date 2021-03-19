﻿using System;
using System.Collections.Generic;
using ASTLib;
using ASTLib.Nodes;
using ASTLib.Nodes.ExpressionNodes;
using ASTLib.Nodes.ExpressionNodes.OperationNodes;
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
            List<double> results = new List<double>();
            foreach (ExportNode n in node.Exports) results.Add(_realHelper.ExportReal(n, new List<object>()));
            return results;
        }

        public int DispatchInt(ExpressionNode node, List<Object> parameters)
        {
            return node switch
            {
                SubtractionExpression e     => _integerHelper.SubtractionInteger(e, parameters),
                AdditionExpression e        => _integerHelper.AdditionInteger(e, parameters),
                MultiplicationExpression e  => _integerHelper.MultiplicationInteger(e, parameters),
                DivisionExpression e        => _integerHelper.DivisionInteger(e, parameters),
                FunctionCallExpression e    => _integerHelper.FunctionCallInteger(e, parameters),
                _ => throw new Exception($"{node.GetType()} has not been implemented in DispatchFunction")
            };
        }

        public double DispatchReal(ExpressionNode node, List<Object> parameters)
        {
            throw new NotImplementedException();
        }

        public int DispatchFunction(ExpressionNode node, List<Object> parameters)
        {
            return node switch
            {
                IdentifierExpression e => _functionHelper.IdentifierFunction(e, parameters),
                FunctionCallExpression e => _functionHelper.FunctionCallFunction(e, parameters),
                _ => throw new Exception($"{node.GetType()} has not been implemented in DispatchFunction")
            };
        }
    }
}