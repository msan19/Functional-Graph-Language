using ASTLib;
using ASTLib.Nodes;
using ASTLib.Nodes.ExpressionNodes;
using ASTLib.Nodes.TypeNodes;
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
            booleanHelper.SetASTRoot(ast);
            return booleanHelper;
        }

        public static Interpreter GetFullyMockedIntepretor()
        {
            IFunctionHelper functionHelper = Substitute.For<IFunctionHelper>();
            IIntegerHelper integerHelper = Substitute.For<IIntegerHelper>();
            IRealHelper realHelper = Substitute.For<IRealHelper>();
            IBooleanHelper booleanHelper = Substitute.For<IBooleanHelper>();
            return new Interpreter(functionHelper, integerHelper, realHelper, booleanHelper);
        }
        
        public static Interpreter GetIntepretorOnlyWith(IFunctionHelper functionHelper)
        {
            IIntegerHelper integerHelper = Substitute.For<IIntegerHelper>();
            IRealHelper realHelper = Substitute.For<IRealHelper>();
            IBooleanHelper booleanHelper = Substitute.For<IBooleanHelper>();
            return new Interpreter(functionHelper, integerHelper, realHelper, booleanHelper);
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
        
        public static Interpreter GetIntepretorOnlyWith(IIntegerHelper integerHelper)
        {
            IFunctionHelper functionHelper = Substitute.For<IFunctionHelper>();
            IRealHelper realHelper = Substitute.For<IRealHelper>();
            IBooleanHelper booleanHelper = Substitute.For<IBooleanHelper>();
            return new Interpreter(functionHelper, integerHelper, realHelper, booleanHelper);
        }
        
        public static Interpreter GetIntepretorOnlyWith(IRealHelper realHelper)
        {
            IFunctionHelper functionHelper = Substitute.For<IFunctionHelper>();
            IIntegerHelper integerHelper = Substitute.For<IIntegerHelper>();
            IBooleanHelper booleanHelper = Substitute.For<IBooleanHelper>();
            return new Interpreter(functionHelper, integerHelper, realHelper, booleanHelper);
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

        public static Interpreter GetIntepretorOnlyWith(IBooleanHelper booleanHelper)
        {
            IFunctionHelper functionHelper = Substitute.For<IFunctionHelper>();
            IIntegerHelper integerHelper = Substitute.For<IIntegerHelper>();
            IRealHelper realHelper = Substitute.For<IRealHelper>();
            return new Interpreter(functionHelper, integerHelper, realHelper, booleanHelper);
        }

        public static IntegerLiteralExpression GetIntLitExpression()
        {
            return new IntegerLiteralExpression("", 0, 0);
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
    }
}