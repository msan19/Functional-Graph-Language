using ASTLib;
using ASTLib.Interfaces;
using ASTLib.Nodes;
using ASTLib.Nodes.ExpressionNodes;
using ASTLib.Nodes.TypeNodes;

namespace TypeCheckerLib
{
    public class TypeHelper : ITypeHelper
    {
        public TypeChecker TypeChecker { get; set; }

        public void CheckTypes(AST root)
        {
            // Call dispatch
        }

        public void VisitExport(ExportNode exportNode)
        {
            
        }
        
        public void VisitFunction(FunctionNode functionNode)
        {
            
        }

        public TypeNode VisitExpression(ExpressionNode expressionNode)
        {
            return null;
        }

        public TypeNode VisitBinaryNumOp(IBinaryNumberOperator binaryOpNode)
        {
            return null;
        }

        public TypeNode VisitFunctionCall(FunctionCallExpression fCallExpNode)
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