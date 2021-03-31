using ASTLib.Nodes;
using System;
using System.Collections.Generic;
using System.Text;

namespace ASTLib.Exceptions
{
    public class IdenticalParameterIdentifiersException : CompilerException
    {
        public IdenticalParameterIdentifiersException(List<string> parameters) : 
            base(null, $"In the parameter list {parameters} there are duplicate identifiers")
        {
            
        }

        private string ConvertToSingleString(List<string> stringList)
        {
            string res = "";
            foreach (string str in stringList)
            {
                res += str;
            }
            return res;
        }
    }
}
