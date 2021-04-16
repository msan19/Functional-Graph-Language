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
using TypeSetHelper = TypeCheckerLib.Helpers.SetHelper;
using InterpreterSetHelper = InterpreterLib.Helpers.SetHelper;

namespace Main
{
    public class Program
    {
        public List<string> Lines { get; }

        private readonly string _fileName = "test.fgl";
        private readonly string _input;

        private readonly LexParser _lexParse;
        private readonly ReferenceHandler _referenceHandler;
        private readonly TypeChecker _typeChecker;
        private readonly Interpreter _interpreter;
        private readonly FileGenerator _fileGenerator;

        static void Main(string[] args)
        {
            if (ShouldThrowExceptions(args))
                RunWithoutExecptionPrinting(args);
            else
                RunWithExceptionPrinting(args);
        }

        private static bool ShouldThrowExceptions(string[] args)
        {
            return args.Length >= 1 && args[0] == "throw";
        }

        private static void RunWithExceptionPrinting(string[] args)
        {
            Program program = new Program(args);
            IExceptionPrinter exceptionPrinter = new ExceptionPrinter(program.Lines);

            try
            {
                program.Run();
            }
            catch (CompilerException e)
            {
                exceptionPrinter.Print(e);
            }
            catch (ParserException e)
            {
                exceptionPrinter.Print(e);
            }
        }

        private static void RunWithoutExecptionPrinting(string[] args)
        {
            Program program = new Program(args);
            program.Run();
        }

        public Program(string[] args)
        {
            _lexParse = new LexParser(new ASTBuilder(new ExpressionHelper()));
            _referenceHandler = new ReferenceHandler(new ReferenceHelper());
            _typeChecker = new TypeChecker(new DeclarationHelper(), 
                                           new NumberHelper(), 
                                           new CommonOperatorHelper(), 
                                           new TypeBooleanHelper(), 
                                           new TypeSetHelper());
            _interpreter = new Interpreter(new GenericHelper(), 
                                           new FunctionHelper(), 
                                           new IntegerHelper(), 
                                           new RealHelper(), 
                                           new InterpBooleanHelper(),
                                           new InterpreterSetHelper());
            _fileGenerator = new FileGenerator(new FileHelper());

            _input = FileReader.Read(_fileName);
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