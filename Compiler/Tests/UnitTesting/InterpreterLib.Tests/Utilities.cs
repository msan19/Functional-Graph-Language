using ASTLib;
using ASTLib.Nodes;
using ASTLib.Nodes.ExpressionNodes;
using ASTLib.Nodes.ExpressionNodes.CastExpressionNodes;
using ASTLib.Nodes.ExpressionNodes.NumberOperationNodes;
using ASTLib.Nodes.TypeNodes;
using ASTLib.Objects;
using InterpreterLib.Helpers;
using InterpreterLib.Interfaces;
using NSubstitute;
using System;
using System.Collections.Generic;

namespace InterpreterLib.Tests
{
    public static class Utilities
    {
        public static BooleanHelper GetBooleanHelper(IInterpreterBoolean interpreter)
        {
            BooleanHelper booleanHelper = new BooleanHelper();
            booleanHelper.SetInterpreter(interpreter);
            return booleanHelper;
        }

        public static BooleanHelper GetBooleanHelper(IInterpreterBoolean interpreter, AST ast)
        {
            BooleanHelper booleanHelper = GetBooleanHelper(interpreter);
            return booleanHelper;
        }

        internal static IInterpreterElement GetIntepreterElement(List<IntegerLiteralExpression> childs, int[] ids)
        {
            var res = Substitute.For<IInterpreterElement>();
            for (int i = 0; i < childs.Count; i++)
            {
                res.DispatchInt(childs[i], Arg.Any<List<object>>()).Returns(ids[i]);
            }
            return res;
        }

        internal static IInterpreterElement GetIntepreterElementWithParamsOut(IntegerLiteralExpression child, int id, Action<List<object>> action)
        {
            var res = Substitute.For<IInterpreterElement>();
            res.DispatchInt(child, Arg.Do<List<object>>(x => action(x))).Returns(id);
            return res;
        }

        internal static ElementHelper GetElementHelper(IInterpreterElement parent)
        {
            var res = new ElementHelper();
            res.SetInterpreter(parent);
            return res;
        }

        public static GenericHelper GetGenericHelper(IInterpreterGeneric interpreter)
        {
            GenericHelper helper = new GenericHelper();
            helper.SetInterpreter(interpreter);
            return helper;
        }
        public static GenericHelper GetGenericHelper(IInterpreterGeneric interpreter, AST ast)
        {
            GenericHelper helper = GetGenericHelper(interpreter);
            helper.SetASTRoot(ast);
            return helper;
        }

        internal static CastFromIntegerExpression GetCastNode(IntegerLiteralExpression inputNode)
        {
            return new CastFromIntegerExpression(inputNode, 0, 0);
        }

        internal static CastFromRealExpression GetCastNode(RealLiteralExpression inputNode)
        {
            return new CastFromRealExpression(inputNode, 0, 0);
        }

        internal static CastFromBooleanExpression GetCastNode(BooleanLiteralExpression inputNode)
        {
            return new CastFromBooleanExpression(inputNode, 0, 0);
        }

        internal static IInterpreterString GetStringInterpreter()
        {
            return Substitute.For<IInterpreterString>();
        }

        public static IInterpreterGeneric GetGenericInterpreter()
        {
            return Substitute.For<IInterpreterGeneric>();
        }

        public static Interpreter GetFullyMockedIntepreter()
        {
            IFunctionHelper functionHelper = Substitute.For<IFunctionHelper>();
            IIntegerHelper integerHelper = Substitute.For<IIntegerHelper>();
            IRealHelper realHelper = Substitute.For<IRealHelper>();
            IBooleanHelper booleanHelper = Substitute.For<IBooleanHelper>();
            IGenericHelper genericHelper = Substitute.For<IGenericHelper>();
            ISetHelper setHelper = Substitute.For<ISetHelper>();
            IElementHelper elemHelper = Substitute.For<IElementHelper>();
            IStringHelper stringHelper = Substitute.For<IStringHelper>();
            IGraphHelper graphHelper = Substitute.For<IGraphHelper>();
            return new Interpreter(genericHelper, functionHelper, integerHelper, realHelper, booleanHelper, setHelper, elemHelper, stringHelper, graphHelper, false);
        }
        
        public static Interpreter GetIntepreterOnlyWith(IFunctionHelper functionHelper)
        {
            IIntegerHelper integerHelper = Substitute.For<IIntegerHelper>();
            IRealHelper realHelper = Substitute.For<IRealHelper>();
            IBooleanHelper booleanHelper = Substitute.For<IBooleanHelper>();
            IGenericHelper genericHelper = Substitute.For<IGenericHelper>();
            ISetHelper setHelper = Substitute.For<ISetHelper>();
            IElementHelper elemHelper = Substitute.For<IElementHelper>();
            IStringHelper stringHelper = Substitute.For<IStringHelper>();
            IGraphHelper graphHelper = Substitute.For<IGraphHelper>();
            return new Interpreter(genericHelper, functionHelper, integerHelper, realHelper, booleanHelper, setHelper, elemHelper, stringHelper, graphHelper, false);
        }

