﻿using System;
using System.Collections.Generic;
using System.Text;

namespace ASTLib.Nodes.ExpressionNodes
{
    public class IdentifierExpression : ExpressionNode
    {
        public int reference { get; set; }

        public string id { get; }

        public IdentifierExpression(String id) : base(null) {}
    }
}
