using System;
using System.Collections.Generic;
using System.Text;

namespace ASTLib.Nodes.ExpressionNodes
{
    public class FunctionCallExpression : ExpressionNode
    {
        public const int NO_LOCAL_REF = -1; 
        
        public List<int> GlobalReferences { get; set; }

        public int LocalReference { get; set; } = NO_LOCAL_REF;

        public string Identifier { get; }

        public FunctionCallExpression(string identifier, List<ExpressionNode> children, int line, int letter) : base(children, line, letter) 
        {
            Identifier = identifier;
            GlobalReferences = new List<int>();
        }
    }
}
