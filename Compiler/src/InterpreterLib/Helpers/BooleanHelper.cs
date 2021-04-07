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
            throw new NotImplementedException();
            
            
            // return _functionBoolean
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
        
        public bool ConditionBoolean(ConditionNode node, List<object> parameters)
        {
            throw new NotImplementedException();
        }
        
        public bool EqualBoolean(EqualExpression node, List<object> parameters)
        {
            bool res = default;
            ExpressionNode lhs = node.Children[0];
            ExpressionNode rhs = node.Children[1];

            if (node.Type == TypeEnum.Boolean)
            {
                bool lhsValue = _interpreter.DispatchBoolean(lhs, parameters);
                bool rhsValue = _interpreter.DispatchBoolean(rhs, parameters);
                res = lhsValue == rhsValue;
            } 
            else if (node.Type == TypeEnum.Real)
            {
                double lhsValue = _interpreter.DispatchReal(lhs, parameters);
                double rhsValue = _interpreter.DispatchReal(rhs, parameters);
                res = lhsValue == rhsValue;
            } 
            else if (node.Type == TypeEnum.Integer)
            {
                int lhsValue = _interpreter.DispatchInt(lhs, parameters);
                int rhsValue = _interpreter.DispatchInt(rhs, parameters);
                res = lhsValue == rhsValue;
            }

            return res;
        }
        
        public bool GreaterBoolean(GreaterExpression node, List<object> parameters)
        {
            ExpressionNode lhs = node.Children[0];
            ExpressionNode rhs = node.Children[1];
            double lhsValue = _interpreter.DispatchReal(lhs, new List<object>());
            double rhsValue = _interpreter.DispatchReal(rhs, new List<object>());
            return lhsValue > rhsValue;
        }

        public bool GreaterEqualBoolean(GreaterEqualExpression node, List<object> parameters)
        {
            ExpressionNode lhs = node.Children[0];
            ExpressionNode rhs = node.Children[1];
            double lhsValue = _interpreter.DispatchReal(lhs, new List<object>());
            double rhsValue = _interpreter.DispatchReal(rhs, new List<object>());
            return lhsValue >= rhsValue;
        }

        public bool IdentifierBoolean(IdentifierExpression node, List<object> parameters)
        {
            throw new NotImplementedException();
        }

        public bool LessBoolean(LessExpression node, List<object> parameters)
        {
            ExpressionNode lhs = node.Children[0];
            ExpressionNode rhs = node.Children[1];
            double lhsValue = _interpreter.DispatchReal(lhs, new List<object>());
            double rhsValue = _interpreter.DispatchReal(rhs, new List<object>());
            return lhsValue < rhsValue;
        }

        public bool LessEqualBoolean(LessEqualExpression node, List<object> parameters)
        {
            ExpressionNode lhs = node.Children[0];
            ExpressionNode rhs = node.Children[1];
            double lhsValue = _interpreter.DispatchReal(lhs, new List<object>());
            double rhsValue = _interpreter.DispatchReal(rhs, new List<object>());
            return lhsValue <= rhsValue;
        }

        public bool NotBoolean(NotExpression node, List<object> parameters)
        {
            throw new NotImplementedException();
        }

        public bool NotEqualBoolean(NotEqualExpression node, List<object> parameters)
        {
            throw new NotImplementedException();
        }
    }
}
