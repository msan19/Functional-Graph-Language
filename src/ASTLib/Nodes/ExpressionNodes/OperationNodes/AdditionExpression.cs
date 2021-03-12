﻿using System;
using System.Collections.Generic;
using System.Text;

namespace ASTLib.Nodes.ExpressionNodes
{
    public class AdditionExpression : ExpressionNode
    {
        public AdditionExpression(ExpressionNode leftExpression, ExpressionNode rightExpression) 
            : base(new List<ExpressionNode> { leftExpression, rightExpression }) {}
    }
}
