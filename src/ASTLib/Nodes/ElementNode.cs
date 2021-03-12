using ASTLib.Nodes.ExpressionNodes;
using System;
using System.Collections.Generic;
using System.Text;

namespace ASTLib.Nodes
{
    public class ElementNode: Node
    {
        public string ElementIdentifier { get; private set; }
        public List<string> IndexIdentifiers { get; private set; }

        public ElementNode(string elementName, List<string> identifiers)
        {
            ElementIdentifier = elementName;
            IndexIdentifiers = identifiers;
        }
    }
}
