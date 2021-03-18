using ASTLib;
using ASTLib.Interfaces;
using ASTLib.Nodes;
using ASTLib.Nodes.ExpressionNodes;
using ASTLib.Nodes.TypeNodes;

namespace TypeCheckerLib
{
    public class TypeHelper : ITypeHelper
    {
        public ITypeChecker TypeChecker { get; set; }
        
        public void VisitExport(ExportNode exportNode)
        {

        }

        public void VisitFunction(FunctionNode functionNode)
        {

        }

        public TypeNode VisitBinaryNumOp(IBinaryNumberOperator binaryNode)
        {
            return null;
        }

        public TypeNode VisitFunctionCall(FunctionCallExpression funcCallExpNode)
        {
            return null;
        }

        public TypeNode VisitIdentifier(IdentifierExpression idExpressionNode)
        {
            return null;
        }

        public TypeNode VisitIntegerLiteral(IntegerLiteralExpression intLiteralExpressionNode)
        {
            return null;
        }

        public TypeNode VisitRealLiteral(RealLiteralExpression realLiteralExpressionNode)
        {
            return null;
        }
    }
}