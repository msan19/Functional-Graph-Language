using ASTLib.Nodes.ExpressionNodes;
using System;
using System.Collections.Generic;
using System.Text;

namespace ASTLib.Nodes
{
    public class ElementNode: Node
    {
        private const int REFERENCE_NOT_SET = -1;
        public string ElementIdentifier { get; }
        public List<string> IndexIdentifiers { get; }
        public int Reference { get; set; }

        public ElementNode(string elementName, List<string> identifiers, int line, int letter) : base(line, letter)
        {
            ElementIdentifier = elementName;
            IndexIdentifiers = identifiers;
            Reference = REFERENCE_NOT_SET;
        }
    }
}
