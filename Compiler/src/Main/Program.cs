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
using System.IO;
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
                    if (e.Node != null)
                    {
                        program.PrintError(e.Node.LineNumber, 
                                           e.Node.LetterNumber, 
                                           e.Message, 
                                           program.Lines[e.Node.LineNumber - 1]);
                    }
                    else
                        Console.WriteLine($"\nAn error was detected:\n{e.Message}");
                } 
                catch (ParserException e)
                {
                    for(int i = 0; i < e.Messages.Count; i++)
                    {
                        program.PrintError(e.Lines[i],
                                           e.Letters[i],
                                           e.Messages[i],
                                           program.Lines[e.Lines[i] - 1]);
                    }
                }
            }
        }

        private void PrintError(int line, int letter, string message, string code)
        {
            string s = letter <= 0 ? "" : new string(' ', letter - 1) + "\u2191";
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"\nAn error was detected on line {line}:\n{message}");
            Console.ResetColor();
            Console.WriteLine(code);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(s);
            Console.ResetColor();
        }

        public Program(string[] args)
        {
            _lexParse = new LexParser(new ASTBuilder());
            _referenceHandler = new ReferenceHandler(new ReferenceHelper());
            _typeChecker = new TypeChecker(new DeclarationHelper(), new NumberHelper(), new CommonOperatorHelper(), new TypeBooleanHelper());
            _interpreter = new Interpreter(new FunctionHelper(), new IntegerHelper(), new RealHelper(), new InterpBooleanHelper());
            _fileGenerator = new FileGenerator(new FileHelper());

            // string file = @"..\..\..\..\..\Calculator.fgl";
            string fileName = "test.fgl";
            _input = FileReader.Read(fileName);
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

        

    }
}