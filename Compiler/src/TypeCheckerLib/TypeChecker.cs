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
            return node switch
            {
                IBinaryNumberOperator n     => _numberHelper.VisitBinaryNumOp(n, parameterTypes),
                PowerExpression n           => _numberHelper.VisitPower(n, parameterTypes),
                FunctionCallExpression n    => _declarationHelper.VisitFunctionCall(n, parameterTypes),
                IdentifierExpression n      => _declarationHelper.VisitIdentifier(n, parameterTypes),
                IntegerLiteralExpression n  => _declarationHelper.VisitIntegerLiteral(n, parameterTypes),
                RealLiteralExpression n     => _declarationHelper.VisitRealLiteral(n, parameterTypes),
                AdditionExpression n        => _commonOperatorHelper.VisitAddition(n, parameterTypes),
                SubtractionExpression n     => _commonOperatorHelper.VisitSubtraction(n, parameterTypes),
                _ => throw new UnimplementedTypeCheckerException(node, "Dispatch"),
            };
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