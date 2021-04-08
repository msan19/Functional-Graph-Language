using ASTLib;

namespace LexParserLib
{
    public interface ILexParser
    {
        AST Run(string input, bool print);
    }
}