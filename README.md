# P4
Functional Graph Language

![fgl](Compiler/src/Main/fgl.ico)

---

## Relevant Folder Structure
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
    - [UnitTesting](./Compiler/Tests/UnitTesting)
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
