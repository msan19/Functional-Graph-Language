using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;

namespace ASTLib.Nodes.ExpressionNodes
{
    public class RealLiteralExpression : ExpressionNode
    {
        public double Value { get; set; }

        public RealLiteralExpression(string token) : base(null) 
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
                Console.WriteLine($"Unable to parse string '{str}'");
            }
            return result;
        }
    }
}
