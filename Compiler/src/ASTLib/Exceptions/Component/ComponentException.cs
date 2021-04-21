using System;
using System.Collections.Generic;

namespace ASTLib.Exceptions.Component
{
    public class ComponentException : Exception
    {
        public List<CompilerException> Exceptions { get; }

        public bool IsEmpty => Exceptions.Count == 0;

        public ComponentException()
        {
            Exceptions = new List<CompilerException>();
        }

        public void Add(CompilerException e)
        {
            Exceptions.Add(e);
        }

    }
}
