using ASTLib;
using ASTLib.Interfaces;
using ASTLib.Nodes;
using ASTLib.Nodes.ExpressionNodes;
using ASTLib.Nodes.ExpressionNodes.OperationNodes;
using ASTLib.Nodes.TypeNodes;
using System;
using System.Collections.Generic;

namespace TypeCheckerLib
{
    public class TypeHelper : ITypeHelper
    {
        public ITypeChecker TypeChecker { get; set; }
        private List<FunctionNode> _functions;
        private List<TypeNode> _typeNodes;

        public void SetAstRoot(AST root)
        {
            _functions = root.Functions;
        }

        
        public void VisitExport(ExportNode exportNode)
        {
            // Call dispatch and check that return type is double
            // If integer insert CastNode 
        }

        public void VisitFunction(FunctionNode functionNode)
        {
            // Set current type nodes.
            // For each condition:
                // If ReturnExpression of Condition do not match the declared return type of the function: Try Insert CastNode.
                    // Check that LHS is type bool.
                    // Check that RHS is type correct - this may be casted.
                    
            _typeNodes = functionNode.FunctionType.ParameterTypes;
            TypeNode expressionTypeNode = TypeChecker.Dispatch(functionNode.Conditions[0].ReturnExpression);
            if (!TypesAreEqual(expressionTypeNode, functionNode.FunctionType.ReturnType))
                throw new Exception();
        }

        public TypeNode VisitBinaryNumOp(IBinaryNumberOperator binaryNode)
        {
            TypeNode left = GetType(binaryNode.Children[0]);
            TypeNode right = GetType(binaryNode.Children[1]);

            if (left.Type == right.Type)
                return new TypeNode(left.Type, 0, 0);
            else
            {
                CastToReal(binaryNode, left, 0);
                CastToReal(binaryNode, right, 1);
                return new TypeNode(TypeEnum.Real, 0, 0);
            }
        }

        // TODO: Match Local Call reference 
        public TypeNode VisitFunctionCall(FunctionCallExpression funcCallExpNode)
        {
            // TODO: Check local scope
            //       If local reference is found, empty funcCallExpNode.GlobalReferences
            GetMatchingFunctions(funcCallExpNode);

            // Insert an error check here.
            
            return null;
        }

        private void GetMatchingFunctions(FunctionCallExpression funcCallExpNode)
        {
            // TODO: Remove irrelevant references in funcCallExpNode.GlobalReferences
            
            List<FunctionNode> matches = new List<FunctionNode>();
            foreach (int i in funcCallExpNode.GlobalReferences)
            {
                var func = _functions[i];
                if (FunctionIsMatch(func.FunctionType.ParameterTypes, funcCallExpNode))
                    matches.Add(func);
            }
        }

        private bool FunctionIsMatch(List<TypeNode> parameterTypes, FunctionCallExpression funcCallExpNode)
        {
            for (int i = 0; i < parameterTypes.Count; i++)
            {
                var typeNode = TypeChecker.Dispatch(funcCallExpNode.Children[i]);
                if(!TypesAreEqual(typeNode, parameterTypes[i]))
                    return false;
            }
            return true;
        }

        private bool TypesAreEqual(TypeNode a, TypeNode b)
        {
            if (a.GetType() == typeof(FunctionTypeNode))
            {
                if (b.GetType() == typeof(FunctionTypeNode))
                   return IsFunctionTypesEqual((FunctionTypeNode)a, (FunctionTypeNode)b);
                else
                    return false;
            }

            return a.Type == b.Type;
        }

        private bool IsFunctionTypesEqual(FunctionTypeNode a, FunctionTypeNode b)
        {
            if (!TypesAreEqual(a.ReturnType, b.ReturnType))
                return false;
            if (a.ParameterTypes.Count != b.ParameterTypes.Count)
                return false;

            for (int i = 0; i < a.ParameterTypes.Count; i++)
            {
                if (!TypesAreEqual(a.ParameterTypes[i], b.ParameterTypes[i]))
                    return false;
            }
            return true;
        }

        public TypeNode VisitIdentifier(IdentifierExpression idExpressionNode)
        {
            // If isLocal
                // Lookup locally and return
            // Else
                // Lookup globally and return
            
            return null;
        }

        public TypeNode VisitIntegerLiteral(IntegerLiteralExpression intLiteralExpressionNode)
        {
            return new TypeNode(TypeEnum.Integer, 0, 0);
        }

        public TypeNode VisitRealLiteral(RealLiteralExpression realLiteralExpressionNode)
        {
            return new TypeNode(TypeEnum.Real, 0, 0);
        }

        /// 
        /// Private Helpers
        /// 

        private TypeNode GetType(ExpressionNode node)
        {
            return TypeChecker.Dispatch(node);
        }

        private void CastToReal(IBinaryNumberOperator binaryNode, TypeNode nodeType, int child)
        {
            if (nodeType.Type != TypeEnum.Real)
                InsertCastNode(binaryNode, child, TypeEnum.Real);
        }

        private void InsertCastNode(IBinaryNumberOperator binaryNode, int child, TypeEnum type)
        {
            CastFromIntegerExpression cast = new CastFromIntegerExpression(binaryNode.Children[child], 0, 0);
            binaryNode.Children[child] = cast;
        }
    }
}