using System.Collections.Generic;
using ASTLib.Nodes;
using ASTLib.Nodes.ExpressionNodes;
using ASTLib.Objects;

namespace ASTLib.Exceptions.Invalid
{
    public class InvalidElementException : CompilerException
    {
        public InvalidElementException(ExportNode node, Element e) :
            base(node, GetMessage(e))
        {
        }

        private static string GetMessage(Element e)
        {
            string s = "";
            foreach (int i in e.Indices)
                s += i + ", ";
            return "No vertex exists in the vertex set with identifiers " + s;
        }
    }
}
