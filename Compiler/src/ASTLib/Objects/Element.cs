using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace ASTLib.Objects
{
    public class Element: IEquatable<Element>, IComparable<Element>
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

        public bool Equals(Element other)
        {
            return CompareTo(other) == 0;
        }

        public int CompareTo(Element y)
        {
            if (this.Indices.Count < y.Indices.Count)
                return -1;
            else if (this.Indices.Count > y.Indices.Count)
                return 1;
            else
            {
                for (int i = 0; i < this.Indices.Count; i++)
                    if (this.Indices[i] < y.Indices[i])
                        return -1;
                    else if (this.Indices[i] > y.Indices[i])
                        return 1;
                return 0;
            }
        }
    }
}
