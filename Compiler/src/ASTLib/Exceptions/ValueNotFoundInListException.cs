using System;
using System.Collections.Generic;
using System.Text;

namespace ASTLib.Exceptions
{
    public class ValueNotFoundInListException : CompilerException
    {
        public ValueNotFoundInListException(string id, List<string> list) : base(null, $"Node with id: {id} is not present in list: {list}")
        {

        }
    }
}
