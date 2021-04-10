using System;
using System.Collections.Generic;
using System.Text;

namespace ASTLib.Objects
{
    public class Set
    {
        public List<Element> Elements { get; }

        public Set(List<Element> elements)
        {
            Elements = elements;
        }

        public Set(Element element)
        {
            Elements = new List<Element> { element };
        }
    }
}
