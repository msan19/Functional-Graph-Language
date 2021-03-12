﻿using System;
using ASTLib;
using FileGeneratorLib;
using InterpreterLib;
using LexParserLib;
using ReferenceHandlerLib;
using TypeCheckerLib;

namespace Main
{
    class Program
    {
        static void Main(string[] args)
        {
            LexParser lexParse = new LexParser(new ASTBuilder());
            ReferenceHandler referenceHandler = new ReferenceHandler(new ReferenceHelper());
            TypeChecker typeChecker = new TypeChecker(new TypeHelper());
            Interpreter interpreter = new Interpreter(new InterpretorHelper());
            FileGenerator fileGenerator = new FileGenerator(new FileHelper());
            
            AST ast = lexParse.Run();
            referenceHandler.InsertReferences(ast);
            typeChecker.CheckTypes(ast);
            string output = interpreter.Interpret(ast);
            fileGenerator.Export(output);
        }
    }
}