using System;
using System.Collections.Generic;
using ASTLib.Nodes.ExpressionNodes;
using ASTLib.Nodes.ExpressionNodes.CastExpressionNodes;
using ASTLib.Nodes.ExpressionNodes.NumberOperationNodes;
using ASTLib.Nodes.ExpressionNodes.OperationNodes;

namespace InterpreterLib.Interfaces
{
    public interface IStringHelper : IInterpreterHelper
    {
        void SetInterpreter(IInterpreterString interpreter);

        string AdditionString(AdditionExpression node, List<Object> parameters);
        string LiteralString(StringLiteralExpression node, List<Object> parameters);
        string CastIntegerToString(CastFromIntegerExpression node, List<Object> parameters);
        string CastBooleanToString(CastFromBooleanExpression node, List<Object> parameters);
        string CastRealToString(CastFromRealExpression node, List<Object> parameters);
    }
}
