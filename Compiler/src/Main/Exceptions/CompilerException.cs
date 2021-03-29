using ASTLib.Nodes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Main
{
    public class CompilerException: Exception
    {
        private Node _node;

        public CompilerException(Node node)
        {
            _node = node;
        }

    }
}
