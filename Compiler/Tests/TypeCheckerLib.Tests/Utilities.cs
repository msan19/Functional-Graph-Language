using System.Collections.Generic;
using ASTLib.Nodes.TypeNodes;
using NSubstitute;
using TypeCheckerLib.Interfaces;

namespace TypeCheckerLib.Tests
{
    public static class Utilities
    {
        public static ITypeChecker GetTypeCheckerOnlyWith(IDeclarationHelper declarationHelper)
        {
            ICommonOperatorHelper commonOperatorHelper = Substitute.For<ICommonOperatorHelper>();
            INumberHelper numberHelper = Substitute.For<INumberHelper>();
            IBooleanHelper booleanHelper = Substitute.For<IBooleanHelper>();
            return new TypeChecker(declarationHelper, numberHelper, commonOperatorHelper, booleanHelper);
        }
        
        public static ITypeChecker GetTypeCheckerOnlyWith(ICommonOperatorHelper commonOperatorHelper)
        {
            IDeclarationHelper declarationHelper = Substitute.For<IDeclarationHelper>();
            INumberHelper numberHelper = Substitute.For<INumberHelper>();
            IBooleanHelper booleanHelper = Substitute.For<IBooleanHelper>();
            return new TypeChecker(declarationHelper, numberHelper, commonOperatorHelper, booleanHelper);
        }
        
        public static ITypeChecker GetTypeCheckerOnlyWith(INumberHelper numberHelper)
        {
            IDeclarationHelper declarationHelper = Substitute.For<IDeclarationHelper>();
            ICommonOperatorHelper commonOperatorHelper = Substitute.For<ICommonOperatorHelper>();
            IBooleanHelper booleanHelper = Substitute.For<IBooleanHelper>();
            return new TypeChecker(declarationHelper, numberHelper, commonOperatorHelper, booleanHelper);
        }
        
        public static ITypeChecker GetTypeCheckerOnlyWith(IBooleanHelper booleanHelper)
        {
            IDeclarationHelper declarationHelper = Substitute.For<IDeclarationHelper>();
            ICommonOperatorHelper commonOperatorHelper = Substitute.For<ICommonOperatorHelper>();
            INumberHelper numberHelper = Substitute.For<INumberHelper>();
            return new TypeChecker(declarationHelper, numberHelper, commonOperatorHelper, booleanHelper);
        }
        

        public static FunctionTypeNode GetFunctionType(TypeEnum returnType, List<TypeEnum> inputTypes)
        {
            var inputs = new List<TypeNode>();
            foreach (var input in inputTypes)
                inputs.Add(new TypeNode(input, 0, 0));
            return new FunctionTypeNode(new TypeNode(returnType, 0, 0), inputs, 0, 0);
        }
        
        public static FunctionTypeNode GetFunctionType(TypeEnum returnType, FunctionTypeNode inputType)
        {
            return new FunctionTypeNode(new TypeNode(returnType, 0, 0), new List<TypeNode>() { inputType }, 0, 0);
        }
    }
}