using ASTLib.Exceptions;
using ASTLib.Nodes;
using ASTLib.Nodes.ExpressionNodes;
using ASTLib.Nodes.TypeNodes;
using System;
using System.Collections.Generic;
using System.Text;

namespace TypeCheckerLib.Tests.HelperTests
{
    public class NoMatchingFunctionFoundException : CompilerException
    {
        public NoMatchingFunctionFoundException(FunctionCallExpression node) : 
            base(node, GetMessage(node))
        {
        }
        private static string GetMessage(FunctionCallExpression node)
        {
            string message =
                $"No function matching " +
                $"the name { node.Identifier } was found.";
            return message;
        }
    }
}
