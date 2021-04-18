using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;
using ASTLib.Interfaces;

namespace ASTLib.Nodes.ExpressionNodes
{
    public class RealLiteralExpression : ExpressionNode, INonIdentifierExpression
    {
        public double Value { get; }

        public RealLiteralExpression(string token, int line, int letter) :
            base(new List<ExpressionNode>(), line, letter) 
        {
            Value = ConvertToDouble(token);
        }
        
        public RealLiteralExpression(double value, int line, int letter) : base(null, line, letter) 
        {
            Value = value;
            Children = new List<ExpressionNode>();
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
