using ASTLib.Nodes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Main.Exceptions
{
    public class FunctionIdentifierMatchException : CompilerException
    {
   
        public string FuncID { get; }

        public string TypeID { get; }

        public FunctionIdentifierMatchException(string funcID, string typeID, Node node) : base(node)
        {
            FuncID = funcID;
            TypeID = typeID;
        }

    }
}
