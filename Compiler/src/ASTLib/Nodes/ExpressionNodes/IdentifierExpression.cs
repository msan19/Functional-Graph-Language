using System;
using System.Collections.Generic;
using System.Text;

namespace ASTLib.Nodes.ExpressionNodes
{
    public class IdentifierExpression : ExpressionNode
    {
        public int Reference { get; set; }

        public bool IsLocal { get; set; }

        public string ID { get; }

        public IdentifierExpression(String id, int line, int letter) : base(null, line, letter) 
        {
            ID = id;
            Reference = -1;
        }

    }
}
