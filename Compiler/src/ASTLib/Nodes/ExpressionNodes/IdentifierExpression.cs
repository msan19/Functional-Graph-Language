using System;
using System.Collections.Generic;
using System.Text;

namespace ASTLib.Nodes.ExpressionNodes
{
    public class IdentifierExpression : ExpressionNode
    {
        public int Reference { get; set; }

        public string Id { get; }

        public IdentifierExpression(String id, int line, int letter) : base(null, line, letter) 
        {
            Id = id;
        }

    }
}
