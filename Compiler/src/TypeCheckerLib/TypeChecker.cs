using System;
using System.Collections.Generic;
using ASTLib;
using ASTLib.Exceptions;
using ASTLib.Interfaces;
using ASTLib.Nodes.ExpressionNodes;
using ASTLib.Nodes.ExpressionNodes.BooleanOperationNodes;
using ASTLib.Nodes.ExpressionNodes.CommonOperationNodes;
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
        private readonly ISetHelper _setHelper;

        public TypeChecker(IDeclarationHelper declarationHelper, INumberHelper numberHelper, 
                           ICommonOperatorHelper commonOperatorHelper, IBooleanHelper booleanHelper, ISetHelper setHelper)
        {
            _declarationHelper = declarationHelper;
            _numberHelper = numberHelper;
            _commonOperatorHelper = commonOperatorHelper;
            _booleanHelper = booleanHelper;
            _setHelper = setHelper;
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
                IBinaryBooleanOperator n    => _booleanHelper.VisitBinaryBoolOp(n, parameterTypes),
                IBinarySetOperator n        => _setHelper.VisitBinarySetOp(n, parameterTypes),
                NotExpression n             => _booleanHelper.VisitNot(n, parameterTypes),
                PowerExpression n           => _numberHelper.VisitPower(n, parameterTypes),
                FunctionCallExpression n    => _declarationHelper.VisitFunctionCall(n, parameterTypes),
                IdentifierExpression n      => _declarationHelper.VisitIdentifier(n, parameterTypes),
                IntegerLiteralExpression n  => _declarationHelper.VisitIntegerLiteral(),
                RealLiteralExpression n     => _declarationHelper.VisitRealLiteral(),
                BooleanLiteralExpression n  => _declarationHelper.VisitBooleanLiteral(),
                AdditionExpression n        => _commonOperatorHelper.VisitAddition(n, parameterTypes),
                SubtractionExpression n     => _commonOperatorHelper.VisitSubtraction(n, parameterTypes),
                AbsoluteValueExpression n   => _commonOperatorHelper.VisitAbsoluteValue(n, parameterTypes),
                IRelationOperator n         => _commonOperatorHelper.VisitRelationalOperator(n, parameterTypes),
                IEquivalenceOperator n      => _commonOperatorHelper.VisitEquivalenceOperator(n, parameterTypes),
                NegativeExpression n        => _numberHelper.VisitNegative(n, parameterTypes),

                _ => throw new UnimplementedTypeCheckerException(node, "Dispatch"),
            };
        }

        private void InitializeHelpers(AST root)
        {
            _declarationHelper.Initialize(root, Dispatch);
            _numberHelper.Initialize(root, Dispatch);
            _commonOperatorHelper.Initialize(root, Dispatch);
            _booleanHelper.Initialize(root, Dispatch);
            _setHelper.Initialize(root, Dispatch);
        }
    }
}