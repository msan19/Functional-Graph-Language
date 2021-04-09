using ASTLib.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Main
{
    public class ExceptionPrinter : IExceptionPrinter
    {
        private readonly List<string> _lines;

        public ExceptionPrinter(List<string> lines)
        {
            _lines = lines;
        }

        public void Print(CompilerException e)
        {
            if (e.Node != null)
            {
                PrintError(e.Node.LineNumber,
                                    e.Node.LetterNumber,
                                    e.Message,
                                    _lines[e.Node.LineNumber - 1]);
            }
            else
                Console.WriteLine($"\nAn error was detected:\n{e.Message}");
        }

        public void Print(ParserException e)
        {
            for (int i = 0; i < e.Messages.Count; i++)
            {
                PrintError(e.Lines[i],
                                    e.Letters[i],
                                    e.Messages[i],
                                    _lines[e.Lines[i] - 1]);
            }
        }

        private void PrintError(int line, int letter, string message, string code)
        {
            string s = letter <= 0 ? "" : new string(' ', letter - 1) + "\u2191";
            PrintLine($"\nAn error was detected on line {line}:\n{message}", ConsoleColor.Red);
            PrintLine(code);
            PrintLine(s, ConsoleColor.Red);
        }

        private void PrintLine(string message, ConsoleColor red)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            Console.ResetColor();
        }
        private void PrintLine(string message)
        {
            Console.WriteLine(message);
        }
    }
}