        public static Interpreter GetIntepreterOnlyWith(ISetHelper setHelper)
        {
            IIntegerHelper integerHelper = Substitute.For<IIntegerHelper>();
            IRealHelper realHelper = Substitute.For<IRealHelper>();
            IBooleanHelper booleanHelper = Substitute.For<IBooleanHelper>();
            IGenericHelper genericHelper = Substitute.For<IGenericHelper>();
            IFunctionHelper functionHelper = Substitute.For<IFunctionHelper>();
            IElementHelper elemHelper = Substitute.For<IElementHelper>();
            IStringHelper stringHelper = Substitute.For<IStringHelper>();
            IGraphHelper graphHelper = Substitute.For<IGraphHelper>();
            return new Interpreter(genericHelper, functionHelper, integerHelper, realHelper, booleanHelper, setHelper, elemHelper, stringHelper, graphHelper, false);
        }

        public static Interpreter GetIntepreterOnlyWith(IGenericHelper genericHelper)
        {
            IIntegerHelper integerHelper = Substitute.For<IIntegerHelper>();
            IRealHelper realHelper = Substitute.For<IRealHelper>();
            IBooleanHelper booleanHelper = Substitute.For<IBooleanHelper>();
            IFunctionHelper functionHelper = Substitute.For<IFunctionHelper>();
            ISetHelper setHelper = Substitute.For<ISetHelper>();
            IElementHelper elemHelper = Substitute.For<IElementHelper>();
            IStringHelper stringHelper = Substitute.For<IStringHelper>();
            IGraphHelper graphHelper = Substitute.For<IGraphHelper>();
            return new Interpreter(genericHelper, functionHelper, integerHelper, realHelper, booleanHelper, setHelper, elemHelper, stringHelper, graphHelper, false);
        }

        internal static FunctionNode GetFunction()
        {
            return new FunctionNode("", null, null, null, 0, 0);
        }
        internal static FunctionNode GetFunction(FunctionTypeNode funcType)
        {
            return new FunctionNode("", null, null, funcType, 0, 0);
        }

        internal static AST GetAst()
        {
            return new AST(new List<FunctionNode>(), new List<ExportNode>(), 0, 0);
        }

        internal static AST GetAst(FunctionNode func)
        {
            return new AST(new List<FunctionNode>() { func }, new List<ExportNode>(), 0, 0);
        }

        internal static AST GetAst(int funcCount)
        {
            var ast = GetAst();
            for (int i = 0; i < funcCount; i++)
                ast.Functions.Add(GetFunction());
            return ast;
        }

        public static List<object> GetParameterList() => new List<object>();
        public static List<object> GetParameterList(int count)
        {
            var list = GetParameterList();
            for (int i = 0; i < count; i++)
                list.Add(null);
            return list;
        }
        
        public static Interpreter GetIntepreterOnlyWith(IIntegerHelper integerHelper)
        {
            IFunctionHelper functionHelper = Substitute.For<IFunctionHelper>();
            IRealHelper realHelper = Substitute.For<IRealHelper>();
            IBooleanHelper booleanHelper = Substitute.For<IBooleanHelper>();
            IGenericHelper genericHelper = Substitute.For<IGenericHelper>();
            ISetHelper setHelper = Substitute.For<ISetHelper>();
            IElementHelper elemHelper = Substitute.For<IElementHelper>();
            IStringHelper stringHelper = Substitute.For<IStringHelper>();
            IGraphHelper graphHelper = Substitute.For<IGraphHelper>();
            return new Interpreter(genericHelper, functionHelper, integerHelper, realHelper, booleanHelper, setHelper, elemHelper, stringHelper, graphHelper, false);
        }
        
        public static Interpreter GetIntepreterOnlyWith(IRealHelper realHelper)
        {
            IFunctionHelper functionHelper = Substitute.For<IFunctionHelper>();
            IIntegerHelper integerHelper = Substitute.For<IIntegerHelper>();
            IBooleanHelper booleanHelper = Substitute.For<IBooleanHelper>();
            IGenericHelper genericHelper = Substitute.For<IGenericHelper>();
            ISetHelper setHelper = Substitute.For<ISetHelper>();
            IElementHelper elemHelper = Substitute.For<IElementHelper>();
            IStringHelper stringHelper = Substitute.For<IStringHelper>();
            IGraphHelper graphHelper = Substitute.For<IGraphHelper>();
            return new Interpreter(genericHelper, functionHelper, integerHelper, realHelper, booleanHelper, setHelper, elemHelper, stringHelper, graphHelper, false);
        }

