using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace ASTLib.Objects
{
    public class ElementComparer : IComparer<Element>, IEqualityComparer<Element>
    {
        public int Compare(Element x, Element y)
        {
            return x.CompareTo(y);
        }

        public bool Equals(Element x, Element y)
        {
            return x.CompareTo(y) == 0;
        }

        public int GetHashCode(Element obj)
        {
            long l = 0;
            foreach (int i in obj.Indices)
                l *= i;
            return (int)(l % int.MaxValue);
        }
    }
}
