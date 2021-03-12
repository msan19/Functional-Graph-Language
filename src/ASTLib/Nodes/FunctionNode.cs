using System;
using System.Collections.Generic;
using System.Text;

namespace ASTLib.Nodes
{
    public abstract class FunctionNode : Node
    {
        public int Index { get; private set; }
        public List<ConditionNode> conditions { get; private set; }

        public FunctionNode()
        {

        }
    }
}
