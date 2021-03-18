using ASTLib;
using ASTLib.Interfaces;
using ASTLib.Nodes;
using ASTLib.Nodes.ExpressionNodes;
using ASTLib.Nodes.TypeNodes;

namespace TypeCheckerLib
{
    public interface ITypeHelper
    {
        TypeChecker TypeChecker { get; set; }
        void CheckTypes(AST root);
        void VisitExport(ExportNode exportNode);
        void VisitFunction(FunctionNode functionNode);
        TypeNode VisitExpression(ExpressionNode expressionNode);
        TypeNode VisitBinaryNumOp(IBinaryNumberOperator binaryOpNode);
        TypeNode VisitFunctionCall(FunctionCallExpression fCallExpNode);
        TypeNode VisitIdentifier(IdentifierExpression idExpressionNode);
        TypeNode VisitIntegerLiteral(IntegerLiteralExpression intLiteralExpressionNode);
        TypeNode VisitRealLiteral(RealLiteralExpression realLiteralExpressionNode);
    }
}