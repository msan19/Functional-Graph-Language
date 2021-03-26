using System;
using System.Collections.Generic;
using System.Text;

namespace ASTLib.Nodes.ExpressionNodes
{
    public class FunctionCallExpression : ExpressionNode
    {
        public List<int> GlobalReferences { get; set; } = new List<int>();

        public int LocalReference { get; set; } = -1;

        public string Identifier { get; }

        public FunctionCallExpression(string identifier, List<ExpressionNode> children, int line, int letter) : base(children, line, letter) 
        {
            Identifier = identifier;
        }
    }
}
