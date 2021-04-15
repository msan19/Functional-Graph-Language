using System;
using System.Collections.Generic;
using ASTLib;
using ASTLib.Nodes;
using ASTLib.Nodes.ExpressionNodes;
using ASTLib.Nodes.ExpressionNodes.BooleanOperationNodes;
using ASTLib.Nodes.ExpressionNodes.CommonOperationNodes;
using ASTLib.Nodes.ExpressionNodes.CommonOperationNodes.ElementAndSetOperations;
using ASTLib.Nodes.ExpressionNodes.CommonOperationNodes.RelationalOperationNodes;
using ASTLib.Nodes.TypeNodes;

namespace InterpreterLib.Interfaces
{
    public interface IBooleanHelper : IInterpreterHelper
    {

        void SetInterpreter(IInterpreterBoolean interpreter);

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

        bool InBoolean(InExpression node, List<object> parameters);
    }
}
