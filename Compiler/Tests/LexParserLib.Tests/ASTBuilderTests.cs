using ASTLib;
using ASTLib.Nodes.ExpressionNodes;
using Hime.Redist;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace LexParserLib.Tests
{
    [TestClass]
    public class ASTBuilderTests
    {
        [TestMethod]
        public void Export_2Doubles_lastNode()
        {
            string input = "export 5.5 + 33.3";

            AST ast = new LexParser(new ASTBuilder()).Run(input);

            RealLiteralExpression node = (RealLiteralExpression)ast.Exports[0].ExportValue.Children[1];
            Assert.AreEqual(14, node.LetterNumber);
            Assert.AreEqual(33.3, node.Value);
        }

        [TestMethod]
        public void Export_2Doubles2Ints_2ndLastNode()
        {
            string input = "export 5.5 + 33.3 + 2 + 1";

            AST ast = new LexParser(new ASTBuilder()).Run(input);

            IntegerLiteralExpression node = (IntegerLiteralExpression)ast.Exports[0].ExportValue.Children[0].Children[1];
            Assert.AreEqual(21, node.LetterNumber);
            Assert.AreEqual(2, node.Value);
        }

        [TestMethod]
        public void Function_int_int()
        {
            string input =      "func: (integer) -> integer " +
                                "func(x) = x + 1";

            AST ast = new LexParser(new ASTBuilder()).Run(input);

            var func = ast.Functions[0];
            Assert.AreEqual("x", func.ParameterIdentifiers[0]);
            Assert.AreEqual("func", func.Identifier);
            Assert.AreEqual(ASTLib.Nodes.TypeNodes.Type.Integer, func.FunctionType.ParameterTypes[0].Type);
            Assert.AreEqual(ASTLib.Nodes.TypeNodes.Type.Integer, func.FunctionType.ReturnType.Type);
        }
    }
}
