using System;
using System.Collections.Generic;
using ASTLib;
using ASTLib.Exceptions;
using ASTLib.Nodes;
using ASTLib.Nodes.ExpressionNodes;
using ASTLib.Nodes.ExpressionNodes.BooleanOperationNodes;
using ASTLib.Nodes.ExpressionNodes.CommonOperationNodes;
using ASTLib.Nodes.ExpressionNodes.CommonOperationNodes.ElementAndSetOperations;
using ASTLib.Nodes.ExpressionNodes.CommonOperationNodes.RelationalOperationNodes;
using ASTLib.Nodes.TypeNodes;
using ASTLib.Objects;
using InterpreterLib.Interfaces;

namespace InterpreterLib.Helpers
{
    public class BooleanHelper : IBooleanHelper
    {
        private IInterpreterBoolean _interpreter;

        public void SetInterpreter(IInterpreterBoolean interpreter)
        {
            _interpreter = interpreter;
        }

        public bool LiteralBoolean(BooleanLiteralExpression node) => node.Value;

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
            return !_interpreter.DispatchBoolean(node.Children[0], parameters);
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
            else if (type == TypeEnum.Set)
            {
                Set lhsValue = _interpreter.DispatchSet(lhs, parameters);
                Set rhsValue = _interpreter.DispatchSet(rhs, parameters);

                res = EquivalentSets(lhsValue, rhsValue);
            }
            else if (type == TypeEnum.Element)
            {
                Element lhsValue = _interpreter.DispatchElement(lhs, parameters);
                Element rhsValue = _interpreter.DispatchElement(rhs, parameters);

                res = lhsValue.Equals(rhsValue);
            }
            else
                throw new Exception($"The type '{type}' cannot be used for equivalence");
            return res;
        }

        private bool EquivalentSets(Set a, Set b)
        {
            if (a.Elements.Count != b.Elements.Count)
                return false;
            for (int i = 0; i < a.Elements.Count; i++)
                if (!a.Elements[i].Equals(b.Elements[i]))
                    return false;
            return true;
        }

        public bool InBoolean(InExpression node, List<object> parameters)
        {
            ExpressionNode lhs = node.Children[0];
            ExpressionNode rhs = node.Children[1];
            Element lhsValue   = _interpreter.DispatchElement(lhs, parameters);
            Set rhsValue       = _interpreter.DispatchSet(rhs, parameters);

            return rhsValue.Elements.Contains(lhsValue);
        }
    }
}
