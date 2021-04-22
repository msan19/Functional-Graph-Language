using ASTLib.Exceptions;
using System.Collections.Generic;

namespace Main
{
    public interface IExceptionPrinter
    {
        void SetLines(List<string> lines);
        void Print(CompilerException e);
        void Print(ParserException e);
    }
}