using ASTLib.Nodes;
using System;
using System.Collections.Generic;
using System.Text;

namespace ASTLib.Exceptions
{
    public class InvalidIdentifierException : CompilerException
    {
        public InvalidIdentifierException(string id) : base(null, $"Node with id: {id} is not a valid identifier")
        {

        }
    }
}
