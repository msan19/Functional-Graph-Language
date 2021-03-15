using System;
using System.Collections.Generic;
using System.Text;

namespace ASTLib.Nodes
{
    public abstract class Node
    {
        public int LineNumber { get; }
        public int LetterNumber { get; }

        public Node(int line, int letter)
        {
            LineNumber = line;
            LetterNumber = letter;
        }
    }
}
