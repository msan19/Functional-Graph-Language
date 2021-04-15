using ASTLib.Nodes.ExpressionNodes;
using System;
using System.Collections.Generic;
using System.Text;

namespace ASTLib.Exceptions
{
    public class OverloadedFunctionIdentifierException : CompilerException
    {
        public OverloadedFunctionIdentifierException(IdentifierExpression node) : base(node, $"{node.ID} points to an overloaded function")
        {

        }
    }
}
