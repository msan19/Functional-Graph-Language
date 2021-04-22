using System;
using System.Collections.Generic;
using ASTLib;
using ASTLib.Exceptions;
using ASTLib.Exceptions.Component;
using ASTLib.Interfaces;
using ASTLib.Nodes;
using ASTLib.Nodes.ExpressionNodes;

namespace ReferenceHandlerLib
{
    public class ReferenceHandler : IReferenceHandler
    {
        private readonly IReferenceHelper _helper;
        private bool _catchExceptions;
        private ComponentException _exceptions;

        public ReferenceHandler(IReferenceHelper helper, bool catchExceptions) : this(helper)
        {
            _catchExceptions = catchExceptions;
            _exceptions = new ComponentException();
        }

        public ReferenceHandler(IReferenceHelper helper)
        {
            _helper = helper;
            _helper.SetDispatch(Dispatch);
        }

        public void InsertReferences(AST root)
        {
            _helper.BuildTables(root.Functions);
            foreach (FunctionNode f in root.Functions)
                Try(() => _helper.VisitFunction(f));

            foreach (ExportNode e in root.Exports)
                Try(() => _helper.VisitExport(e));

            if (_catchExceptions && !_exceptions.IsEmpty)
                throw _exceptions;
        }

        public void Dispatch(ExpressionNode node, List<string> parameters)
        {
            if (node.GetType() == typeof(IdentifierExpression))
                Console.WriteLine();

            Try(() =>
            {
                switch (node)
                {
                    case IdentifierExpression e: _helper.VisitIdentifier(e, parameters); break;
                    case INonIdentifierExpression e: _helper.VisitNonIdentifier(e, parameters); break;
                    case FunctionCallExpression e: _helper.VisitFunctionCall(e, parameters); break;
                    case SetExpression e: _helper.VisitSet(e, parameters); break;

                    default: throw new UnimplementedReferenceHandlerException(node);
                }
            });
        }

        private void Try(Action action)
        {
            if (_catchExceptions)
            {
                try
                {
                    action();
                }
                catch (CompilerException e)
                {
                    _exceptions.Add(e);
                }
            }
            else
                action();
        }

    }
}