using System;
using ASTLib;
using FileGeneratorLib;
using InterpreterLib;
using InterpreterLib.Helpers;
using LexParserLib;
using ReferenceHandlerLib;
using TypeCheckerLib;
using TypeCheckerLib.Helpers;

namespace Main
{
    public class Program
    {
        static void Main(string[] args)
        {
            new Program(args);
        }

        public Program(string[] args)
        {
            LexParser lexParse = new LexParser(new ASTBuilder());
            ReferenceHandler referenceHandler = new ReferenceHandler(new ReferenceHelper());
            TypeChecker typeChecker = new TypeChecker(new DeclarationHelper(), new NumberHelper(), new CommonOperatorHelper(), new TypeCheckerLib.Helpers.BooleanHelper());
            Interpreter interpreter = new Interpreter(new FunctionHelper(), new IntegerHelper(), new RealHelper());
            FileGenerator fileGenerator = new FileGenerator(new FileHelper());

            string input = "export 5.5 + 33.3 export 5.5 * func(33) " +
                                   "func: (integer) -> real " +
                                   "func(p) = p * 17 + p / 2 ^ (17 - 0.1 mod 2)";
            string file = "";
            //input = ReadFile(file);

            AST ast = lexParse.Run(input);
            referenceHandler.InsertReferences(ast);
            typeChecker.CheckTypes(ast);
            var output = interpreter.Interpret(ast);
            fileGenerator.Export(output, file);
        }

        private string ReadFile(string file)
        {
            try
            {
                return System.IO.File.ReadAllText(file);
            } catch
            {
                throw;
            }
        }

    }
}