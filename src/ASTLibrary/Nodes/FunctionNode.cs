using System;
using System.Collections.Generic;
using System.Text;

namespace ASTLibrary.Nodes
{
    public abstract class FunctionNode: Node
    {
        public int Index { get; private set; }

        public FunctionNode()
        {

        }
    }
}
