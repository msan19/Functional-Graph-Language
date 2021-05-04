using ASTLib.Exceptions;
using System.Collections.Generic;

namespace Main
{
    public interface IExceptionPrinter
    {
        void SetLines(string[][] lines, string[] files);
        void Print(CompilerException e);
        void Print(ParserException e);
    }
}