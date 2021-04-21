using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using ASTLib;
using ASTLib.Exceptions;
using ASTLib.Exceptions.Component;
using ASTLib.Nodes;
using ASTLib.Nodes.ExpressionNodes;
using ASTLib.Nodes.ExpressionNodes.BooleanOperationNodes;
using ASTLib.Nodes.ExpressionNodes.CommonOperationNodes;
using ASTLib.Nodes.ExpressionNodes.CommonOperationNodes.ElementAndSetOperations;
using ASTLib.Nodes.ExpressionNodes.CommonOperationNodes.RelationalOperationNodes;
using ASTLib.Nodes.ExpressionNodes.NumberOperationNodes;
using ASTLib.Nodes.ExpressionNodes.OperationNodes;
using ASTLib.Nodes.ExpressionNodes.SetOperationNodes;
using ASTLib.Nodes.TypeNodes;
using ASTLib.Objects;
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
        private readonly ISetHelper _setHelper;
        private readonly IElementHelper _elementHelper;
        private readonly IStringHelper _stringHelper;
        private readonly IGraphHelper _graphHelper;


        public Interpreter(IGenericHelper genericHelper, IFunctionHelper functionHelper, IIntegerHelper integerHelper, IRealHelper realHelper, IBooleanHelper booleanHelper, ISetHelper setHelper, IElementHelper elementHelper, IStringHelper stringHelper, IGraphHelper graphHelper)
        {
            _functionHelper = functionHelper;

            _integerHelper = integerHelper;
            _integerHelper.SetInterpreter(this);

            _realHelper = realHelper;
            _realHelper.SetInterpreter(this);

            _booleanHelper = booleanHelper;
            _booleanHelper.SetInterpreter(this);

            _genericHelper = genericHelper;
            _genericHelper.SetInterpreter(this);

            _setHelper = setHelper;
            _setHelper.SetInterpreter(this);

            _elementHelper = elementHelper;
            _elementHelper.SetInterpreter(this);

            _stringHelper = stringHelper;
            _stringHelper.SetInterpreter(this);

            _graphHelper = graphHelper;
            _graphHelper.SetInterpreter(this);
        }

        public List<Set> Interpret(AST node)
        {
            /*
            _genericHelper.SetASTRoot(node);
            List<Set> results = new List<Set>();
            foreach (ExportNode n in node.Exports) 
                results.Add(_setHelper.ExportSet(n));
            return results;
            */
            return null;
        }

        public Set DispatchSet(ExpressionNode node, List<Object> parameters)
        {
            return node switch
            {
                SetExpression e             => _setHelper.SetExpression(e, parameters),
                UnionExpression e           => _setHelper.UnionSet(e, parameters),
                IntersectionExpression e    => _setHelper.IntersectionSet(e, parameters),
                SubtractionExpression e     => _setHelper.SubtractionSet(e, parameters),
                FunctionCallExpression e    => _genericHelper.FunctionCall<Set>(e, parameters),
                _ => throw new UnimplementedInterpreterException(node, "DispatctSet")
            };
        }

        public Element DispatchElement(ExpressionNode node, List<Object> parameters)
        {
            return node switch
            {
                ElementExpression e         => _elementHelper.Element(e, parameters),
                IdentifierExpression e      => _genericHelper.Identifier<Element>(e, parameters),
                FunctionCallExpression e    => _genericHelper.FunctionCall<Element>(e, parameters),
                _ => throw new UnimplementedInterpreterException(node, "DispatctSet")
            };
        }

        public int DispatchInt(ExpressionNode node, List<Object> parameters)
        {
            return node switch
            {
                PowerExpression e           => _integerHelper.PowerInteger(e, parameters),
                IntegerLiteralExpression e  => _integerHelper.LiteralInteger(e, parameters),
                IdentifierExpression e      => _genericHelper.Identifier<int>(e, parameters),
                SubtractionExpression e     => _integerHelper.SubtractionInteger(e, parameters),
                AdditionExpression e        => _integerHelper.AdditionInteger(e, parameters),
                MultiplicationExpression e  => _integerHelper.MultiplicationInteger(e, parameters),
                DivisionExpression e        => _integerHelper.DivisionInteger(e, parameters),
                ModuloExpression e          => _integerHelper.ModuloInteger(e, parameters),
                AbsoluteValueExpression e   => _integerHelper.AbsoluteInteger(e, parameters),
                NegativeExpression e        => _integerHelper.NegativeInteger(e, parameters),
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
                IdentifierExpression e      => _genericHelper.Identifier<double>(e, parameters),
                SubtractionExpression e     => _realHelper.SubtractionReal(e, parameters),
                AdditionExpression e        => _realHelper.AdditionReal(e, parameters),
                MultiplicationExpression e  => _realHelper.MultiplicationReal(e, parameters),
                DivisionExpression e        => _realHelper.DivisionReal(e, parameters),
                ModuloExpression e          => _realHelper.ModuloReal(e, parameters),
                AbsoluteValueExpression e   => _realHelper.AbsoluteReal(e, parameters),
                NegativeExpression e        => _realHelper.NegativeReal(e, parameters),
                FunctionCallExpression e    => _genericHelper.FunctionCall<double>(e, parameters),
                _ => throw new UnimplementedInterpreterException(node, "DispatchReal")
            };
        }

        public Function DispatchFunction(ExpressionNode node, List<Object> parameters)
        {
            return node switch
            {
                IdentifierExpression e      => _functionHelper.IdentifierFunction(e, parameters),
                FunctionCallExpression e    => _genericHelper.FunctionCall<Function>(e, parameters),
                _ => throw new UnimplementedInterpreterException(node, "DispatchFunction")
            };
        }

        public bool DispatchBoolean(ExpressionNode node, List<Object> parameters)
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
                IdentifierExpression e      => _genericHelper.Identifier<bool>(e, parameters),
                FunctionCallExpression e    => _genericHelper.FunctionCall<bool>(e, parameters),
                InExpression e              => _booleanHelper.InBoolean(e, parameters),
                SubsetExpression e          => _booleanHelper.SubsetBoolean(e, parameters),
                BooleanLiteralExpression e  => _booleanHelper.LiteralBoolean(e),
                _ => throw new UnimplementedInterpreterException(node, "DispatchBoolean")
            };
        }

        /*
         * AdditionExpression
         * StringLiteralExpression
         * FunctionCallExpression
         * IdentifierExpression*/
        public string DispatchString(ExpressionNode node, List<Object> parameters)
        {
            return node switch
            {
                AdditionExpression      e => _stringHelper.AdditionString(e, parameters),           
                _ => throw new UnimplementedInterpreterException(node, "DispatchString")
            };
        }

        public Graph DispatchGraph(ExpressionNode node, List<object> parameters)
        {
            return node switch
            {
                GraphExpression e => _graphHelper.GraphExpression(e, parameters),
                _ => throw new UnimplementedInterpreterException(node, "DispatchGraph")
            };
        }


        public Object Dispatch(ExpressionNode node, List<Object> parameters, TypeEnum type)
        {
            return type switch
            {
                TypeEnum.Integer    => (Object) DispatchInt(node, parameters),
                TypeEnum.Real       => (Object) DispatchReal(node, parameters),
                TypeEnum.Function   => (Object) DispatchFunction(node, parameters),
                TypeEnum.Boolean    => (Object) DispatchBoolean(node, parameters),
                TypeEnum.Set        => (Object) DispatchSet(node, parameters),
                TypeEnum.Element    => (Object) DispatchElement(node, parameters),
                TypeEnum.String     => (Object) DispatchString(node, parameters),
                TypeEnum.Graph      => (Object) DispatchGraph(node, parameters),

                _ => throw new UnimplementedASTException(type.ToString(), "type")
            };
        }

        public T Function<T>(FunctionNode node, List<Object> parameters)
        {
            T result = default;
            int returnedValues = 0;
            ConditionNode defaultCase = null;
            List<ConditionNodeException> exceptionNodes = new List<ConditionNodeException>();
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
                        
                        exceptionNodes.Add(new ConditionNodeException(child));
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
            {
                ComponentException exceptions = new ComponentException();
                exceptions.Exceptions.Add(new UnacceptedConditionsException(returnedValues));
                exceptions.Exceptions.AddRange(exceptionNodes);
                throw exceptions;
            }
                
            return result;
        }
    }
}