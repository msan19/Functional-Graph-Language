using InterpreterLib.Interfaces;
using NSubstitute;

namespace InterpreterLib.Tests
{
    public static class Utilities
    {
        public static Interpreter GetFullyMockedIntepretor()
        {
            IFunctionHelper functionHelper = Substitute.For<IFunctionHelper>();
            IIntegerHelper integerHelper = Substitute.For<IIntegerHelper>();
            IRealHelper realHelper = Substitute.For<IRealHelper>();
            IBooleanHelper booleanHelper = Substitute.For<IBooleanHelper>();
            return new Interpreter(functionHelper, integerHelper, realHelper, booleanHelper);
        }
        
        public static Interpreter GetIntepretorOnlyWith(IFunctionHelper functionHelper)
        {
            IIntegerHelper integerHelper = Substitute.For<IIntegerHelper>();
            IRealHelper realHelper = Substitute.For<IRealHelper>();
            IBooleanHelper booleanHelper = Substitute.For<IBooleanHelper>();
            return new Interpreter(functionHelper, integerHelper, realHelper, booleanHelper);
        }
        
        public static Interpreter GetIntepretorOnlyWith(IIntegerHelper integerHelper)
        {
            IFunctionHelper functionHelper = Substitute.For<IFunctionHelper>();
            IRealHelper realHelper = Substitute.For<IRealHelper>();
            IBooleanHelper booleanHelper = Substitute.For<IBooleanHelper>();
            return new Interpreter(functionHelper, integerHelper, realHelper, booleanHelper);
        }
        
        public static Interpreter GetIntepretorOnlyWith(IRealHelper realHelper)
        {
            IFunctionHelper functionHelper = Substitute.For<IFunctionHelper>();
            IIntegerHelper integerHelper = Substitute.For<IIntegerHelper>();
            IBooleanHelper booleanHelper = Substitute.For<IBooleanHelper>();
            return new Interpreter(functionHelper, integerHelper, realHelper, booleanHelper);
        }
        
        public static Interpreter GetIntepretorOnlyWith(IBooleanHelper booleanHelper)
        {
            IFunctionHelper functionHelper = Substitute.For<IFunctionHelper>();
            IIntegerHelper integerHelper = Substitute.For<IIntegerHelper>();
            IRealHelper realHelper = Substitute.For<IRealHelper>();
            return new Interpreter(functionHelper, integerHelper, realHelper, booleanHelper);
        }
    }
}