using ASTLib.Exceptions;

namespace Main
{
    public interface IExceptionPrinter
    {
        void Print(CompilerException e);
        void Print(ParserException e);
    }
}