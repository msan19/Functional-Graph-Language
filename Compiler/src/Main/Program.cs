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
using ASTLib.Exceptions.Component;
using TypeSetHelper = TypeCheckerLib.Helpers.SetHelper;
using InterpreterSetHelper = InterpreterLib.Helpers.SetHelper;
using TypeCheckerLib.Interfaces;
using InterpreterLib.Interfaces;

namespace Main
{
    public class Program
    {
        public List<string> Lines { get; }

        private List<string> _fileNames;
        private bool _shouldThrowExceptions;
        private bool _printCode;
        private bool _printParseTree;
        private bool _printOutput;
        private bool _saveOutput;
        private bool _projectFolder;

        private readonly string _input;

        private readonly ILexParser _lexParse;
        private readonly IReferenceHandler _referenceHandler;
        private readonly ITypeChecker _typeChecker;
        private readonly IInterpreter _interpreter;
        private readonly IFileGenerator _fileGenerator;

        static void Main(string[] args)
        {
            Program program = new Program(args);
            program.Compile();
        }

        public Program(string[] args)
        {
            ParseArgs(args);
            _lexParse = new LexParser(new ASTBuilder(new ExpressionHelper()));
            _referenceHandler = new ReferenceHandler(new ReferenceHelper(), !_shouldThrowExceptions);
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
                                           new InterpreterSetHelper(),
                                           new ElementHelper(),
                                           new StringHelper(),
                                           new GraphHelper());
            _fileGenerator = new FileGenerator(new GmlGenerator(), new FileHelper());

            _input = _fileGenerator.Read(_fileNames, _projectFolder);
            _input = _input.Replace('\t', ' ');
            Lines = _input.Split("\n").ToList();
            if(_printCode) Console.WriteLine(_input);
        }

        private void Compile()
        {
            if (_shouldThrowExceptions)
                Run();
            else
                RunWithExceptionPrinting();
        }

        private void RunWithExceptionPrinting()
        {
            IExceptionPrinter exceptionPrinter = new ExceptionPrinter(Lines);

            try
            {
                Run();
            }
            catch (ComponentException e)
            {
                foreach (CompilerException ce in e.Exceptions)
                    exceptionPrinter.Print(ce);
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

        public void Run()
        {
            AST ast = _lexParse.Run(_input, _printParseTree);
            _referenceHandler.InsertReferences(ast);
            _typeChecker.CheckTypes(ast);
            var output = _interpreter.Interpret(ast);
            _fileGenerator.Export(output, _printOutput, _saveOutput, _projectFolder);
        }

        private bool ParseArgs(string[] args)
        {
            _fileNames = new List<string>() { "Cycle.fgl", "bTree.fgl" };
            _saveOutput = true;
            foreach (string s in args)
            {
                if (s == "throw")
                    _shouldThrowExceptions = true;
                else if (s == "parseTree")
                    _printParseTree = true;
                else if (s == "code")
                    _printCode = true;
                else if (s == "output")
                    _printOutput = true;
                else if (s == "noWrite")
                    _saveOutput = false;
                else if (s == "project")
                    _projectFolder = true;
                else if (s == "help")
                    PrintHelp();
                else
                    _fileNames.Add(s);
            }

            return args.Length >= 1 && args.Contains("throw");
        }

        private void PrintHelp()
        {
            Console.WriteLine("Compiler options:");
            Console.WriteLine("\t'help'\t" + "\tThe list of compiler option is shown");
            Console.WriteLine("\t'throw'\t" + "\tExceptions are unhandled");
            Console.WriteLine("\t'parseTree'" + "\tThe parse tree is shown");
            Console.WriteLine("\t'code'\t" + "\tThe source code is shown");
            Console.WriteLine("\t'output'" + "\tThe output is shown");
            Console.WriteLine("\t'noWrite'" + "\tThe output is no longer saved");
            Console.WriteLine("\t'project'" + "\tThe input and output uses the project folder");
        }
    }
}