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
using ASTLib.Exceptions.Component;
using ASTLib.Objects;
using FileGeneratorLib.Interfaces;
using FileUtilities;
using FileUtilities.Interfaces;
using TypeSetHelper = TypeCheckerLib.Helpers.SetHelper;
using InterpreterSetHelper = InterpreterLib.Helpers.SetHelper;
using TypeCheckerLib.Interfaces;
using InterpreterLib.Interfaces;
using LexParserLib.ASTBuilding;
using LexParserLib.Hime;
using Microsoft.Extensions.Configuration;

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
        private readonly GmlGenerator _gmlGenerator;

        private IConfiguration config;

        static void Main(string[] args)
        {
            Program program = new Program(args);
            program.Compile();
        }

        public Program(string[] args)
        {
            config = GetConfig();
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
            _gmlGenerator = new GmlGenerator();
            _fileGenerator = new FileGenerator(new FileHelper());
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
            if (_fileNames.Count == 0)
                Console.WriteLine("No input files have been given!");
            else
            {
                string input = GetInput();
                AST ast = _lexParse.Run(input, _printParseTree);
                _referenceHandler.InsertReferences(ast);
                _typeChecker.CheckTypes(ast);
                List<LabelGraph> output = _interpreter.Interpret(ast);
                List<ExtensionalGraph> gmlGraphs = _gmlGenerator.Generate(output);
                _fileGenerator.Export(gmlGraphs, _printOutput, _saveOutput, _projectFolder);
            }
        }

        private string GetInput()
        {
            List<string> input = _fileReader.Read(_fileNames, _projectFolder);
            string[][] lines = new string[_fileNames.Count][];
            for (int i = 0; i < _fileNames.Count; i++)
            {
                lines[i] = input[i].Split("\n");
                if (_printCode) Console.WriteLine($"\n//File: {_fileNames[i]}\n{input[i]}\n\n");
            }
            _exceptionPrinter.SetLines(lines, _fileNames.ToArray());
            return input.Aggregate((a, s) => a + "\n" + s);
        }

        private void ParseArgs(string[] args)
        {
            _fileNames = new List<string>(); //"{ "Cycle.fgl", "Anonymous.fgl" };
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
            Console.WriteLine( config["Main:HelpTextHeader"] );
            Console.WriteLine( GetArgStr("help") );
            Console.WriteLine( GetArgStr("throw") );
            Console.WriteLine( GetArgStr("parseTree") );
            Console.WriteLine( GetArgStr("code") );
            Console.WriteLine( GetArgStr("output") );
            Console.WriteLine( GetArgStr("noWrite") );
            Console.WriteLine( GetArgStr("project") );
        }

        private string GetArgStr(string arg)
        {
            return GetArgStrName(arg) + GetArgStrDesc(arg);
        }

        private string GetArgStrName(string arg)
        {
            return config[$"Main:Arguments:{arg}:Name"];
        }
        
        private string GetArgStrDesc(string arg)
        {
            return config[$"Main:Arguments:{arg}:Desc"];
        }

        private IConfiguration GetConfig()
        {
            return new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", true, true)
                .Build();
        }
    }
}