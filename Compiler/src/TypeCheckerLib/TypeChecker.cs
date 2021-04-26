using System;
using System.Collections.Generic;
using ASTLib;
using ASTLib.Exceptions;
using ASTLib.Interfaces;
using ASTLib.Nodes.ExpressionNodes;
using ASTLib.Nodes.ExpressionNodes.BooleanOperationNodes;
using ASTLib.Nodes.ExpressionNodes.CommonOperationNodes.ElementAndSetOperations;
using ASTLib.Nodes.ExpressionNodes.NumberOperationNodes;
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

            //Foreach not posible because anonymous function is added
            for (int i = 0; i < root.Functions.Count; i++)
                _declarationHelper.VisitFunction(root.Functions[i]);
        }

        public TypeNode Dispatch(ExpressionNode node, List<TypeNode> parameterTypes)
        {
            return node switch
            {
                IBinaryNumberOperator n     => _numberHelper.VisitBinaryNumOp(n, parameterTypes),
                IBinaryBooleanOperator n    => _booleanHelper.VisitBinaryBoolOp(n, parameterTypes),
                IBinarySetOperator n        => _setHelper.VisitBinarySetOp(n, parameterTypes),
                SubsetExpression n          => _setHelper.VisitSubset(n, parameterTypes),
                SetExpression n             => _setHelper.VisitSet(n, parameterTypes),
                NotExpression n             => _booleanHelper.VisitNot(n, parameterTypes),
                FunctionCallExpression n    => _declarationHelper.VisitFunctionCall(n, parameterTypes),
                IdentifierExpression n      => _declarationHelper.VisitIdentifier(n, parameterTypes),
                IntegerLiteralExpression _  => _declarationHelper.VisitIntegerLiteral(),
                RealLiteralExpression _     => _declarationHelper.VisitRealLiteral(),
                BooleanLiteralExpression _  => _declarationHelper.VisitBooleanLiteral(),
                StringLiteralExpression _   => _declarationHelper.VisitStringLiteral(),
                EmptySetLiteralExpression _ => _declarationHelper.VisitEmptySetLiteral(),
                AdditionExpression n        => _commonOperatorHelper.VisitAddition(n, parameterTypes),
                SubtractionExpression n     => _commonOperatorHelper.VisitSubtraction(n, parameterTypes),
                AbsoluteValueExpression n   => _commonOperatorHelper.VisitAbsoluteValue(n, parameterTypes),
                IRelationOperator n         => _commonOperatorHelper.VisitRelationalOperator(n, parameterTypes),
                IEquivalenceOperator n      => _commonOperatorHelper.VisitEquivalenceOperator(n, parameterTypes),
                NegativeExpression n        => _numberHelper.VisitNegative(n, parameterTypes),
                ElementExpression n         => _commonOperatorHelper.VisitElement(n, parameterTypes),
                ISetGraphField n            => _commonOperatorHelper.VisitISetGraphField(n, parameterTypes),
                IFunctionGraphField n       => _commonOperatorHelper.VisitIFunctionGraphField(n, parameterTypes),
                GraphExpression n           => _commonOperatorHelper.VisitGraph(n, parameterTypes),
                AnonymousFunctionExpression n => _declarationHelper.VisitAnonymousFunction(n, parameterTypes),
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