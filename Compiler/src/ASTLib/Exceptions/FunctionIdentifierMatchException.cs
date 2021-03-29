using ASTLib.Nodes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Main.Exceptions
{
    public class FunctionIdentifierMatchException : CompilerException
    {
 
        public FunctionIdentifierMatchException(string funcID, string typeID, Node node) : 
            base(node, $"{typeID} and {funcID} should be equivalent")
        {
        }

    }
}
