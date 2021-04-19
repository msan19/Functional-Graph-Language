using ASTLib;
using ASTLib.Interfaces;
using ASTLib.Nodes;
using ASTLib.Nodes.ExpressionNodes;
using ASTLib.Exceptions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace ReferenceHandlerLib
{
    public class ReferenceHelper : IReferenceHelper
    {
        private const int DUPLICATE_IDENTIFIER_FOUND = FunctionCallExpression.NO_LOCAL_REF;
        private const int MATCHING_ID_NOT_FOUND = -1;
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
                    functionIdentifierTable[identifier] = DUPLICATE_IDENTIFIER_FOUND;
            }
            return functionIdentifierTable;
        }

        public void VisitExport(ExportNode node)
        {
            _dispatch(node.ExportValue, new List<string>() { });
        }

        public void VisitFunction(FunctionNode node)
        {
            if (!HasUniqueParameters(node.ParameterIdentifiers))
                throw new IdenticalParameterIdentifiersException(node.ParameterIdentifiers);
            foreach (ConditionNode conditionNode in node.Conditions)
                VisitCondition(conditionNode, node.ParameterIdentifiers);
        }

        private bool HasUniqueParameters(List<string> parameters)
        {
            return parameters.Count == parameters.Distinct().ToList().Count;
        }

        public void VisitCondition(ConditionNode node, List<string> identifiers)
        {
            if (node.IsDefaultCase)
            {
                CheckCondition(node, identifiers);
                _dispatch(node.ReturnExpression, identifiers);
            }
            else
            {
                EnsureElementAreUnique(node.Elements);
                SetElementReference(node.Elements, identifiers);
                List<string> copyOfIdentifiers = identifiers.ToList();
                AddElementIndicesToIdentifierList(node.Elements, copyOfIdentifiers);
                CheckCondition(node, copyOfIdentifiers);
                _dispatch(node.ReturnExpression, copyOfIdentifiers);
            }
        }

        private void CheckCondition(ConditionNode node, List<string> identifiers)
        {
            if (node.Condition != null)
                _dispatch(node.Condition, identifiers);
        }

        private void EnsureElementAreUnique(List<ElementNode> elementsNodes)
        {
            List<string> uniqueIndices = new List<string>();
            foreach (ElementNode elementNode in elementsNodes)
                EnsureElementIndicesAreUnique(uniqueIndices, elementNode);
        }

        private void EnsureElementIndicesAreUnique(List<string> uniqueIndices, ElementNode elementNode)
        {
            foreach (var indexId in elementNode.IndexIdentifiers)
            {
                if (!uniqueIndices.Contains(indexId))
                    uniqueIndices.Add(indexId);
                else
                    throw new DuplicateElementIndexException(elementNode, indexId);
            }
        }

        private void SetElementReference(List<ElementNode> elementNodes, List<string> identifiers)
        {
            foreach (ElementNode elementNode in elementNodes)
            {
                int positionInParameterlist = FindPositionForMatchingIdentifier(elementNode, identifiers);
                elementNode.Reference = positionInParameterlist;
            }
        }

        private int FindPositionForMatchingIdentifier(ElementNode elementNode, List<string> identifiers)
        {
            for (var i = 0; i < identifiers.Count; i++)
            {
                string identifier = identifiers[i];
                if (identifier == elementNode.ElementIdentifier)
                    return i;
            }
            throw new NoMatchingIdentifierFoundException(elementNode, elementNode.ElementIdentifier);
        }

        private void AddElementIndicesToIdentifierList(List<ElementNode> elementNodes, List<string> identifiers)
        {
            for (var index = 0; index < elementNodes.Count; index++)
            {
                ElementNode elementNode = elementNodes[index];
                AddIndexIdentifiers(elementNode.IndexIdentifiers, identifiers);
            }
        }

        private void AddIndexIdentifiers(List<string> elementNodeIndexIdentifiers, List<string> identifiers)
        {
            for (int i = 0; i < elementNodeIndexIdentifiers.Count; i++)
            {
                string id = elementNodeIndexIdentifiers[i];
                identifiers.Add(id);
            }
        }

        public void VisitNonIdentifier(INonIdentifierExpression node, List<string> identifiers)
        {
            if (node.Children != null)
            {
                foreach (ExpressionNode child in node.Children)
                    _dispatch(child, identifiers);
            }
        }

        public void VisitIdentifier(IdentifierExpression node, List<string> identifiers)
        {
            for (int i = 0; i < identifiers.Count; i++)
            {
                if (identifiers[i] == node.ID)
                    node.Reference = i;
            }
            node.IsLocal = (node.Reference != DUPLICATE_IDENTIFIER_FOUND);
            if (!node.IsLocal)
            {
                if (_functionIdentifierTable.ContainsKey(node.ID))
                    node.Reference = _functionIdentifierTable[node.ID];
                else 
                    throw new InvalidIdentifierException(node);

                if (node.Reference == DUPLICATE_IDENTIFIER_FOUND)
                    throw new OverloadedFunctionIdentifierException(node);
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

        /*
            GetSet:(integer) -> set
            GetSet(n) = {e[i, j] | 0 <= [i] < n, 0 < [j] < n * n, i < j}
            GetSet(n) = {e[i, j] | 0 <= [i] < n, 0 < [j] < n * n, e in {v[a, b] | 0 <= [a] < n, 0 < [b] < n * n, a < b}}
        */
        public void VisitSet(SetExpression node, List<string> parameters)
        {
            ThrowExceptionIfIdentifiersAreInParameters(node.Element.IndexIdentifiers, parameters);
            ThrowExceptionIfIdentifiersAreNotUnique(node.Element.IndexIdentifiers);
            ThrowExceptionIfBoundsAndIndentifiersDoNotMatch(node);
            ThrowExceptionIfBoundsAreInParameters(node.Bounds, parameters, node);
            ThrowExceptionIfElementAreInParameters(node.Element, parameters);

            node.Bounds = GetSortedBounds(node);
            DispatchBounds(node, parameters);
            DispatchPredicate(node, parameters);
        }

        private void DispatchPredicate(SetExpression node, List<string> parameters)
        {
            var newParameters = parameters.ToList();
            newParameters.Add(node.Element.ElementIdentifier);
            newParameters.AddRange(node.Element.IndexIdentifiers);
            _dispatch(node.Predicate, newParameters);
        }

        private void DispatchBounds(SetExpression node, List<string> parameters)
        {
            foreach (var bound in node.Bounds)
            {
                _dispatch(bound.MinValue, parameters);
                _dispatch(bound.MaxValue, parameters);
            }
        }

        private void ThrowExceptionIfIdentifiersAreInParameters(List<string> indexIdentifiers, List<string> parameters)
        {
            foreach (var id in indexIdentifiers)
            {
                if (parameters.Contains(id))
                    throw new IdenticalParameterIdentifiersException(parameters);
            }
        }

        private void ThrowExceptionIfBoundsAndIndentifiersDoNotMatch(SetExpression node)
        {
            if (node.Element.IndexIdentifiers.Count != node.Bounds.Count)
                throw new BoundException(node, "The number of bounds is not the same as element identifiers.");
            ThrowExceptionIfBoundsAreNotUnique(node.Bounds);
        }

        private void ThrowExceptionIfElementAreInParameters(ElementNode element, List<string> parameters)
        {
            if(parameters.Contains(element.ElementIdentifier))
                throw new IdenticalParameterIdentifiersException(parameters);
        }

        private void ThrowExceptionIfBoundsAreInParameters(List<BoundNode> bounds, List<string> parameters, SetExpression set)
        {
            foreach (var bound in bounds)
            {
                if (parameters.Contains(bound.Identifier))
                    throw new InvalidIdentifierException(bound.Identifier, set);
            }
        }

        private void ThrowExceptionIfIdentifiersAreNotUnique(List<string> indexIdentifiers)
        {
            if(!StringsAreUnique(indexIdentifiers))
                throw new IdenticalParameterIdentifiersException(indexIdentifiers);
        }

        private void ThrowExceptionIfBoundsAreNotUnique(List<BoundNode> bounds)
        {
            if(!StringsAreUnique(bounds.ConvertAll(x => x.Identifier)))
                throw new IdenticalParameterIdentifiersException(bounds.ConvertAll(x => x.Identifier));
        }

        private bool StringsAreUnique(List<string> strings)
        {
            var dic = new Dictionary<string, int>();
            foreach (var id in strings)
            {
                if (dic.ContainsKey(id))
                    dic[id]++;
                else
                    dic.Add(id, 1);
            }

            foreach (var pair in dic)
            {
                if (pair.Value > 1)
                    return false;
            }
            return true;
        }

        private List<BoundNode> GetSortedBounds(SetExpression node)
        {
            List<BoundNode> bounds = new List<BoundNode>();
            foreach (var id in node.Element.IndexIdentifiers)
            {
                bounds.Add(GetBound(id, node.Bounds, node));
            }
            return bounds;
        }

        private BoundNode GetBound(string id, List<BoundNode> bounds, SetExpression set)
        {
            foreach (var bound in bounds)
            {
                if (bound.Identifier.Equals(id))
                    return bound;
            }
            throw new InvalidIdentifierException(id, set);
        }
    }
}