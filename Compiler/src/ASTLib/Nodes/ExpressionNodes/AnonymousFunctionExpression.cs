using ASTLib.Nodes.TypeNodes;
using System;
using System.Collections.Generic;
using System.Text;

namespace ASTLib.Nodes.ExpressionNodes
{
    public class AnonymousFunctionExpression : ExpressionNode
    {
        public List<string> Identifiers { get; }
        public List<TypeNode> Types { get; }
        public int Reference { get; set; }

        public AnonymousFunctionExpression(List<string> identifiers, List<TypeNode> types,
                                           ExpressionNode returnValue, int line, int letter) : 
                                           base(new List<ExpressionNode> { returnValue }, line, letter) 
        {
            Identifiers = identifiers;
            Types = types;
            Reference = -1;
        }

        public ExpressionNode ReturnValue => Children[0];

    }
}
