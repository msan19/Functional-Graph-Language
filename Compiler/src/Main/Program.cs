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
using System.Collections.Generic;
using System.Linq;

namespace Main
{
    public class Program
    {
        public List<string> Lines { get; }

        private readonly string _input;

        private readonly LexParser _lexParse;
        private readonly ReferenceHandler _referenceHandler;
        private readonly TypeChecker _typeChecker;
        private readonly Interpreter _interpreter;
        private readonly FileGenerator _fileGenerator;

        static void Main(string[] args)
        {
            Program program = new Program(args);

            if (args.Length >= 1 && args[0] == "throw")
                program.Run();
            else
            {
                try
                {
                    program.Run();
                }
                catch (CompilerException e)
                {
                    Console.WriteLine();
                    if (e.Node != null)
                    {
                        string s = e.Node.LetterNumber <= 0 ? "" : "\n" + new string(' ', e.Node.LetterNumber - 1) + "\u2191";
                        Console.WriteLine($"An error was detected on line {e.Node.LineNumber}:\n" +
                                          $"{e.Message}\n{program.Lines[e.Node.LineNumber - 1]}{s}");
                    }
                    else
                        Console.WriteLine($"An error was detected:\n{e.Message}");
                }
            }
        }

        public Program(string[] args)
        {
            _lexParse = new LexParser(new ASTBuilder());
            _referenceHandler = new ReferenceHandler(new ReferenceHelper());
            _typeChecker = new TypeChecker(new DeclarationHelper(), new NumberHelper(), new CommonOperatorHelper(), new TypeBooleanHelper());
            _interpreter = new Interpreter(new FunctionHelper(), new IntegerHelper(), new RealHelper(), new InterpBooleanHelper());
            _fileGenerator = new FileGenerator(new FileHelper());

            string file = @"..\..\..\..\..\Calculator.fgl";
            _input = ReadFile(file);
            _input = _input.Replace('\t', ' ');
            Lines = _input.Split("\n").ToList();
            Console.WriteLine(_input);
        }

        public void Run()
        {
            AST ast = _lexParse.Run(_input, false);
            _referenceHandler.InsertReferences(ast);
            _typeChecker.CheckTypes(ast);
            var output = _interpreter.Interpret(ast);
            _fileGenerator.Export(output, "");
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