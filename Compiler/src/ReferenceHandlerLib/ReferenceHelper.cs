using ASTLib;
using ASTLib.Interfaces;
using ASTLib.Nodes;
using ASTLib.Nodes.ExpressionNodes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ReferenceHandlerLib
{
    public class ReferenceHelper : IReferenceHelper
    {
        private Dictionary<string, List<int>> _functionTable;
        private Dictionary<string, int> _functionIdentifierTable;

        private Action<ExpressionNode, List<string>> _dispatch;

        public void SetDispatch(Action<ExpressionNode, List<string>> dispatch)
        {
            _dispatch = dispatch;
        }

        public void BuildTables(List<FunctionNode> functions)
        {
            Dictionary<string, List<int>> table = new Dictionary<string, List<int>>();
            Dictionary<string, int> functionIdentifierTable = new Dictionary<string, int>();

            for (int i = 0; i < functions.Count; i++)
            {
                string name = functions[i].ParameterIdentifiers.Count + functions[i].Identifier;
                if (!table.ContainsKey(name))
                {
                    table.Add(name, new List<int>() { i });
                }
                else
                {
                    table[name].Add(i);
                }
            }
            _functionTable = table;

            for (int i = 0; i < functions.Count; i++)
            {
                string identifier = functions[i].Identifier;
                if (!functionIdentifierTable.ContainsKey(identifier))
                {
                    functionIdentifierTable.Add(identifier, i);
                }
                else
                {
                    functionIdentifierTable[identifier] = -1;
                } 
            }
            _functionIdentifierTable = functionIdentifierTable;
        }

        public void VisitExport(ExportNode node)
        {
            _dispatch(node.ExportValue, new List<string>() { });
        }

        public void VisitFunction(FunctionNode node)
        {
            if (HasUniqueParameters(node.ParameterIdentifiers))
            {
                throw new Exception($"Two or more of same parameter identifiers");
            }
            VisitCondition(node.Conditions[0], node.ParameterIdentifiers);
        }

        private bool HasUniqueParameters(List<string> parameters)
        {
            return (parameters.Count != parameters.Distinct().ToList().Count);
        }

        private void VisitCondition(ConditionNode node, List<string> identifiers)
        {
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
                if (identifiers[i] == node.Id)
                {
                    node.Reference = i;
                }
            }
            node.IsLocal = (node.Reference != -1);
            if (!node.IsLocal)
            {
                if (_functionIdentifierTable.ContainsKey(node.Id))
                {
                    node.Reference = _functionIdentifierTable[node.Id];
                }
                else throw new Exception($"{node.Id} is not a valid identifier");

                if (node.Reference == -1)
                {
                    throw new Exception($"points to an overloaded function");
                }
            }
        }

        public void VisitFunctionCall(FunctionCallExpression node, List<string> identifiers)
        {
            List<int> list = new List<int>();
            list = _functionTable[node.Children.Count + node.Identifier];
            node.GlobalReferences = list.ToList();
        }


    }
}