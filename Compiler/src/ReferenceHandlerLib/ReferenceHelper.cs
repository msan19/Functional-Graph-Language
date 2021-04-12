using ASTLib;
using ASTLib.Interfaces;
using ASTLib.Nodes;
using ASTLib.Nodes.ExpressionNodes;
using ASTLib.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ReferenceHandlerLib
{
    public class ReferenceHelper : IReferenceHelper
    {
        private const int NO_LOCAL_REF = FunctionCallExpression.NO_LOCAL_REF;
        private Dictionary<string, List<int>> _functionTable;
        private Dictionary<string, int> _functionIdentifierTable;

        private Action<ExpressionNode, List<string>> _dispatch;

        public void SetDispatch(Action<ExpressionNode, List<string>> dispatch)
        {
            _dispatch = dispatch;
        }

        public void BuildTables(List<FunctionNode> functions)
        {
            _functionTable = GetFunctionTable(functions);
            _functionIdentifierTable = GetFunctionIdentifierTable(functions);
        }

        private Dictionary<string, List<int>> GetFunctionTable(List<FunctionNode> functions)
        {
            Dictionary<string, List<int>> table = new Dictionary<string, List<int>>();

            for (int i = 0; i < functions.Count; i++)
            {
                string name = functions[i].ParameterIdentifiers.Count + functions[i].Identifier;
                if (!table.ContainsKey(name))
                    table.Add(name, new List<int>() { i });
                else
                    table[name].Add(i);
            }
            return table;
        }
        
        private Dictionary<string, int> GetFunctionIdentifierTable(List<FunctionNode> functions)
        {
            Dictionary<string, int> functionIdentifierTable = new Dictionary<string, int>();

            for (int i = 0; i < functions.Count; i++)
            {
                string identifier = functions[i].Identifier;
                if (!functionIdentifierTable.ContainsKey(identifier))
                    functionIdentifierTable.Add(identifier, i);
                else
                    functionIdentifierTable[identifier] = NO_LOCAL_REF;
            }
            return functionIdentifierTable;
        }

        public void VisitExport(ExportNode node)
        {
            _dispatch(node.ExportValue, new List<string>() { });
        }

        public void VisitFunction(FunctionNode node)
        {
            if (HasUniqueParameters(node.ParameterIdentifiers))
            {
                throw new IdenticalParameterIdentifiersException(node.ParameterIdentifiers);
            }
            foreach (ConditionNode conditionNode in node.Conditions)
            {
                VisitCondition(conditionNode, node.ParameterIdentifiers);
            }
        }

        private bool HasUniqueParameters(List<string> parameters)
        {
            return (parameters.Count != parameters.Distinct().ToList().Count);
        }

        private void VisitCondition(ConditionNode node, List<string> identifiers)
        {
            if (node.Condition != null)
                _dispatch(node.Condition, identifiers);
            _dispatch(node.ReturnExpression, identifiers);
        }

        public void VisitNonIdentifier(INonIdentifierExpression node, List<string> identifiers)
        {
            if (node.Children != null)
            {
                foreach (ExpressionNode child in node.Children)
                {
                    _dispatch(child, identifiers);
                }
            }
        }

        public void VisitIdentifier(IdentifierExpression node, List<string> identifiers)
        {
            for (int i = 0; i < identifiers.Count; i++)
            {
                if (identifiers[i] == node.ID)
                {
                    node.Reference = i;
                }
            }
            node.IsLocal = (node.Reference != NO_LOCAL_REF);
            if (!node.IsLocal)
            {
                if (_functionIdentifierTable.ContainsKey(node.ID))
                {
                    node.Reference = _functionIdentifierTable[node.ID];
                }
                else throw new InvalidIdentifierException(node);

                if (node.Reference == NO_LOCAL_REF)
                {
                    throw new OverloadedFunctionIdentifierException(node);
                }
            }
        }

        public void VisitFunctionCall(FunctionCallExpression node, List<string> identifiers)
        {
            string key = node.Children.Count + node.Identifier;
            if (_functionTable.ContainsKey(key))
                node.GlobalReferences = _functionTable[key].ToList();
            node.LocalReference = identifiers.IndexOf(node.Identifier);

            foreach (ExpressionNode n in node.Children)
                _dispatch(n, identifiers);
        }

        public void VisitSet(SetExpression node, List<string> identifiers)
        {

        }
    }
}