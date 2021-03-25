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
        public IReferenceHandler ReferenceHandler { get; set; }
        private Dictionary<string, List<int>> _functionTable;

        public void BuildTable(List<FunctionNode> functions)
        {
            Dictionary<string, List<int>> table = new Dictionary<string, List<int>>();
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
        }

        public void VisitExport(ExportNode node)
        {
            ReferenceHandler.Dispatch(node.ExportValue, new List<string>() { });
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
            ReferenceHandler.Dispatch(node.ReturnExpression, identifiers);
        }

        public void VisitNonIdentifier(INonIdentifierExpression node, List<string> identifiers)
        {
            foreach (ExpressionNode child in node.Children)
            {
                ReferenceHandler.Dispatch(child, identifiers);
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
            if (node.Reference == -1)
            {
                throw new Exception($"{node.Id} is not a valid parameter identifier");
            }
        }

        public void VisitFunctionCall(FunctionCallExpression node, List<string> identifiers)
        {
            node.GlobalReferences = _functionTable[node.Children.Count + node.Identifier];
        }


    }
}