using ASTLib.Interfaces;
using ASTLib.Nodes.TypeNodes;
using System;
using System.Collections.Generic;
using System.Text;

namespace ASTLib.Nodes.ExpressionNodes
{
    public class IntegerLiteralExpression : ExpressionNode, INonIdentifierExpression
    {
        public int Value { get; }

        public IntegerLiteralExpression(string token, int line, int letter) : 
            base(new List<ExpressionNode>(), line, letter) 
        {
            Value = ConvertToInt(token);
        }

        public IntegerLiteralExpression(int value, int line, int letter) : base(null, line, letter)
        {
            Value = value;
            Children = new List<ExpressionNode>();
        }

        private int ConvertToInt(string str)
        {
            int result = -1;
            try
            {
                result = int.Parse(str);
            }
            catch (FormatException)
            {
                Console.WriteLine($"Unable to parse string '{str}' to an integer");
            }
            return result;
        }
    }
}
