using System;
using System.Collections.Generic;
using System.Text;

namespace ASTLib.Objects
{
    public class Element
    {
        public List<int> Indices { get; }

        public Element(List<int> indices)
        {
            Indices = indices;
        }

        public Element(int index)
        {
            Indices = new List<int> { index };
        }

    }
}
