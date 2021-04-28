using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using ASTLib.Nodes;
using ASTLib.Nodes.ExpressionNodes;
using ASTLib.Nodes.TypeNodes;

namespace ReferenceHandlerLib.Tests
{
    public static class Utilities
    {
        public static ElementNode GetElementNode(string name, List<string> indexIdentifiers)
        {
            return new ElementNode(name, indexIdentifiers, 0, 0);
        }

        public static  ConditionNode GetConditionNodeOnlyWith(List<ElementNode> elementNodes)
        {
            return new ConditionNode(elementNodes, null, null, 0, 0);
        }
        
        public static List<ConditionNode> GetDummyConditionList()
        {
            return new List<ConditionNode>() { new ConditionNode(null, null, null, 0, 0) };
        }

        public static FunctionNode GetFunctionNodeWith(List<string> paramIDs, TypeNode returnType, List<TypeNode> paramTypes)
        {
            FunctionTypeNode fType = new FunctionTypeNode(returnType, paramTypes, 0, 0);
            return new FunctionNode("f", null, paramIDs, fType, 1, 1);
        }
        
        public static FunctionTypeNode GetFunctionTypeNodeWith(TypeNode returnType, List<TypeNode> paramTypes)
        {
            return new FunctionTypeNode(returnType, paramTypes, 0, 0);
        }

        internal static StringLiteralExpression GetStringLit()
        {
            return new StringLiteralExpression("hej", 0, 0);
        }
    }
}