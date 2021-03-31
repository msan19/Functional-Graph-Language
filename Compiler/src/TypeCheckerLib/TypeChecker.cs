using System;
using System.Collections.Generic;
using ASTLib;
using ASTLib.Exceptions;
using ASTLib.Interfaces;
using ASTLib.Nodes.ExpressionNodes;
using ASTLib.Nodes.ExpressionNodes.OperationNodes;
using ASTLib.Nodes.TypeNodes;
using TypeCheckerLib.Interfaces;

namespace TypeCheckerLib
{
    public class TypeChecker : ITypeChecker
    {
        private readonly IDeclarationHelper _declarationHelper;
        private readonly INumberHelper _numberHelper;
        private readonly ICommonOperatorHelper _commonOperatorHelper;
        private readonly IBooleanHelper _booleanHelper;

        public TypeChecker(IDeclarationHelper declarationHelper, INumberHelper numberHelper, 
                           ICommonOperatorHelper commonOperatorHelper, IBooleanHelper booleanHelper)
        {
            _declarationHelper = declarationHelper;
            _numberHelper = numberHelper;
            _commonOperatorHelper = commonOperatorHelper;
            _booleanHelper = booleanHelper;
        }

        public void CheckTypes(AST root)
        {
            InitializeHelpers(root);
            
            foreach (var exportNode in root.Exports)
                _declarationHelper.VisitExport(exportNode);

            foreach (var functionNode in root.Functions)
                _declarationHelper.VisitFunction(functionNode);
        }

        public TypeNode Dispatch(ExpressionNode node, List<TypeNode> parameterTypes)
        {
            switch (node)
            {
                case IBinaryNumberOperator n:
                    return _numberHelper.VisitBinaryNumOp(n, parameterTypes);
                case FunctionCallExpression n:
                    return _declarationHelper.VisitFunctionCall(n, parameterTypes);
                case IdentifierExpression n:
                    return _declarationHelper.VisitIdentifier(n, parameterTypes);
                case IntegerLiteralExpression n:
                    return _declarationHelper.VisitIntegerLiteral(n, parameterTypes);
                case RealLiteralExpression n:
                    return _declarationHelper.VisitRealLiteral(n, parameterTypes);
                case AdditionExpression n:
                    return _commonOperatorHelper.VisitAddition(n, parameterTypes);
                case SubtractionExpression n:
                    return _commonOperatorHelper.VisitSubtraction(n, parameterTypes);
                default:
                    throw new UnimplementedTypeCheckerException(node, "Dispatch");
            }
        }

        private void InitializeHelpers(AST root)
        {
            _declarationHelper.Initialize(root, Dispatch);
            _numberHelper.Initialize(root, Dispatch);
            _commonOperatorHelper.Initialize(root, Dispatch);
            _booleanHelper.Initialize(root, Dispatch);
        }
    }
}