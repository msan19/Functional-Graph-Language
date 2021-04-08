﻿using ASTLib.Nodes;
using System;
using System.Collections.Generic;
using System.Text;

namespace ASTLib.Exceptions
{
    public class UnacceptedConditionsException : CompilerException
    {

        public UnacceptedConditionsException(Node node, int validCaseCount) : 
            base(node, $"{validCaseCount} cases where valid, only one case can " +
                       $"be true at a time apart from the default case")
        {
        }

        public UnacceptedConditionsException(Node node) :
            base(node, "The default case returned an invalid result")
        {
        }

    }
}