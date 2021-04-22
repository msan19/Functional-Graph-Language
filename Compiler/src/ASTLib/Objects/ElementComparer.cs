using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace ASTLib.Objects
{
    public class ElementComparer : IComparer<Element>
    {
        public int Compare(Element x, Element y)
        {
            return x.CompareTo(y);
        }
    }
}
