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
        public List<String> ParameterIdentifiers { get; }
        public FunctionTypeNode FunctionType { get; }

        public FunctionNode(ConditionNode condition, List<String> parameterIdentifiers, 
            FunctionTypeNode functionType, int line, int letter) : base(line, letter)
        {
            Conditions = new List<ConditionNode> { condition };
            ParameterIdentifiers = parameterIdentifiers;
            FunctionType = functionType;
        }
    }
}
