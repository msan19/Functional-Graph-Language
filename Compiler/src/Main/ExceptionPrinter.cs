using ASTLib.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Main
{
    public class ExceptionPrinter : IExceptionPrinter
    {
        private string[][] _lines;
        private string[] _files;

        public void SetLines(string[][] lines, string[] files)
        {
            _files = files;
            _lines = lines;
        }

        public void Print(CompilerException e)
        {
            if (e.Node != null)
            {
                PrintError(e.Node.LineNumber,
                                    e.Node.LetterNumber,
                                    e.Message,
                                    GetLine(e.Node.LineNumber - 1));
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
                                    GetLine(e.Lines[i] - 1));
            }
        }

        private void PrintError(int line, int letter, string message, string code)
        {
            string s = letter <= 0 ? "" : new string(' ', letter - 1) + "\u2191";
            PrintLine($"\nAn error was detected on line {GetLineNumber(line)} in {GetFileName(line)}:\n{message}", ConsoleColor.Red);
            PrintLine(code);
            PrintLine(s, ConsoleColor.Red);
        }

        private void PrintLine(string message, ConsoleColor c)
        {
            Console.ForegroundColor = c;
            Console.WriteLine(message);
            Console.ResetColor();
        }

        private void PrintLine(string message)
        {
            Console.WriteLine(message);
        }

        private string GetLine(int line)
        {
            return GetGeneric(line, (i, ii) => _lines[ii][i]);
        }

        private int GetLineNumber(int line)
        {
            return GetGeneric(line, (i, ii) => i);
        }

        private string GetFileName(int line)
        {
            return GetGeneric(line, (i, ii) => _files[ii]);
        }

        private T GetGeneric<T>(int line, Func<int, int, T> func)
        {
            int i = line;
            for (int ii = 0; ii < _lines.Length; ii++)
                if (i < _lines[ii].Length)
                    return func(i, ii);
                else
                    i -= _lines[ii].Length;
            return default;
        }
    }
}
