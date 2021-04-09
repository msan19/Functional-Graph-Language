using ASTLib.Nodes;
using System;
using System.Collections.Generic;
using System.Text;

namespace ASTLib.Exceptions
{
    public class ParserException : Exception
    {

        public List<string> Messages { get; }
        public List<int> Lines { get; }

        public List<int> Letters { get; }

        public ParserException(List<string> messages, List<int> lines, List<int> letters)
        {
            Messages = messages;
            Lines = lines;
            Letters = letters;
        }

    }
}
