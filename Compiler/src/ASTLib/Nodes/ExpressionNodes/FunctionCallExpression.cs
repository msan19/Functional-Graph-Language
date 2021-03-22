using System;
using System.Collections.Generic;
using System.Text;

namespace ASTLib.Nodes.ExpressionNodes
{
    public class FunctionCallExpression : ExpressionNode
    {
        public List<int> GlobalReferences { get; set; }

        public int LocalReference { get; set; }

        public string Identifier { get; }

        public FunctionCallExpression(string identifier, List<ExpressionNode> children, int line, int letter) : base(children, line, letter) 
        {
            Identifier = identifier;
        }
    }
}
