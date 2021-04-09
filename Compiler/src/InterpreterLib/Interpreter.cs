using System;
using System.Collections.Generic;
using ASTLib;
using ASTLib.Exceptions;
using ASTLib.Nodes;
using ASTLib.Nodes.ExpressionNodes;
using ASTLib.Nodes.ExpressionNodes.BooleanOperationNodes;
using ASTLib.Nodes.ExpressionNodes.CommonOperationNodes;
using ASTLib.Nodes.ExpressionNodes.CommonOperationNodes.RelationalOperationNodes;
using ASTLib.Nodes.ExpressionNodes.OperationNodes;
using ASTLib.Nodes.TypeNodes;
using InterpreterLib.Helpers;
using InterpreterLib.Interfaces;
using InterpreterLib.MatchPair;

namespace InterpreterLib
{
    public class Interpreter : IInterpreter
    {
        private readonly IFunctionHelper _functionHelper;
        private readonly IIntegerHelper _integerHelper;
        private readonly IRealHelper _realHelper;
        private readonly IBooleanHelper _booleanHelper;
        private readonly IGenericHelper _genericHelper;

        public Interpreter(IGenericHelper genericHelper, IFunctionHelper functionHelper, IIntegerHelper integerHelper, IRealHelper realHelper, IBooleanHelper booleanHelper)
        {
            _functionHelper = functionHelper;
            _functionHelper.SetInterpreter(this);

            _integerHelper = integerHelper;
            _integerHelper.SetInterpreter(this);

            _realHelper = realHelper;
            _realHelper.SetInterpreter(this);

            _booleanHelper = booleanHelper;
            _booleanHelper.SetInterpreter(this);

            _genericHelper = genericHelper;
            _genericHelper.SetInterpreter(this);
        }

        public List<double> Interpret(AST node)
        {
            
            _genericHelper.SetASTRoot(node);
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
                FunctionCallExpression e    => _genericHelper.FunctionCall<int>(e, parameters),
                _ => throw new UnimplementedInterpreterException(node, "DispatchInt")
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
                FunctionCallExpression e    => _genericHelper.FunctionCall<double>(e, parameters),
                _ => throw new UnimplementedInterpreterException(node, "DispatchReal")
            };
        }

        public int DispatchFunction(ExpressionNode node, List<Object> parameters)
        {
            return node switch
            {
                IdentifierExpression e      => _functionHelper.IdentifierFunction(e, parameters),
                FunctionCallExpression e    => (int) _genericHelper.FunctionCall<long>(e, parameters),
                _ => throw new UnimplementedInterpreterException(node, "DispatchFunction")
            };
        }

        public bool DispatchBoolean(ExpressionNode node, List<object> parameters)
        {
            return node switch
            {
                GreaterExpression e         => _booleanHelper.GreaterBoolean(e, parameters),
                LessExpression e            => _booleanHelper.LessBoolean(e, parameters),
                GreaterEqualExpression e    => _booleanHelper.GreaterEqualBoolean(e, parameters),
                LessEqualExpression e       => _booleanHelper.LessEqualBoolean(e, parameters),
                EqualExpression e           => _booleanHelper.EqualBoolean(e, parameters),
                NotEqualExpression e        => _booleanHelper.NotEqualBoolean(e, parameters),
                NotExpression e             => _booleanHelper.NotBoolean(e, parameters),
                AndExpression e             => _booleanHelper.AndBoolean(e, parameters),
                OrExpression e              => _booleanHelper.OrBoolean(e, parameters),
                FunctionCallExpression e    => _genericHelper.FunctionCall<bool>(e, parameters),
                _ => throw new UnimplementedInterpreterException(node, "DispatchBoolean")
            };
        }
        
        public object Dispatch(ExpressionNode node, List<object> parameters, TypeEnum type)
        {
            return type switch
            {
                TypeEnum.Integer    => (object) DispatchInt(node, parameters),
                TypeEnum.Real       => (object) DispatchReal(node, parameters),
                TypeEnum.Function   => (object) DispatchFunction(node, parameters),
                TypeEnum.Boolean    => (object) DispatchBoolean(node, parameters),
                _ => throw new UnimplementedASTException(type.ToString(), "type")
            };
        }

        public T Function<T>(FunctionNode node, List<Object> parameters)
        {
            T result = default;
            int returnedValues = 0;
            ConditionNode defaultCase = null;
            ConditionNode exceptionNode = null;
            foreach (ConditionNode child in node.Conditions)
            {
                if (child.IsDefaultCase)
                    defaultCase = child;
                else
                {
                    MatchPair<T> value = _genericHelper.Condition<T>(child, parameters);
                    if (value.IsCalculated)
                    {
                        result = value.Element;
                        returnedValues++;
                        if (returnedValues > 0)
                            exceptionNode = child;
                    }
                }
            }
            if (returnedValues == 0 && defaultCase != null)
            {
                T a = _genericHelper.Condition<T>(defaultCase, parameters).Element;
                if (a == null)
                    throw new UnacceptedConditionsException(defaultCase);
                return a;
            }
            else if (returnedValues != 1)
                throw new UnacceptedConditionsException(exceptionNode, returnedValues);
            return result;
        }
    }
}