using ASTLib.Interfaces;
using ASTLib.Nodes.TypeNodes;
using System;
using System.Collections.Generic;
using System.Text;

namespace ASTLib.Nodes.ExpressionNodes
{
    public class ElementExpression : ExpressionNode, INonIdentifierExpression
    {

        public ElementExpression(List<ExpressionNode> children, int line, int letter) : 
                             base(children, line, letter) 
        {
        }
    }
}
