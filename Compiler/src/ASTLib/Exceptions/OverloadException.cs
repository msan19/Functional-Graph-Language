using ASTLib.Nodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ASTLib.Exceptions
{
    public class OverloadException : CompilerException
    {
        public List<FunctionNode> Nodes { get; private set; }

        public OverloadException(List<FunctionNode> nodes) : 
            base(nodes.FirstOrDefault(), GetMessage(nodes))
        {

        }

        private static string GetMessage(List<FunctionNode> nodes)
        {
            var n = nodes.FirstOrDefault();
            string message = 
                $"There are more than one function matching " +
                $"the name { n.Identifier } with the declaration (";
            for (int i = 0; i < n.FunctionType.ParameterTypes.Count; i++)
            {
                Nodes.TypeNodes.TypeNode p = n.FunctionType.ParameterTypes[i];
                if (i > 0)
                    message += ", ";
                message += $"{ p.Type }";
            }
            message += $") -> { n.FunctionType.ReturnType.Type }.";
            return message;
        }
    }
}
