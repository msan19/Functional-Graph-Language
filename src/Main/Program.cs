using System;
using ASTLibrary;
using LexParse;

namespace Main
{
    class Program
    {
        static void Main(string[] args)
        {
            LexParser lexParse = new LexParser();
            AST ast = lexParse.Run();
            
        }
    }
}