using System;
using System.Collections.Generic;
using ASTLib;
using ASTLib.Nodes;
using ASTLib.Nodes.ExpressionNodes;
using ASTLib.Nodes.ExpressionNodes.BooleanOperationNodes;
using ASTLib.Nodes.ExpressionNodes.CommonOperationNodes;
using ASTLib.Nodes.ExpressionNodes.CommonOperationNodes.RelationalOperationNodes;
using ASTLib.Nodes.TypeNodes;
using InterpreterLib.Interfaces;

namespace InterpreterLib.Helpers
{
    public class BooleanHelper : IBooleanHelper
    {
        private IInterpreterBoolean _interpreter;
        private AST _root;
        private List<FunctionNode> _functions => _root.Functions;

        public void SetASTRoot(AST root)
        {
            _root = root;
        }

        public void SetInterpreter(IInterpreterBoolean interpreter)
        {
            _interpreter = interpreter;
        }
        
        public bool FunctionCallBoolean(FunctionCallExpression node, List<object> parameters)
        {
            FunctionNode funcNode = GetFuncNode(node, parameters);
            return GetResult(node, funcNode, parameters);
        }

        private bool GetResult(FunctionCallExpression node, FunctionNode funcNode, List<object> parameters)
        {
            List<object> funcParameters = new List<object>();
            for (int i = 0; i < node.Children.Count; i++)
            {
                TypeEnum type = funcNode.FunctionType.ParameterTypes[i].Type;
                funcParameters.Add(_interpreter.Dispatch(node, parameters, type));
            }
            return _interpreter.FunctionBoolean(funcNode, funcParameters);
        }

        private FunctionNode GetFuncNode(FunctionCallExpression node, List<object> parameters)
        {
            if (node.LocalReference == FunctionCallExpression.NO_LOCAL_REF)
                return _functions[node.GlobalReferences[0]];
            else
                return _functions[(int)parameters[node.LocalReference]];
        }

        public bool AndBoolean(AndExpression node, List<object> parameters)
        {
            bool leftOperand =  _interpreter.DispatchBoolean(node.Children[0], parameters);
            bool rightOperand = _interpreter.DispatchBoolean(node.Children[1], parameters);

            return leftOperand && rightOperand;
        }

        public bool OrBoolean(OrExpression node, List<object> parameters)
        {
            bool leftOperand = _interpreter.DispatchBoolean(node.Children[0], parameters);
            bool rightOperand = _interpreter.DispatchBoolean(node.Children[1], parameters);

            return leftOperand || rightOperand;
        }
        
        public bool? ConditionBoolean(ConditionNode node, List<object> parameters)
        {
            throw new NotImplementedException();
        }

        public bool GreaterBoolean(GreaterExpression node, List<object> parameters)
        {
            ExpressionNode lhs = node.Children[0];
            ExpressionNode rhs = node.Children[1];
            double lhsValue = _interpreter.DispatchReal(lhs, parameters);
            double rhsValue = _interpreter.DispatchReal(rhs, parameters);
            return lhsValue > rhsValue;
        }

        public bool GreaterEqualBoolean(GreaterEqualExpression node, List<object> parameters)
        {
            ExpressionNode lhs = node.Children[0];
            ExpressionNode rhs = node.Children[1];
            double lhsValue = _interpreter.DispatchReal(lhs, parameters);
            double rhsValue = _interpreter.DispatchReal(rhs, parameters);
            return lhsValue >= rhsValue;
        }

        public bool IdentifierBoolean(IdentifierExpression node, List<object> parameters)
        {
            return parameters[node.Reference].Equals("true");
        }

        public bool LessBoolean(LessExpression node, List<object> parameters)
        {
            ExpressionNode lhs = node.Children[0];
            ExpressionNode rhs = node.Children[1];
            double lhsValue = _interpreter.DispatchReal(lhs, parameters);
            double rhsValue = _interpreter.DispatchReal(rhs, parameters);
            return lhsValue < rhsValue;
        }

        public bool LessEqualBoolean(LessEqualExpression node, List<object> parameters)
        {
            ExpressionNode lhs = node.Children[0];
            ExpressionNode rhs = node.Children[1];
            double lhsValue = _interpreter.DispatchReal(lhs, parameters);
            double rhsValue = _interpreter.DispatchReal(rhs, parameters);
            return lhsValue <= rhsValue;
        }

        public bool NotBoolean(NotExpression node, List<object> parameters)
        {
            return !_interpreter.DispatchBoolean(node.Children[0], null);
        }

        public bool EqualBoolean(EqualExpression node, List<object> parameters)
        {
            ExpressionNode lhs = node.Children[0];
            ExpressionNode rhs = node.Children[1];

            return IsEqual(lhs, rhs, node.Type, parameters);
        }

        public bool NotEqualBoolean(NotEqualExpression node, List<object> parameters)
        {
            ExpressionNode lhs = node.Children[0];
            ExpressionNode rhs = node.Children[1];

            return !IsEqual(lhs, rhs, node.Type, parameters);
        }

        private bool IsEqual(ExpressionNode lhs, ExpressionNode rhs, TypeEnum type, List<object> parameters)
        {
            bool res = false;
            if (type == TypeEnum.Boolean)
            {
                bool lhsValue = _interpreter.DispatchBoolean(lhs, parameters);
                bool rhsValue = _interpreter.DispatchBoolean(rhs, parameters);
                res = lhsValue == rhsValue;
            }
            else if (type == TypeEnum.Real)
            {
                double lhsValue = _interpreter.DispatchReal(lhs, parameters);
                double rhsValue = _interpreter.DispatchReal(rhs, parameters);
                res = lhsValue == rhsValue;
            }
            else if (type == TypeEnum.Integer)
            {
                int lhsValue = _interpreter.DispatchInt(lhs, parameters);
                int rhsValue = _interpreter.DispatchInt(rhs, parameters);
                res = lhsValue == rhsValue;
            }
            else if (type == TypeEnum.Function)
            {
                int lhsValue = _interpreter.DispatchFunction(lhs, parameters);
                int rhsValue = _interpreter.DispatchFunction(rhs, parameters);
                res = lhsValue == rhsValue;
            }
            return res;
        }
    }
}
