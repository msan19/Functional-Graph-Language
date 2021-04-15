using System.Collections.Generic;
using ASTLib.Nodes;

namespace ReferenceHandlerLib.Tests
{
    public static class Utilities
    {
        public static ElementNode GetElementNode(string name, List<string> indexIdentifiers)
        {
            return new ElementNode(name, indexIdentifiers, 0, 0);
        }

        public static  ConditionNode GetConditionNodeOnlyWith(List<ElementNode> elementNodes)
        {
            return new ConditionNode(elementNodes, null, null, 0, 0);
        }
    }
}