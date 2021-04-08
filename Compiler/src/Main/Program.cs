using System;
using ASTLib;
using FileGeneratorLib;
using InterpreterLib;
using InterpreterLib.Helpers;
using LexParserLib;
using ReferenceHandlerLib;
using TypeCheckerLib;
using TypeCheckerLib.Helpers;
using TypeBooleanHelper = TypeCheckerLib.Helpers.BooleanHelper;
using InterpBooleanHelper = InterpreterLib.Helpers.BooleanHelper;
using ASTLib.Exceptions;

namespace Main
{
    public class Program
    {
        static void Main(string[] args)
        {
            try
            {
                new Program(args);
            } catch (CompilerException e)
            {
                Console.WriteLine($"An error was detected on line {e.Node.LineNumber}:\n{e.Message}");
                throw;
            }
        }

        public Program(string[] args)
        {
            LexParser lexParse = new LexParser(new ASTBuilder());
            ReferenceHandler referenceHandler = new ReferenceHandler(new ReferenceHelper());
            TypeChecker typeChecker = new TypeChecker(new DeclarationHelper(), new NumberHelper(), new CommonOperatorHelper(), new TypeBooleanHelper());
            Interpreter interpreter = new Interpreter(new FunctionHelper(), new IntegerHelper(), new RealHelper(), new InterpBooleanHelper());
            FileGenerator fileGenerator = new FileGenerator(new FileHelper());

            string file = @"..\..\..\..\..\Calculator.fgl";
            //string input = ReadFile(file);
            string input = "export 5.5 + 33.3 " +
                           "export 5.5 * func(33) " +
                           "func: (integer)->real " +
                           "func(p) = | p | *17 + p / 2 ^ (17 - 0.1 mod 2)";

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