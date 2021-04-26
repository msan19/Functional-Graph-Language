using ASTLib.Nodes;
using System;
using System.Collections.Generic;
using System.Text;

namespace ASTLib.Exceptions
{
    public class IdenticalParameterIdentifiersException : CompilerException
    {
        public IdenticalParameterIdentifiersException(List<string> parameters) : 
            base(null, $"In the parameter list ({ConvertToSingleString(parameters)}) there are duplicate identifiers")
        {
            
        }

        private static string ConvertToSingleString(List<string> stringList)
        {
            string res = "";
            for (int i = 0; i < stringList.Count; i++)
                res += (i==0? "" : ", ") + stringList[i];
            return res;
        }
    }
}
