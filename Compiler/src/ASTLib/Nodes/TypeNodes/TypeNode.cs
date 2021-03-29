﻿using System;
using System.Collections.Generic;
using System.Text;

namespace ASTLib.Nodes.TypeNodes
{
    public enum TypeEnum
    {
        Real,
        Integer,
        Boolean,
        Function
    }
    
    public class TypeNode: Node
    {
        public TypeEnum Type { get; }

        public TypeNode(TypeEnum type, int line, int letter) : base(line, letter)
        {
            Type = type;
        }
    }
}
