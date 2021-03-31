using System;
using System.Collections.Generic;
using System.Text;

namespace ASTLib.Exceptions
{
    public class OverloadedFunctionIdentifierException : CompilerException
    {
        public OverloadedFunctionIdentifierException(string id) : base(null, $"Identifier with id {id} points to an overloaded function")
        {

        }
    }
}
