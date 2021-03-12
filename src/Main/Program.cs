using System;
using ASTLib;
using LexParserLib;

namespace Main
{
    class Program
    {
        static void Main(string[] args)
        {
            LexParser lexParse = new LexParser(new ASTBuilder());
            AST ast = lexParse.Run();
            
        }
    }
}