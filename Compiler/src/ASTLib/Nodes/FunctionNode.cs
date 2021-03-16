using ASTLib.Nodes.TypeNodes;
using System;
using System.Collections.Generic;
using System.Text;

namespace ASTLib.Nodes
{
    public class FunctionNode : Node
    {
        public int Index { get; }
        public List<ConditionNode> Conditions { get; }
        public List<string> ParameterIdentifiers { get; }
        public FunctionTypeNode FunctionType { get; }
        public string Identifier { get; }

        public FunctionNode(string identifier, int index, ConditionNode condition, List<string> parameterIdentifiers, 
            FunctionTypeNode functionType, int line, int letter) : base(line, letter)
        {
            Index = index;
            Conditions = new List<ConditionNode> { condition };
            ParameterIdentifiers = parameterIdentifiers;
            FunctionType = functionType;
            Identifier = identifier;
        }
    }
}
