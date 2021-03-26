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

        public void SetAstRoot(AST root)
        {
            _functions = root.Functions;
        }

        
        public void VisitExport(ExportNode exportNode)
        {
            // Call dispatch and check that return type is double
            // If integer insert CastNode 
            var type = TypeChecker.Dispatch(exportNode.ExportValue, new List<TypeNode>()).Type;
            if (type == TypeEnum.Real)
                return;
            else if (type == TypeEnum.Integer)
                InsertCastNode(exportNode);
            else
                throw new Exception("Export recived " + type.ToString() + " instead of Integer or Double");
        }

        private void InsertCastNode(ExportNode node)
        {
            CastFromIntegerExpression cast = new CastFromIntegerExpression(node.ExportValue, 0, 0);
            node.ExportValue = cast;
        }

        public void VisitFunction(FunctionNode functionNode)
        {
            // Set current type nodes.
            // For each condition:
            //  If ReturnExpression of Condition do not match the declared return type of the function: Try Insert CastNode.
            //      Check that LHS is type bool.
            //      Check that RHS is type correct - this may be casted.
            List<TypeNode> parameterTypes = functionNode.FunctionType.ParameterTypes;

            foreach (var condition in functionNode.Conditions)
            {
                CheckConditionNode(functionNode.FunctionType.ReturnType.Type, condition, parameterTypes);
            }
        }

        private void CheckConditionNode(TypeEnum rhsType, ConditionNode condition, List<TypeNode> parameterTypes)
        {
            var type = TypeChecker.Dispatch(condition.ReturnExpression, parameterTypes).Type;
            if (type != rhsType)
            {
                if (type == TypeEnum.Integer && rhsType == TypeEnum.Real)
                    InsertCastNode(condition);
                else
                    throw new Exception("Function body returns " + type.ToString()
                        + ", expected " + rhsType.ToString());
            }
        }

        private void InsertCastNode(ConditionNode conditionNode)
        {
            CastFromIntegerExpression cast = new CastFromIntegerExpression(conditionNode.ReturnExpression, 0, 0);
            conditionNode.ReturnExpression = cast;
        }


        public TypeNode VisitBinaryNumOp(IBinaryNumberOperator binaryNode, List<TypeNode> parameterTypes)
        {
            TypeNode left = GetType(binaryNode.Children[0], parameterTypes);
            TypeNode right = GetType(binaryNode.Children[1], parameterTypes);
            
            if (left.Type == TypeEnum.Function || right.Type == TypeEnum.Function)
                throw new Exception("One of the arguments is of type Function.");

            if (left.Type != right.Type)
            {
                CastToReal(binaryNode, left, 0);
                CastToReal(binaryNode, right, 1);
                return new TypeNode(TypeEnum.Real, 0, 0);
            }
            return new TypeNode(left.Type, 0, 0);
        }

        // TODO: Match Local Call reference 
        public TypeNode VisitFunctionCall(FunctionCallExpression funcCallExpNode, List<TypeNode> parameterTypes)
        {
            // TODO: Check local scope
            //       If local reference is found, empty funcCallExpNode.GlobalReferences
            // Insert an error check here.
            
            return null;
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

        public TypeNode VisitIdentifier(IdentifierExpression idExpressionNode, List<TypeNode> parameterTypes)
        {
            // If isLocal
                // Lookup locally and return
            // Else
                // Lookup globally and return
            
            return null;
        }

        public TypeNode VisitIntegerLiteral(IntegerLiteralExpression intLiteralExpressionNode,
            List<TypeNode> parameterTypes)
        {
            return new TypeNode(TypeEnum.Integer, 0, 0);
        }

        public TypeNode VisitRealLiteral(RealLiteralExpression realLiteralExpressionNode, List<TypeNode> parameterTypes)
        {
            return new TypeNode(TypeEnum.Real, 0, 0);
        }

        /// 
        /// Private Helpers
        /// 

        private TypeNode GetType(ExpressionNode node, List<TypeNode> parameterTypes)
        {
            return TypeChecker.Dispatch(node, parameterTypes);
        }

        private void CastToReal(IBinaryNumberOperator binaryNode, TypeNode nodeType, int child)
        {
            if (nodeType.Type != TypeEnum.Real)
                InsertCastNode(binaryNode, child);
        }

        private void InsertCastNode(IBinaryNumberOperator binaryNode, int child)
        {
            CastFromIntegerExpression cast = new CastFromIntegerExpression(binaryNode.Children[child], 0, 0);
            binaryNode.Children[child] = cast;
        }
    }
}