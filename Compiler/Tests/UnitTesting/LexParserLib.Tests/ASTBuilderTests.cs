using ASTLib;
using ASTLib.Nodes.ExpressionNodes;
using Hime.Redist;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using LexParserLib.ASTBuilding;
using LexParserLib.Hime;

namespace LexParserLib.Tests
{
    [TestClass]
    public class ASTBuilderTests
    {
        #region Check dobule value 33.3
        [TestMethod]
        public void Export_2Doubles_lastNode_LetterNum()
        {
            string input = "export 5.5 + 33.3 {\"\"}";

            AST ast = new LexParser(new ASTBuilder(new ExpressionHelper())).Run(input, false);

            RealLiteralExpression node = (RealLiteralExpression)ast.Exports[0].ExportValue.Children[1];
            Assert.AreEqual(14, node.LetterNumber);
        }
        [TestMethod]
        public void Export_2Doubles_lastNode_Value()
        {
            string input = "export 5.5 + 33.3 {\"\"}";

            AST ast = new LexParser(new ASTBuilder(new ExpressionHelper())).Run(input, false);

            RealLiteralExpression node = (RealLiteralExpression)ast.Exports[0].ExportValue.Children[1];
            Assert.AreEqual(33.3, node.Value);
        }
        #endregion

        #region Check int value 2
        [TestMethod]
        public void Export_2Doubles2Ints_2ndLastNode_LetterNum()
        {
            string input = "export 5.5 + 33.3 + 2 + 1 {\"\"}";

            AST ast = new LexParser(new ASTBuilder(new ExpressionHelper())).Run(input, false);

            IntegerLiteralExpression node = (IntegerLiteralExpression)ast.Exports[0].ExportValue.Children[0].Children[1];
            Assert.AreEqual(21, node.LetterNumber);
        }
        [TestMethod]
        public void Export_2Doubles2Ints_2ndLastNode_Value()
        {
            string input = "export 5.5 + 33.3 + 2 + 1 {\"\"}";

            AST ast = new LexParser(new ASTBuilder(new ExpressionHelper())).Run(input, false);

            IntegerLiteralExpression node = (IntegerLiteralExpression)ast.Exports[0].ExportValue.Children[0].Children[1];
            Assert.AreEqual(2, node.Value);
        }
        #endregion

        #region Check Function
        [TestMethod]
        public void Function_int_int_ParamName()
        {
            string input =      "func: (integer) -> integer " +
                                "func(x) = x + 1";

            AST ast = new LexParser(new ASTBuilder(new ExpressionHelper())).Run(input, false);

            var func = ast.Functions[0];
            Assert.AreEqual("x", func.ParameterIdentifiers[0]);
        }
        [TestMethod]
        public void Function_int_int_FuncName()
        {
            string input = "func: (integer) -> integer " +
                                "func(x) = x + 1";

            AST ast = new LexParser(new ASTBuilder(new ExpressionHelper())).Run(input, false);

            var func = ast.Functions[0];
            Assert.AreEqual("func", func.Identifier);
        }
        [TestMethod]
        public void Function_int_int_InputType()
        {
            string input = "func: (integer) -> integer " +
                                "func(x) = x + 1";

            AST ast = new LexParser(new ASTBuilder(new ExpressionHelper())).Run(input, false);

            var func = ast.Functions[0];
            Assert.AreEqual(ASTLib.Nodes.TypeNodes.TypeEnum.Integer, func.FunctionType.ParameterTypes[0].Type);
        }
        [TestMethod]
        public void Function_int_int_OutputType()
        {
            string input = "func: (integer) -> integer " +
                                "func(x) = x + 1";

            AST ast = new LexParser(new ASTBuilder(new ExpressionHelper())).Run(input, false);

            var func = ast.Functions[0];
            Assert.AreEqual(ASTLib.Nodes.TypeNodes.TypeEnum.Integer, func.FunctionType.ReturnType.Type);
        }
        #endregion
    }
}
