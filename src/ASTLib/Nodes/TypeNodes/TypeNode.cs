using System;
using System.Collections.Generic;
using System.Text;

namespace ASTLib.Nodes.TypeNodes
{
    public enum Type
    {
        Real,
        Integer,
        Function
    }
    
    public class TypeNode: Node
    {
        public Type Type { get; private set; }

        public TypeNode(Type type)
        {
            Type = type;
        }
    }
}
