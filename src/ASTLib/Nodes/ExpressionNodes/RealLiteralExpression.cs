using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;

namespace ASTLib.Nodes.ExpressionNodes
{
    public class RealLiteralExpression : ExpressionNode
    {
        public double Value { get; }

        public RealLiteralExpression(string token, int line, int letter) : base(null, line, letter) 
        {
            Value = ConvertToDouble(token);
        }
        private double ConvertToDouble(string str)
        {
            double result = -1.00;
            try
            {
                result = double.Parse(str, NumberStyles.AllowDecimalPoint, NumberFormatInfo.InvariantInfo);
            }
            catch (FormatException)
            {
                Console.WriteLine($"Unable to parse string '{str}' to a real");
            }
            return result;
        }
    }
}
