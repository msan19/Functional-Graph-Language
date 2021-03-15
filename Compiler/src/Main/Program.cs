using System;
using ASTLib;
using FileGeneratorLib;
using InterpreterLib;
using LexParserLib;
using ReferenceHandlerLib;
using TypeCheckerLib;

namespace Main
{
    public class Program
    {
        static void Main(string[] args)
        {
            Run(args);
        }

        public static void Run(string[] args)
        {
            LexParser lexParse = new LexParser(new ASTBuilder());
            ReferenceHandler referenceHandler = new ReferenceHandler(new ReferenceHelper());
            TypeChecker typeChecker = new TypeChecker(new TypeHelper());
            Interpreter interpreter = new Interpreter(new InterpretorHelper());
            FileGenerator fileGenerator = new FileGenerator(new FileHelper());

            string input = "export 5.5 + 33.3 export 5.5 + 33.3 " +
                                   "func: (integer) -> integer " +
                                   "func(p) = |z| * y + x / i ^ (17 - 0.1 mod 2)";

            AST ast = lexParse.Run(input);
            referenceHandler.InsertReferences(ast);
            typeChecker.CheckTypes(ast);
            var output = interpreter.Interpret(ast);
            fileGenerator.Export(output);
        }
    }
}