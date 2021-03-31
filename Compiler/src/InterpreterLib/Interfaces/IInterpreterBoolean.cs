using System;
using System.Collections.Generic;
using ASTLib;
using ASTLib.Nodes;
using ASTLib.Nodes.ExpressionNodes;
using ASTLib.Nodes.TypeNodes;

namespace InterpreterLib.Interfaces
{
    public interface IInterpreterBoolean
    {

        double DispatchReal(ExpressionNode node, List<object> parameters);

        bool DispatchBoolean(ExpressionNode node, List<object> parameters);

        object Dispatch(ExpressionNode node, List<object> parameters, TypeEnum type);

        public bool FunctionBoolean(FunctionNode node, List<object> parameters);
    }
}