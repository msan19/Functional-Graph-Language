using ASTLib.Nodes;
using System;
using System.Collections.Generic;
using System.Text;

namespace ASTLib.Exceptions
{
    public class ParserException : CompilerException
    {

        public ParserException(List<string> messages, List<int> lines) : base(null, GetMessage(messages, lines))
        {
        }

        private static string GetMessage(List<string> messages, List<int> lines)
        {
            string message = "";
            for(int i = 0; i < messages.Count; i++)
            {
                message += $"At line {lines[i]}: {message}\n";
            }

            return message;
        }

    }
}
