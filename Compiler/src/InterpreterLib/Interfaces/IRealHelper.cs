using System;
using System.Collections.Generic;
using ASTLib;
using ASTLib.Nodes;
using ASTLib.Nodes.ExpressionNodes;
using ASTLib.Nodes.ExpressionNodes.CastExpressionNodes;
using ASTLib.Nodes.ExpressionNodes.CommonOperationNodes;
using ASTLib.Nodes.ExpressionNodes.NumberOperationNodes;
using ASTLib.Nodes.ExpressionNodes.OperationNodes;
using ASTLib.Nodes.TypeNodes;

namespace InterpreterLib.Interfaces
{
    public interface IRealHelper : IInterpreterHelper
    {
        void SetInterpreter(IInterpreterReal interpreter);

        double ExportReal(ExportNode node, List<object> parameters);

        double AdditionReal(AdditionExpression node, List<object> parameters);

        double SubtractionReal(SubtractionExpression node, List<object> parameters);

        double MultiplicationReal(MultiplicationExpression node, List<object> parameters);

        double DivisionReal(DivisionExpression node, List<object> parameters);

        double ModuloReal(ModuloExpression node, List<object> parameters);

        double AbsoluteReal(AbsoluteValueExpression node, List<object> parameters);

        double PowerReal(PowerExpression node, List<object> parameters);

        double LiteralReal(RealLiteralExpression node, List<object> parameters);

        double CastIntegerToReal(CastFromIntegerExpression node, List<object> parameters);

        double NegativeReal(NegativeExpression node, List<object> parameters);
    }
}