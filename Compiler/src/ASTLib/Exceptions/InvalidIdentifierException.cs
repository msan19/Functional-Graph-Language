using ASTLib.Nodes;
using ASTLib.Nodes.ExpressionNodes;
using System;
using System.Collections.Generic;
using System.Text;

namespace ASTLib.Exceptions
{
    public class InvalidIdentifierException : CompilerException
    {
        public InvalidIdentifierException(IdentifierExpression node) : base(node, $"{node.ID} is not defined as a parameter or function")
        {

        }
    }
}
