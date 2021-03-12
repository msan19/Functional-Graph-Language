using System;
using System.Collections.Generic;
using System.Text;

namespace ASTLib.Nodes.ExpressionNodes
{
    public class IntegerLiteralExpression : ExpressionNode
    {
        public int Value { get; set; }

        public IntegerLiteralExpression(string token) : base(null) 
        {
            Value = ConvertToInt(token);
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
                Console.WriteLine($"Unable to parse string '{str}'");
            }
            return result;
        }
    }
}
