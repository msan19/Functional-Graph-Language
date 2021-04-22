using System;
using System.Collections.Generic;
using ASTLib;
using ASTLib.Nodes;
using ASTLib.Nodes.ExpressionNodes;
using ASTLib.Nodes.TypeNodes;
using ASTLib.Objects;

namespace InterpreterLib.Interfaces
{
    public interface IInterpreter: 
        IInterpreterGeneric, IInterpreterInteger, IInterpreterReal, IInterpreterBoolean
        , IInterpreterFunction, IInterpreterSet, IInterpreterElement, IInterpreterString, IInterpreterGraph
    {
        List<LabelGraph> Interpret(AST node);
    }
}