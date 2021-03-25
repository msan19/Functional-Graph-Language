using System;
using ASTLib;
using ASTLib.Interfaces;
using ASTLib.Nodes.ExpressionNodes;
using ASTLib.Nodes.ExpressionNodes.OperationNodes;
using ASTLib.Nodes.TypeNodes;

namespace TypeCheckerLib
{
    public class TypeChecker : ITypeChecker
    {
        private readonly ITypeHelper _helper;

        public TypeChecker(ITypeHelper helper)
        {
            _helper = helper;
            _helper.TypeChecker = this;
        }

        public void CheckTypes(AST root)
        {
            _helper.SetAstRoot(root);
            foreach (var exportNode in root.Exports)
                _helper.VisitExport(exportNode);

            for (var index = 0; index < root.Functions.Count; index++)
            {
                var functionNode = root.Functions[index];
                _helper.VisitFunction(functionNode);
            }
        }

        public TypeNode Dispatch(ExpressionNode node)
        {
            switch (node)
            {
                case IBinaryNumberOperator n:
                    return _helper.VisitBinaryNumOp(n);
                case FunctionCallExpression n:
                    return _helper.VisitFunctionCall(n);
                case IdentifierExpression n:
                    return _helper.VisitIdentifier(n);
                case IntegerLiteralExpression n:
                    return _helper.VisitIntegerLiteral(n);
                case RealLiteralExpression n:
                    return _helper.VisitRealLiteral(n);
                default:
                    throw new ArgumentException("The argument was not a recognized ExpressionNode");
                    
            }
        }

    }
}