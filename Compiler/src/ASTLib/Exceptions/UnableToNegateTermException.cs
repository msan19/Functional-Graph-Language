using System.Linq.Expressions;
using ASTLib.Nodes;
using ASTLib.Nodes.ExpressionNodes;

namespace ASTLib.Exceptions
{
    public class UnableToNegateTermException : CompilerException
    {
        public UnableToNegateTermException(ExpressionNode term, string termTypeName) 
            : base(term, $"Could not negate the term which evaluated to type {termTypeName}")
        {
        }
    }
}