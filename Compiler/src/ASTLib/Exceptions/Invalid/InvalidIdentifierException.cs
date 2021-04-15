using ASTLib.Nodes;
using ASTLib.Nodes.ExpressionNodes;
using System;
using System.Collections.Generic;
using System.Text;

namespace ASTLib.Exceptions
{
    public class InvalidIdentifierException : CompilerException
    {
        public InvalidIdentifierException(IdentifierExpression node) : base(node, $"{node.ID} is not defined as a parameter or function")
        {

        }

        public InvalidIdentifierException(string id, SetExpression set) : base(set, GetBoundMessage(id, set))
        {

        }

        private static string GetBoundMessage(string id, SetExpression set)
        {
            string s = $"{ id } is not defined as an identifier in " +
                $"{ set.Element.ElementIdentifier }[{ GetIdentifiers(set.Element.IndexIdentifiers) }]";

            return s;
        }

        private static string GetIdentifiers(List<string> indexIdentifiers)
        {
            var s = "";
            foreach (var id in indexIdentifiers)
            {
                if (s.Length == 0)
                    s = id;
                else
                    s += ", " + id;
            }
            return s;
        }
    }
}
