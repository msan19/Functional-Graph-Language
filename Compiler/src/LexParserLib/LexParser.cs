using System;
using ASTLib;
using Hime.Redist;
using System.Linq;
using System.Collections.Generic;
using ASTLib.Exceptions;

namespace LexParserLib
{
    public class LexParser : ILexParser
    {
        private ASTBuilder _astBuilder;

        public LexParser(ASTBuilder astBuilder)
        {
            _astBuilder = astBuilder;
        }

        public AST Run(string input, bool print)
        {
            GrammarLexer lexer = new GrammarLexer(input);
            GrammarParser parser = new GrammarParser(lexer);
            // Executes the parsing
            ParseResult result = parser.Parse();
            CheckErrors(result);
            // Prints the produced syntax tree
            if(print) 
                Print(result.Root, new bool[] { });
            return _astBuilder.GetAST(result.Root);
        }

        private void CheckErrors(ParseResult result)
        {
            if (result.Errors.Count > 0)
            {
                List<ParseError> list = result.Errors.ToList();
                List<int> lines = list.ConvertAll(e => e.Position.Line);
                List<int> letters = list.ConvertAll(e => e.Position.Column);
                List<string> messages = list.ConvertAll(e => e.Message);
                throw new ParserException(messages, lines, letters);
            }
        }

        private void Print(ASTNode node, bool[] crossings)
        {
            ASTNode node1 = node;
            for (int i = 0; i < crossings.Length - 1; i++)
                Console.Write(crossings[i] ? "|   " : "    ");
            if (crossings.Length > 0)
                Console.Write("+-> ");
            Console.WriteLine(node.ToString());
            for (int i = 0; i != node.Children.Count; i++)
            {
                bool[] childCrossings = new bool[crossings.Length + 1];
                Array.Copy(crossings, childCrossings, crossings.Length);
                childCrossings[childCrossings.Length - 1] = (i < node.Children.Count - 1);
                Print(node.Children[i], childCrossings);
            }
        }
    }
}