using ASTLib.Nodes.TypeNodes;
using System;
using System.Collections.Generic;
using System.Text;

namespace ASTLib.Nodes
{
    public class FunctionNode : Node
    {
        public int Index { get; private set; }
        public List<ConditionNode> Conditions { get; private set; }
        public List<String> ParameterIdentifiers { get; private set; }
        public FunctionTypeNode FunctionType { get; private set; }

        public FunctionNode(ConditionNode condition, List<String> parameterIdentifiers, FunctionTypeNode functionType)
        {
            Conditions = new List<ConditionNode> { condition };
            ParameterIdentifiers = parameterIdentifiers;
            FunctionType = functionType;
        }
    }
}
