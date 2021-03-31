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
        public IInterpreter Interpreter { get; set; }
        private Func<ExpressionNode, List<object>, bool> _dispatchBoolean;
        private Func<ExpressionNode, List<object>, int> _dispatchInteger;
        private Func<ExpressionNode, List<object>, double> _dispatchReal;
        private Func<ExpressionNode, List<object>, TypeEnum, object> _dispatch;
        private Func<FunctionNode,   List<object>, bool> _functionBoolean;
        private AST _root;

        public void SetASTRoot(AST root)
        {
            _root = root;
        }

        public void SetUpFuncs(Func<ExpressionNode, List<object>, bool> dispatchBoolean,
            Func<ExpressionNode, List<object>, int> dispatchInteger,
            Func<ExpressionNode, List<object>, double> dispatchReal,
            Func<ExpressionNode, List<object>, TypeEnum, object> dispatch,
            Func<FunctionNode,   List<object>, bool> functionBoolean)
        {
            _dispatchBoolean = dispatchBoolean;
            _dispatchInteger = dispatchInteger;
            _dispatchReal = dispatchReal;
            _dispatch = dispatch;
            _functionBoolean = functionBoolean;
        }
        
        public bool FunctionCallBoolean(FunctionCallExpression node, List<object> parameters)
        {
            throw new NotImplementedException();
            
            
            // return _functionBoolean
        }
        
        public bool AndBoolean(AndExpression node, List<object> parameters)
        {
            bool leftOperand =  _dispatchBoolean(node.Children[0], parameters);
            bool rightOperand = _dispatchBoolean(node.Children[1], parameters);

            return leftOperand && rightOperand;
        }

        public bool OrBoolean(OrExpression node, List<object> parameters)
        {
            bool leftOperand =  _dispatchBoolean(node.Children[0], parameters);
            bool rightOperand = _dispatchBoolean(node.Children[1], parameters);

            return leftOperand || rightOperand;
        }
        
        public bool ConditionBoolean(ConditionNode node, List<object> parameters)
        {
            throw new NotImplementedException();
        }

        public bool EqualBoolean(EqualExpression node, List<object> parameters)
        {
            throw new NotImplementedException();
        }

        public bool GreaterBoolean(GreaterExpression node, List<object> parameters)
        {
            throw new NotImplementedException();
        }

        public bool GreaterEqualBoolean(GreaterEqualExpression node, List<object> parameters)
        {
            throw new NotImplementedException();
        }

        public bool IdentifierBoolean(IdentifierExpression node, List<object> parameters)
        {
            throw new NotImplementedException();
        }

        public bool LessBoolean(LessExpression node, List<object> parameters)
        {
            throw new NotImplementedException();
        }

        public bool LessEqualBoolean(LessEqualExpression node, List<object> parameters)
        {
            throw new NotImplementedException();
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
