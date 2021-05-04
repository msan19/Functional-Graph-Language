using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ASTLib.Objects
{
    public class Set
    {
        public List<Element> List => Elements.ToList();
        public HashSet<Element> Elements { get; }

        public Set(List<Element> elements)
        {
            Elements = new HashSet<Element>(elements, new ElementComparer());
        }

        public Set(Element element) : this(new List<Element> { element })
        {
        }

        public Set()
        {
            Elements = Set.GetNewHashSet;
        }

        public Set(HashSet<Element> elements)
        {
            Elements = elements;
        }

        public HashSet<Element> SetCopy => Elements.ToHashSet(new ElementComparer());

        public static HashSet<Element> GetNewHashSet => new HashSet<Element>(new ElementComparer());
    }
}
