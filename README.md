# Functional Graph Language

![fgl](Compiler/src/Main/fgl.ico)

FGL is a purely functional language that leans on the syntax of mathematics.
It uses set-builder notation to construct sets, and graphs are constructed using the formula _G = (V,E,src,dst)_.
An FGL file is compiled to GML code, and because of the compiler's component architecture, other formats like DOT can be added with relative ease.

## Running the Compiler
Opening the _Compiler.sln_ file in the folder _Compiler_ will result in all projects being opened. 
When opened the startup project should be set to _Main_.

The .flg file which is parsed is specified in the function `ParseArgs` found in _Compiler/src/Main/Program.cs_.
The file which is specified here is found in _Compiler/src/Main/InputFiles_. 
It is possible to add new .fgl files here and reference them in `ParseArgs`.

If the .sln file is open using Visual Studio 2019, two debug profiles will be available, `Main` and `Exception Catched`.
 - `Main` runs the compiler with the following arguments: _throw_ and _project_.
 - `Exception Catched` runs the compiler with the following arguments: _help_, _code_, _project_, and  _output_.

## Compiler Arguments
- `help`          shows all possible compiler arguments with a short description
- `throw`         the compiler does not handle exceptions (used when debugging the compiler itself)
- `noWrite`       the output is no longer saved in a .gml file
- `project`       uses the input and output folders used in the .sln project
- `parseTree`     prints the parse tree to the console 
- `code`          prints the source code to the console
- `output`        prints the output to the console

## Folder Structure
The following folder structure is founder the folder _Compiler_.

All source code is found in the folder _src_ where each subfolder is a .NET Core 3.1 project. 
All Testing is found in the folder _Tests_ which is split up into Unit- and Integration Tests.

- [src](./Compiler/src)
    - ASTLib
    - FileGeneratorLib
    - FileUtilities
    - InterpreterLib
    - LexParserLib
    - Main
    - ReferenceHandlerLib
    - TypeCheckerLIb
- Tests
    - [Unit Testing](./Compiler/Tests/UnitTesting)
        - ASTLib.Tests
        - FileGeneratorLib.Tests
        - FileUtilities.Tests
        - InterpreterLib.Tests
        - LexParserLib.Tests
        - Main.Tests
        - ReferenceHandlerLib.Tests
        - TypeCheckerLIb.Tests
    - IntegrationTesting
        - [Single Component](./Compiler/Tests/IntegrationTesting/Single)
            - SingleCompIntepreterLib.Tests
            - SingleCompReferenceHandlerLib.Tests
            - SingleCompTypeCheckerLib.Tests
        - [Multiple Component](./Compiler/Tests/IntegrationTesting/Multiple)
            - IntepreterAndGmlGenerator.Tests
            - ReferenceHandlerAndTypeChecker.Tests
            - TypeCheckerAndIntepreter.Tests
