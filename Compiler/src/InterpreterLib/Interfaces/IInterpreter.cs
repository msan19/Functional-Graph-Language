using ASTLib;
using ASTLib.Nodes.ExpressionNodes;
using System.Collections.Generic;

namespace InterpreterLib
{
    public interface IInterpreter
    {
        int DispatchFunction(ExpressionNode node, List<object> parameters);
        int DispatchInt(ExpressionNode node, List<object> parameters);
        double DispatchReal(ExpressionNode node, List<object> parameters);
        List<double> Interpret(AST node);
    }
}