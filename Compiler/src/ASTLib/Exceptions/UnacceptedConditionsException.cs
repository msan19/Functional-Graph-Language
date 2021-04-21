using ASTLib.Nodes;
using System.Collections.Generic;

namespace ASTLib.Exceptions
{
    public class UnacceptedConditionsException : CompilerException
    {
        public UnacceptedConditionsException(int validCaseCount) 
            : base($"{validCaseCount} cases where valid, only one case can " +
                   $"be true at a time apart from the default case")
        {
        }
        
        public UnacceptedConditionsException(Node node) :
            base(node, "The default case returned an invalid result")
        {
        }

    }
}
