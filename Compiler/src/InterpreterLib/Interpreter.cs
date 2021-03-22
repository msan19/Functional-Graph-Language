﻿using System;
using System.Collections.Generic;
using ASTLib;
using ASTLib.Nodes;
using ASTLib.Nodes.ExpressionNodes;
using ASTLib.Nodes.ExpressionNodes.OperationNodes;
using ASTLib.Nodes.TypeNodes;
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
                IntegerLiteralExpression e  => _integerHelper.LiteralInteger(e, parameters),
                IdentifierExpression e      => _integerHelper.IdentifierInteger(e, parameters),
                SubtractionExpression e     => _integerHelper.SubtractionInteger(e, parameters),
                AdditionExpression e        => _integerHelper.AdditionInteger(e, parameters),
                MultiplicationExpression e  => _integerHelper.MultiplicationInteger(e, parameters),
                DivisionExpression e        => _integerHelper.DivisionInteger(e, parameters),
                ModuloExpression e          => _integerHelper.ModuloInteger(e, parameters),
                AbsoluteValueExpression e   => _integerHelper.AbsoluteInteger(e, parameters),
                FunctionCallExpression e    => _integerHelper.FunctionCallInteger(e, parameters),
                _ => throw new Exception($"{node.GetType()} has not been implemented in DispatchFunction")
            };
        }

        public double DispatchReal(ExpressionNode node, List<Object> parameters)
        {
            return node switch
            {
                PowerExpression e           => _realHelper.PowerReal(e, parameters),
                CastFromIntegerExpression e => _realHelper.CastIntegerToReal(e, parameters),
                RealLiteralExpression e     => _realHelper.LiteralReal(e, parameters),
                IdentifierExpression e      => _realHelper.IdentifierReal(e, parameters),
                SubtractionExpression e     => _realHelper.SubtractionReal(e, parameters),
                AdditionExpression e        => _realHelper.AdditionReal(e, parameters),
                MultiplicationExpression e  => _realHelper.MultiplicationReal(e, parameters),
                DivisionExpression e        => _realHelper.DivisionReal(e, parameters),
                ModuloExpression e          => _realHelper.ModuloReal(e, parameters),
                AbsoluteValueExpression e   => _realHelper.AbsoluteReal(e, parameters),
                FunctionCallExpression e    => _realHelper.FunctionCallReal(e, parameters),
                _ => throw new Exception($"{node.GetType()} has not been implemented in DispatchFunction")
            };
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

        public object Dispatch(ExpressionNode node, List<object> parameters, TypeEnum type)
        {
            return type switch
            {
                TypeEnum.Integer    => (object) DispatchInt(node, parameters),
                TypeEnum.Real       => (object) DispatchReal(node, parameters),
                TypeEnum.Function   => (object) DispatchFunction(node, parameters),
                _ => throw new Exception("no type")
            };
        }

        public int FunctionInteger(FunctionNode node, List<Object> parameters)
        {
            return _integerHelper.ConditionInteger(node.Conditions[0], parameters);
        }

        public double FunctionReal(FunctionNode node, List<object> parameters)
        {
            return _realHelper.ConditionReal(node.Conditions[0], parameters);
        }

        public int FunctionFunction(FunctionNode node, List<Object> parameters)
        {
            return _functionHelper.ConditionFunction(node.Conditions[0], parameters);
        }
    }
}