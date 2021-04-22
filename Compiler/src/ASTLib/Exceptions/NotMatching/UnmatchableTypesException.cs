using ASTLib.Nodes;
using ASTLib.Nodes.TypeNodes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ASTLib.Exceptions.NotMatching
{
    public class UnmatchableTypesException : CompilerException
    {
 
        public UnmatchableTypesException(Node node, TypeEnum left, TypeEnum right, string expected) : 
            base(node, $"Cannot match the types '{left}' and '{right}' to a {expected}")
        {
        }

        public UnmatchableTypesException(Node node, TypeEnum actual, TypeEnum expected) :
            base(node, $"Got '{actual}' and expected '{expected}'")
        {
        }

        public UnmatchableTypesException(Node node, List<TypeEnum> actuals, TypeEnum expected) :
            base(node, $"Got '{GetStringList(actuals)}' and expected '{expected}'")
        {
        }

        private static string GetStringList(List<TypeEnum> types)
        {
            string s = "";
            foreach (var type in types)
            {
                if (s.Length == 0)
                    s += "(" + type.ToString();
                else
                    s += ", " + type.ToString();
            }
            s += ")";
            return s;
        }
    }
}