        internal static FunctionTypeNode GetFunctionTypeNode(int expectedElementCount, TypeEnum returnType)
        {
            var inputParams = new List<TypeNode>();
            for (int i = 0; i < expectedElementCount; i++)
                inputParams.Add(new TypeNode(returnType, 0, 0));
            return new FunctionTypeNode(new TypeNode(TypeEnum.Boolean, 0, 0), inputParams, 0, 0);
        }

        internal static List<ExpressionNode> GetIntLitExprNodes(int count)
        {
            var res = new List<ExpressionNode>();
            for (int i = 0; i < count; i++)
                res.Add(new IntegerLiteralExpression("0", 0, 0));
            return res;
        }

        internal static IEnumerable<Element> GetElements(int n, int dims)
        {
            var res = new List<Element>();
            for (int i = 0; i < n; i++)
                res.Add(GetElement(dims));
            return res;
        }

        private static Element GetElement(int dims)
        {
            var ids = new List<int>();
            for (int i = 0; i < dims; i++)
                ids.Add(i);
            return new Element(ids);
        }

        internal static IEnumerable<object> ConvertElementNodesToInts(List<ElementNode> elements)
        {
            var res = new List<object>();
            foreach (var e in elements)
            {
                for (int i = 0; i < e.IndexIdentifiers.Count; i++)
                {
                    res.Add(i);
                }
            }
            return res;
        }

        internal static List<ElementNode> GetElementNodess(int n, int dims, int refOffset)
        {
            var res = new List<ElementNode>();
            for (int i = 0; i < n; i++)
            {
                res.Add(GetElementNode(i + refOffset, dims));
            }
            return res;
        }

        private static ElementNode GetElementNode(int r, int dims)
        {
            var ids = new List<string>();
            for (int i = 0; i < dims; i++)
                ids.Add(((char)(i + 65)).ToString());
            var e = new ElementNode("", ids, 0, 0);
            e.Reference = r;
            return e;
        }

        internal static ConditionNode GetConditionNode(List<ElementNode> elements, ExpressionNode conditionExpr, ExpressionNode returnExpr)
        {
            return new ConditionNode(elements, conditionExpr, returnExpr, 0, 0);
        }

        public static Interpreter GetIntepreterOnlyWith(IBooleanHelper booleanHelper)
        {
            IFunctionHelper functionHelper = Substitute.For<IFunctionHelper>();
            IIntegerHelper integerHelper = Substitute.For<IIntegerHelper>();
            IRealHelper realHelper = Substitute.For<IRealHelper>(); 
            IGenericHelper genericHelper = Substitute.For<IGenericHelper>();
            ISetHelper setHelper = Substitute.For<ISetHelper>();
            IElementHelper elemHelper = Substitute.For<IElementHelper>();
            IStringHelper stringHelper = Substitute.For<IStringHelper>();
            IGraphHelper graphHelper = Substitute.For<IGraphHelper>();
            return new Interpreter(genericHelper, functionHelper, integerHelper, realHelper, booleanHelper, setHelper, elemHelper, stringHelper, graphHelper, false);
        }

        public static IntegerLiteralExpression GetIntLitExpression()
        {
            return new IntegerLiteralExpression("", 0, 0);
        }

        internal static List<IntegerLiteralExpression> GetIntLitExpressions(int n)
        {
            var res = new List<IntegerLiteralExpression>();
            for (int i = 0; i < n; i++)
                res.Add(GetIntLitExpression());
            return res;
        }

        public static RealLiteralExpression GetRealLitExpression()
        {
            return new RealLiteralExpression("1.0", 0, 0);
        }
        
        public static BooleanLiteralExpression GetBoolLitExpression()
        {
            return new BooleanLiteralExpression(default, 0, 0);
        }
        
        public static BooleanLiteralExpression GetBoolLitExpression(bool val)
        {
            return new BooleanLiteralExpression(val, 0, 0);
        }

        public static FunctionCallExpression GetFuncCallExpresssion()
        {
            return new FunctionCallExpression("", new List<ExpressionNode>(), 0, 0);
        }

        public static ElementExpression GetElementExpression()
        {
            return new ElementExpression(new List<ExpressionNode>(), 0, 0);
        }

        public static ElementExpression GetElementExpression(IntegerLiteralExpression child)
        {
            return new ElementExpression(new List<ExpressionNode>() { child }, 0, 0);
        }

        internal static ElementExpression GetElementExpression(List<IntegerLiteralExpression> childs)
        {
            return new ElementExpression(childs.ConvertAll<ExpressionNode>(x => x), 0, 0);
        }

        public static SetExpression GetSetExpresssion()
        {
            return new SetExpression(null, null, null, 0, 0);
        }
    }
}