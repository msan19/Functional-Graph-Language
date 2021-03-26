using System.Collections.Generic;
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
        TypeNode VisitBinaryNumOp(IBinaryNumberOperator binaryNode, List<TypeNode> parameterTypes);
        void VisitExport(ExportNode exportNode);
        void VisitFunction(FunctionNode functionNode);
        TypeNode VisitFunctionCall(FunctionCallExpression funcCallExpNode, List<TypeNode> parameterTypes);
        TypeNode VisitIdentifier(IdentifierExpression idExpressionNode, List<TypeNode> parameterTypes);
        TypeNode VisitIntegerLiteral(IntegerLiteralExpression intLiteralExpressionNode, List<TypeNode> parameterTypes);
        TypeNode VisitRealLiteral(RealLiteralExpression realLiteralExpressionNode, List<TypeNode> parameterTypes);
    }
}