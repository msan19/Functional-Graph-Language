using System;
using System.Collections.Generic;
using ASTLib;
using ASTLib.Nodes;
using ASTLib.Nodes.ExpressionNodes;
using ASTLib.Nodes.TypeNodes;
using InterpreterLib.MatchPair;

namespace InterpreterLib.Interfaces
{
    public interface IGenericHelper : IInterpreterHelper
    {

        T Identifier<T>(IdentifierExpression node, List<object> parameters);

        void SetInterpreter(IInterpreterGeneric interpreter);

        void SetASTRoot(AST root);

        T FunctionCall<T>(FunctionCallExpression node, List<Object> parameters);

        MatchPair<T> Condition<T>(ConditionNode node, List<object> parameters);
    }
}