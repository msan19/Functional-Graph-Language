﻿using System;
using System.Collections.Generic;
using System.Text;

namespace ASTLib.Nodes.ExpressionNodes
{
    public class SubtractionExpression : ExpressionNode
    {
        public SubtractionExpression(ExpressionNode leftExpression, ExpressionNode rightExpression, int line, int letter) 
            : base(new List<ExpressionNode> { leftExpression, rightExpression }, line, letter) {}
    }
}
