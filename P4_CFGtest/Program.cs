using System;
using Hime.Redist; // the namespace for the Hime Runtime
using P4CfgtestIteration1; // default namespace for the parser is the grammar's name

namespace P4_CFGtest
{
    class Program
    {
        public static void Main(string[] args)
        {
            // Creates the lexer and parser
            P4_CFGtest_Iteration1Lexer lexer = new P4_CFGtest_Iteration1Lexer("export 2.2 + 2.3");
            P4_CFGtest_Iteration1Parser parser = new P4_CFGtest_Iteration1Parser(lexer);
            // Executes the parsing
            ParseResult result = parser.Parse();
            // Prints the produced syntax tree
            Print(result.Root, new bool[] {});
        }

        private static void Print(ASTNode node, bool[] crossings)
        {
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