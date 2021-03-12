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
<<<<<<< HEAD
            
=======
            referenceHandler.InsertReferences(ast);
            typeChecker.CheckTypes(ast);
            string output = interpreter.Interpret(ast);
            fileGenerator.Export(output);
>>>>>>> parent of c86d9f8... Remove throws, change argument for FileGenerator.Export, removed UTestLib
        }
    }
}