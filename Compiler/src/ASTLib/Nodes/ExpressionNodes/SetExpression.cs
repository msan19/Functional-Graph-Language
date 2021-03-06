using ASTLib.Interfaces;
using ASTLib.Nodes.TypeNodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ASTLib.Nodes.ExpressionNodes
{
    public class SetExpression : ExpressionNode
    {
        public ElementNode Element { get; }
        public List<BoundNode> Bounds { get; set; }

        public SetExpression(ElementNode element, List<BoundNode> bounds,
                             ExpressionNode predicate, int line, int letter) :
                             base(new List<ExpressionNode>() { predicate }, line, letter)
        {
            Element = element;
            Bounds = bounds;
        }

        public SetExpression(List<ExpressionNode> elements, int line, int letter) :
            base(elements, line, letter)
        {
        }

        public bool IsSetBuilder => Element != null && Bounds != null;

        public ExpressionNode Predicate => Children.First();
    }
}
