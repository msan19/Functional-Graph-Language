using System;
using System.Collections.Generic;
using ASTLib;
using ASTLib.Nodes;
using ASTLib.Nodes.ExpressionNodes;
using ASTLib.Nodes.ExpressionNodes.BooleanOperationNodes;
using ASTLib.Nodes.ExpressionNodes.CommonOperationNodes;
using ASTLib.Nodes.ExpressionNodes.CommonOperationNodes.RelationalOperationNodes;
using ASTLib.Nodes.TypeNodes;

namespace InterpreterLib.Interfaces
{
    public interface IBooleanHelper : IInterpretorHelper
    {
        IInterpreter Interpreter { get; set; }
        
        void SetUpFuncs(Func<ExpressionNode, List<object>, bool> dispatchBoolean,
                        Func<ExpressionNode, List<object>, int> dispatchInteger,
                        Func<ExpressionNode, List<object>, double> dispatchReal,
                        Func<ExpressionNode, List<object>, TypeEnum, object> dispatch,
                        Func<FunctionNode,   List<object>, bool> functionBoolean);

        bool ConditionBoolean(ConditionNode node, List<object> parameters);

        bool IdentifierBoolean(IdentifierExpression node, List<object> parameters);

        bool NotBoolean(NotExpression node, List<object> parameters);

        bool AndBoolean(AndExpression node, List<object> parameters);

        bool OrBoolean(OrExpression node, List<object> parameters);

        bool EqualBoolean(EqualExpression node, List<object> parameters);

        bool NotEqualBoolean(NotEqualExpression node, List<object> parameters);

        bool GreaterEqualBoolean(GreaterEqualExpression node, List<object> parameters);

        bool GreaterBoolean(GreaterExpression node, List<object> parameters);

        bool LessEqualBoolean(LessEqualExpression node, List<object> parameters);

        bool LessBoolean(LessExpression node, List<object> parameters);

        bool FunctionCallBoolean(FunctionCallExpression node, List<object> parameters);
    }
}
