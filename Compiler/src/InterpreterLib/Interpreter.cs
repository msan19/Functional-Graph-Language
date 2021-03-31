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

namespace InterpreterLib
{
    public class Interpreter : IInterpreter
    {
        private readonly IFunctionHelper _functionHelper;
        private readonly IIntegerHelper _integerHelper;
        private readonly IRealHelper _realHelper;
        private readonly IBooleanHelper _booleanHelper;

        public Interpreter(IFunctionHelper functionHelper, IIntegerHelper integerHelper, IRealHelper realHelper, IBooleanHelper booleanHelper)
        {
            _functionHelper = functionHelper;
            _functionHelper.SetUpFuncs(DispatchFunction, Dispatch, FunctionFunction);

            _integerHelper = integerHelper;
            _integerHelper.SetUpFuncs(DispatchInt, Dispatch, FunctionInteger);

            _realHelper = realHelper;
            _realHelper.SetUpFuncs(DispatchReal, DispatchInt, Dispatch, FunctionReal);
            
            _booleanHelper = booleanHelper;
            _booleanHelper.SetUpFuncs(DispatchBoolean,  DispatchInt, DispatchReal, Dispatch, FunctionBoolean);
        }

        public List<double> Interpret(AST node)
        {
            _functionHelper.SetAST(node);
            _integerHelper.SetASTRoot(node);
            _realHelper.SetASTRoot(node);
            _booleanHelper.SetASTRoot(node);
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
                FunctionCallExpression e    => _realHelper.FunctionCallReal(e, parameters),
                _ => throw new UnimplementedInterpreterException(node, "DispatchReal")
            };
        }

        public int DispatchFunction(ExpressionNode node, List<Object> parameters)
        {
            return node switch
            {
                IdentifierExpression e => _functionHelper.IdentifierFunction(e, parameters),
                FunctionCallExpression e => _functionHelper.FunctionCallFunction(e, parameters),
                _ => throw new UnimplementedInterpreterException(node, "DispatchFunction")
            };
        }

        public bool DispatchBoolean(ExpressionNode node, List<object> parameters)
        {
            return node switch
            {
                //GreaterExpression e => _booleanHelper.GreaterBoolean(e, parameters),
                //LessExpression e => _booleanHelper.LessBoolean(e, parameters),
                //GreaterEqualExpression e => _booleanHelper.GreaterEqualBoolean(e, parameters),
                //LessEqualExpression e => _booleanHelper.LessEqualBoolean(e, parameters),
                //EqualExpression e => _booleanHelper.EqualBoolean(e, parameters),
                //NotEqualExpression e => _booleanHelper.NotEqualBoolean(e, parameters),
                //NotExpression e => _booleanHelper.NotBoolean(e, parameters),
                //AndExpression e => _booleanHelper.AndBoolean(e, parameters),
                //OrExpression e => _booleanHelper.OrBoolean(e, parameters),
            };
        }
        
        public object Dispatch(ExpressionNode node, List<object> parameters, TypeEnum type)
        {
            return type switch
            {
                TypeEnum.Integer    => (object) DispatchInt(node, parameters),
                TypeEnum.Real       => (object) DispatchReal(node, parameters),
                TypeEnum.Function   => (object) DispatchFunction(node, parameters),
                _ => throw new UnimplementedASTException(type.ToString(), "type")
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

        public bool FunctionBoolean(FunctionNode node, List<object> parameters)
        {
            throw new NotImplementedException();
        }

        public int FunctionFunction(FunctionNode node, List<Object> parameters)
        {
            return _functionHelper.ConditionFunction(node.Conditions[0], parameters);
        }
    }
}