using System;
using System.Collections.Generic;
using System.Text;

namespace InterpreterLib.MatchPair
{
    public class MatchPair<T>
    {
        public T Element { get; }
        public bool IsCalculated { get; }

        public MatchPair(bool isCalculated, T element)
        {
            IsCalculated = isCalculated;
            Element = element;
        }

    }
}
