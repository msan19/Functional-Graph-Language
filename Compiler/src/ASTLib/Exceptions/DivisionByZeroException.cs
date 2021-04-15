using ASTLib.Nodes;
using ASTLib.Nodes.ExpressionNodes;
using ASTLib.Nodes.TypeNodes;
using System;
using System.Collections.Generic;
using System.Text;

namespace ASTLib.Exceptions
{
    public class InvalidSetTypeException : CompilerException
    {

        public InvalidSetTypeException(BoundNode node, TypeEnum type) : 
            base(node, $"Expected integer types for bounds but found '{type}'")
        {
        }

        public InvalidSetTypeException(ExpressionNode node, TypeEnum type) :
            base(node, $"Expected a boolean type for predicate of set but found '{type}'")
        {
        }

    }
}
