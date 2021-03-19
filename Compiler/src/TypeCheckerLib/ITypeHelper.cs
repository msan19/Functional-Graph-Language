using ASTLib;
using ASTLib.Interfaces;
using ASTLib.Nodes;
using ASTLib.Nodes.ExpressionNodes;
using ASTLib.Nodes.TypeNodes;

namespace TypeCheckerLib
{
    public interface ITypeHelper
    {
        ITypeChecker TypeChecker { get; set; }

        void SetAstRoot(AST root);
        TypeNode VisitBinaryNumOp(IBinaryNumberOperator binaryNode);
        void VisitExport(ExportNode exportNode);
        void VisitFunction(FunctionNode functionNode);
        TypeNode VisitFunctionCall(FunctionCallExpression funcCallExpNode);
        TypeNode VisitIdentifier(IdentifierExpression idExpressionNode);
        TypeNode VisitIntegerLiteral(IntegerLiteralExpression intLiteralExpressionNode);
        TypeNode VisitRealLiteral(RealLiteralExpression realLiteralExpressionNode);
    }
}