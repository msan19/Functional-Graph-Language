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
using FileUtilities;
using FileUtilities.Interfaces;
using TypeSetHelper = TypeCheckerLib.Helpers.SetHelper;
using InterpreterSetHelper = InterpreterLib.Helpers.SetHelper;
using TypeCheckerLib.Interfaces;
using InterpreterLib.Interfaces;

namespace Main
{
    public class Program
    {
        private List<string> _fileNames;
        private bool _shouldThrowExceptions;
        private bool _printCode;
        private bool _printParseTree;
        private bool _printOutput;
        private bool _saveOutput;
        private bool _projectFolder;

        private readonly ILexParser _lexParse;
        private readonly IReferenceHandler _referenceHandler;
        private readonly ITypeChecker _typeChecker;
        private readonly IInterpreter _interpreter;
        private readonly IFileGenerator _fileGenerator;
        private readonly IExceptionPrinter _exceptionPrinter;
        private readonly IFileReader _fileReader;

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
                                           new GraphHelper(),
                                           !_shouldThrowExceptions);
            _fileGenerator = new FileGenerator(new GmlGenerator(), new FileHelper());
            _exceptionPrinter = new ExceptionPrinter();
            _fileReader = new FileReader(new FileHelper());
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

            try
            {
                Run();
            }
            catch (ComponentException e)
            {
                foreach (CompilerException ce in e.Exceptions)
                    _exceptionPrinter.Print(ce);
            }
            catch (CompilerException e)
            {
                _exceptionPrinter.Print(e);
            }
            catch (ParserException e)
            {
                _exceptionPrinter.Print(e);
            } catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public void Run()
        {
            string input = GetInput();

            if (input == "")
                Console.WriteLine("No input files have been given!");
            else
            {
                AST ast = _lexParse.Run(input, _printParseTree);
                _referenceHandler.InsertReferences(ast);
                _typeChecker.CheckTypes(ast);
                var output = _interpreter.Interpret(ast);
                _fileGenerator.Export(output, _printOutput, _saveOutput, _projectFolder);
            }
        }

        private string GetInput()
        {
            string input = _fileReader.Read(_fileNames, _projectFolder);
            input = input.Replace('\t', ' ');
            _exceptionPrinter.SetLines(input.Split("\n").ToList());
            if (_printCode) Console.WriteLine(input);
            return input;
        }

        private void ParseArgs(string[] args)
        {
            _fileNames = new List<string>() { "Grid.fgl" };
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