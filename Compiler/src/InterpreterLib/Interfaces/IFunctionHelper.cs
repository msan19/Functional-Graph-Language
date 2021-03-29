using System;
using System.Collections.Generic;
using ASTLib;
using ASTLib.Nodes;
using ASTLib.Nodes.ExpressionNodes;
using ASTLib.Nodes.TypeNodes;

namespace InterpreterLib.Interfaces
{
    public interface IFunctionHelper
    {
        void SetAST(AST root);

        public void SetUpFuncs(Func<ExpressionNode, List<Object>, int> dispatchFunction,
                               Func<ExpressionNode, List<object>, TypeEnum, Object> dispatch,
                               Func<FunctionNode, List<Object>, int> functionFunction);

        int ConditionFunction(ConditionNode node, List<Object> parameters);

        int IdentifierFunction(IdentifierExpression node, List<Object> parameters);

        int FunctionCallFunction(FunctionCallExpression node, List<Object> parameters);

    }
}